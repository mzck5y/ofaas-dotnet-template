using Microsoft.AspNetCore.Mvc;
using System;

namespace Oni.Serverless.Function
{
    public class FunctionHandler : ControllerBase
    {
        [HttpPost("/")]
        public IActionResult Run()
        {
            return Ok($"Hello from serverless function using the .net 5.0 runtime. Today is {DateTime.Now}");
        }
    }
}
