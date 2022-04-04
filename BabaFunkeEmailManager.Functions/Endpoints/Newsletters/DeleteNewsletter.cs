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
    public class DeleteNewsletter
    {
        private readonly IService<Newsletter> _service;

        public DeleteNewsletter(IService<Newsletter> service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [FunctionName("DeleteNewsletter")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "deletenewsletter/{newsletterId}")] HttpRequest req,
            string newsletterId,
            ILogger log)
        {
            try
            {
                var response = await _service.DeleteItem(newsletterId);

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