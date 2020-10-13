using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using PingIsService.Shared;

namespace PingIsService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private static IConfiguration Configuration { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSingleton<IApplicationInsightsPublisher, ApplicationInsightsPublisher>();
            services.AddSingleton(Configuration);
            services.Configure<ApplicationSettings>(Configuration.GetSection(nameof(ApplicationSettings)));

            AddSwagger(services);
        }

        private ApplicationSettings ApplicationSettings => Configuration.GetSection(nameof(ApplicationSettings))
            .Get<ApplicationSettings>();

        private void AddSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(
                c => c.SwaggerDoc(
                    ApplicationSettings.ApiVersion,
                    new OpenApiInfo
                    {
                        Title = ApplicationSettings.Name,
                        Version = ApplicationSettings.ApiVersion
                    }
                )
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseSwagger();

                app.UseSwaggerUI(
                    c => c.SwaggerEndpoint(
                        $"/swagger/{ApplicationSettings.ApiVersion}/swagger.json",
                        ApplicationSettings.Name
                    )
                );
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(
                endpoints =>
                {
                    endpoints.MapControllers();
                }
            );
        }
    }
}
