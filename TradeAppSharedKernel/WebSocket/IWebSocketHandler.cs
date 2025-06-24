using System.Net.WebSockets;

namespace SharedKernel.WebSockets
{
    public interface IWebSocketHandler
    {
        Task HandleAsync(string socketId, WebSocket socket, string token);
        Task SendMessageAsync(string socketId, string message);
        Task BroadcastMessageAsync(string message);
    }
}
