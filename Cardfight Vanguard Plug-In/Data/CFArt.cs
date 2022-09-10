using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IGamePlugInBase;

namespace CFVanguard.Data
{
    public class CFArt
    {
        public string Id { get; private set; }

        public string ImageLocation { get; private set; }

        private string flavorText { get; set; }

        private string illustrator { get; set; }

        private string downloadLink { get; set; }

        public CardArtOrientation ArtOrientation { get; private set; }

        public CFArt(string id, string imageLocation, string flavorText, string illustrator)
        {
            Id = id;
            ImageLocation = imageLocation;
            this.flavorText = flavorText;
            this.illustrator = illustrator;
            ArtOrientation = CardArtOrientation.Portrait;
        }
    }
}
