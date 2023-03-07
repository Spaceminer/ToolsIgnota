using System.Net.WebSockets;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using ToolsIgnota.Contracts;
using ToolsIgnota.Contracts.Services;
using ToolsIgnota.Data;
using ToolsIgnota.Data.Models;

namespace ToolsIgnota.Services;

public class CombatManagerService : ICombatManagerService, IDisposable, IAsyncDisposable
{
    private const string IpAddressSettingsKey = "CombatManager.IpAddress";
    private readonly ILocalSettingsService _localSettingsService;
    private readonly ISubject<IEnumerable<CMCreature>> _creaturesSubject = new BehaviorSubject<IEnumerable<CMCreature>>(Enumerable.Empty<CMCreature>());
    private readonly ISubject<int?> _roundSubject = new BehaviorSubject<int?>(null);
    private readonly ISubject<bool> _connectionSubject = new BehaviorSubject<bool>(false);

    private CombatManagerConnection? _connection;
    private IDisposable? _combatManagerSubscription;
    private bool _disposed;

    public string IpAddress { get; set; } = "localhost:12457";

    public IObservable<bool> Connected => _connectionSubject;
    public IObservable<IEnumerable<CMCreature>> Creatures => _creaturesSubject;
    public IObservable<int?> Round => _roundSubject;

    public CombatManagerService(ILocalSettingsService localSettingsService)
    {
        _localSettingsService = localSettingsService;
    }

    public async Task InitializeAsync()
    {
        IpAddress = await LoadIpAddressFromSettings();
    }

    public async Task StartupAsync()
    {
        await Connect();
    }

    public async Task SetIpAddress(string ipAddress)
    {
        IpAddress = ipAddress;
        await SaveIpAddressInSettings(ipAddress);
    }

    public async Task<bool> Connect()
    {
        if (_connection == null)
        {
            _connection = new CombatManagerConnection(IpAddress);
        }
        else if (_connection.Uri != IpAddress || _connection.Connected == false)
        {
            await _connection.DisposeAsync();
            _connection = new CombatManagerConnection(IpAddress);
        }

        try
        {
            var observable = await _connection.Connect();
            _combatManagerSubscription = (await _connection.Connect()).Subscribe(
                x =>
                {
                    App.MainWindow.DispatcherQueue.TryEnqueue(() =>
                    {
                        _creaturesSubject.OnNext(x.Data.CombatList);
                        _roundSubject.OnNext(x.Data.Round);
                    });
                }, 
                ex =>
                {
                    App.MainWindow.DispatcherQueue.TryEnqueue(() =>
                    {
                        _connectionSubject.OnNext(false);
                    });
                }
            );
            _connectionSubject.OnNext(true);
            return true;
        }
        catch(WebSocketException)
        {
            _connectionSubject.OnNext(false);
            return false;
        }
    }

    private async Task<string> LoadIpAddressFromSettings()
    {
        var ipAddress = await _localSettingsService.ReadSettingAsync<string>(IpAddressSettingsKey);

        return ipAddress ?? "localhost:12457";
    }

    private async Task SaveIpAddressInSettings(string ipAddress)
    {
        await _localSettingsService.SaveSettingAsync(IpAddressSettingsKey, ipAddress);
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
                _connection?.Dispose();
                _combatManagerSubscription?.Dispose();
                _connection = null;
            }
            _disposed = true;
        }
    }
    protected async virtual ValueTask DisposeAsyncCore()
    {
        if(_connection != null)
            await _connection.DisposeAsync().ConfigureAwait(false);
        _combatManagerSubscription?.Dispose();
        _connection = null;
    }
}
