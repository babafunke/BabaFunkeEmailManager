using BabaFunkeEmailManager.Data.Models;
using BabaFunkeEmailManager.Service.Services.Implementations;
using FluentEmail.Core;
using FluentEmail.Core.Models;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace BabaFunkeEmailManager.Tests.ServicesFacts
{
    public class EmailServiceFacts
    {
        private readonly EmailService _sut;
        private readonly Mock<IFluentEmailFactory> _fluentEmail;

        public EmailServiceFacts()
        {
            _fluentEmail = new Mock<IFluentEmailFactory>();
            _sut = new EmailService(_fluentEmail.Object);
        }

        [Fact]
        public async Task SendEmail_ShouldReturnSendResponse_IfValid()
        {
            var requestDetail = new RequestDetail("1","Greetings....", "Hello", "abc@example.com", "Bayo");

            var mockFluentEmail = new Mock<IFluentEmail>();
            mockFluentEmail.Setup(m => m.To(It.IsAny<string>())).Returns(mockFluentEmail.Object);
            mockFluentEmail.Setup(m => m.Subject(It.IsAny<string>())).Returns(mockFluentEmail.Object);
            mockFluentEmail.Setup(m => m.Body(It.IsAny<string>(), true)).Returns(mockFluentEmail.Object);
            mockFluentEmail.Setup(m => m.SendAsync(null)).ReturnsAsync(new SendResponse());

            _fluentEmail.Setup(f => f.Create()).Returns(mockFluentEmail.Object);

            var result = await _sut.SendEmail(requestDetail);

            Assert.IsType<SendResponse>(result);
        }

    }
}