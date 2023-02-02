using Cardfight_Vanguard_Plug_In;
using CFVanguard.Data;
using IGamePlugInBase;

namespace CFVanguard.Decks
{
    internal class StartingVanguard : IDeck
    {
        public const string StartingVanguardName = "startvg";

        public StartingVanguard()
        {
            RestrictedSVG = Enumerable.Empty<string>();
        }

        public StartingVanguard(IEnumerable<string> restrictedSVG)
        {
            RestrictedSVG = restrictedSVG;
        }

        public string Name => StartingVanguardName;

        public string Label => "Starting Vanguard";

        public int ExpectedDeckSize => 1;

        public IEnumerable<string> RestrictedSVG { get; }

        public bool ValidateAdd(DeckBuilderCard card, IEnumerable<DeckBuilderCard> deck)
        {
            if (RestrictedSVG.Contains(card.CardID)) { return false; }
            if (deck.Count() >= 1) { return false; }
            CFCardArt? cfCard = CFDBLoader.GetCard(card);
            return cfCard != null &&
                cfCard.Grade == 0 && cfCard.CardType.IsMainUnit();
        }

        public string[] ValidateDeck(IEnumerable<DeckBuilderCard> deck)
        {
            if (deck.Count() != 1) { return new string[1] { "You must have only 1 Starting Vanguard." }; }

            var firstCard = deck.First();
            CFCardArt? cfCard = CFDBLoader.GetCard(firstCard);
            if (cfCard != null && cfCard.Grade != 0 && (cfCard.CardType != CFType.NormalUnit && cfCard.CardType != CFType.TriggerUnit))
            {
                return new string[1] { "Starting Vanguard must be a Grade 0 Unit." };
            }

            return new string[0];
        }
    }
}
