using BabaFunkeEmailManager.Client.Data;
using BabaFunkeEmailManager.Client.IRepositories;
using BabaFunkeEmailManager.Client.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace BabaFunkeEmailManager.Client
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddHttpClient("httpclient", options =>
            {
                options.BaseAddress = new Uri(Configuration.GetValue<string>("ApiUrl"));
                /*
                 * Uncomment below if your Azure Functions' AuthorizationLevel are not set to Anonymous when published live in Azure. 
                 * For example, mine have AuthorizationLevel.Function so they need the ApiKey to access
                 * To get the ApiKey, go to the hosted AzureFunctions project in Azure, select 'App Keys' under the 'Functions' category 
                 * on the lefthand menu and simply create one or use the default under 'Host keys (all functions)'
                 */
                //options.DefaultRequestHeaders.Add("x-functions-key", Configuration.GetValue<string>("ApiKey"));
            });

            services.AddTransient<ISubscriber, SubscriberService>();
            services.AddTransient<INewsletter, NewsletterService>();
            services.AddTransient<IRequest, RequestService>();
            services.AddTransient<IReport, ReportService>();

            services.AddDbContext<BabaFunkeEmailManagerClientContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("BabaFunkeEmailManagerClientContextConnection")));

            services.AddDefaultIdentity<IdentityUser>(options =>
            options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<BabaFunkeEmailManagerClientContext>();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}