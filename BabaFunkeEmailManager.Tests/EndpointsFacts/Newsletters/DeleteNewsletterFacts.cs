using BabaFunkeEmailManager.Data.Models;
using BabaFunkeEmailManager.Functions.Endpoints.Newsletters;
using BabaFunkeEmailManager.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace BabaFunkeEmailManager.Tests.EndpointsFacts.Newsletters
{
    public class DeleteNewsletterFacts
    {
        private readonly DeleteNewsletter _sut;
        private readonly Mock<IService<Newsletter>> _service;
        private readonly Mock<HttpRequest> _httpRequest;
        private readonly Mock<ILogger> _logger;

        public DeleteNewsletterFacts()
        {
            _service = new Mock<IService<Newsletter>>();
            _httpRequest = new Mock<HttpRequest>();
            _logger = new Mock<ILogger>();
            _sut = new DeleteNewsletter(_service.Object);
        }

        [Fact]
        public async Task DeleteNewsletter_ShouldCallServiceOnce()
        {
            _service.Setup(s => s.DeleteItem(It.IsAny<string>()))
                .ReturnsAsync(new ServiceResponse<Newsletter>());

            await _sut.Run(_httpRequest.Object, "Issue_1", _logger.Object);

            _service.Verify(s => s.DeleteItem(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task DeleteNewsletter_ShouldReturnOkObjectResult_IfSuccessful()
        {
            _service.Setup(s => s.DeleteItem(It.IsAny<string>()))
                .ReturnsAsync(new ServiceResponse<Newsletter>() { IsSuccess = true });

            var result = await _sut.Run(_httpRequest.Object, "Issue_1", _logger.Object);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task DeleteNewsletter_ShouldBadRequestObjectResult_IfFailed()
        {
            _service.Setup(s => s.DeleteItem(It.IsAny<string>()))
                .ReturnsAsync(new ServiceResponse<Newsletter>() { IsSuccess = false });

            var result = await _sut.Run(_httpRequest.Object, "Issue_1", _logger.Object);

            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}