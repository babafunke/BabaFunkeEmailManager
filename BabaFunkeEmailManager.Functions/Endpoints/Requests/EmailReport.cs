using BabaFunkeEmailManager.Data.Entities;
using BabaFunkeEmailManager.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace BabaFunkeEmailManager.Functions.Endpoints.Requests
{
    public static class EmailReport
    {
        [FunctionName("EmailReport")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "getallreports")] HttpRequest req,
            [Table("EmailReport")] CloudTable cloudTable,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var query = new TableQuery<EmailResponseEntity>();
            var entities = await cloudTable.ExecuteQuerySegmentedAsync(query,null);

            var reports = entities.Select(e => new EmailResponse
            {
                Id = e.RowKey,
                RequestHeaderId = e.PartitionKey,
                Status = e.Status,
                Email = e.Email,
                ErrorMessages = e.ErrorMessages,
                DateCreated = e.Timestamp,
            }).ToList();

            return new OkObjectResult(reports);
        }
    }
}