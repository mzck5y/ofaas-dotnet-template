using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace host.Function
{
    public class FunctionHandler
    {
        #region Fields

        private readonly ILogger _logger;

        #endregion

        #region Constructors

        public FunctionHandler(ILogger<FunctionHandler> logger)
        {
            _logger = logger;
        }

        #endregion

        #region Public Methods

        public async Task HandlerAsync(HttpContext ctx)
        {
            string name = ctx.Request.Query["name"];

            ctx.Response.StatusCode = (int) HttpStatusCode.OK;
            await ctx.Response.WriteAsync($"Hello {name}");
        }

        #endregion
    }
}
