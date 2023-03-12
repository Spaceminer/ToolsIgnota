using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace ToolsIgnota.Views;

[ObservableObject]
public sealed partial class InitiativeCreatureControl : UserControl
{
    public static readonly DependencyProperty CreatureImageProperty = 
        DependencyProperty.Register(
            nameof(CreatureImage), 
            typeof(string), 
            typeof(InitiativeCreatureControl), 
            new PropertyMetadata(".."));

    public static readonly DependencyProperty CreatureNameProperty = 
        DependencyProperty.Register(nameof(CreatureName), 
            typeof(string), 
            typeof(InitiativeCreatureControl), 
            new PropertyMetadata(""));

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ImageVisibility))]
    private string _creatureImage = "..";

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Initials))]
    private string _creatureName = "";

    public Visibility ImageVisibility => CreatureImage == ".." ? Visibility.Collapsed : Visibility.Visible;
    public Visibility InitialsVisibility => CreatureImage == ".." ? Visibility.Visible : Visibility.Collapsed;
    public string Initials => string.Join("", CreatureName.Split(" ", 2).Select(x => x.Substring(0, 1)));

    public InitiativeCreatureControl()
    {
        InitializeComponent();
    }
}
