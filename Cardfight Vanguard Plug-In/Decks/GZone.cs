using IGamePlugInBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFVanguard.Decks
{
    internal class GZone : IDeck
    {
        public const string GZoneName = "gzone";
        public string Name => GZoneName;

        public string Label => "G Zone";

        public int ExpectedDeckSize => throw new NotImplementedException();

        public bool ValidateAdd(DeckBuilderCard card, IEnumerable<DeckBuilderCard> deck)
        {
            throw new NotImplementedException();
        }

        public string[] ValidateDeck(IEnumerable<DeckBuilderCard> deck)
        {
            throw new NotImplementedException();
        }
    }
}
