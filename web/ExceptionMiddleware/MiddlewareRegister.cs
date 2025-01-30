using Microsoft.AspNetCore.Builder;

namespace ExceptionMiddleware
{
    public static class MiddlewareRegister
    {
        public static void UseExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
