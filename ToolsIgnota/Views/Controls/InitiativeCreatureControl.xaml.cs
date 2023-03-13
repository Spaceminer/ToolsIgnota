using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using ToolsIgnota.Core.Helpers;

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
    [NotifyPropertyChangedFor(nameof(BorderColor))]
    private string _creatureName = "";

    public Visibility ImageVisibility => CreatureImage == ".." ? Visibility.Collapsed : Visibility.Visible;
    public Visibility InitialsVisibility => CreatureImage == ".." ? Visibility.Visible : Visibility.Collapsed;
    public string Initials => CreatureName.Initials();
    public Brush BorderColor
    {
        get
        {
            var name = CreatureName.ToLower();
            if (name.Contains("red"))
                return new SolidColorBrush(Colors.Red);
            if (name.Contains("blue"))
                return new SolidColorBrush(Colors.Blue);
            if (name.Contains("green"))
                return new SolidColorBrush(Colors.Green);
            if (name.Contains("yellow"))
                return new SolidColorBrush(Colors.Yellow);
            return new SolidColorBrush(Colors.Goldenrod);
        }
    }

    public InitiativeCreatureControl()
    {
        InitializeComponent();
    }

    
}
