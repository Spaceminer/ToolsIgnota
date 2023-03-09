using ToolsIgnota.Helpers;

namespace ToolsIgnota;

public sealed partial class DisplayWindow : WindowEx
{
    public DisplayWindow()
    {
        InitializeComponent();

        AppWindow.SetIcon(Path.Combine(AppContext.BaseDirectory, "Assets/WindowIcon.ico"));
        Content = null;
        Title = "AppDisplayName".GetLocalized();
    }
}
