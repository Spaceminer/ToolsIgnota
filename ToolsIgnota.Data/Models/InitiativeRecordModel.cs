using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace ToolsIgnota.UI.Models
{
    public partial class InitiativeRecordModel : ObservableObject
    {
        public Guid CreatureId { get; init; }

        [ObservableProperty]
        public string _image;

        [ObservableProperty]
        public string _displayName;

        [ObservableProperty]
        public bool _isHighlighted;
    }
}
