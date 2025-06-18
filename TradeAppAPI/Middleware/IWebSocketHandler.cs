using System.Net.WebSockets;

namespace TradeAppAPI.Middleware
{
    public interface IWebSocketHandler
    {
        string Endpoint { get; }
        Task HandleAsync(HttpContext context, WebSocket webSocket);
    }
}
