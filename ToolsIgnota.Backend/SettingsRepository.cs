using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolsIgnota.Backend.Abstractions;
using ToolsIgnota.Backend.Models;

namespace ToolsIgnota.Backend
{
    public class SettingsRepository : ISettings
    {
        public IEnumerable<CreatureImage> GetCreatureImages()
        {
            throw new NotImplementedException();
        }

        public void SaveCreatureImages(IEnumerable<CreatureImage> imageNamePairs)
        {
            throw new NotImplementedException();
        }
    }
}
