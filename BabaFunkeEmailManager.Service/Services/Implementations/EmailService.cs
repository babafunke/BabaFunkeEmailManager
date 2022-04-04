using BabaFunkeEmailManager.Data.Models;
using BabaFunkeEmailManager.Service.Services.Interfaces;
using BabaFunkeEmailManager.Service.Utilities;
using FluentEmail.Core;
using FluentEmail.Core.Models;
using System;
using System.Text;
using System.Threading.Tasks;

namespace BabaFunkeEmailManager.Service.Services.Implementations
{
    public class EmailService : IEmailService
    {
        private readonly IFluentEmailFactory _fluentEmailFactory;

        public EmailService(IFluentEmailFactory fluentEmailFactory)
        {
            _fluentEmailFactory = fluentEmailFactory;
        }

        public async Task<SendResponse> SendEmail(RequestDetail requestDetail)
        {
            try
            {
                var mail = _fluentEmailFactory.Create()
                        .To($"{requestDetail.SubscriberEmail}")
                        .Subject(requestDetail.EmailSubject)
                        .Body(BuildEmail(requestDetail.SubscriberEmail, requestDetail.EmailBody, requestDetail.SubscriberFirstname), true);

                return await mail.SendAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        private string BuildEmail(string email, string body, string name)
        {
            var unsubscribeLink = $"{ServiceUtil.ClientUrl}Subscriber/Unsubscribe?email=" + email;

            var builder = new StringBuilder();

            var subscriberFirstname = string.IsNullOrEmpty(name) ? "Friend" : name;

            var header = $"Hello {subscriberFirstname},<p></p>";
            var footer = $"<p></p><p></p>If you no longer wish to hear from me, click <a href={unsubscribeLink}>Unsubscribe</a>." +
                $"<p>Best regards,</p><p>Adebayo Adegbembo</p>" +
                $"<p><a href='https://www.linkedin.com/in/adebayoadegbembo/'>LinkedIn</a> | <a href='https://daddycreates.com/'>Blog</a></p>";

            builder.AppendLine(header);
            builder.AppendLine(body);
            builder.AppendLine(footer);

            return builder.ToString();
        }
    }
}