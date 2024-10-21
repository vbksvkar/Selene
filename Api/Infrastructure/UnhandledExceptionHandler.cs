using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;

namespace Api.Infrastructure
{
    public class UnhandledExceptionHandler : IUnhandledExceptionHandler
    {
        public async Task OnUnhandledException(HttpContext httpContext)
        {
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            httpContext.Response.ContentType = "application/json";
            var exception = httpContext.Features.Get<IExceptionHandlerFeature>()?.Error;
            var response = new Dictionary<string, string>
            {
                { "message", "An error occurred while processing your request." },
                { "exception", exception?.Message }
            };
            await JsonSerializer.SerializeAsync(httpContext.Response.Body, response);
        }
    }
}