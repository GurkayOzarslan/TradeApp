using System.Net.WebSockets;
using System.Collections.Concurrent;

namespace SharedKernel.WebSockets
{
    public class WebSocketConnectionManager
    {
        private readonly ConcurrentDictionary<string, WebSocket> _sockets = new();

        public string AddSocket(WebSocket socket)
        {
            var id = Guid.NewGuid().ToString();
            _sockets.TryAdd(id, socket);
            return id;
        }

        public ConcurrentDictionary<string, WebSocket> GetAll() => _sockets;

        public WebSocket? GetSocketById(string id) =>
            _sockets.TryGetValue(id, out var socket) ? socket : null;

        public async Task RemoveSocketAsync(string id)
        {
            if (_sockets.TryRemove(id, out var socket))
            {
                if (socket.State == WebSocketState.Open || socket.State == WebSocketState.CloseReceived)
                {
                    await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by server", CancellationToken.None);
                }
            }
        }
    }
}
