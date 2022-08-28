using IGamePlugInBase;

namespace CFVanguard
{
    internal class VPremium : IFormat
    {
        public string Name => "vformat";

        public string LongName => "V Premium";

        public byte[] Icon => Properties.Resources.V_Premium;

        public string Description => "Only cards with the 'V' symbol on the bottom left of the card are allowed in this format.";

        public ICard[] CardList => new ICard[0];

        public IDeck[] Decks => new IDeck[2] { new StartingVanguard(), new MainDeck() };

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
            return "";
        }

        public bool ValidateMaximum(DeckBuilderCard card, Dictionary<string, IEnumerable<DeckBuilderCard>> decks)
        {
            throw new NotImplementedException();
        }
    }
}
