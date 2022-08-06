using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolsIgnota.Backend.Models;

namespace ToolsIgnota.UI.Models
{
    public class CreatureImageModel : ObservableObject
    {
        private readonly CreatureImage _data;
        public Guid Id { get; set; } = Guid.NewGuid();

        public CreatureImageModel(CreatureImage data)
        {
            _data = data;
        }

        public string Name
        {
            get => _data.Name;
            set => SetProperty(_data.Name, value, _data, (x, y) => x.Name = y);
        }

        public string Image
        {
            get => _data.Image;
            set => SetProperty(_data.Image, value, _data, (x, y) => x.Image = y);
        }
    }
}
