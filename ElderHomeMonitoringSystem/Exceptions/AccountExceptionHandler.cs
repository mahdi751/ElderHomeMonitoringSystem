using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace ElderHomeMonitoringSystem.Exceptions
{
    public class AccountExceptionHandler : IAsyncExceptionFilter
    {
        private readonly ILogger<AccountExceptionHandler> _logger;

        public AccountExceptionHandler(ILogger<AccountExceptionHandler> logger)
        {
            _logger = logger;
        }

        public Task OnExceptionAsync(ExceptionContext context)
        {
            context.HttpContext.Response.ContentType = "application/json";

            string message;
            string errorType;

            if (context.Exception is AccountExceptions.UsernameTakenException)
            {
                message = context.Exception.Message;
                errorType = "UsernameTaken";
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            else if (context.Exception is AccountExceptions.EmailTakenException)
            {
                message = context.Exception.Message;
                errorType = "EmailTaken";
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            else if (context.Exception is AccountExceptions.UserNotFoundException)
            {
                message = context.Exception.Message;
                errorType = "UserNotFound";
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
            }
            else if (context.Exception is AccountExceptions.PasswordMismatchException)
            {
                message = context.Exception.Message;
                errorType = "PasswordMismatch";
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            else if (context.Exception is UnauthorizedAccessException)
            {
                message = "Unauthorized";
                errorType = "Unauthorized";
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            }
            else
            {
                message = "An unexpected error occurred";
                errorType = "InternalServerError";
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }

            var errorResponse = new ErrorResponse
            {
                Message = message,
                ErrorType = errorType
            };

            var jsonErrorResponse = JsonSerializer.Serialize(errorResponse);
            context.Result = new ContentResult
            {
                Content = jsonErrorResponse,
                StatusCode = context.HttpContext.Response.StatusCode,
                ContentType = "application/json"
            };

            _logger.LogError(context.Exception, $"An error occurred: {context.Exception.Message}");

            return Task.CompletedTask;
        }
    }
}
