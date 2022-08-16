using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolsIgnota.Data.Models;

namespace ToolsIgnota.Data.Abstractions
{
    public interface ICombatManagerService
    {
        public IObservable<IEnumerable<CMCreature>> GetCreatures();
    }
}
