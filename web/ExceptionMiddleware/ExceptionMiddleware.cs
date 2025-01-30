using Microsoft.AspNetCore.Http;
using Serilog;
using System.Net;
using System.Threading.Tasks;

namespace ExceptionMiddleware
{
    public class ExceptionMiddleware
    {
        private RequestDelegate Next { get; }

        public ExceptionMiddleware(RequestDelegate next)
        {
            Next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await Next(context);
            }
            catch (System.Exception ex)
            {
                
                Log.Fatal(ex, "System Exception");
                HandleException(context);
            }
        }

        private void HandleException(HttpContext httpContext)
        {
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        }
    }
}
