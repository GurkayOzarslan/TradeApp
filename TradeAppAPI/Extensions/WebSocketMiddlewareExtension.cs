
using TradeApp.Infrastructure.Middleware;

public static class WebSocketMiddlewareExtension
{
    public static IApplicationBuilder UseWebSocketHandler(this IApplicationBuilder app)
    {
        return app.UseMiddleware<WebSocketMiddleware>();
    }
}
