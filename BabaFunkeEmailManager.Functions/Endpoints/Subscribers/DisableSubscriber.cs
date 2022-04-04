using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using BabaFunkeEmailManager.Data.Models;
using BabaFunkeEmailManager.Services.Interfaces;

namespace BabaFunkeEmailManager.Functions.Endpoints.Subscribers
{
    public class DisableSubscriber
    {
        private readonly ISubscriberService _service;

        public DisableSubscriber(ISubscriberService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [FunctionName("DisableSubscriber")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "patch", Route = "unsubscribe")] HttpRequest req,
            ILogger log)
        {
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var subscriber = JsonSerializer.Deserialize<Subscriber>(requestBody, options);

                var result = await _service.DisableItem(subscriber.Email);

                if (!result.IsSuccess)
                {
                    return new BadRequestObjectResult(result);
                }

                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message, ex);
                return new BadRequestObjectResult("Problem unsubscribing Subscriber");
            }
        }
    }
}