using ToolsIgnota.Data.Models;
using ToolsIgnota.Services;

namespace ToolsIgnota.Contracts.Services;

public interface ICombatManagerService : IOnInitialize, IOnStartup
{
    string IpAddress { get; }
    IObservable<IEnumerable<CMCreature>> Creatures { get; }
    IObservable<int?> Round { get; }
    IObservable<bool> Connected { get; }

    Task SetIpAddress(string ipAddress);

    Task<bool> Connect();
}
