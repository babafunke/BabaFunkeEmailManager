using BabaFunkeEmailManager.Data.Models;
using BabaFunkeEmailManager.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BabaFunkeEmailManager.Functions.Endpoints.Newsletters
{
    public class GetAllNewsletters
    {
        private readonly IService<Newsletter> _service;

        public GetAllNewsletters(IService<Newsletter> service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [FunctionName("GetAllNewsletters")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "getallnewsletters")] HttpRequest req,
            ILogger log)
        {
            try
            {
                var newsletters = await _service.GetAllItems();

                if (newsletters == null)
                {
                    return new BadRequestResult();
                }

                return new OkObjectResult(newsletters);
            }
            catch (Exception ex)
            {
                log.LogError($"Problem getting list of newsletters: {ex.Message}");
                return new BadRequestResult();
            }
        }
    }
}
