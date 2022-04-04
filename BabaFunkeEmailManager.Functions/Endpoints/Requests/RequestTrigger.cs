using BabaFunkeEmailManager.Data.Entities;
using BabaFunkeEmailManager.Data.Models;
using BabaFunkeEmailManager.Service.Services.Interfaces;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.WebJobs;
using System.Text.Json;
using System.Threading.Tasks;

namespace BabaFunkeEmailManager.Functions
{
    public class RequestTrigger
    {
        private readonly IEmailService _emailService;

        public RequestTrigger(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [FunctionName("RequestTrigger")]
        public async Task Run([QueueTrigger("requests")] string myQueueItem,
            [Table("EmailReport")] CloudTable table)
        {
            var requestDetail = JsonSerializer.Deserialize<RequestDetail>(myQueueItem);

            var response = await _emailService.SendEmail(requestDetail);

            var entity = new EmailResponseEntity(requestDetail.RequestHeaderId)
            {
                Status = response.Successful,
                Email = requestDetail.SubscriberEmail,
                ErrorMessages = response.Successful ? null : response.ErrorMessages[1]
            };

            var operation = TableOperation.Insert(entity);

            await table.ExecuteAsync(operation);
        }
    }
}