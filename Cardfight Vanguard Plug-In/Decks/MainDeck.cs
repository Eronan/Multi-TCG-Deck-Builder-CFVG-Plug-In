using IGamePlugInBase;

namespace CFVanguard.Decks
{
    internal class MainDeck : IDeck
    {
        public string Name => "maindeck";

        public string Label => "Main Deck";

        public int ExpectedDeckSize => 49;

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
