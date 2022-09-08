using CFVanguard.Decks;
using IGamePlugInBase;

namespace CFVanguard.Formats
{
    internal class VPremium : IFormat, IDeckBuilderService
    {
        public string Name => "vformat";

        public string LongName => "V Premium";

        public byte[] Icon => Properties.Resources.V_Premium;

        public string Description => "Only cards with the 'V' symbol on the bottom left of the card are allowed in this format.";

        public IEnumerable<IDeck> Decks => new IDeck[2] { new StartingVanguard(), new MainDeck() };

        public IDeckBuilderService DeckBuilderService { get => this; }

        public IEnumerable<DeckBuilderCardArt> CardList { get; set; } = Enumerable.Empty<DeckBuilderCardArt>();

        public IEnumerable<SearchField> SearchFields { get; set; } = new List<SearchField>();
    }
}
