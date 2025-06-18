using System.Collections.Concurrent;
using System.Security.Claims;
using TradeAppAPI.Middleware;
using TradeAppSharedKernel.Infrastructure.Helpers.Token;

namespace TradeApp.Infrastructure.Middleware;

public  class WebSocketMiddleware
{
    private  readonly ConcurrentDictionary<string, ConcurrentDictionary<string, int>> UserEndpointConnections = new();

    public  void UseWebSocketEndpoints(this WebApplication app, ConfigurationManager configuration)
    {
        app.Map("/ws/{endpoint}/{id?}", async context =>
        {
            var token = context.Request.Query["token"].ToString();
            var key = configuration["Token:Secret"]!;

            if (string.IsNullOrEmpty(token))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Token is required.");

                return;
            }
            var secretKey = configuration["Token:Secret"]!;

            if (!JwtTokenGenerator.ValidateToken(token, secretKey, out var principal))
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync("Token is invalid.");

                return;
            }

            if (principal == null)
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync("Token is invalid.");
                return;
            }



            var endpoint = context.Request.RouteValues["endpoint"]?.ToString();

            if (endpoint is null)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("Endpoint is required.");

                return;
            }

            var id = context.Request.RouteValues["id"]?.ToString() ?? "default";
            var userId = principal?.FindFirstValue("UserId");

            if (string.IsNullOrEmpty(userId))
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync("User ID is invalid.");

                return;
            }

            var connections = UserEndpointConnections.GetOrAdd(userId, _ => new ConcurrentDictionary<string, int>());
            var count = connections.GetOrAdd(endpoint, 0);

            if (count >= 5)
            {
                context.Response.StatusCode = 429;
                await context.Response.WriteAsync("Maximum connections reached for this endpoint.");

                return;
            }

            connections.AddOrUpdate(endpoint, 1, (_, count) => count + 1);

            try
            {
                var handler = context.RequestServices
                    .GetServices<IWebSocketHandler>()
                    .FirstOrDefault(h => h.Endpoint == endpoint);

                if (handler != null)
                {
                    //await handler.HandleWebSocketAsync(context, principal!.GetUser(), id);
                }
                else
                {
                    context.Response.StatusCode = 404;
                    await context.Response.WriteAsync("Endpoint was not found.");
                }
            }
            finally
            {
                connections.AddOrUpdate(endpoint, 0, (_, count) => Math.Max(count - 1, 0));

                if (connections.Values.Sum() == 0)
                {
                    UserEndpointConnections.TryRemove(userId, out _);
                }
            }
        });
    }
}
