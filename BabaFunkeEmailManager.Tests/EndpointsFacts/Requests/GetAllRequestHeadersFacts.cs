using BabaFunkeEmailManager.Data.Models;
using BabaFunkeEmailManager.Functions.Endpoints.Requests;
using BabaFunkeEmailManager.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BabaFunkeEmailManager.Tests.EndpointsFacts.Requests
{
    public class GetAllRequestHeadersFacts
    {
        private readonly GetAllRequestHeaders _sut;
        private readonly Mock<IService<RequestHeader>> _service;
        private readonly Mock<HttpRequest> _httpRequest;
        private readonly Mock<ILogger> _logger;
        private readonly IEnumerable<RequestHeader> _RequestHeaders;

        public GetAllRequestHeadersFacts()
        {
            _service = new Mock<IService<RequestHeader>>();
            _httpRequest = new Mock<HttpRequest>();
            _logger = new Mock<ILogger>();
            _sut = new GetAllRequestHeaders(_service.Object);
            _RequestHeaders = new List<RequestHeader>{
                new RequestHeader(),
                new RequestHeader()
            };
        }

        [Fact]
        public async Task GetAllRequestHeaders_ShouldCallServiceOnce()
        {
            _service.Setup(s => s.GetAllItems())
                .ReturnsAsync(_RequestHeaders);

            await _sut.Run(_httpRequest.Object, _logger.Object);

            _service.Verify(s => s.GetAllItems(), Times.Once);
        }

        [Fact]
        public async Task GetAllRequestHeaders_ShouldReturnAllRequestHeaders_IfSuccessful()
        {
            const int Total = 2;

            _service.Setup(s => s.GetAllItems())
                .ReturnsAsync(_RequestHeaders);

            var result = (OkObjectResult)await _sut.Run(_httpRequest.Object, _logger.Object);

            var model = result.Value as IEnumerable<RequestHeader>;

            Assert.Equal(Total, model.Count());
        }

        [Fact]
        public async Task GetAllRequestHeaders_ShouldReturnOkObjectResult_IfNotNull()
        {
            _service.Setup(s => s.GetAllItems())
                .ReturnsAsync(_RequestHeaders);

            var result = await _sut.Run(_httpRequest.Object, _logger.Object);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetAllRequestHeaders_ShouldReturnBadRequestResult_IfNull()
        {
            _service.Setup(s => s.GetAllItems())
                .ReturnsAsync(() => null);

            var result = await _sut.Run(_httpRequest.Object, _logger.Object);

            Assert.IsType<BadRequestResult>(result);
        }
    }
}