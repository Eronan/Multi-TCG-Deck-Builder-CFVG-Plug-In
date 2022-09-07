using IGamePlugInBase;

namespace CFVanguard
{
    internal class DStandard : IFormat
    {
        /*
        

        public ICard[] CardList => new ICard[0];

        

        public string DefaultDeckName(DeckBuilderCard card)
        {
            throw new NotImplementedException();
        }

        public string GetDetailedStats(Dictionary<string, IEnumerable<DeckBuilderCard>> decks)
        {
            throw new NotImplementedException();
        }

        public string GetStats(Dictionary<string, IEnumerable<DeckBuilderCard>> decks)
        {
            throw new NotImplementedException();
        }

        public bool ValidateMaximum(DeckBuilderCard card, Dictionary<string, IEnumerable<DeckBuilderCard>> decks)
        {
            throw new NotImplementedException();
        }
        */
        public string Name => "dformat";

        public string LongName => "Standard";

        public byte[] Icon => Properties.Resources.OverDress;

        public string Description => "Only cards with the 'D' symbol in the bottom left-hand corner are allowed in this format.";

        public IEnumerable<IDeck> Decks => new IDeck[0];
    }
}
