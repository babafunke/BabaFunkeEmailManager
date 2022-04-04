using Azure.Storage.Queues;
using BabaFunkeEmailManager.Data.Models;
using BabaFunkeEmailManager.Service.Services.Implementations;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace BabaFunkeEmailManager.Tests.ServicesFacts
{
    public class QueueServiceFacts
    {
        private readonly QueueService _sut;
        private readonly Mock<QueueClient> _client;

        public QueueServiceFacts()
        {
            _client = new Mock<QueueClient>();
            _sut = new QueueService(_client.Object);
        }

        [Fact]
        public async Task AddItemToQueue_ShouldCallSendMessageAsyncOnce()
        {
            var requestDetail = new RequestDetail("1","Greetings....", "Hello", "abc@example.com", "Bayo");

            await _sut.AddItemToQueue(requestDetail);

            _client.Verify(c => c.SendMessageAsync(It.IsAny<string>()), Times.Once);
        }
    }
}