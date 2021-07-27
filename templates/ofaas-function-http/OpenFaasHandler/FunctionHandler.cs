using Microsoft.AspNetCore.Http;
using System.Net;
using System.Threading.Tasks;

namespace host.Function
{
    public class FunctionHandler
    {
        #region Public Methods

        // [HttpTrigger(
        //     "function-name",
        //     AuthorizationType.function-auth,
        //     "function-route",
        //     function-method)]
        public async Task RunAsync(HttpContext ctx)
        {
            string name = ctx.Request.Query["name"];

            ctx.Response.StatusCode = (int) HttpStatusCode.OK;
            await ctx.Response.WriteAsync($"Hello {name}");
        }

        #endregion
    }
}
