using Cardfight_Vanguard_Plug_In;
using CFVanguard.Data;
using CFVanguard.Decks;
using IGamePlugInBase;

namespace CFVanguard.Formats
{
    internal class Premium : CFBaseDBService, IFormat
    {
        public Premium()
        {
            // Add Card Restrictions

        }

        public string Name => "premium";

        public string LongName => "Premium";

        public byte[] Icon => Properties.Resources.Premium;

        public string Description => "All unrestricted cards are allowed in this format.";

        public IEnumerable<IDeck> Decks => new IDeck[3] { new StartingVanguard(), new MainDeck(), new GZone() };

        public IDeckBuilderService DeckBuilderService => this;

        protected override long GetDeckType(CFCardArt cfCard, bool hasGyze)
        {
            var deckType = cfCard.ClanFightDecks;

            // Gyze Ruling
            if (hasGyze && cfCard.Races != null && cfCard.Races.Contains("Zeroth Dragon"))
            {
                return long.MaxValue;
            }
            
            // Blaster Dark Premium
            if (cfCard.Name == "Blaster Dark")
            {
                deckType = deckType | 1;
            }

            // Reverse Link Joker
            if (cfCard.Name.Contains("Я"))
            {
                deckType = deckType | 8192;
            }

            return deckType;
        }

        public override void InitializeService()
        {
            CFDBLoader.InitializeDataset("");

            var clanList = new List<string>() { "Any", "None", "Cray Elemental" };
            clanList.AddRange(CFDBLoader.ClanDecks!.Keys);

            // Get SearchFields
            SearchFields = new SearchField[16]
            {
                new SearchField("name", "Name", 50),
                new SearchField("type", "Type", new string[7] {"Any", "Normal Unit", "Trigger Unit", "Normal Order", "Blitz Order", "Set Order", "Trigger Order"}, "Any"),
                new SearchField("subtype", "Subtype", 20),
                new SearchField("trigger", "Trigger", new string[7] { "Any", "None", "Critical", "Draw", "Heal", "Stand", "Front"}, "Any"),
                new SearchField("rideskill", "Ride Skill", new string[6] { "Any", "None", "Accel", "Force", "Protect", "Persona Ride"}, "Any"),
                new SearchField("grade", "Grade", 0, 5),
                new SearchField("power", "Power", 0, 50000),
                new SearchField("shield", "Shield", 0, 50000),
                new SearchField("clan", "Clan", clanList.ToArray(), "Any"),
                new SearchField("clan", "Clan", clanList.ToArray(), "Any"),
                new SearchField("clan", "Clan", clanList.ToArray(), "Any"),
                new SearchField("nation", "Nation", new string[13] { "Any", "None", "United Sanctuary", "Keter Sanctuary", "Dragon Empire", "Dark Zone", "Dark States", "Star Gate", "Brandt Gate", "Magallanica", "Zoo", "Stoicheia", "Lyrical Monasterio"}, "Any"),
                new SearchField("race", "Race", 50),
                new SearchField("effect", "Effect"),
                new SearchField("effect", "Effect"),
                new SearchField("format", "Format", new string[4] { "Any", "Premium Only", "V Series", "D Series" }, "Any")
            };
        }
    }
}
