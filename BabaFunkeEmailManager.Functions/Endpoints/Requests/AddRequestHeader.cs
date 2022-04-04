using BabaFunkeEmailManager.Data.Models;
using BabaFunkeEmailManager.Service.Services.Interfaces;
using BabaFunkeEmailManager.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace BabaFunkeRequestManager.Functions.Endpoints.Requests
{
    public class AddRequestHeader
    {
        private readonly IService<RequestHeader> _requestHeaderService;
        private readonly IQueueService<RequestDetail> _queueService;
        private readonly ISubscriberService _subscriberService;
        private readonly IService<Newsletter> _newsletterService;

        public AddRequestHeader(IService<RequestHeader> requestHeaderService,
            IQueueService<RequestDetail> queueService,
            ISubscriberService subscriberService,
            IService<Newsletter> newsletterService)
        {
            _requestHeaderService = requestHeaderService ?? throw new ArgumentNullException(nameof(requestHeaderService));
            _queueService = queueService ?? throw new ArgumentNullException(nameof(queueService));
            _subscriberService = subscriberService ?? throw new ArgumentNullException(nameof(subscriberService));
            _newsletterService = newsletterService ?? throw new ArgumentNullException(nameof(newsletterService));
        }

        [FunctionName("AddRequest")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "addrequest")] HttpRequest req,
            ILogger log)
        {
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var requestHeader = JsonSerializer.Deserialize<RequestHeader>(requestBody, options);

                var result = await _requestHeaderService.AddItem(requestHeader);

                if (!result.IsSuccess)
                {
                    return new BadRequestObjectResult(result);
                }

                await ProcessRequest(requestHeader);

                return new OkObjectResult("Request Header Add Processing complete");
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message, ex);
                return new BadRequestObjectResult("Problem adding new Request");
            }
        }

        /// <summary>
        /// Process the RequestHeader by creating an instance of RequestDetail for each record
        /// Note the split into batches if more than 500 as MessageQueues have a limit to the number
        /// of messages that can be held at a time. The delay allows room for it to offload some messages
        /// before adding new ones.
        /// </summary>
        private async Task ProcessRequest(RequestHeader requestHeader)
        {
            var filteredSubscribers = await _subscriberService.GetAllActiveSubscribersBySubCategory(requestHeader.SubscriberSubCategory);

            var newsletterResponse = await _newsletterService.GetItem(requestHeader.NewsletterId);

            var newsletter = newsletterResponse.Data;

            double total = filteredSubscribers.Count();
            int batchSize = 500;
            if (total > batchSize)
            {
                var batchTotal = (int)Math.Ceiling(total / batchSize);
                for (var i = 0; i < batchTotal; i++)
                {
                    var subscribers = filteredSubscribers.Skip(batchSize * i).Take(batchSize);
                    foreach (var subscriber in subscribers)
                    {
                        await _queueService
                            .AddItemToQueue(new RequestDetail(requestHeader.RequestHeaderId, newsletter.Body, newsletter.Subject, subscriber.Email, subscriber.Firstname));
                    }
                    await Task.Delay(5000);
                }
            }
            else
            {
                foreach (var subscriber in filteredSubscribers)
                {
                    await _queueService
                        .AddItemToQueue(new RequestDetail(requestHeader.RequestHeaderId, newsletter.Body, newsletter.Subject, subscriber.Email, subscriber.Firstname));
                }
            }
        }
    }
}