using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Middlewares
{
    public class ErrorWrappingMiddleware
    {
        private readonly ILogger _logger;
        private readonly RequestDelegate _next;
        public ErrorWrappingMiddleware(ILogger logger, RequestDelegate next)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var errorMessage = string.Empty;
            try
            {
                await _next.Invoke(context);
            }
            catch (ValidationException ex)
            {

                throw;
            }
        }
    }
}
