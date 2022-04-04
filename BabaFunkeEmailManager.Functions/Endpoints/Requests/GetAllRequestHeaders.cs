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
    /// <summary>
    /// Gets a list of RequestHeaders from TableStorage
    /// </summary>
    public class GetAllRequestHeaders
    {
        private readonly IService<RequestHeader> _service;

        public GetAllRequestHeaders(IService<RequestHeader> service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [FunctionName("GetAllRequestHeaders")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "getallrequestheaders")] HttpRequest req,
            ILogger log)
        {
            try
            {
                var requestHeaders = await _service.GetAllItems();

                if (requestHeaders == null)
                {
                    return new BadRequestResult();
                }

                return new OkObjectResult(requestHeaders);
            }
            catch (Exception ex)
            {
                log.LogError($"Problem getting list of Request Headers: {ex.Message}");
                return new BadRequestResult();
            }
        }
    }
}