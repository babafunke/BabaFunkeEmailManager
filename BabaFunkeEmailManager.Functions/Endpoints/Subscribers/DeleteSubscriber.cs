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
    public class DeleteSubscriber
    {
        private readonly ISubscriberService _service;

        public DeleteSubscriber(ISubscriberService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [FunctionName("DeleteSubscriber")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "deletesubscriber/{email}")] HttpRequest req,
            string email,
            ILogger log)
        {
            try
            {
                var response = await _service.DeleteItem(email);

                if (!response.IsSuccess)
                {
                    return new BadRequestObjectResult(response);
                }

                return new OkObjectResult(response);
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message, ex);
                throw new Exception(ex.ToString());
            }
        }
    }
}