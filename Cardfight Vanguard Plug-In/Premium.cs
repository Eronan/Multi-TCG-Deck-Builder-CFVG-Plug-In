using IGamePlugInBase;

namespace CFVanguard
{
    internal class Premium : IFormat
    {
        public string Name => "premium";

        public string LongName => "Premium";

        public byte[] Icon => Properties.Resources.Premium;

        public string Description => "All unrestricted cards are allowed in this format.";

        public ICard[] CardList => new ICard[0];

        public IDeck[] Decks => new IDeck[0];

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
    }
}
