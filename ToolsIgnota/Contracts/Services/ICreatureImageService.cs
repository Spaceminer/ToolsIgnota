using ToolsIgnota.Core.Models;

namespace ToolsIgnota.Contracts.Services;
public interface ICreatureImageService: IOnInitialize
{
    public Task SaveCreatureImages(IEnumerable<CreatureImage> imageNamePairs);
    public IObservable<IEnumerable<CreatureImage>> CreatureImages {
        get;
    }
}
