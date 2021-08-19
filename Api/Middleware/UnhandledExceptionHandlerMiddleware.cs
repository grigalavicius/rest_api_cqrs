using System;
using System.Net;
using System.Threading.Tasks;
using Application.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace RestApiTask.Middleware
{
    internal class UnhandledExceptionHandlerMiddleware
    {
        private readonly RequestDelegate next;

        public UnhandledExceptionHandlerMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception e)
            {
                var (errorStatusCode, errorMessage) = HandleException(e);

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = errorStatusCode;

                var result = JsonConvert.SerializeObject(new { message = errorMessage });

                await context.Response.WriteAsync(result);
            }
        }

        private static (int, string errorMessage) HandleException(Exception exception)
        {
            switch (exception)
            {
                case ValidationException _:
                    return ((int)HttpStatusCode.BadRequest, exception.Message);
                default:
                {
                    var msg = exception?.InnerException is not null ? exception.InnerException.Message : exception?.Message;
                    return ((int)HttpStatusCode.InternalServerError, msg ?? "An unhandled error occurred. The request could not be processed.");
                }
            }
        }
    }
    
    internal static class UnhandledExceptionHandlerMiddlewareExtensions
    {
        public static void UseUnhandledExceptionHandler(this IApplicationBuilder app)
        {
            app.UseMiddleware<UnhandledExceptionHandlerMiddleware>();
        }
    }
}