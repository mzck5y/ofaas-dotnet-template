using Serverless.Function.Handler;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OniCloud.OpenFaas.Utils;
#if(includeJwt)
using Microsoft.IdentityModel.Tokens;
#endif
#if(hasSignature)
using OniCloud.OpenFaas.Models;
using OniCloud.OpenFaas.Http;
#endif

namespace Serverless.Function
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
                            #if (hasSignature)
                            services.Configure<SignatureValidationOptions>(op =>
                            {
                                op.SigningKey = OpenFaasUtils.GetSecret("sig-key");
                                op.HeaderName = OpenFaasUtils.GetSecret("sig-header-name");
                                op.SigningAlg = sig-alg;
                            });
                            #endif
                            #if (includeJwt)
                            services.AddAuthentication("Bearer")
                                .AddJwtBearer("Bearer", options =>
                                {
                                    options.Authority = OpenFaasUtils.GetSecret("autority-server-secret");;
                                    options.TokenValidationParameters = new TokenValidationParameters
                                    {
                                        ValidateAudience = false
                                    };
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

                            #if (includeJwt)
                            app.UseAuthorization();
                            app.UseAuthentication();
                            #endif
                            #if (hasSignature)
                            app.UseSignatureValidator();
                            #endif
                            
                            app.UseEndpoints(endpoints =>
                            {
                                #if(includeJwt)
                                endpoints.MapMethods("function-route", new[] { "function-method" }, async context =>
                                {
                                    await func.RunAsync(context);
                                }).RequireAuthorization();
                                #else
                                endpoints.MapMethods("function-route", new[] { "function-method" }, async context =>
                                {
                                    await func.RunAsync(context);
                                });
                                #endif
                            });
                        });
                });
    }
}
