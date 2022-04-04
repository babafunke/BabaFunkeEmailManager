using BabaFunkeEmailManager.Data.Models;
using BabaFunkeEmailManager.Functions.Endpoints.Requests;
using BabaFunkeEmailManager.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace BabaFunkeEmailManager.Tests.EndpointsFacts.Requests
{
    public class DeleteRequestHeaderFacts
    {
        private readonly DeleteRequestHeader _sut;
        private readonly Mock<IService<RequestHeader>> _service;
        private readonly Mock<HttpRequest> _httpRequest;
        private readonly Mock<ILogger> _logger;

        public DeleteRequestHeaderFacts()
        {
            _service = new Mock<IService<RequestHeader>>();
            _httpRequest = new Mock<HttpRequest>();
            _logger = new Mock<ILogger>();
            _sut = new DeleteRequestHeader(_service.Object);
        }

        [Fact]
        public async Task DeleteRequestHeader_ShouldCallServiceOnce()
        {
            _service.Setup(s => s.DeleteItem(It.IsAny<string>()))
                .ReturnsAsync(new ServiceResponse<RequestHeader>());

            await _sut.Run(_httpRequest.Object, "1", _logger.Object);

            _service.Verify(s => s.DeleteItem(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task DeleteRequestHeader_ShouldReturnOkObjectResult_IfSuccessful()
        {
            _service.Setup(s => s.DeleteItem(It.IsAny<string>()))
                .ReturnsAsync(new ServiceResponse<RequestHeader>() { IsSuccess = true });

            var result = await _sut.Run(_httpRequest.Object, "1", _logger.Object);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task DeleteRequestHeader_ShouldBadRequestObjectResult_IfFailed()
        {
            _service.Setup(s => s.DeleteItem(It.IsAny<string>()))
                .ReturnsAsync(new ServiceResponse<RequestHeader>() { IsSuccess = false });

            var result = await _sut.Run(_httpRequest.Object, "1", _logger.Object);

            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}