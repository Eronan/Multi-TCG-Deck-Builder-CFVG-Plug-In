using IGamePlugInBase;

namespace CFVanguard
{
    internal class StartingVanguard : IDeck
    {
        public string Name => "startvg";

        public string Label => "Starting Vanguard";

        public int ExpectedDeckSize => 1;

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
