using ToolsIgnota.Data.Abstractions;
using ToolsIgnota.UI.Views.Windows;

namespace ToolsIgnota.UI.Services
{
    public class WindowService : IWindowService
    {
        private DisplayWindow DisplayWindow { get; set; }

        public void CloseDisplayWindow()
        {
            DisplayWindow?.Close();
        }

        public dynamic GetDispatcher()
        {
            return App.Window.DispatcherQueue;
        }

        public void OpenDisplayWindow()
        {
            DisplayWindow = new DisplayWindow();
            DisplayWindow.Activate();
        }
    }
}
