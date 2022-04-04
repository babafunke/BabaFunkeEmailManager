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
    public class UpdateNewsletterFacts
    {
        private readonly UpdateNewsletter _sut;
        private readonly Mock<IService<Newsletter>> _service;
        private readonly Mock<HttpRequest> _httpRequest;
        private readonly Mock<ILogger> _logger;
        private readonly Newsletter _Newsletter;
        private readonly string newsletterId = "Issue_1";

        public UpdateNewsletterFacts()
        {
            _service = new Mock<IService<Newsletter>>();
            _httpRequest = new Mock<HttpRequest>();
            _logger = new Mock<ILogger>();
            _sut = new UpdateNewsletter(_service.Object);
            _Newsletter = new Newsletter() { NewsletterId = newsletterId };

            string NewsletterJson = JsonSerializer.Serialize(_Newsletter);

            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(NewsletterJson));

            _httpRequest.Setup(h => h.Body).Returns(stream);
        }

        [Fact]
        public async Task UpdateNewsletter_ShouldCallServiceOnce()
        {
            _service.Setup(s => s.UpdateItem(It.IsAny<string>(), It.IsAny<Newsletter>()))
                .ReturnsAsync(new ServiceResponse<Newsletter>());

            await _sut.Run(_httpRequest.Object, newsletterId, _logger.Object);

            _service.Verify(s => s.UpdateItem(It.IsAny<string>(), It.IsAny<Newsletter>()), Times.Once);
        }

        [Fact]
        public async Task UpdateNewsletter_ShouldReturnOkObjectResult_IfSuccessful()
        {
            _service.Setup(s => s.UpdateItem(It.IsAny<string>(), It.IsAny<Newsletter>()))
                .ReturnsAsync(new ServiceResponse<Newsletter>() { IsSuccess = true });

            var result = await _sut.Run(_httpRequest.Object, newsletterId, _logger.Object);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task UpdateNewsletter_ShouldReturnBadRequestObjectResult_IfFailed()
        {
            _service.Setup(s => s.UpdateItem(It.IsAny<string>(), It.IsAny<Newsletter>()))
                .ReturnsAsync(new ServiceResponse<Newsletter>() { IsSuccess = false });

            var result = await _sut.Run(_httpRequest.Object, newsletterId, _logger.Object);

            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}