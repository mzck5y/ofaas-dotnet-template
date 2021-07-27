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
                    webBuilder.ConfigureServices(services =>
                        {
                            services.AddTransient<FunctionHandler>();
                            #if hasSignature
                            services.Configure<SignatureValidationOptions>(op =>
                            {
                                op.SigningKey = OpenFaasUtils.GetSecret("sig-key");
                                op.HeaderName = OpenFaasUtils.GetSecret("sig-header-name");
                                op.Alg = "sig-alg"
                            });
                            #endif
                        })
                        .Configure(app =>
                        {
                            ILoggerFactory loggerFactory = app.ApplicationServices.GetRequiredService<ILoggerFactory>();
                            ILogger logger = loggerFactory.CreateLogger("function-host");
                            IWebHostEnvironment env = app.ApplicationServices.GetRequiredService<IWebHostEnvironment>();

                            FunctionHandler func = app.ApplicationServices.GetRequiredService<FunctionHandler>();

                            logger.LogInformation("Function Host Strarting...");

                            if (env.IsDevelopment())
                            {
                                app.UseDeveloperExceptionPage();
                            }

                            app.UseRouting();

                            #if hasSignature
                            app.UseSignatureValidator();
                            #endif
                            app.UseEndpoints(endpoints =>
                            {
                                endpoints.Map("function-route", async context =>
                                {
                                    await func.RunAsync(context);
                                });
                            });
                        });
                });
    }
}
