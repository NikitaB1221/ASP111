using ASP111.Data;

namespace ASP111.Middleware
{
    public class MarkerMiddleware
    {
        private readonly RequestDelegate _next;
        private static int _cnt;
        private static uint getRs = 0;
        private static uint postRs = 0;


        public MarkerMiddleware(RequestDelegate next)
        {
            _cnt = 0;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, DataContext dataContext)
        {
            context.Items.Add("marker", $"Users: N/w, requests: { ++ _cnt}");
            if (context.Request.Method.Equals("GET", StringComparison.OrdinalIgnoreCase))
            {
                ++getRs;
            }
            else if (context.Request.Method.Equals("POST", StringComparison.OrdinalIgnoreCase))
            {
                ++postRs;
            }
            context.Items["GET_Req-s"] = getRs;
            context.Items["POST_Req-s"] = postRs;
            await _next(context);
        }
    }

    public static class MarkerMiddlewareExtension
    {
        public static IApplicationBuilder UseMarker(this IApplicationBuilder app)
        {
            return app.UseMiddleware<MarkerMiddleware>();
        }
    }
}
