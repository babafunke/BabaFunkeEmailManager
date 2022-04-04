using Azure.Storage.Queues;
using BabaFunkeEmailManager.Functions.Extensions;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

namespace BabaFunkeEmailManager.Functions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.AddServices();

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            builder.Services.AddSingleton(x =>
            {
                CloudStorageAccount cloudStorage = CloudStorageAccount.Parse(config.GetValue<string>("ConnectionStrings:AzureStorageAccount"));
                CloudTableClient cloudTableClient = cloudStorage.CreateCloudTableClient();
                return cloudTableClient;
            });

            builder.Services.AddSingleton(x =>
            {
                QueueClient queueClient = new QueueClient(config.GetValue<string>("ConnectionStrings:AzureStorageAccount"), "requests");
                return queueClient;
            });

            builder.Services
                .AddFluentEmail(config.GetValue<string>("EmailSettings:SenderEmail"), "SENDER'S_NAME")
                .AddSendGridSender(config.GetValue<string>("EmailSettings:SendGridKey"));

        }
    }
}