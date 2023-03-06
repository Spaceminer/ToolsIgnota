using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using ToolsIgnota.UI.ViewModels;

namespace ToolsIgnota.UI.Views.Pages
{
    public sealed partial class InitiativeDisplayPage : Page
    {
        private readonly InitiativeDisplayViewModel VM;

        public InitiativeDisplayPage()
        {
            this.InitializeComponent();
            var container = ((App)Application.Current).Container;
            VM = ActivatorUtilities.GetServiceOrCreateInstance<InitiativeDisplayViewModel>(container);
        }
    }
}
