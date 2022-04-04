using BabaFunkeEmailManager.Data.Models;
using BabaFunkeEmailManager.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BabaFunkeEmailManager.Functions.Endpoints.Subscribers
{
    public class GetAllSubscribers
    {
        private readonly ISubscriberService _service;

        public GetAllSubscribers(ISubscriberService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [FunctionName("GetAllSubscribers")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "getallsubscribers")] HttpRequest req,
            ILogger log)
        {
            try
            {
                var subscribers = await _service.GetAllItems();

                if (subscribers == null)
                {
                    return new BadRequestResult();
                }

                return new OkObjectResult(subscribers);
            }
            catch (Exception ex)
            {
                log.LogError($"Problem getting list of subscribers: {ex.Message}");
                return new BadRequestResult();
            }
        }
    }
}