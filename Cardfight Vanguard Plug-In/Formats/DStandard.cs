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
            SearchFields = new SearchField[14]
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
                new SearchField("effect", "Effect"),
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

        public override bool ValidateMaximum(DeckBuilderCard card, Dictionary<string, IEnumerable<DeckBuilderCard>> decks)
        {
            CFCardArt? cfCard = card as CFCardArt ?? CFDBLoader.GetCard(card);
            if (cfCard == null) { return true; }
            int maximum = SpecialMaximums.GetValueOrDefault(cfCard.Name, 4);
            int copies = 0;
            int sentinel = cfCard.Subtype == "Sentinel" ? 1 : 0;
            int trigger = cfCard.CardType.IsTrigger() ? 1 : 0;
            int nontrigger = cfCard.CardType.IsMainNonTrigger() ? 1 : 0;
            int over = cfCard.Trigger == "Over" ? 1 : 0;
            int heal = cfCard.Trigger == "Heal" ? 1 : 0;
            int othertrigger = cfCard.Trigger == "Front" || cfCard.Trigger == "Draw" || cfCard.Trigger == "Critical" ? 1 : 0;

            // Clan Fight/Nation Fight Restrictions
            long deckType = GetDeckType(cfCard, false);
            if (deckType == 0) { return true; }

            // Card Restrictions
            int restriction = Restrictions.GetValueOrDefault(cfCard.CardID, -1);
            if (restriction == 0) { return true; } // Banned Cards
            else if (restriction == 1) { maximum = 1; } // Restricted Cards

            // Choice Restrictions
            IEnumerable<string>? ChoiceRestrict = ChoiceRestrictions.FirstOrDefault(list => list.Contains(cfCard.CardID));

            foreach (var keyValue in decks)
            {
                var cfDecklist = keyValue.Value.Select(crd => CFDBLoader.GetCard(crd));

                foreach (var cfDeckCard in cfDecklist)
                {
                    if (cfDeckCard == null) { return true; }

                    // Card Restrictions
                    if (restriction == 1)
                    {
                        int deckCardRestriction = Restrictions.GetValueOrDefault(cfDeckCard.CardID, -1);
                        if (deckCardRestriction == 1) { return true; }
                    }

                    // Choice Restriction
                    if (ChoiceRestrict != null && cfCard.CardID != cfDeckCard.CardID && ChoiceRestrict.Contains(cfDeckCard.CardID)) { return true; }

                    // Make sure Cards are the same Clan for Clan Fight Formats
                    deckType = deckType & GetDeckType(cfDeckCard, false);
                    if (deckType == 0) { return true; }

                    // Increment Name Copies
                    if (cfDeckCard.Name == cfCard.Name)
                    {
                        copies++;
                        if (copies >= maximum)
                        {
                            return true;
                        }
                    }

                    // Only 4 Sentinels
                    if (sentinel > 0 && cfDeckCard.Subtype == "Sentinel")
                    {
                        sentinel++;

                        if (sentinel > 4) { return true; }
                    }

                    // Only 34 Non-Triggger Units in the Main Deck
                    if (nontrigger > 0 && cfDeckCard.CardType.IsMainNonTrigger())
                    {
                        nontrigger++;

                        if (nontrigger > 34) { return true; }
                    }

                    // Only 16 Triggers
                    if (trigger > 0 && cfDeckCard.CardType.IsTrigger())
                    {
                        trigger++;

                        if (trigger > 16) { return true; }
                    }

                    // Only 4 Heal Triggers
                    if (heal > 0 && cfDeckCard.Trigger == "Heal")
                    {
                        heal++;

                        if (heal > 4) { return true; }
                    }

                    // Only 1 Over Trigger or Zeroth Dragon of Zenith Peak, Ultima Special Restriction
                    if (over > 0 && cfDeckCard.Trigger == "Over")
                    {
                        return true;
                    }

                    if (othertrigger > 0 && cfCard.Trigger == cfDeckCard.Trigger)
                    {
                        othertrigger++;

                        if (othertrigger > 8) { return true; }
                    }
                }
            }

            return false;
        }
    }
}
