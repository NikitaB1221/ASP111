using ASP111.Data;

namespace ASP111.Middleware
{
    public class MarkerMiddleware
    {
        private readonly RequestDelegate _next;
        private static int _cnt;

        public MarkerMiddleware(RequestDelegate next)
        {
            _cnt = 0;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, DataContext dataContext)
        {
            context.Items.Add("marker", $"Users: {dataContext.Users.Count()}, requests: { ++ _cnt}");
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
