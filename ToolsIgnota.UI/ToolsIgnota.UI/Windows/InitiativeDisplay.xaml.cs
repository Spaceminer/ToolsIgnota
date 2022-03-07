using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using ToolsIgnota.UI.Pages;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ToolsIgnota.UI.Windows
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class InitiativeDisplayWindow : Window
    {
        private readonly InitiativeControlPage _controlPage;

        public InitiativeDisplayWindow(InitiativeControlPage controlPage)
        {
            this.InitializeComponent();
            _controlPage = controlPage ?? throw new ArgumentNullException(nameof(controlPage));
        }

        public void SetBackgroundImage(Uri path)
        {
            ((ImageBrush)container.Background).ImageSource = new BitmapImage(path);
        }

        private void Window_Closed(object sender, WindowEventArgs args)
        {
            _controlPage.DisplayWindowClosed();
        }
    }
}
