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

namespace BabaFunkeEmailManager.Functions.Endpoints.Newsletters
{
    public class UpdateNewsletter
    {
        private readonly IService<Newsletter> _service;

        public UpdateNewsletter(IService<Newsletter> service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [FunctionName("UpdateNewsletter")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "updatenewsletter/{newsletterId}")] HttpRequest req,
            string newsletterId,
            ILogger log)
        {
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var newsletter = JsonSerializer.Deserialize<Newsletter>(requestBody, options);

                var result = await _service.UpdateItem(newsletterId, newsletter);

                if (!result.IsSuccess)
                {
                    return new BadRequestObjectResult(result);
                }

                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message, ex);
                return new BadRequestObjectResult("Problem updating Newsletter");
            }
        }
    }
}
