using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using ToolsIgnota.Data.Models;
using Windows.Storage;

namespace ToolsIgnota.Data.Utilities
{
    public static class LocalSettings
    {
        private readonly static ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

        public static string CombatManagerUri
        {
            get 
            { 
                return localSettings.Values.ContainsKey(nameof(CombatManagerUri)) 
                    ? (string)localSettings.Values[nameof(CombatManagerUri)]
                    : "localhost:12457"; 
            }
            set 
            { 
                localSettings.Values[nameof(CombatManagerUri)] = value; 
            }
        }

        public static IEnumerable<CreatureImage> CreatureImages
        {
            get 
            { 
                return localSettings.Values.ContainsKey(nameof(CreatureImages))
                    ? JsonSerializer.Deserialize<IEnumerable<CreatureImage>>((string)localSettings.Values[nameof(CreatureImages)])
                    : Enumerable.Empty<CreatureImage>(); 
            }
            set 
            { 
                localSettings.Values[nameof(CreatureImages)] = JsonSerializer.Serialize(value);
            }
        }
    }
}
