using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace ERP.Models.Logger
{
    public class SeriLogMiddleware
    {
        private readonly RequestDelegate _next;

        // To catch unhandled exceptions, exceptions of controllers etc.
        public SeriLogMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                if (httpContext == null) throw new ArgumentNullException(nameof(httpContext));

                await _next.Invoke(httpContext);
            }
            catch (Exception ex)
            {
                SeriLogTools.LogErrorWithContext(ex);

                throw;
            }
        }
    }
}
