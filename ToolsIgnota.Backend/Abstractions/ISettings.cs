using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolsIgnota.Backend.Models;

namespace ToolsIgnota.Backend.Abstractions
{
    public interface ISettings
    {
        public void SaveCreatureImages(IEnumerable<CreatureImage> imageNamePairs);
        public IEnumerable<CreatureImage> GetCreatureImages();
    }
}
