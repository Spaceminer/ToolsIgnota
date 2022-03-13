using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using ToolsIgnota.Backend.Models;

namespace ToolsIgnota.Backend
{
    public class CombatManagerClient
    {
        private readonly ClientWebSocket _client;
        private readonly CancellationTokenSource _cancellation;

        public CombatManagerClient()
        {
            _client = new ClientWebSocket();
            _cancellation = new CancellationTokenSource();
        }

        public async Task StartListening(Action<CombatManagerResponse<CMState>> callback)
        {
            await _client.ConnectAsync(new Uri("ws://localhost:12457/api/notifications"), _cancellation.Token);

            _ = Task.Factory.StartNew(
                async () =>
                {
                    string message = "";

                    var bytes = new byte[128];
                    var buffer = new ArraySegment<byte>(bytes);
                    while (_client.State == WebSocketState.Open)
                    {
                        WebSocketReceiveResult receiveResult = await _client.ReceiveAsync(buffer, _cancellation.Token);
                        byte[] messageBytes = buffer.Skip(buffer.Offset).Take(receiveResult.Count).ToArray();
                        message += Encoding.UTF8.GetString(messageBytes);

                        if (receiveResult.EndOfMessage && receiveResult.CloseStatus == null)
                        {
                            var responseObject = JsonSerializer.Deserialize<CombatManagerResponse<CMState>>(
                                    message,
                                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                            callback(responseObject);
                            message = "";
                        }
                    }
                }, _cancellation.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        public async Task StopListening()
        {
            await _client.CloseAsync(WebSocketCloseStatus.NormalClosure, null, new CancellationToken());
            _cancellation.Cancel();
        }
    }
}
