using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace Example.Api.Controllers
{
    /// <summary>
    /// Error handlling controller
    /// </summary>
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : ControllerBase
    {
        private readonly ILogger<ErrorController> _logger;
        public ErrorController(ILogger<ErrorController> logger) {
            _logger = logger;
        }

        /// <summary>
        /// Catch unexpected/unhandled exceptions on developement environment
        /// </summary>
        /// <param name="webHostEnvironment"></param>
        /// <returns></returns>
        [Route("/error-local-development")]
        public IActionResult ErrorLocalDevelopment(
        [FromServices] IWebHostEnvironment webHostEnvironment)
        {
            if (webHostEnvironment.EnvironmentName != "Development")
            {
                throw new InvalidOperationException(
                    "This shouldn't be invoked in non-development environments.");
            }

            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();

            _logger.LogError("Unexpected error occured: {0} {1}", context.Error.Message, context.Error.StackTrace);
            
            return Problem(
                detail: context.Error.StackTrace,
                title: context.Error.Message);
        }
        /// <summary>
        /// Catch unexpected/unhandled exceptions on production environment
        /// </summary>
        /// <returns></returns>
        [Route("/error")]
        public IActionResult Error()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            _logger.LogError("Unexpected error occured: {0} {1}", context.Error.Message, context.Error.StackTrace);
            
            return Problem();
        }
    }
}
