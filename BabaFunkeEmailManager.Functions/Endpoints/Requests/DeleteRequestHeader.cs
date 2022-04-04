using BabaFunkeEmailManager.Data.Models;
using BabaFunkeEmailManager.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BabaFunkeEmailManager.Functions.Endpoints.Requests
{
    public class DeleteRequestHeader
    {
        private readonly IService<RequestHeader> _service;

        public DeleteRequestHeader(IService<RequestHeader> service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [FunctionName("DeleteRequestHeader")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "deleterequestheader/{requestHeaderId}")] HttpRequest req,
            string requestHeaderId,
            ILogger log)
        {
            try
            {
                var response = await _service.DeleteItem(requestHeaderId);

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