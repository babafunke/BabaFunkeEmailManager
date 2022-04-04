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
    public class GetSubscriber
    {
        private readonly ISubscriberService _service;

        public GetSubscriber(ISubscriberService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [FunctionName("GetSubscriber")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "getsubscriber/{email}")] HttpRequest req,
            string email,
            ILogger log)
        {
            try
            {
                var response = await _service.GetItem(email);

                if (!response.IsSuccess)
                {
                    return new BadRequestObjectResult(response);
                }

                return new OkObjectResult(response.Data);
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message, ex);
                throw new Exception(ex.ToString());
            }
        }
    }
}
