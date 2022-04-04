using BabaFunkeEmailManager.Service.Services.Implementations;
using BabaFunkeEmailManager.Service.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace BabaFunkeEmailManager.Webjob
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var builder = new HostBuilder();

            builder.ConfigureWebJobs(config =>
            {
                config.AddAzureStorageCoreServices(); //allow for use of diagnostics etc
                config.AddCosmosDB();
                config.AddAzureStorage();

                config.Services.AddSingleton<IEmailService, EmailService>();

                config.Services
                .AddFluentEmail("SENDER'S_EMAIL_ADDRESS", "SENDER'S_NAME") //Your email address and name
                .AddSendGridSender("YOUR_SENDGRID_API_KEY");//Visit https://sendgrid.com/ to set this up
            });

            builder.ConfigureLogging(config =>
            {
                config.AddConsole();
            });

            var host = builder.Build();

            using (host)
            {
                await host.RunAsync();
            }
        }
    }
}