using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace BookStoreApp.filters
{
    public class RateLimitAttribute : ActionFilterAttribute
    {
        private readonly int _maxRequests;
        private readonly int _durationInSeconds;

        public RateLimitAttribute(int maxRequests, int durationInSeconds)
        {
            _maxRequests = maxRequests;
            _durationInSeconds = durationInSeconds;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var cache = context.HttpContext.RequestServices.GetService(typeof(IMemoryCache)) as IMemoryCache;

            var ip = context.HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var endpoint = context.HttpContext.Request.Path.ToString().ToLower();
            var key = $"rl:{ip}:{endpoint}:{context.HttpContext.Request.Method}";

            int count = cache.GetOrCreate<int>(key, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(_durationInSeconds);
                return 0;
            });

            if (count >= _maxRequests)
            {
                context.Result = new ContentResult
                {
                    StatusCode = 429,
                    Content = "Too many requests, Please try again later!"
                };
                return;
            }

            cache.Set(key, ++count, TimeSpan.FromSeconds(_durationInSeconds));
        }
    }
}
