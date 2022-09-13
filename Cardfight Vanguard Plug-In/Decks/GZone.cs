using Cardfight_Vanguard_Plug_In;
using CFVanguard.Data;
using IGamePlugInBase;

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
            if (deck.Count() >= 16) { return false; }
            CFCardArt? cfCard = CFDBLoader.GetCard(card);
            if (cfCard == null) { return false; }
            return cfCard.CardType == CFType.GUnit;
        }

        public string[] ValidateDeck(IEnumerable<DeckBuilderCard> deck)
        {
            List<string> errors = new List<string>();
            if (deck.Count() > 16) { errors.Add("You can only have 16 or less G Units in your G Zone."); }
            if (deck.Select(CFDBLoader.GetCard).Any(crd => crd == null || crd.CardType != CFType.GUnit)) { errors.Add("Your G Zone can only contain G Units."); }
            return errors.ToArray();
        }
    }
}
