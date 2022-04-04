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
using Microsoft.Azure.Functions.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(BabaFunkeEmailManager.Functions.Startup))]
namespace BabaFunkeEmailManager.Functions.Endpoints.Newsletters
{
    public class AddNewsletter
    {
        private readonly IService<Newsletter> _service;

        public AddNewsletter(IService<Newsletter> service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [FunctionName("AddNewsletter")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function,"post", Route = "addnewsletter")] HttpRequest req,
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

                var result = await _service.AddItem(newsletter);

                if(!result.IsSuccess)
                {
                    return new BadRequestObjectResult(result);
                }

                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message, ex);
                return new BadRequestObjectResult("Problem adding new Newsletter");
            }
        }
    }
}