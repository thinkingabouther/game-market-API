using System;
using game_market_API.ExceptionHandling;
using game_market_API.Exceptions;
using game_market_API.Services.ExceptionLoggingService;
using game_market_API.Utilities;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace game_market_API.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : ControllerBase
    {
        

        [Route("/error")]
        public ActionResult<ErrorResponse> Error()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var exception = context?.Error; // Your exception
            var code = 500; // Internal Server Error by default

            if (exception is ItemNotFoundException)
                code = 404;
            else if (exception is NotAuthorizedException)
                code = 401;
            else if (exception is BadRequestException)
                code = 400;
            
            Response.StatusCode = code;

            return new ErrorResponse(exception);

        }
    }
}