using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using BabaFunkeEmailManager.Services.Interfaces;
using BabaFunkeEmailManager.Data.Models;

namespace BabaFunkeEmailManager.Functions.Endpoints.Subscribers
{
    public class UpdateSubscriber
    {
        private readonly ISubscriberService _service;

        public UpdateSubscriber(ISubscriberService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [FunctionName("UpdateSubscriber")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "updatesubscriber/{email}")] HttpRequest req,
            string email,
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

                var result = await _service.UpdateItem(email, subscriber);

                if (!result.IsSuccess)
                {
                    return new BadRequestObjectResult(result);
                }

                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message, ex);
                return new BadRequestObjectResult("Problem updating Subscriber");
            }
        }
    }
}
