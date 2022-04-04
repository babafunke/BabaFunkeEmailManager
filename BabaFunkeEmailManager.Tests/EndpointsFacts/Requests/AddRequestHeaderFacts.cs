using BabaFunkeEmailManager.Data.Models;
using BabaFunkeEmailManager.Service.Services.Interfaces;
using BabaFunkeEmailManager.Services.Interfaces;
using BabaFunkeRequestManager.Functions.Endpoints.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace BabaFunkeEmailManager.Tests.EndpointsFacts.Requests
{
    public class AddRequestHeaderFacts
    {
        private readonly AddRequestHeader _sut;
        private readonly Mock<IService<RequestHeader>> _requestHeaderService;
        private readonly Mock<IQueueService<RequestDetail>> _queueService;
        private readonly Mock<ISubscriberService> _subscriberService;
        private readonly Mock<IService<Newsletter>> _newsletterService;
        private readonly Mock<HttpRequest> _httpRequest;
        private readonly Mock<ILogger> _logger;
        private readonly RequestHeader _requestHeader;

        public AddRequestHeaderFacts()
        {
            _requestHeaderService = new Mock<IService<RequestHeader>>();
            _queueService = new Mock<IQueueService<RequestDetail>>();
            _subscriberService = new Mock<ISubscriberService>();
            _newsletterService = new Mock<IService<Newsletter>>();
            _httpRequest = new Mock<HttpRequest>();
            _logger = new Mock<ILogger>();
            _sut = new AddRequestHeader(_requestHeaderService.Object, _queueService.Object, _subscriberService.Object, _newsletterService.Object);

            _requestHeader = new RequestHeader()
            {
                NewsletterId = "Issue_1",
                SubscriberSubCategory = "Demo"
            };

            string subscriberJson = JsonSerializer.Serialize(_requestHeader);

            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(subscriberJson));

            _httpRequest.Setup(h => h.Body).Returns(stream);
        }

        [Fact]
        public async Task AddRequest_ShouldCallServiceOnce()
        {
            _requestHeaderService.Setup(s => s.AddItem(It.IsAny<RequestHeader>()))
                .ReturnsAsync(new ServiceResponse<RequestHeader>());

            await _sut.Run(_httpRequest.Object, _logger.Object);

            _requestHeaderService.Verify(s => s.AddItem(It.IsAny<RequestHeader>()), Times.Once);
        }

        [Fact]
        public async Task AddRequest_ShouldReturnOkObjectResult_IfSuccessful()
        {
            _requestHeaderService.Setup(s => s.AddItem(It.IsAny<RequestHeader>()))
                .ReturnsAsync(new ServiceResponse<RequestHeader>() { IsSuccess = true });

            _subscriberService.Setup(s => s.GetAllItems())
                .ReturnsAsync(new List<Subscriber>
                {
                    new Subscriber(){IsSubscribed = false, Category = "Demo", SubCategory = "Test"},
                    new Subscriber(){IsSubscribed = true, Category = "Demo", SubCategory = "Test"}
                });

            _newsletterService.Setup(n => n.GetItem(It.IsAny<string>()))
                .ReturnsAsync(new ServiceResponse<Newsletter> { IsSuccess = true, Data = new Newsletter() });

            _queueService.Setup(q => q.AddItemToQueue(It.IsAny<RequestDetail>()));

            var result = await _sut.Run(_httpRequest.Object, _logger.Object);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task AddRequest_ShouldReturnBadRequestObjectResult_IfFailed()
        {
            _requestHeaderService.Setup(s => s.AddItem(It.IsAny<RequestHeader>()))
                .ReturnsAsync(new ServiceResponse<RequestHeader>() { IsSuccess = false });

            var result = await _sut.Run(_httpRequest.Object, _logger.Object);

            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}