using Cardfight_Vanguard_Plug_In;
using CFVanguard.Data;
using CFVanguard.Decks;
using IGamePlugInBase;

namespace CFVanguard.Formats
{
    internal class VPremium : CFBaseDBService, IFormat
    {
        const string selectCardString = "WHERE cards.formats & 2 == 2";

        public VPremium()
        {
            // Add Card Restrictions
            Restrictions.Add("Black Observe, Hamiel", 0);
            Restrictions.Add("Variants Hardleg", 0);
            Restrictions.Add("Violence Flanger", 0);
            Restrictions.Add("Bluish Flame Liberator, Percival (V Series)", 1);
            Restrictions.Add("Prudent Blue, Miep", 1);
            Restrictions.Add("PR♥ISM-Image, Rosa (V Series)", 0);

            ChoiceRestrictions.Add(new string[3] { "Dragheart, Luard (V Series)", "Skull Witch, Nemain (V Series)", "Black Sage, Charon (V Series)" });
            ChoiceRestrictions.Add(new string[2] { "Silver Singer, Cutire", "Mermaid Idol, Elly (V Series)" });
        }

        protected override long GetDeckType(CFCardArt cfCard, bool hasGyze)
        {
            var deckType = cfCard.ClanFightDecks;

            if (cfCard.Name.Contains("Я"))
            {
                deckType = deckType | 8192;
            }

            return deckType;
        }

        public string Name => "vformat";

        public string LongName => "V Premium";

        public byte[] Icon => Properties.Resources.V_Premium;

        public string Description => "Only cards with the 'V' symbol on the bottom left of the card are allowed in this format.";

        public IEnumerable<IDeck> Decks => new IDeck[2] { new StartingVanguard(), new MainDeck() };

        public IDeckBuilderService DeckBuilderService { get => this; }

        public override void InitializeService()
        {
            CFDBLoader.InitializeDataset(selectCardString);

            var clanList = new List<string>() { "Any", "None" };
            clanList.AddRange(CFDBLoader.ClanDecks!.Keys);
            clanList.Remove("Touken Ranbu");
            clanList.Remove("BanG Dream!");
            clanList.Remove("Record of Ragnarok");
            clanList.Remove("Monster Strike");
            clanList.Remove("SHAMAN KING");

            // Get SearchFields
            SearchFields = new SearchField[14]
            {
                new SearchField("name", "Name", 50),
                new SearchField("type", "Type", new string[7] {"Any", "Normal Unit", "Trigger Unit", "Normal Order", "Blitz Order", "Set Order", "Trigger Order"}, "Any"),
                new SearchField("subtype", "Subtype", 20),
                new SearchField("trigger", "Trigger", new string[6] { "Any", "None", "Critical", "Draw", "Heal", "Front"}, "Any"),
                new SearchField("rideskill", "Imaginary Gift", new string[5] { "Any", "None", "Accel", "Force", "Protect"}, "Any"),
                new SearchField("grade", "Grade", 0, 5),
                new SearchField("power", "Power", 0, 50000),
                new SearchField("shield", "Shield", 0, 50000),
                new SearchField("clan", "Clan", clanList.ToArray(), "Any"),
                new SearchField("clan", "Clan", clanList.ToArray(), "Any"),
                new SearchField("nation", "Nation", new string[8] { "Any", "None", "United Sanctuary", "Dragon Empire", "Dark Zone", "Star Gate", "Magallanica", "Zoo"}, "Any"),
                new SearchField("race", "Race", 50),
                new SearchField("effect", "Effect"),
                new SearchField("effect", "Effect")
            };
        }
    }
}
