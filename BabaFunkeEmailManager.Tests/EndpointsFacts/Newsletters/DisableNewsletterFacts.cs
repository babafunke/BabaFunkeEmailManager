using BabaFunkeEmailManager.Data.Models;
using BabaFunkeEmailManager.Functions.Endpoints.Newsletters;
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

namespace BabaFunkeEmailManager.Tests.EndpointsFacts.Newsletters
{
    public class DisableNewsletterFacts
    {
        private readonly DisableNewsletter _sut;
        private readonly Mock<IService<Newsletter>> _service;
        private readonly Mock<HttpRequest> _httpRequest;
        private readonly Mock<ILogger> _logger;
        private readonly Newsletter _Newsletter;

        public DisableNewsletterFacts()
        {
            _service = new Mock<IService<Newsletter>>();
            _httpRequest = new Mock<HttpRequest>();
            _logger = new Mock<ILogger>();
            _sut = new DisableNewsletter(_service.Object);
            _Newsletter = new Newsletter();

            string NewsletterJson = JsonSerializer.Serialize(_Newsletter);

            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(NewsletterJson));

            _httpRequest.Setup(h => h.Body).Returns(stream);
        }

        [Fact]
        public async Task DisableNewsletter_ShouldCallServiceOnce()
        {
            _service.Setup(s => s.DisableItem(It.IsAny<string>()))
                .ReturnsAsync(new ServiceResponse<Newsletter>());

            await _sut.Run(_httpRequest.Object, _logger.Object);

            _service.Verify(s => s.DisableItem(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task DisableNewsletter_ShouldReturnOkObjectResult_IfSuccessful()
        {
            _service.Setup(s => s.DisableItem(It.IsAny<string>()))
                .ReturnsAsync(new ServiceResponse<Newsletter>() { IsSuccess = true });

            var result = await _sut.Run(_httpRequest.Object, _logger.Object);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task DisableNewsletter_ShouldReturnBadRequestObjectResult_IfFailed()
        {
            _service.Setup(s => s.DisableItem(It.IsAny<string>()))
                .ReturnsAsync(new ServiceResponse<Newsletter>() { IsSuccess = false });

            var result = await _sut.Run(_httpRequest.Object, _logger.Object);

            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}