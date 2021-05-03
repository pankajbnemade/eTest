using Microsoft.AspNetCore.Http;
using System;

namespace ERP.Models.Logger
{
    public static class ContextAccessor
    {
        private static IHttpContextAccessor _httpContextAccessor;

        public static void Configure(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public static HttpContext HttpContext => null == _httpContextAccessor ? throw new ArgumentNullException(nameof(_httpContextAccessor)) : _httpContextAccessor.HttpContext;
    }
}
