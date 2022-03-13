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
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ToolsIgnota.UI.UserControls
{
    public sealed partial class InitiativeRecord : UserControl
    {
        public string Image
        {
            set { picture.ProfilePicture = value != null ? new BitmapImage(new Uri(value)) : null; }
        }

        public string DisplayName
        {
            set { picture.DisplayName = value; }
        }

        private bool _isHighlighted = false;
        public bool IsHighlighted
        {
            get { return _isHighlighted; }
            set { outline.BorderThickness = value ? new Thickness(5) : new Thickness(0); _isHighlighted = value; }
        }

        public InitiativeRecord()
        {
            this.InitializeComponent();
        }

    }
}
