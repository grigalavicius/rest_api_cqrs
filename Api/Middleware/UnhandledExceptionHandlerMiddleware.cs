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
                context.Response.StatusCode = (int)errorStatusCode;

                var result = JsonConvert.SerializeObject(new { message = errorMessage });

                await context.Response.WriteAsync(result);
            }
        }

        private static (HttpStatusCode statusCode, string errorMessage) HandleException(Exception exception)
        {
            switch (exception)
            {
                case ValidationException _:
                {
                    return (HttpStatusCode.BadRequest, exception.Message);
                }
                default:
                {
                    return (HttpStatusCode.InternalServerError, "An unhandled error occurred. The request could not be processed.");
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