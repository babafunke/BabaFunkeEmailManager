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
    public class AddSubscriberFacts
    {
        private readonly AddSubscriber _sut;
        private readonly Mock<ISubscriberService> _service;
        private readonly Mock<HttpRequest> _httpRequest;
        private readonly Mock<ILogger> _logger;
        private readonly Subscriber _subscriber;

        public AddSubscriberFacts()
        {
            _service = new Mock<ISubscriberService>();
            _httpRequest = new Mock<HttpRequest>();
            _logger = new Mock<ILogger>();
            _sut = new AddSubscriber(_service.Object);
            _subscriber = new Subscriber();

            string subscriberJson = JsonSerializer.Serialize(_subscriber);

            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(subscriberJson));

            _httpRequest.Setup(h => h.Body).Returns(stream);
        }

        [Fact]
        public async Task AddSubscriber_ShouldCallServiceOnce()
        {
            _service.Setup(s => s.AddItem(It.IsAny<Subscriber>()))
                .ReturnsAsync(new ServiceResponse<Subscriber>());

            await _sut.Run(_httpRequest.Object, _logger.Object);

            _service.Verify(s => s.AddItem(It.IsAny<Subscriber>()), Times.Once);
        }

        [Fact]
        public async Task AddSubscriber_ShouldReturnOkObjectResult_IfSuccessful()
        {
            _service.Setup(s => s.AddItem(It.IsAny<Subscriber>()))
                .ReturnsAsync(new ServiceResponse<Subscriber>() { IsSuccess = true });

            var result = await _sut.Run(_httpRequest.Object, _logger.Object);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task AddSubscriber_ShouldReturnBadRequestObjectResult_IfFailed()
        {
            _service.Setup(s => s.AddItem(It.IsAny<Subscriber>()))
                .ReturnsAsync(new ServiceResponse<Subscriber>() { IsSuccess = false });

            var result = await _sut.Run(_httpRequest.Object, _logger.Object);

            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}