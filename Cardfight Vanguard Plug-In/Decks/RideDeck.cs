using Cardfight_Vanguard_Plug_In;
using CFVanguard.Data;
using IGamePlugInBase;

namespace CFVanguard.Decks
{
    internal class RideDeck : IDeck
    {
        public const string RideDeckName = "ridedeck";

        public RideDeck()
        {
            RestrictedSVG = Enumerable.Empty<string>();
        }

        public RideDeck(IEnumerable<string> restrictedSVG)
        {
            RestrictedSVG = restrictedSVG;
        }

        public string Name => RideDeckName;

        public string Label => "Ride Deck";

        public int ExpectedDeckSize => throw new NotImplementedException();

        public IEnumerable<string> RestrictedSVG { get; }

        public bool ValidateAdd(DeckBuilderCard card, IEnumerable<DeckBuilderCard> deck)
        {
            if (RestrictedSVG.Contains(card.CardID)) { return false; }
            if (deck.Count() >= 4) { return false; }
            CFCardArt? cfCard = CFDBLoader.GetCard(card);

            return cfCard != null && cfCard.Grade <= 3 && cfCard.CardType.IsMainUnit() &&
                !deck.Any(crd => CFDBLoader.GetCard(crd)!.Grade == cfCard.Grade);
        }

        public string[] ValidateDeck(IEnumerable<DeckBuilderCard> deck)
        {
            if (deck.Count() != 4) { return new string[1] { "You must have 4 cards in your Ride Deck." }; }
            var cfDeck = deck.Select(CFDBLoader.GetCard);
            if (cfDeck.Count(crd => crd != null && crd.Grade == 0) != 1) { return new string[1] { "The Ride Deck must contain exactly 1 each of a Grade 0, Grade 1, Grade 2, Grade 3." }; }
            if (cfDeck.Count(crd => crd != null && crd.Grade == 1) != 1) { return new string[1] { "The Ride Deck must contain exactly 1 each of a Grade 0, Grade 1, Grade 2, Grade 3." }; }
            if (cfDeck.Count(crd => crd != null && crd.Grade == 2) != 1) { return new string[1] { "The Ride Deck must contain exactly 1 each of a Grade 0, Grade 1, Grade 2, Grade 3." }; }
            if (cfDeck.Count(crd => crd != null && crd.Grade == 3) != 1) { return new string[1] { "The Ride Deck must contain exactly 1 each of a Grade 0, Grade 1, Grade 2, Grade 3." }; }
            return new string[0];
        }
    }
}
