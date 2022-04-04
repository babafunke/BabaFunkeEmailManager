using BabaFunkeEmailManager.Data.Entities;
using BabaFunkeEmailManager.Data.Models;
using BabaFunkeEmailManager.Service.Mappers;
using BabaFunkeEmailManager.Service.Repositories.Implementations;
using BabaFunkeEmailManager.Service.Repositories.Interfaces;
using BabaFunkeEmailManager.Service.Services.Implementations;
using BabaFunkeEmailManager.Service.Services.Interfaces;
using BabaFunkeEmailManager.Services.Interfaces;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace BabaFunkeEmailManager.Functions.Extensions
{
    public static class ServicesExtension
    {
        public static void AddServices(this IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton<ISubscriberRepository, SubscriberRepository>();
            builder.Services.AddSingleton<ISubscriberService, SubscriberService>();
            builder.Services.AddSingleton<IMapper<Subscriber, SubscriberEntity>, SubscriberMapper>();

            builder.Services.AddSingleton<IRepository<NewsletterEntity>, NewsletterRepository>();
            builder.Services.AddSingleton<IService<Newsletter>, NewsletterService>();
            builder.Services.AddSingleton<IMapper<Newsletter, NewsletterEntity>, NewsletterMapper>();

            builder.Services.AddSingleton<IRepository<RequestHeaderEntity>, RequestHeaderRepository>();
            builder.Services.AddSingleton<IService<RequestHeader>, RequestHeaderService>();
            builder.Services.AddSingleton<IMapper<RequestHeader, RequestHeaderEntity>, RequestHeaderMapper>();

            builder.Services.AddSingleton<IQueueService<RequestDetail>, QueueService>();
            builder.Services.AddSingleton<IEmailService, EmailService>();
        }
    }
}