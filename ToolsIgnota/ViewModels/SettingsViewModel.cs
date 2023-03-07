using System.Reflection;
using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using ToolsIgnota.Contracts.Services;
using ToolsIgnota.Helpers;

using Windows.ApplicationModel;

namespace ToolsIgnota.ViewModels;

public sealed partial class SettingsViewModel : ObservableRecipient, IDisposable
{
    private readonly IThemeSelectorService _themeSelectorService;
    private readonly ICombatManagerService _combatManagerService;
    private readonly List<IDisposable> _subscriptions = new();

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ConnectCombatManagerCommand))]
    [NotifyPropertyChangedFor(nameof(CombatManagerConnectSymbol))] 
    private string _combatManagerIp;
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ConnectCombatManagerCommand))]
    [NotifyPropertyChangedFor(nameof(CombatManagerConnectSymbol))]
    private string _combatManagerPort;
    [ObservableProperty] 
    [NotifyCanExecuteChangedFor(nameof(ConnectCombatManagerCommand))]
    [NotifyPropertyChangedFor(nameof(CombatManagerConnectSymbol))]
    private bool _combatManagerConnected;

    [ObservableProperty] private ElementTheme _elementTheme;
    [ObservableProperty] private string _versionDescription;
    [ObservableProperty] private string _combatManagerConnectTooltip = "Settings_CombatManager_Connection_Tooltip_Disconnected".GetLocalized();
    
    public Symbol CombatManagerConnectSymbol 
    { 
        get
        {
            if (FullIpAddress != _combatManagerService.IpAddress)
                return Symbol.Refresh;
            if (CombatManagerConnected)
                return Symbol.Accept;
            return Symbol.Cancel;
        } 
    }

    public SettingsViewModel(
        IThemeSelectorService themeSelectorService,
        ICombatManagerService combatManagerService)
    {
        _themeSelectorService = themeSelectorService;
        _combatManagerService = combatManagerService;

        _elementTheme = _themeSelectorService.Theme;
        _combatManagerIp = _combatManagerService.IpAddress.Split(":")[0];
        _combatManagerPort = _combatManagerService.IpAddress.Split(":")[1];
        _versionDescription = GetVersionDescription();
        _subscriptions.Add(_combatManagerService.Connected.Subscribe(x =>
        {
            CombatManagerConnected = x;
            CombatManagerConnectTooltip = x
                ? "Settings_CombatManager_Connection_Tooltip_Connected".GetLocalized()
                : "Settings_CombatManager_Connection_Tooltip_Error".GetLocalized();
            OnPropertyChanged(nameof(CombatManagerConnectSymbol));
        }));
    }

    private bool CanConnectCombatManager => !CombatManagerConnected || FullIpAddress != _combatManagerService.IpAddress;
    [RelayCommand(CanExecute = nameof(CanConnectCombatManager))]
    public async Task ConnectCombatManager()
    {
        await _combatManagerService.SetIpAddress(FullIpAddress);
        await _combatManagerService.Connect();
    }

    [RelayCommand]
    public async Task SwitchTheme(ElementTheme param)
    {
        if (ElementTheme != param)
        {
            ElementTheme = param;
            await _themeSelectorService.SetThemeAsync(param);
        }
    }

    private string FullIpAddress => $"{CombatManagerIp}:{CombatManagerPort}";

    private static string GetVersionDescription()
    {
        Version version;

        if (RuntimeHelper.IsMSIX)
        {
            var packageVersion = Package.Current.Id.Version;

            version = new(packageVersion.Major, packageVersion.Minor, packageVersion.Build, packageVersion.Revision);
        }
        else
        {
            version = Assembly.GetExecutingAssembly().GetName().Version!;
        }

        return $"{"AppDisplayName".GetLocalized()} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
    }

    public void Dispose()
    {
        _subscriptions.ForEach(x => x.Dispose());
    }
}
