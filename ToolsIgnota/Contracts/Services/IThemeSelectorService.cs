using Microsoft.UI.Xaml;

namespace ToolsIgnota.Contracts.Services;

public interface IThemeSelectorService : IOnInitialize, IOnStartup
{
    ElementTheme Theme
    {
        get;
    }

    Task SetThemeAsync(ElementTheme theme);

    Task SetRequestedThemeAsync();
}
