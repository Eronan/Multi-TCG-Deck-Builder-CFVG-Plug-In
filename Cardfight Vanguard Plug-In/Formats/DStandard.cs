using Cardfight_Vanguard_Plug_In;
using CFVanguard.Data;
using CFVanguard.Decks;
using IGamePlugInBase;

namespace CFVanguard.Formats
{
    internal class DStandard : CFBaseDBService, IFormat
    {
        const string selectCardString = "WHERE cards.formats & 4 == 4";

        public DStandard()
        {
            Restrictions.Add("Dark Strain Dragon", 1);

            ChoiceRestrictions.Add(new string[2] { "Sylvan Horned Beast Emperor, Magnolia Elder", "Blue Artillery Dragon, Inlet Pulse Dragon" });
        }

        public string Name => "dformat";

        public string LongName => "Standard";

        public byte[] Icon => Properties.Resources.OverDress;

        public string Description => "Only cards with the 'D' symbol in the bottom left-hand corner are allowed in this format.";

        public IEnumerable<IDeck> Decks => new IDeck[3] { new RideDeck(), new MainDeck(4), new GZone() };

        public IDeckBuilderService DeckBuilderService { get => this; }

        protected override long GetDeckType(CFCardArt cfCard, bool hasGyze)
        {
            var deckType = cfCard.OverDressDecks;

            if (hasGyze && cfCard.Races != null && cfCard.Races.Contains("Zeroth Dragon"))
            {
                deckType = long.MaxValue;
            }

            return deckType;
        }

        public override void InitializeService()
        {
            CFDBLoader.InitializeDataset(selectCardString);

            var nationList = new List<string>() { "Any", "None" };
            nationList.AddRange(CFDBLoader.NationDecks!.Keys);

            // Get SearchFields
            SearchFields = new SearchField[13]
            {
                new SearchField("name", "Name", 50),
                new SearchField("type", "Type", new string[8] {"Any", "Normal Unit", "Trigger Unit", "G Unit", "Normal Order", "Blitz Order", "Set Order", "Trigger Order"}, "Any"),
                new SearchField("subtype", "Subtype", 20),
                new SearchField("trigger", "Trigger", new string[7] { "Any", "None", "Critical", "Draw", "Heal", "Front", "Over"}, "Any"),
                new SearchField("rideskill", "Imaginary Gift", new string[5] { "Any", "None", "Accel", "Force", "Protect"}, "Any"),
                new SearchField("grade", "Grade", 0, 5),
                new SearchField("power", "Power", 0, 50000),
                new SearchField("shield", "Shield", 0, 50000),
                new SearchField("dnation", "Nation", nationList.ToArray(), "Any"),
                new SearchField("dnation", "Nation", nationList.ToArray(), "Any"),
                new SearchField("dclan", "Clan", 20),
                new SearchField("race", "Race", 50),
                new SearchField("effect", "Effect")
            };
        }

        public override string DefaultDeckName(DeckBuilderCard card)
        {
            CFCardArt? cfCard = CFDBLoader.GetCard(card);
            if (cfCard == null) return MainDeck.MainDeckName;

            if (cfCard.CardType == CFType.GUnit)
            {
                return GZone.GZoneName;
            }

            if (cfCard.CardType == CFType.NormalUnit && cfCard.Grade == 0)
            {
                return RideDeck.RideDeckName;
            }

            return MainDeck.MainDeckName;
        }
    }
}
