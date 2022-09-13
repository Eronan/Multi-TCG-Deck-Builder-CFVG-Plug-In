using Cardfight_Vanguard_Plug_In;
using CFVanguard.Data;
using CFVanguard.Decks;
using IGamePlugInBase;
using System.Text;

namespace CFVanguard.Formats
{
    internal abstract class CFBaseDBService : IDeckBuilderService
    {
        protected Dictionary<string, int> SpecialMaximums = new Dictionary<string, int>();

        protected IList<IEnumerable<string>> ChoiceRestrictions = new List<IEnumerable<string>>();

        protected Dictionary<string, int> Restrictions = new Dictionary<string, int>();

        protected virtual long GetDeckType(CFCardArt cfCard, bool hasGyze)
        {
            return cfCard.ClanFightDecks;
        }

        protected virtual bool IsNeonGyze(DeckBuilderCard card)
        {
            var cfCard = CFDBLoader.GetCard(card);
            return cfCard != null && cfCard.Name == "Neon Gyze";
        }

        protected virtual bool searchFilter(DeckBuilderCard card, IEnumerable<SearchField> searchFields)
        {
            CFCardArt? cfCard = CFDBLoader.GetCard(card);
            if (cfCard == null) { return false; }

            foreach (var field in searchFields)
            {
                switch (field.Id)
                {
                    case "name":
                        if (!cfCard.Name.Contains(field.Value) ^ field.Comparison == SearchFieldComparison.NotEquals) return false;
                        break;
                    case "type":
                        switch (field.Value)
                        {
                            case "Any":
                                continue;
                            case "Normal Unit":
                                if (cfCard.CardType != CFType.NormalUnit ^ field.Comparison == SearchFieldComparison.NotEquals) { return false; }
                                break;
                            case "Trigger Unit":
                                if (cfCard.CardType != CFType.TriggerUnit ^ field.Comparison == SearchFieldComparison.NotEquals) { return false; }
                                break;
                            case "G Unit":
                                if (cfCard.CardType != CFType.GUnit ^ field.Comparison == SearchFieldComparison.NotEquals) { return false; }
                                break;
                            case "Normal Order":
                                if (cfCard.CardType != CFType.NormalOrder ^ field.Comparison == SearchFieldComparison.NotEquals) { return false; }
                                break;
                            case "Blitz Order":
                                if (cfCard.CardType != CFType.BlitzOrder ^ field.Comparison == SearchFieldComparison.NotEquals) { return false; }
                                break;
                            case "Set Order":
                                if (cfCard.CardType != CFType.SetOrder ^ field.Comparison == SearchFieldComparison.NotEquals) { return false; }
                                break;
                            case "Trigger Order":
                                if (cfCard.CardType != CFType.TriggerOrder ^ field.Comparison == SearchFieldComparison.NotEquals) { return false; }
                                break;
                        }
                        break;
                    case "subtype":
                        if (field.Value == "None" && (!string.IsNullOrEmpty(cfCard.Subtype) ^ field.Comparison == SearchFieldComparison.NotEquals)) { return false; }
                        else if (field.Value == "None") { continue; }
                        else if (cfCard.Subtype == null || !cfCard.Subtype.Contains(field.Value)) return false;
                        break;
                    case "trigger":
                        if (field.Value == "Any") { continue; }
                        else if (field.Value == "None" && (!string.IsNullOrEmpty(cfCard.Trigger) ^ field.Comparison == SearchFieldComparison.NotEquals)) { return false; }
                        else if (field.Value == "None") { continue; }
                        else if (field.Value != cfCard.Trigger ^ field.Comparison == SearchFieldComparison.NotEquals) { return false; }
                        break;
                    case "rideskill":
                        if (field.Value == "Any") { continue; }
                        else if (field.Value == "None" && (!string.IsNullOrEmpty(cfCard.RideSkill) ^ field.Comparison == SearchFieldComparison.NotEquals)) { return false; }
                        else if (field.Value == "None") { continue; }
                        else if (field.Value != cfCard.RideSkill ^ field.Comparison == SearchFieldComparison.NotEquals) { return false; }
                        break;
                    case "grade":
                        switch (field.Comparison)
                        {
                            case SearchFieldComparison.Equals:
                                if (cfCard.Grade != int.Parse(field.Value)) { return false; }
                                break;
                            case SearchFieldComparison.NotEquals:
                                if (cfCard.Grade == int.Parse(field.Value)) { return false; }
                                break;
                            case SearchFieldComparison.LessThan:
                                if (cfCard.Grade > int.Parse(field.Value)) { return false; }
                                break;
                            case SearchFieldComparison.GreaterThan:
                                if (cfCard.Grade < int.Parse(field.Value)) { return false; }
                                break;
                        }
                        break;
                    case "power":
                        switch (field.Comparison)
                        {
                            case SearchFieldComparison.Equals:
                                if (cfCard.Power == null || cfCard.Power != int.Parse(field.Value)) { return false; }
                                break;
                            case SearchFieldComparison.NotEquals:
                                if (cfCard.Power != null && cfCard.Power == int.Parse(field.Value)) { return false; }
                                break;
                            case SearchFieldComparison.LessThan:
                                if (cfCard.Power == null || cfCard.Power > int.Parse(field.Value)) { return false; }
                                break;
                            case SearchFieldComparison.GreaterThan:
                                if (cfCard.Power == null || cfCard.Power < int.Parse(field.Value)) { return false; }
                                break;
                        }
                        break;
                    case "shield":
                        switch (field.Comparison)
                        {
                            case SearchFieldComparison.Equals:
                                if (cfCard.Shield == null || cfCard.Shield != int.Parse(field.Value)) { return false; }
                                break;
                            case SearchFieldComparison.NotEquals:
                                if (cfCard.Shield != null && cfCard.Shield == int.Parse(field.Value)) { return false; }
                                break;
                            case SearchFieldComparison.LessThan:
                                if (cfCard.Shield == null || cfCard.Shield > int.Parse(field.Value)) { return false; }
                                break;
                            case SearchFieldComparison.GreaterThan:
                                if (cfCard.Shield == null || cfCard.Shield < int.Parse(field.Value)) { return false; }
                                break;
                        }
                        break;
                    case "clan":
                        if (field.Value == "Any") { continue; }
                        else if (field.Value == "None" && (!string.IsNullOrEmpty(cfCard.Clans) ^ field.Comparison == SearchFieldComparison.NotEquals)) { return false; }
                        else if (field.Value == "Cray Elemental" && ((cfCard.Clans == null || !cfCard.Clans.Contains("Cray Elemental")) ^ field.Comparison == SearchFieldComparison.NotEquals)) { return false; }
                        else if (field.Value != "None" && field.Value != "Cray Elemental")
                        {
                            long binary = CFDBLoader.ClanDecks![field.Value];
                            if ((binary & cfCard.ClanFightDecks) == 0 ^ field.Comparison == SearchFieldComparison.NotEquals) { return false; }
                        }
                        break;
                    case "dclan":
                        if (field.Value == "None" && (!string.IsNullOrEmpty(cfCard.Clans) ^ field.Comparison == SearchFieldComparison.NotEquals)) { return false; }
                        else if (field.Value == "None") { continue; }
                        else if ((cfCard.Clans == null || !cfCard.Clans.Contains(field.Value)) ^ field.Comparison == SearchFieldComparison.NotEquals) { return false; }
                        break;
                    case "nation":
                        if (field.Value == "Any") { continue; }
                        else if (field.Value == "None" && (!string.IsNullOrEmpty(cfCard.Nations) ^ field.Comparison == SearchFieldComparison.NotEquals)) { return false; }
                        else if (field.Value == "None") { continue; }
                        else if ((cfCard.Nations == null || !cfCard.Nations.Contains(field.Value)) ^ field.Comparison == SearchFieldComparison.NotEquals) { return false; }
                        break;
                    case "dnation":
                        if (field.Value == "Any") { continue; }
                        else if (field.Value == "None" && (!string.IsNullOrEmpty(cfCard.Nations) ^ field.Comparison == SearchFieldComparison.NotEquals)) { return false; }
                        else if (field.Value != "None")
                        {
                            long binary = CFDBLoader.NationDecks![field.Value];
                            if ((binary & cfCard.OverDressDecks) == 0 ^ field.Comparison == SearchFieldComparison.NotEquals) { return false; }
                        }
                        break;
                    case "race":
                        if (field.Value == "Any") { continue; }
                        else if (field.Value == "None" && (!string.IsNullOrEmpty(cfCard.Races) ^ field.Comparison == SearchFieldComparison.NotEquals)) { return false; }
                        else if (field.Value == "None") { continue; }
                        else if (field.Value != "None" && (cfCard.Races == null || !cfCard.Races.Contains(field.Value)) ^ field.Comparison == SearchFieldComparison.NotEquals) { return false; }
                        break;
                    case "effect":
                        if (field.Value == "None" && (!string.IsNullOrEmpty(cfCard.Effects) ^ field.Comparison == SearchFieldComparison.NotEquals)) { return false; }
                        else if (field.Value == "None") { continue; }
                        else if ((cfCard.Effects == null || !cfCard.Effects.Contains(field.Value)) ^ field.Comparison == SearchFieldComparison.NotEquals) return false;
                        break;
                    case "format":
                        switch (field.Value)
                        {
                            case "Any":
                                continue;
                            case "Premium Only":
                                if (cfCard.Format != Format.Premium ^ field.Comparison == SearchFieldComparison.NotEquals) { return false; }
                                break;
                            case "V Series":
                                if (!cfCard.Format.HasFlag(Format.VPremium) ^ field.Comparison == SearchFieldComparison.NotEquals) { return false; }
                                break;
                            case "D Series":
                                if (!cfCard.Format.HasFlag(Format.DStandard) ^ field.Comparison == SearchFieldComparison.NotEquals) { return false; }
                                break;

                        }
                        break;
                }
            }

            return true;
        }

        public CFBaseDBService()
        {
            SpecialMaximums.Add("Ancestor Soul of the Tao Family", 16);
            SpecialMaximums.Add("Battle of the Abyss", 8);
            SpecialMaximums.Add("Fighting Each Other is the Battle of Men", 8);
            SpecialMaximums.Add("Integrity Flower Maiden, Ritaleena", 16);
            SpecialMaximums.Add("Mike the Shipwright", 16);
            SpecialMaximums.Add("Neatness Meteor Showr", 16);
            SpecialMaximums.Add("Red - leaf Dragon", 16);
            SpecialMaximums.Add("Ryu's Buddies", 9);
            SpecialMaximums.Add("The Strongest by Nature", 8);
            SpecialMaximums.Add("Trouble Varidol, Pressiv", 16);
            SpecialMaximums.Add("Transcend Idol, Aqua", 6);
            SpecialMaximums.Add("Elementaria Sanctitude", 1);
        }

        public IEnumerable<DeckBuilderCardArt> CardList { get => CFDBLoader.CardData; }

        public IEnumerable<SearchField> SearchFields { get; internal set; } = Enumerable.Empty<SearchField>();

        public virtual void InitializeService()
        {
            CFDBLoader.InitializeDataset("");

            // Get SearchFields
            SearchFields = new SearchField[10]
            {
                new SearchField("name", "Name", 50),
                new SearchField("type", "Type", System.Enum.GetNames(typeof(CFType))),
                new SearchField("subtype", "Subtype", 20),
                new SearchField("trigger", "Trigger", new string[6] { "Any", "None", "Critical", "Draw", "Heal", "Front"}),
                new SearchField("grade", "Grade", 0, 5),
                new SearchField("power", "Power", 0, 50000),
                new SearchField("shield", "Shield", 0, 50000),
                new SearchField("clan", "Clan", CFDBLoader.ClanDecks!.Keys.ToArray()),
                new SearchField("race", "Race", 50),
                new SearchField("effect", "Effect")
            };
        }

        public virtual bool ValidateMaximum(DeckBuilderCard card, Dictionary<string, IEnumerable<DeckBuilderCard>> decks)
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
            bool hasGyze = decks.Values.Any(dck => dck.Any(IsNeonGyze));

            // Clan Fight/Nation Fight Restrictions
            long deckType = GetDeckType(cfCard, hasGyze);
            if (deckType == 0) { return true; }

            // Card Restrictions
            int restriction = Restrictions.GetValueOrDefault(cfCard.CardID, -1);
            if (restriction == 0) { return true; } // Banned Cards
            else if (restriction == 1) { maximum = 1; } // Restricted Cards

            // Choice Restrictions
            IEnumerable<string>? ChoiceRestrict = ChoiceRestrictions.FirstOrDefault(list => list.Contains(cfCard.CardID));

            // Special "Transcend Idol, Aqua" Effect
            bool topIdolAqua = cfCard.Name == "Transcend Idol, Aqua";

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
                    deckType = deckType & GetDeckType(cfDeckCard, hasGyze);
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
                        // Special "Transcend Idol, Aqua" Effect
                        if (topIdolAqua ? cfDeckCard.Name != "Transcend Idol, Aqua" : cfDeckCard.Name == "Transcend Idol, Aqua")
                        {
                            return true;
                        }

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
                    if (over > 0 && (cfDeckCard.Trigger == "Over" || cfDeckCard.CardID == "Zeroth Dragon of Zenith Peak, Ultima"))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public virtual string GetStats(Dictionary<string, IEnumerable<DeckBuilderCard>> decks)
        {
            int sentinel = 0;
            int trigger = 0;
            int order = 0;
            int grade0unit = 0;
            int grade1unit = 0;
            int grade2unit = 0;
            int grade3unit = 0;
            foreach (var cfDeck in decks)
            {
                foreach (var card in cfDeck.Value)
                {
                    CFCardArt? cfCard = CFDBLoader.GetCard(card);
                    if (cfCard == null) { continue; }

                    if (cfCard.Subtype == "Sentinel")
                    {
                        sentinel++;
                    }

                    if (cfCard.CardType.IsTrigger())
                    {
                        trigger++;
                    }

                    if (cfCard.CardType.IsOrder())
                    {
                        order++;
                    }

                    if (cfCard.CardType.IsUnit())
                    {
                        switch (cfCard.Grade)
                        {
                            case 0:
                                grade0unit++;
                                break;
                            case 1:
                                grade1unit++;
                                break;
                            case 2:
                                grade2unit++;
                                break;
                            case 3:
                                grade3unit++;
                                break;
                        }
                    }
                }
            }

            var stringBuilder = new StringBuilder();
            stringBuilder.Append("Grade 0: ");
            stringBuilder.Append(grade0unit);
            stringBuilder.Append("\t");
            stringBuilder.Append("Triggers: ");
            stringBuilder.Append(trigger);
            stringBuilder.Append("\t");
            stringBuilder.Append("Sentinels: ");
            stringBuilder.Append(sentinel);
            stringBuilder.AppendLine();
            stringBuilder.Append("Grade 1: ");
            stringBuilder.Append(grade1unit);
            stringBuilder.Append("\t");
            stringBuilder.Append("Grade 2: ");
            stringBuilder.Append(grade2unit);
            stringBuilder.Append("\t");
            stringBuilder.Append("Grade 3: ");
            stringBuilder.Append(grade3unit);
            return stringBuilder.ToString();
        }

        public virtual string GetDetailedStats(Dictionary<string, IEnumerable<DeckBuilderCard>> decks)
        {
            int sentinel = 0;
            int trigger = 0;
            int order = 0;
            int gunit = 0;
            int gguard = 0;
            int grade0unit = 0;
            int grade1unit = 0;
            int grade2unit = 0;
            int grade3unit = 0;
            int grade4unit = 0;
            int heal = 0;
            int draw = 0;
            int stand = 0;
            int front = 0;
            int crit = 0;
            int over = 0;
            foreach (var cfDeck in decks)
            {
                foreach (var card in cfDeck.Value)
                {
                    CFCardArt? cfCard = CFDBLoader.GetCard(card);
                    if (cfCard == null) { continue; }

                    if (cfCard.Subtype == "Sentinel")
                    {
                        sentinel++;
                    }

                    if (cfCard.Subtype == "G guardian")
                    {
                        gguard++;
                    }

                    if (cfCard.CardType.IsTrigger())
                    {
                        trigger++;
                        switch (cfCard.Trigger)
                        {
                            case "Heal":
                                heal++;
                                break;
                            case "Over":
                                over++;
                                break;
                            case "Draw":
                                draw++;
                                break;
                            case "Stand":
                                stand++;
                                break;
                            case "Front":
                                front++;
                                break;
                            case "Critical":
                                crit++;
                                break;
                        }
                    }

                    if (cfCard.CardType.IsOrder())
                    {
                        order++;
                    }

                    if (cfCard.CardType == CFType.GUnit)
                    {
                        gunit++;
                    }

                    if (cfCard.CardType.IsUnit())
                    {
                        switch (cfCard.Grade)
                        {
                            case 0:
                                grade0unit++;
                                break;
                            case 1:
                                grade1unit++;
                                break;
                            case 2:
                                grade2unit++;
                                break;
                            case 3:
                                grade3unit++;
                                break;
                            case 4:
                                grade4unit++;
                                break;
                        }
                    }
                }
            }

            var stringBuilder = new StringBuilder();
            stringBuilder.Append("Grade 0: ");
            stringBuilder.Append(grade0unit);
            stringBuilder.Append("\t");
            stringBuilder.Append("Triggers: ");
            stringBuilder.Append(trigger);
            stringBuilder.Append("\t");
            stringBuilder.Append("\t");
            stringBuilder.Append("Critical: ");
            stringBuilder.Append(crit);
            stringBuilder.AppendLine();
            stringBuilder.Append("Grade 1: ");
            stringBuilder.Append(grade1unit);
            stringBuilder.Append("\t");
            stringBuilder.Append("Sentinels: ");
            stringBuilder.Append(sentinel);
            stringBuilder.Append("\t");
            stringBuilder.Append("Draw: ");
            stringBuilder.Append(draw);
            stringBuilder.AppendLine();
            stringBuilder.Append("Grade 2: ");
            stringBuilder.Append(grade2unit);
            stringBuilder.Append("\t");
            stringBuilder.Append("Orders: ");
            stringBuilder.Append(order);
            stringBuilder.Append("\t");
            stringBuilder.Append("\t");
            stringBuilder.Append("Front: ");
            stringBuilder.Append(front);
            stringBuilder.AppendLine();
            stringBuilder.Append("Grade 3: ");
            stringBuilder.Append(grade3unit);
            stringBuilder.Append("\t");
            stringBuilder.Append("G Units: ");
            stringBuilder.Append(gunit);
            stringBuilder.Append("\t");
            stringBuilder.Append("\t");
            stringBuilder.Append("Stand: ");
            stringBuilder.Append(stand);
            stringBuilder.AppendLine();
            stringBuilder.Append("Grade 4: ");
            stringBuilder.Append(grade4unit);
            stringBuilder.Append("\t");
            stringBuilder.Append("G guardians: ");
            stringBuilder.Append(gguard);
            stringBuilder.Append("\t");
            stringBuilder.Append("Heal: ");
            stringBuilder.Append(heal);
            stringBuilder.AppendLine();
            /*
            */
            return stringBuilder.ToString();
        }

        public virtual IEnumerable<DeckBuilderCardArt> AdvancedFilterSearchList(IEnumerable<DeckBuilderCardArt> cards, IEnumerable<SearchField> searchFields)
        {
            return cards.Where(card => searchFilter(card, searchFields.Where(field => field.Value != null && field.Value != "")));
        }

        public virtual int CompareCards(DeckBuilderCard x, DeckBuilderCard y)
        {
            CFCardArt? cfX = CFDBLoader.GetCard(x);
            CFCardArt? cfY = CFDBLoader.GetCard(y);

            if (cfX == null || cfY == null) { return x.CardID.CompareTo(x.CardID); }

            if (cfX.CardType != cfY.CardType)
            {
                if (cfX.CardType == CFType.TriggerUnit) { return -1; }
                else if (cfY.CardType == CFType.TriggerUnit) { return 1; }
                return cfX.CardType.CompareTo(cfY.CardType);
            }
            else if (cfX.Trigger != null && cfY.Trigger != null && cfX.Trigger != cfY.Trigger) { return cfX.Trigger.CompareTo(cfY.Trigger); }
            else if (cfX.Grade != cfY.Grade) { return cfX.Grade.CompareTo(cfY.Grade); }
            else if (cfX.Power != null && cfY.Power != null && cfX.Power != cfY.Power) { return cfX.Power.Value.CompareTo(cfY.Power.Value); }
            else if (cfX.Shield != null && cfY.Shield != null && cfX.Shield != cfY.Shield) { return cfX.Shield.Value.CompareTo(cfY.Shield.Value); }
            else if (cfX.ClanFightDecks != cfY.ClanFightDecks) { return cfX.ClanFightDecks.CompareTo(cfY.ClanFightDecks); }
            else { return cfX.Name.CompareTo(cfY.Name); }
        }

        public virtual string DefaultDeckName(DeckBuilderCard card)
        {
            CFCardArt? cfCard = CFDBLoader.GetCard(card);
            if (cfCard == null) return MainDeck.MainDeckName;

            if (cfCard.CardType == CFType.GUnit)
            {
                return GZone.GZoneName;
            }

            if (cfCard.CardType == CFType.NormalUnit && cfCard.Grade == 0)
            {
                return StartingVanguard.StartingVanguardName;
            }

            return MainDeck.MainDeckName;
        }
    }
}
