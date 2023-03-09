using System.Collections.Immutable;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.VisualBasic;
using ToolsIgnota.Contracts.Services;
using ToolsIgnota.Data.Models;
using ToolsIgnota.Helpers;
using ToolsIgnota.Models;

namespace ToolsIgnota.ViewModels;

public partial class InitiativeDisplayViewModel : ObservableRecipient
{
    private readonly ICreatureImageService _creatureImageService;
    private readonly ICombatManagerService _combatManagerService;

    [ObservableProperty] private int _activeCreatureIndex = 0;

    public ObservableCollection<InitiativeCreatureModel> CreatureList { get; set; } = new();

    public InitiativeDisplayViewModel(
        ICreatureImageService creatureImageService,
        ICombatManagerService combatManagerService)
    {
        _creatureImageService = creatureImageService ?? throw new ArgumentNullException(nameof(creatureImageService));
        _combatManagerService = combatManagerService ?? throw new ArgumentNullException(nameof(combatManagerService));

        _combatManagerService.Creatures.SubscribeOnDisplay(UpdateCreatureList);
    }

    private void UpdateCreatureList(IEnumerable<CMCreature> creatures)
    {
        // 1. Remove deleted creatures
        var newCreaturesById = creatures.ToImmutableDictionary(x => x.ID);
        var creaturesToRemove = CreatureList.Where(x => !newCreaturesById.ContainsKey(x.Id)).ToList();
        foreach(var c in creaturesToRemove)
        {
            CreatureList.Remove(c);
        }

        // 2. Add new creatures
        var existingCreaturesById = CreatureList.ToImmutableDictionary(x => x.Id);
        var creaturesToAdd = creatures.Where(x => !existingCreaturesById.ContainsKey(x.ID)).ToList();
        foreach (var c in creaturesToAdd)
        {
            CreatureList.Add(new InitiativeCreatureModel(c));
        }

        // 3. Sort by initative
        var initiativeCreaturesById = CreatureList.ToImmutableDictionary(x => x.Id);
        var sortedCreatures = creatures.OrderByDescending(x => x.InitiativeCount).ToList();
        for (var i = 0; i < sortedCreatures.Count(); i++)
        {
            CreatureList.Move(CreatureList.IndexOf(initiativeCreaturesById[sortedCreatures.ElementAt(i).ID]), i);
        }

        // 4. Scroll to active
        var aa = sortedCreatures.TakeWhile(x => !x.IsActive).Count();
        App.DisplayWindow.DispatcherQueue.TryEnqueue(async () =>
        {
            // This is here because the carousel has to recieve the new list
            // before the index is set, or the events fire in the wrong order
            // and no animation plays.
            await Task.Delay(10);
            ActiveCreatureIndex = aa;
        });

        if (!CreatureList.Any())
            CreatureList.Add(new InitiativeCreatureModel());
    }
}
