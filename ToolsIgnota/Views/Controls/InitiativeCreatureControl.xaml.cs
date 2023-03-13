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
    [NotifyPropertyChangedFor(nameof(InitialsVisibility))]
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
            if (name.Contains("blue"))
                return new SolidColorBrush(Colors.DarkBlue);
            if (name.Contains("green"))
                return new SolidColorBrush(Colors.DarkGreen);
            if (name.Contains("yellow"))
                return new SolidColorBrush(Colors.Yellow);
            if (name.Contains("red"))
                return new SolidColorBrush(Colors.DarkRed);
            if (name.Contains("pink"))
                return new SolidColorBrush(Colors.DeepPink);
            if (name.Contains("orange"))
                return new SolidColorBrush(Colors.DarkOrange);
            if (name.Contains("purple"))
                return new SolidColorBrush(Colors.Purple);
            return new SolidColorBrush(Colors.DarkGoldenrod);
        }
    }

    public InitiativeCreatureControl()
    {
        InitializeComponent();
    }

    
}
