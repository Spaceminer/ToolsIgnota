using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using Windows.System;

namespace ToolsIgnota.Data.Abstractions
{
    public interface IWindowService
    {
        public dynamic GetDispatcher();
        public void OpenDisplayWindow();
        public void CloseDisplayWindow();
    }
}
