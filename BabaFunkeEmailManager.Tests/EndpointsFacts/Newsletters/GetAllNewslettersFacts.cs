using BabaFunkeEmailManager.Data.Models;
using BabaFunkeEmailManager.Functions.Endpoints.Newsletters;
using BabaFunkeEmailManager.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BabaFunkeEmailManager.Tests.EndpointsFacts.Newsletters
{
    public class GetAllNewslettersFacts
    {
        private readonly GetAllNewsletters _sut;
        private readonly Mock<IService<Newsletter>> _service;
        private readonly Mock<HttpRequest> _httpRequest;
        private readonly Mock<ILogger> _logger;
        private readonly IEnumerable<Newsletter> _Newsletters;

        public GetAllNewslettersFacts()
        {
            _service = new Mock<IService<Newsletter>>();
            _httpRequest = new Mock<HttpRequest>();
            _logger = new Mock<ILogger>();
            _sut = new GetAllNewsletters(_service.Object);
            _Newsletters = new List<Newsletter>{
                new Newsletter(),
                new Newsletter()
            };
        }

        [Fact]
        public async Task GetAllNewsletters_ShouldCallServiceOnce()
        {
            _service.Setup(s => s.GetAllItems())
                .ReturnsAsync(_Newsletters);

            await _sut.Run(_httpRequest.Object, _logger.Object);

            _service.Verify(s => s.GetAllItems(), Times.Once);
        }

        [Fact]
        public async Task GetAllNewsletters_ShouldReturnAllNewsletters_IfSuccessful()
        {
            const int Total = 2;

            _service.Setup(s => s.GetAllItems())
                .ReturnsAsync(_Newsletters);

            var result = (OkObjectResult)await _sut.Run(_httpRequest.Object, _logger.Object);

            var model = result.Value as IEnumerable<Newsletter>;

            Assert.Equal(Total, model.Count());
        }

        [Fact]
        public async Task GetAllNewsletters_ShouldReturnOkObjectResult_IfNotNull()
        {
            _service.Setup(s => s.GetAllItems())
                .ReturnsAsync(_Newsletters);

            var result = await _sut.Run(_httpRequest.Object, _logger.Object);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetAllNewsletters_ShouldReturnBadRequestResult_IfNull()
        {
            _service.Setup(s => s.GetAllItems())
                .ReturnsAsync(() => null);

            var result = await _sut.Run(_httpRequest.Object, _logger.Object);

            Assert.IsType<BadRequestResult>(result);
        }
    }
}