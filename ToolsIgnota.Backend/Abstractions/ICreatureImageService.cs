using System;
using System.Collections.Generic;
using ToolsIgnota.Data.Models;

namespace ToolsIgnota.Data.Abstractions
{
    public interface ICreatureImageService
    {
        public void SaveCreatureImages(IEnumerable<CreatureImage> imageNamePairs);
        public IObservable<IEnumerable<CreatureImage>> GetCreatureImages();
    }
}
