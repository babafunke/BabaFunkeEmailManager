using BabaFunkeEmailManager.Data.Models;
using BabaFunkeEmailManager.Functions.Endpoints.Subscribers;
using BabaFunkeEmailManager.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace BabaFunkeEmailManager.Tests.EndpointsFacts.Subscribers
{
    public class UpdateSubscriberFacts
    {
        private readonly UpdateSubscriber _sut;
        private readonly Mock<ISubscriberService> _service;
        private readonly Mock<HttpRequest> _httpRequest;
        private readonly Mock<ILogger> _logger;
        private readonly Subscriber _subscriber;
        private readonly string email = "abc@example.com";

        public UpdateSubscriberFacts()
        {
            _service = new Mock<ISubscriberService>();
            _httpRequest = new Mock<HttpRequest>();
            _logger = new Mock<ILogger>();
            _sut = new UpdateSubscriber(_service.Object);
            _subscriber = new Subscriber() { Email = email };

            string subscriberJson = JsonSerializer.Serialize(_subscriber);

            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(subscriberJson));

            _httpRequest.Setup(h => h.Body).Returns(stream);
        }

        [Fact]
        public async Task UpdateSubscriber_ShouldCallServiceOnce()
        {
            _service.Setup(s => s.UpdateItem(It.IsAny<string>(), It.IsAny<Subscriber>()))
                .ReturnsAsync(new ServiceResponse<Subscriber>());

            await _sut.Run(_httpRequest.Object, email, _logger.Object);

            _service.Verify(s => s.UpdateItem(It.IsAny<string>(), It.IsAny<Subscriber>()), Times.Once);
        }

        [Fact]
        public async Task UpdateSubscriber_ShouldReturnOkObjectResult_IfSuccessful()
        {
            _service.Setup(s => s.UpdateItem(It.IsAny<string>(), It.IsAny<Subscriber>()))
                .ReturnsAsync(new ServiceResponse<Subscriber>() { IsSuccess = true });

            var result = await _sut.Run(_httpRequest.Object, email, _logger.Object);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task UpdateSubscriber_ShouldReturnBadRequestObjectResult_IfFailed()
        {
            _service.Setup(s => s.UpdateItem(It.IsAny<string>(), It.IsAny<Subscriber>()))
                .ReturnsAsync(new ServiceResponse<Subscriber>() { IsSuccess = false });

            var result = await _sut.Run(_httpRequest.Object, email, _logger.Object);

            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}