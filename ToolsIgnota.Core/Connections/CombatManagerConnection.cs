using System.Net.WebSockets;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.Json;
using ToolsIgnota.Data.Models;

namespace ToolsIgnota.Data;

public class CombatManagerConnection: IDisposable, IAsyncDisposable
{
    private readonly string _uri;
    private readonly ClientWebSocket _clientWebSocket = new();
    private readonly CancellationTokenSource _websocketCancellationTokenSource = new();
    private readonly ISubject<CombatManagerResponse<CMState>> _subject = new ReplaySubject<CombatManagerResponse<CMState>>(1);

    private Task _websocketTask;
    private bool _disposed;

    public CombatManagerConnection(string uri)
    {
        _uri = uri;
    }

    public string Uri => _uri;
    public bool Connected => _clientWebSocket.State == WebSocketState.Open;

    /// <summary>
    /// Connects to the CM websocket endpoint. 
    /// </summary>
    /// <exception cref="WebSocketException"></exception>
    /// <returns>Observable stream of CMState</returns>
    public async Task<IObservable<CombatManagerResponse<CMState>>> Connect()
    {
        if (_websocketTask == null)
        {
            await _clientWebSocket.ConnectAsync(
                new Uri($"ws://{_uri}/api/notifications"),
                _websocketCancellationTokenSource.Token);

            _websocketTask = await Task.Factory.StartNew(
                CMStateListener,
                _websocketCancellationTokenSource.Token,
                TaskCreationOptions.LongRunning,
                TaskScheduler.Default);
        }
        return _subject.AsObservable();
    }

    private async Task CMStateListener()
    {
        var message = "";
        var bytes = new byte[128];
        var buffer = new ArraySegment<byte>(bytes);   

        while (_clientWebSocket.State == WebSocketState.Open && !_websocketCancellationTokenSource.IsCancellationRequested)
        {
            try
            {
                var receiveResult = await _clientWebSocket.ReceiveAsync(buffer, _websocketCancellationTokenSource.Token);
                var messageBytes = buffer.Skip(buffer.Offset).Take(receiveResult.Count).ToArray();
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
            catch (WebSocketException ex)
            {
                _clientWebSocket.Abort();
                _subject.OnError(ex);
            }
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public async ValueTask DisposeAsync()
    {
        await DisposeAsyncCore().ConfigureAwait(false);
        Dispose(false);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _websocketCancellationTokenSource?.Cancel();
                _websocketTask?.Wait();

                _websocketCancellationTokenSource?.Dispose();
                _websocketTask?.Dispose();
                _clientWebSocket?.Dispose();
            }
            _disposed = true;
        }
    }

    protected async virtual ValueTask DisposeAsyncCore()
    {
        _websocketCancellationTokenSource?.Cancel();
        try
        {
            if (_websocketTask != null)
                await _websocketTask.ConfigureAwait(false);
        }
        catch (TaskCanceledException) { }

        _websocketCancellationTokenSource?.Dispose();
        _websocketTask?.Dispose();
        _clientWebSocket?.Dispose();
    }
}
