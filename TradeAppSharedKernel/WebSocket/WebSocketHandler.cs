using SharedKernel.WebSockets;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using TradeAppSharedKernel.ExternalApiService;

public class WebSocketHandler : IWebSocketHandler
{
    private readonly WebSocketConnectionManager _connectionManager;
    private readonly IExternalApiService _externalApiService;

    public WebSocketHandler(WebSocketConnectionManager connectionManager, IExternalApiService externalApiService)
    {
        _connectionManager = connectionManager;
        _externalApiService = externalApiService;
    }

    public async Task HandleAsync(string socketId, WebSocket socket, string token)
    {
        Console.WriteLine($"[WebSocket] Connection established: {socketId}");

        while (socket.State == WebSocketState.Open)
        {
            try
            {
                var result = await _externalApiService.GetStocksAsync(token);
                if (result != null)
                {
                    var json = JsonSerializer.Serialize(result);
                    var buffer = Encoding.UTF8.GetBytes(json);
                    var segment = new ArraySegment<byte>(buffer);

                    await socket.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[WebSocket] Error: {ex.Message}");
            }

            await Task.Delay(TimeSpan.FromSeconds(20));
        }

        await _connectionManager.RemoveSocketAsync(socketId);
        Console.WriteLine($"[WebSocket] Connection closed: {socketId}");
    }

    public Task SendMessageAsync(string socketId, string message)
    {
        throw new NotImplementedException();
    }


    public Task BroadcastMessageAsync(string message)
    {
        throw new NotImplementedException();
    }
}
