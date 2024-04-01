using System.Net;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using Core.Models;
using FluentValidation;

namespace Core.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (ValidationException ex)
            {
                await HandleValidationException(httpContext, ex);
            }
        }

        private async Task HandleValidationException(HttpContext context, ValidationException exception)
        {           
            var jsonResponse = CreateResultResponse(context, exception, HttpStatusCode.BadRequest);

            await context.Response.WriteAsync(jsonResponse);
        }

        private string CreateResultResponse(HttpContext context, Exception exception, HttpStatusCode statusCode)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var name = _next.Target?.GetType().Name;
            var method = _next.Method.Name;
            var json = JsonSerializer.Serialize(new Result($"{name}.{method} failed", exception.Message));

            return json;
        }
    }
}