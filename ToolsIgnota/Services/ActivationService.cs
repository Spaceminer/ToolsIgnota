using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using ToolsIgnota.Activation;
using ToolsIgnota.Contracts;
using ToolsIgnota.Contracts.Services;
using ToolsIgnota.Views;

namespace ToolsIgnota.Services;

public class ActivationService : IActivationService
{
    private readonly ActivationHandler<LaunchActivatedEventArgs> _defaultHandler;
    private readonly IEnumerable<IActivationHandler> _activationHandlers;
    private readonly IEnumerable<IOnInitialize> _initializeHooks;
    private readonly IEnumerable<IOnStartup> _startupHooks;
    private UIElement? _shell = null;
    private UIElement? _display = null;

    public ActivationService(
        ActivationHandler<LaunchActivatedEventArgs> defaultHandler,
        IEnumerable<IActivationHandler> activationHandlers,
        IEnumerable<IOnInitialize> initializeHooks,
        IEnumerable<IOnStartup> startupHooks)
    {
        _defaultHandler = defaultHandler;
        _activationHandlers = activationHandlers;
        _initializeHooks = initializeHooks;
        _startupHooks = startupHooks;
    }

    public async Task ActivateAsync(object activationArgs)
    {
        // Execute tasks before activation.
        await InitializeAsync();

        // Set the MainWindow Content.
        if (App.MainWindow.Content == null)
        {
            _shell = App.GetService<ShellPage>();
            App.MainWindow.Content = _shell ?? new Frame();
        }

        // Set the DisplayWindow Content.
        if (App.DisplayWindow.Content == null)
        {
            _display = App.GetService<DisplayPage>();
            App.DisplayWindow.Content = _display ?? new Frame();
        }

        // Handle activation via ActivationHandlers.
        await HandleActivationAsync(activationArgs);

        // Activate the Windows.
        App.MainWindow.Activate();
        App.DisplayWindow.Activate();

        // Execute tasks after activation.
        await StartupAsync();
    }

    private async Task HandleActivationAsync(object activationArgs)
    {
        var activationHandler = _activationHandlers.FirstOrDefault(h => h.CanHandle(activationArgs));

        if (activationHandler != null)
        {
            await activationHandler.HandleAsync(activationArgs);
        }

        if (_defaultHandler.CanHandle(activationArgs))
        {
            await _defaultHandler.HandleAsync(activationArgs);
        }
    }

    private async Task InitializeAsync()
    {
        foreach (var hooks in _initializeHooks)
        {
            await hooks.InitializeAsync().ConfigureAwait(false);
        }
        await Task.CompletedTask;
    }

    private async Task StartupAsync()
    {
        foreach (var hook in _startupHooks)
        {
            await hook.StartupAsync().ConfigureAwait(false);
        }
        await Task.CompletedTask;
    }
}
