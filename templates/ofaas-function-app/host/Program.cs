using host.Function;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace host
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    // webBuilder.UseStartup<Startup>();
                    webBuilder
                        .ConfigureServices(services =>
                        {
                            services.AddTransient<FunctionHandler>();
                        })
                        .Configure(app =>
                        {
                            ILoggerFactory loggerFactory = app.ApplicationServices.GetRequiredService<ILoggerFactory>();
                            ILogger logger = loggerFactory.CreateLogger("function-host");
                            FunctionHandler func = app.ApplicationServices.GetRequiredService<FunctionHandler>();
                            IWebHostEnvironment env = app.ApplicationServices.GetRequiredService<IWebHostEnvironment>();

                            logger.LogInformation("Function Host Strarting...");

                            if (env.IsDevelopment())
                            {
                                app.UseDeveloperExceptionPage();
                            }

                            app.UseRouting();

                            app.UseEndpoints(endpoints =>
                            {
                                endpoints.MapGet("/", async context =>
                                {
                                    await func.HandlerAsync(context);
                                });
                            });
                        });
                });
    }
}
