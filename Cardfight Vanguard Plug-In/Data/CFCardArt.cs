using IGamePlugInBase;
using System.Text;

namespace CFVanguard.Data
{
    public class CFCardArt : DeckBuilderCardArt
    {
        public CFCardArt(
            string id,
            string artId,
            string name,
            string fileLocation,
            string downloadLocation,
            CFType cardType,
            string? subtype,
            string? trigger,
            int grade,
            string? rideSkill,
            string? ability,
            int? power,
            int? shield,
            string? nations,
            string? clans,
            string? races,
            string? effects,
            long clanFightDecks,
            long overDressDecks,
            Format format) : base(
                id,
                artId,
                name,
                fileLocation,
                downloadLocation,
                subtype == "Music" ? CardArtOrientation.Landscape : CardArtOrientation.Portrait,
                "")
        {
            CardType = cardType;
            Subtype = subtype;
            Trigger = trigger;
            Grade = grade;
            RideSkill = rideSkill;
            Ability = ability;
            Power = power;
            Shield = shield;
            Nations = nations;
            Clans = clans;
            Races = races;
            Effects = effects;
            ClanFightDecks = clanFightDecks;
            OverDressDecks = overDressDecks;
            Format = format;

            base.ViewDetails = ViewDetails;
        }

        public CFType CardType { get; set; }

        public string? Subtype { get; set; }

        public string? Trigger { get; set; }

        public int Grade { get; set; }

        public string? RideSkill { get; set; }

        public string? Ability { get; set; }

        public int? Power { get; set; }

        public int? Shield { get; set; }

        public string? Nations { get; set; }

        public string? Clans { get; set; }

        public string? Races { get; set; }

        public string? Effects { get; set; }

        /// <summary>
        /// Allowed in which Deck Types based on Binary Value
        /// </summary>
        public long ClanFightDecks { get; set; }

        /// <summary>
        /// Allowed in which Deck Types based on Binary Value
        /// </summary>
        public long OverDressDecks { get; set; }

        public Format Format { get; set; }

        public new string ViewDetails
        {
            get
            {
                // Build String
                StringBuilder stringBuilder = new StringBuilder();
                // Line 1
                stringBuilder.AppendLine(Name);
                // Line 2
                stringBuilder.Append(CardType.GetEnumDisplayName());
                if (!string.IsNullOrEmpty(Subtype))
                {
                    stringBuilder.Append("/");
                    stringBuilder.Append(Subtype);
                }
                if (!string.IsNullOrEmpty(Trigger))
                {
                    stringBuilder.Append("/");
                    stringBuilder.Append(Trigger);
                }
                stringBuilder.AppendLine();
                // Line 3
                stringBuilder.Append("Grade ");
                stringBuilder.Append(Grade);
                if (!string.IsNullOrEmpty(Ability))
                {
                    stringBuilder.Append("/");
                    stringBuilder.Append(Ability);
                }
                if (!string.IsNullOrEmpty(RideSkill))
                {
                    stringBuilder.Append("/");
                    stringBuilder.Append(RideSkill);
                }
                stringBuilder.AppendLine();
                // Line 4?
                if (Power != null)
                {
                    stringBuilder.Append(Power);
                    stringBuilder.Append(" Power");

                    if (Shield != null)
                    {
                        stringBuilder.Append('/');
                    }
                }
                if (Shield != null)
                {
                    stringBuilder.Append(Shield);
                    stringBuilder.Append(" Shield");
                }
                if (Power != null || Shield != null)
                {
                    stringBuilder.AppendLine();
                }
                // Line 5?
                if (!string.IsNullOrEmpty(Clans))
                {
                    stringBuilder.Append(Clans);
                    if (Nations != null) { stringBuilder.Append('/'); }
                    else { stringBuilder.AppendLine(); }
                }
                if (!string.IsNullOrEmpty(Nations))
                {
                    stringBuilder.AppendLine(Nations);
                }
                // Line 6?
                if (!string.IsNullOrEmpty(Races))
                {
                    stringBuilder.AppendLine(Races);
                }
                // Line 7?
                if (!string.IsNullOrEmpty(Effects))
                {
                    stringBuilder.AppendLine();
                    stringBuilder.AppendLine(Effects);
                }

                return stringBuilder.ToString();
            }
        }
    }
}