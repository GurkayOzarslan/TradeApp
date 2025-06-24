using SharedKernel.WebSockets;
using System.Collections.Concurrent;
using TradeAppSharedKernel.Infrastructure.Helpers.Token;

namespace TradeApp.Infrastructure.Middleware;

public static class WebSocketMiddleware
{
    private static readonly ConcurrentDictionary<string, ConcurrentDictionary<string, int>> UserEndpointConnections = new();

    public static void UseWebSocketEndpoints(this WebApplication app, ConfigurationManager configuration)
    {
        app.Use(async (context, next) =>
        {
            if (context.Request.Path == "/ws")
            {
                if (context.WebSockets.IsWebSocketRequest)
                {
                    var token = context.Request.Query["token"].ToString();

                    if (string.IsNullOrWhiteSpace(token))
                    {
                        context.Response.StatusCode = 401;
                        await context.Response.WriteAsync("Token is missing.");
                        return;
                    }

                    var jwtService = context.RequestServices.GetRequiredService<IJwtTokenGenerator>();
                    var isValid = jwtService.ValidateToken(token);
                    if (!isValid)
                    {
                        context.Response.StatusCode = 401;
                        await context.Response.WriteAsync("Invalid token.");
                        return;
                    }

                    var socket = await context.WebSockets.AcceptWebSocketAsync();

                    var handler = context.RequestServices.GetRequiredService<IWebSocketHandler>();
                    var connectionManager = context.RequestServices.GetRequiredService<WebSocketConnectionManager>();

                    var socketId = connectionManager.AddSocket(socket);

                    await handler.HandleAsync(socketId, socket, token);
                }
                else
                {
                    context.Response.StatusCode = 400;
                }
            }
            else
            {
                await next();
            }
        });
    }
}
