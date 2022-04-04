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
    public class GetNewsletter
    {
        private readonly IService<Newsletter> _service;

        public GetNewsletter(IService<Newsletter> service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [FunctionName("GetNewsletter")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "getnewsletter/{newsletterId}")] HttpRequest req,
            string newsletterId,
            ILogger log)
        {
            try
            {
                var response = await _service.GetItem(newsletterId);

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