using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Controls;
using ToolsIgnota.Data.Models;

namespace ToolsIgnota.Models;

public partial class InitiativeCreatureModel : ObservableObject
{
    [ObservableProperty] private string _creatureImage = "..";
    [ObservableProperty] private string _creatureName = "?";

    public Guid Id { get; set; } = Guid.NewGuid();

    public InitiativeCreatureModel() { }

    public InitiativeCreatureModel(CMCreature creature)
    {
        Id = creature.ID;
        _creatureName = creature.Name;
    }
}
