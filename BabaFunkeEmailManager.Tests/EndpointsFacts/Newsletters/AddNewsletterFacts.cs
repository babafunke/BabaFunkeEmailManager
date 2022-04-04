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
    public class AddNewsletterFacts
    {
        private readonly AddNewsletter _sut;
        private readonly Mock<IService<Newsletter>> _service;
        private readonly Mock<HttpRequest> _httpRequest;
        private readonly Mock<ILogger> _logger;
        private readonly Newsletter _Newsletter;

        public AddNewsletterFacts()
        {
            _service = new Mock<IService<Newsletter>>();
            _httpRequest = new Mock<HttpRequest>();
            _logger = new Mock<ILogger>();
            _sut = new AddNewsletter(_service.Object);
            _Newsletter = new Newsletter();

            string NewsletterJson = JsonSerializer.Serialize(_Newsletter);

            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(NewsletterJson));

            _httpRequest.Setup(h => h.Body).Returns(stream);
        }

        [Fact]
        public async Task AddNewsletter_ShouldCallServiceOnce()
        {
            _service.Setup(s => s.AddItem(It.IsAny<Newsletter>()))
                .ReturnsAsync(new ServiceResponse<Newsletter>());

            await _sut.Run(_httpRequest.Object, _logger.Object);

            _service.Verify(s => s.AddItem(It.IsAny<Newsletter>()), Times.Once);
        }

        [Fact]
        public async Task AddNewsletter_ShouldReturnOkObjectResult_IfSuccessful()
        {
            _service.Setup(s => s.AddItem(It.IsAny<Newsletter>()))
                .ReturnsAsync(new ServiceResponse<Newsletter>() { IsSuccess = true });

            var result = await _sut.Run(_httpRequest.Object, _logger.Object);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task AddNewsletter_ShouldReturnBadRequestObjectResult_IfFailed()
        {
            _service.Setup(s => s.AddItem(It.IsAny<Newsletter>()))
                .ReturnsAsync(new ServiceResponse<Newsletter>() { IsSuccess = false });

            var result = await _sut.Run(_httpRequest.Object, _logger.Object);

            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}