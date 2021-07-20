using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using Oni.Core.OpenFaas.Utils;
#if useHmac
using Oni.Core.OpenFaas.HMAC;
#endif

namespace Oni.Serverless.Function
{
    public class Startup
    {
        #region Properties

        public IConfiguration Configuration { get; }

        #endregion

        #region Constructors

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        #endregion

        #region Public Methods

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
#if useHmac
            services.Configure<HMACValidationOptions>(op =>
            {
                op.SigningKey = OpenFaasUtils.GetSecret("function-hmac-key");
                op.HeaderName = OpenFaasUtils.GetSecret("function-hmac-header") ?? "hmac";
            });
#endif
            
            // Database Client
            services.AddSingleton<IMongoClient>(
                new MongoClient(OpenFaasUtils.GetSecret("oni-connection-name")));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

#if useHmac
            app.UseHMACValidator();

#endif
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        #endregion
    }
}
