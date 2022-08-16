using System;
using System.Linq;
using System.Net.WebSockets;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using ToolsIgnota.Data.Models;

namespace ToolsIgnota.Data
{
    public class CombatManagerConnection: IDisposable
    {
        private ClientWebSocket _client;
        private CancellationTokenSource _cancellation;
        private ISubject<CombatManagerResponse<CMState>> _subject;

        public CombatManagerConnection(string uri)
        {
            _client = new ClientWebSocket();
            _cancellation = new CancellationTokenSource();
            _subject = new ReplaySubject<CombatManagerResponse<CMState>>(1);

            _client.ConnectAsync(new Uri($"ws://{uri}/api/notifications"), _cancellation.Token);
            _ = Task.Factory.StartNew(
                CMStateListener, 
                _cancellation.Token, 
                TaskCreationOptions.LongRunning, 
                TaskScheduler.Default);
        }

        public IObservable<CombatManagerResponse<CMState>> GetState()
        {
            return _subject.AsObservable();
        }

        public void Dispose()
        {
            _cancellation.Cancel();
            _client.Dispose();
        }

        private async Task CMStateListener()
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
                    _subject.OnNext(responseObject);
                    message = "";
                }
            }
        }
    }
}
