using Azure.Storage.Queues;
using BabaFunkeEmailManager.Data.Models;
using BabaFunkeEmailManager.Service.Services.Interfaces;
using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BabaFunkeEmailManager.Service.Services.Implementations
{
    public class QueueService : IQueueService<RequestDetail>
    {
        private readonly QueueClient _queueClient;

        public QueueService(QueueClient queueClient)
        {
            _queueClient = queueClient;
        }

        public async Task AddItemToQueue(RequestDetail request)
        {
            _queueClient.Create();
            var plainTextBytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(request));
            var msg = Convert.ToBase64String(plainTextBytes);
            await _queueClient.SendMessageAsync(msg);
        }
    }
}