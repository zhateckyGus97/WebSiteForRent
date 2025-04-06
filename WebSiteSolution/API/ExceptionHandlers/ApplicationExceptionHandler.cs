using Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.ExceptionHandlers
{
    public class ApplicationExceptionHandler(IProblemDetailsService _problemDetailsService) 
        : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, 
            Exception exception, CancellationToken cancellationToken)
        {
            if (exception is not BaseApplicationException e)
                return false;

            httpContext.Response.StatusCode = (int)e.StatusCode;
            httpContext.Response.ContentType = "application/json";
            
            var problemDetails = new ProblemDetails
            {
                Status = (int)e.StatusCode,
                Title = e.Title,
                Detail = exception.Message,
                Instance = httpContext.Request.Path,
                Type = exception.GetType().Name
            };

            var problemDetailsContext = new ProblemDetailsContext
            {
                HttpContext = httpContext,
                ProblemDetails = problemDetails,
                Exception = e
            };

            await _problemDetailsService.WriteAsync(problemDetailsContext);

            return true;
        }
    }
}