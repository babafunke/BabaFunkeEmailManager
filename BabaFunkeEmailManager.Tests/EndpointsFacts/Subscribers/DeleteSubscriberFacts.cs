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
    public class DeleteSubscriberFacts
    {
        private readonly DeleteSubscriber _sut;
        private readonly Mock<ISubscriberService> _service;
        private readonly Mock<HttpRequest> _httpRequest;
        private readonly Mock<ILogger> _logger;

        public DeleteSubscriberFacts()
        {
            _service = new Mock<ISubscriberService>();
            _httpRequest = new Mock<HttpRequest>();
            _logger = new Mock<ILogger>();
            _sut = new DeleteSubscriber(_service.Object);
        }

        [Fact]
        public async Task DeleteSubscriber_ShouldCallServiceOnce()
        {
            _service.Setup(s => s.DeleteItem(It.IsAny<string>()))
                .ReturnsAsync(new ServiceResponse<Subscriber>());

            await _sut.Run(_httpRequest.Object, "abc@example.com", _logger.Object);

            _service.Verify(s => s.DeleteItem(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task DeleteSubscriber_ShouldReturnOkObjectResult_IfSuccessful()
        {
            _service.Setup(s => s.DeleteItem(It.IsAny<string>()))
                .ReturnsAsync(new ServiceResponse<Subscriber>() { IsSuccess = true });

            var result = await _sut.Run(_httpRequest.Object, "abc@example.com", _logger.Object);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task DeleteSubscriber_ShouldBadRequestObjectResult_IfFailed()
        {
            _service.Setup(s => s.DeleteItem(It.IsAny<string>()))
                .ReturnsAsync(new ServiceResponse<Subscriber>() { IsSuccess = false });

            var result = await _sut.Run(_httpRequest.Object, "abc@example.com", _logger.Object);

            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}