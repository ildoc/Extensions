using System;
using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Utils;

namespace Boilerplate.Common.Exceptions
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ILogger<ExceptionFilter> _logger;
        private readonly IWebHostEnvironment _env;

        public ExceptionFilter(ILogger<ExceptionFilter> logger, IWebHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        public override void OnException(ExceptionContext context)
        {
            var statusCode = HttpStatusCode.InternalServerError;
            var handled = false;

            switch (context.Exception)
            {
                case EntityNotFoundException _:
                    statusCode = HttpStatusCode.NotFound;
                    handled = true;
                    break;
                case ForbiddenActionException _:
                    statusCode = HttpStatusCode.Forbidden;
                    handled = true;
                    break;
                case ArgumentNullException _:
                case AlreadyExistingException _:
                case ContractsException _:
                case BadRequestException _:
                    statusCode = HttpStatusCode.BadRequest;
                    handled = true;
                    break;
                case InternalServerErrorException _:
                    statusCode = HttpStatusCode.InternalServerError;
                    handled = true;
                    break;
                case RequestEntityTooLargeException _:
                    statusCode = HttpStatusCode.RequestEntityTooLarge;
                    handled = true;
                    break;
            }

            context.HttpContext.Response.ContentType = "application/json";
            context.HttpContext.Response.StatusCode = (int)statusCode;
            context.Result = !PrintStackTraceInResponse(handled) ?
                new JsonResult(context.Exception.Message) :
                new JsonResult(new { error = new[] { context.Exception.Message }, stackTrace = context.Exception.StackTrace });
            LogException(context.Exception);
        }

        private bool PrintStackTraceInResponse(bool handled)
        {
            return !handled && _env.IsDevelopment();
        }

        private void LogException(Exception exception)
        {
            //_logger.LogError(exception.Message, new[] { exception.Message , exception?.StackTrace });
            _logger.LogError(exception, exception.Message);
            if (exception.InnerException != null)
                LogException(exception.InnerException);
        }
    }
}
