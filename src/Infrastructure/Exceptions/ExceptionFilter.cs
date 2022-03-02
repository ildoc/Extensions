using System.Data;
using System.Net;
using System.Security.Authentication;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Exceptions
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
            _logger.LogError(context.Exception, context.Exception.Message);

            context.HttpContext.Response.StatusCode = context.Exception switch
            {
                EntityNotFoundException => (int)HttpStatusCode.NotFound,
                UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
                InvalidCredentialException => (int)HttpStatusCode.Unauthorized,
                AlreadyExistingException => (int)HttpStatusCode.BadRequest,
                ConstraintException => (int)HttpStatusCode.BadRequest,
                ArithmeticException => (int)HttpStatusCode.BadRequest,
                _ => (int)HttpStatusCode.InternalServerError
            };
            context.HttpContext.Response.ContentType = "application/json";

            var response = _env.IsDevelopment()
                ? new ApiError(context.HttpContext.Response.StatusCode, context.Exception.Message, context.Exception.StackTrace)
                : new ApiError(context.HttpContext.Response.StatusCode, context.Exception.Message);

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var json = JsonSerializer.Serialize(response, options);

            context.HttpContext.Response.WriteAsync(json);
        }
    }

    public class ApiError
    {
        public ApiError(int statusCode, string message = null, string details = null)
        {
            StatusCode = statusCode;
            Message = message;
            Details = details;
        }

        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string Details { get; set; }
    }
}
