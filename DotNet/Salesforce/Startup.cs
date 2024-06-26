using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Salesforce.Helpers;
using Salesforce.Interfaces;
using Salesforce.Services;

namespace Salesforce
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddControllers();
            services.AddHttpClient();
            services.AddScoped<ICampaignService, CampaignService>();
            services.AddScoped<IContactService, ContactService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ILeadService, LeadService>();
            services.AddHttpClient<ApiHelper>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(
                options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
            );
            app.UseRouting();

            

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
