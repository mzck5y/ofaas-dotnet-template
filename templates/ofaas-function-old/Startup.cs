using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
#if useHmac
using Oni.Core.OpenFaas.HMAC;
using Oni.Core.OpenFaas.Utils;
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

#if validateSignature
            services.Configure<SignatureValidationOptions>(op =>
            {
                op.SigningKey = OpenFaasUtils.GetSecret("function-signature-key");
                op.HeaderName = OpenFaasUtils.GetSecret("function-signature-header-name") ?? "hmac";
            });
#endif
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

#if isHmacSHA1
            app.UseHmacSha1Validator();

#elif isHmacSHA256v4
            app.UseHmacSha256v4Validator();

#elif isED25519
            app.UseEd25519Validator();
            
#endif
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        #endregion
    }
}
