using Microsoft.UI.Xaml;
using ToolsIgnota.UI.Views.Pages;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ToolsIgnota.UI.Views.Windows
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DisplayWindow : Window
    {
        public DisplayWindow()
        {
            this.InitializeComponent();
            contentFrame.Navigate(typeof(InitiativeDisplayPage));
        }
    }
}
