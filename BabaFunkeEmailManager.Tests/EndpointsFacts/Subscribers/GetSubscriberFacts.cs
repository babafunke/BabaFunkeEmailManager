using BabaFunkeEmailManager.Data.Models;
using BabaFunkeEmailManager.Functions.Endpoints.Subscribers;
using BabaFunkeEmailManager.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace BabaFunkeEmailManager.Tests.EndpointsFacts.Subscribers
{
    public class GetSubscriberFacts
    {
        private readonly GetSubscriber _sut;
        private readonly Mock<ISubscriberService> _service;
        private readonly Mock<HttpRequest> _httpRequest;
        private readonly Mock<ILogger> _logger;

        public GetSubscriberFacts()
        {
            _service = new Mock<ISubscriberService>();
            _httpRequest = new Mock<HttpRequest>();
            _logger = new Mock<ILogger>();
            _sut = new GetSubscriber(_service.Object);
        }

        [Fact]
        public async Task GetSubscriber_ShouldCallServiceOnce()
        {
            _service.Setup(s => s.GetItem(It.IsAny<string>()))
                .ReturnsAsync(new ServiceResponse<Subscriber>());

            await _sut.Run(_httpRequest.Object, "abc@example.com", _logger.Object);

            _service.Verify(s => s.GetItem(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task GetSubscriber_ShouldReturnOkObjectResult_IfSuccessful()
        {
            _service.Setup(s => s.GetItem(It.IsAny<string>()))
                .ReturnsAsync(new ServiceResponse<Subscriber>() { IsSuccess = true });

            var result = await _sut.Run(_httpRequest.Object, "abc@example.com", _logger.Object);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetSubscriber_ShouldBadRequestObjectResult_IfFailed()
        {
            _service.Setup(s => s.GetItem(It.IsAny<string>()))
                .ReturnsAsync(new ServiceResponse<Subscriber>() { IsSuccess = false });

            var result = await _sut.Run(_httpRequest.Object, "abc@example.com", _logger.Object);

            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}