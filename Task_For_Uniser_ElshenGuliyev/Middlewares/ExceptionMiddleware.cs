using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System;
using Task_For_Uniser_ElshenGuliyev.Models;

namespace Task_For_Uniser_ElshenGuliyev.Middlewares
{
    public class ExceptionMiddleware
    {

        #region Context
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        public ExceptionMiddleware(RequestDelegate next, ILogger<Product> logger)
        {
            _logger = logger;
            _next = next;
        }
        #endregion

        #region InvokeAsync
        public async System.Threading.Tasks.Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ups,Gözlənilməz səhvlik baş verdi: {ex}");
                await HandleExceptionAsync(httpContext, ex);
            }
        }
        #endregion

        #region HandleExceptionAsync
        private async System.Threading.Tasks.Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsync("Ups,Gözlənilməz xəta baş verdi.Zəhmət olmazsa yenidən cəhd edin.Əks halda sayt admini ilə əlaqə saxlayın!");
        }
        #endregion





    }
}
