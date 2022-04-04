using BabaFunkeEmailManager.Data.Entities;
using BabaFunkeEmailManager.Data.Models;
using BabaFunkeEmailManager.Service.Services.Interfaces;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.WebJobs;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace BabaFunkeEmailManager.Webjob
{
    public class ProcessQueue
    {
        private readonly IEmailService _emailService;

        public ProcessQueue(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public async Task Run([QueueTrigger("requests")] string myQueueItem,
            [Table("EmailReport")] CloudTable _cloudTable,
            TextWriter log)
        {
            var requestDetail = JsonSerializer.Deserialize<RequestDetail>(myQueueItem);

            log.WriteLine($"Processing request for {requestDetail.SubscriberEmail}");

            var response = await _emailService.SendEmail(requestDetail);

            var entity = new EmailResponseEntity(requestDetail.RequestHeaderId)
            {
                Status = response.Successful,
                Email = requestDetail.SubscriberEmail,
                ErrorMessages = response.Successful ? null : response.ErrorMessages[1]
            };

            var operation = TableOperation.Insert(entity);

            await _cloudTable.ExecuteAsync(operation);

            log.WriteLine($"Completed request for {requestDetail.SubscriberEmail}");
        }
    }
}