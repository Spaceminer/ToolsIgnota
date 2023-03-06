using Microsoft.Extensions.DependencyInjection;
using System;
using ToolsIgnota.Data.Abstractions;
using ToolsIgnota.Data.Services;
using ToolsIgnota.UI.Services;
using ToolsIgnota.UI.Utilities;

namespace ToolsIgnota.UI
{
    public static class DependencyManager
    {
        public static IServiceProvider GetServiceProvider()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddSingleton<IWindowService, WindowService>();
            serviceCollection.AddSingleton<IFilePickerService, FilePickerService>();

            serviceCollection.AddSingleton<ICreatureImageService, CreatureImageService>();
            serviceCollection.AddSingleton<ICombatManagerService, CombatManagerService>();
            serviceCollection.AddSingleton<IInitiativeDisplayService, InitiativeDisplayService>();

            return serviceCollection.BuildServiceProvider();
        }
    }
}
