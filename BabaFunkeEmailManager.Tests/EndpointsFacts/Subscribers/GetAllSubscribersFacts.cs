using BabaFunkeEmailManager.Data.Models;
using BabaFunkeEmailManager.Functions.Endpoints.Subscribers;
using BabaFunkeEmailManager.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BabaFunkeEmailManager.Tests.EndpointsFacts.Subscribers
{
    public class GetAllSubscribersFacts
    {
        private readonly GetAllSubscribers _sut;
        private readonly Mock<ISubscriberService> _service;
        private readonly Mock<HttpRequest> _httpRequest;
        private readonly Mock<ILogger> _logger;
        private readonly IEnumerable<Subscriber> _subscribers;

        public GetAllSubscribersFacts()
        {
            _service = new Mock<ISubscriberService>();
            _httpRequest = new Mock<HttpRequest>();
            _logger = new Mock<ILogger>();
            _sut = new GetAllSubscribers(_service.Object);
            _subscribers = new List<Subscriber>{
                new Subscriber(),
                new Subscriber()
            };
        }

        [Fact]
        public async Task GetAllSubscribers_ShouldCallServiceOnce()
        {
            _service.Setup(s => s.GetAllItems())
                .ReturnsAsync(_subscribers);

            await _sut.Run(_httpRequest.Object, _logger.Object);

            _service.Verify(s => s.GetAllItems(), Times.Once);
        }

        [Fact]
        public async Task GetAllSubscribers_ShouldReturnAllSubscribers_IfSuccessful()
        {
            const int Total = 2;

            _service.Setup(s => s.GetAllItems())
                .ReturnsAsync(_subscribers);

            var result = (OkObjectResult)await _sut.Run(_httpRequest.Object, _logger.Object);

            var model = result.Value as IEnumerable<Subscriber>;

            Assert.Equal(Total, model.Count());
        }

        [Fact]
        public async Task GetAllSubscribers_ShouldReturnOkObjectResult_IfNotNull()
        {
            _service.Setup(s => s.GetAllItems())
                .ReturnsAsync(_subscribers);

            var result = await _sut.Run(_httpRequest.Object, _logger.Object);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetAllSubscribers_ShouldReturnBadRequestResult_IfNull()
        {
            _service.Setup(s => s.GetAllItems())
                .ReturnsAsync(() => null);

            var result = await _sut.Run(_httpRequest.Object, _logger.Object);

            Assert.IsType<BadRequestResult>(result);
        }
    }
}