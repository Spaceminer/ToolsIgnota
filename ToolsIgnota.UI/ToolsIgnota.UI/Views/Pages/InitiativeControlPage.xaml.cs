using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using ToolsIgnota.UI.ViewModels;

namespace ToolsIgnota.UI.Views.Pages
{
    public sealed partial class InitiativeControlPage : Page
    {
        private readonly InitiativeControlViewModel VM;

        public InitiativeControlPage()
        {
            this.InitializeComponent();
            var container = ((App)Application.Current).Container;
            VM = ActivatorUtilities.GetServiceOrCreateInstance<InitiativeControlViewModel>(container);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            VM.SaveCreatureImages();
        }
    }
}
