using Microsoft.AspNetCore.Http;
using OniCloud.OpenFaas.Attributes;
using OniCloud.OpenFaas.Models;
using System.Net;
using System.Threading.Tasks;

namespace Serverless.Function.Handler
{
    public class FunctionHandler
    {
        #region Public Methods

        [HttpTrigger("function-name", function-auth, "function-route", "function-method")]
        public async Task RunAsync(HttpContext ctx)
        {
            string name = ctx.Request.Query["name"];

            ctx.Response.StatusCode = (int) HttpStatusCode.OK;
            await ctx.Response.WriteAsync($"Hello {name}");
        }

        #endregion
    }
}
