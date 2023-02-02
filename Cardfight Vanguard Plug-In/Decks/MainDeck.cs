using Cardfight_Vanguard_Plug_In;
using CFVanguard.Data;
using IGamePlugInBase;

namespace CFVanguard.Decks
{
    internal class MainDeck : IDeck
    {
        public const string MainDeckName = "maindeck";

        private int otherMainDeckCount;

        public MainDeck(int otherMainDeckCount = 1)
        {
            this.otherMainDeckCount = otherMainDeckCount;
        }

        public string Name => MainDeckName;

        public string Label => "Main Deck";

        public int ExpectedDeckSize => 49;

        public bool ValidateAdd(DeckBuilderCard card, IEnumerable<DeckBuilderCard> deck)
        {
            if (deck.Count() >= 50 - otherMainDeckCount) { return false; }
            CFCardArt? cfCard = CFDBLoader.GetCard(card);
            if (cfCard == null) { return false; }
            return cfCard.CardType != CFType.GUnit;
        }

        public string[] ValidateDeck(IEnumerable<DeckBuilderCard> deck)
        {
            if (deck.Count() != 50 - otherMainDeckCount)
            {
                return new string[1] { "Your deck must include 49 cards." };
            }

            var cfDecklist = deck.Select(crd => CFDBLoader.GetCard(crd));

            int triggerCount = 0;
            int nonTriggerCount = 0;
            foreach (var card in cfDecklist)
            {
                if (card == null) { return new string[1] { "There was a problem reading the Deck." }; }

                if (card.CardType.IsTrigger()) { triggerCount++; }
                else { nonTriggerCount++; }
            }

            if (triggerCount < 16 - otherMainDeckCount || triggerCount > 16)
            {
                return new string[1] { "You must have 16 Triggers in your Deck." };
            }
            if (nonTriggerCount < 34 - otherMainDeckCount || nonTriggerCount > 34)
            {
                return new string[1] { "You must have 16 Triggers in your Deck." };
            }

            return new string[0];
        }
    }
}
