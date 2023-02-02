using CFVanguard.Data;
using IGamePlugInBase;
using System.Data;
using System.Data.SQLite;

namespace Cardfight_Vanguard_Plug_In
{
    internal class CFDBLoader
    {
        const string DatabaseFile = @".\plug-ins\cfvg\cfvg.db";
        static CFDBLoader? _instance;

        private CFDBLoader()
        {

        }

        public static void InitializeDataset(string selectString)
        {
            using (var connection = new SQLiteConnection($"Data Source={Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DatabaseFile)}"))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = $"select cards.*, arts.setno, arts.filelocation, arts.downloadurl from arts inner join cards on cards.id = arts.id {selectString};";

                using (var cardReader = command.ExecuteReader())
                {

                    var cardData = new List<CFCardArt>();
                    while (cardReader.Read())
                    {
                        var id = cardReader.GetFieldValue<string>("id");
                        var name = cardReader.GetFieldValue<string>("name");
                        var setno = cardReader.GetFieldValue<string>("setno");
                        var fileLoc = @".\plug-ins\cfvg\" + cardReader.GetFieldValue<string>("filelocation");
                        var downloadUrl = cardReader.GetFieldValue<string>("downloadurl");
                        var cardtype = (CFType)cardReader.GetFieldValue<long>("cardtype");
                        var subtype = cardReader.GetValue("subtype").ToString();
                        var trigger = cardReader.GetValue("trigger").ToString();
                        var grade = (int)cardReader.GetFieldValue<long>("grade");
                        var rideskill = cardReader.GetValue("rideskill").ToString();
                        var ability = cardReader.GetValue("ability").ToString();
                        var power = cardReader.GetValue("power");
                        var shield = cardReader.GetValue("shield");
                        var nations = cardReader.GetValue("nations").ToString();
                        var clans = cardReader.GetValue("clans").ToString();
                        var races = cardReader.GetValue("races").ToString();
                        var effects = cardReader.GetValue("effects").ToString();
                        var clantypes = cardReader.GetFieldValue<long>("clan_dtype");
                        var nationtypes = cardReader.GetFieldValue<long>("odress_dtype");
                        var formats = (int)cardReader.GetFieldValue<long>("formats");

                        // Get Fields
                        cardData.Add(new CFCardArt(id, setno, name, fileLoc, downloadUrl, cardtype, subtype, trigger,
                            grade, rideskill, ability, Convert.IsDBNull(power) ? null : Convert.ToInt32(power),
                            Convert.IsDBNull(shield) ? null : Convert.ToInt32(shield), nations, clans, races, effects,
                            clantypes, nationtypes, (Format)formats)
                        );
                    }

                    cardReader.Close();
                    CardData = cardData;
                }

                if (ClanDecks == null)
                {
                    ClanDecks = new Dictionary<string, int>();
                    command.CommandText = "select * from clanfight_decks;";

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ClanDecks.Add(reader.GetFieldValue<string>("clanname"), Convert.ToInt32(reader.GetValue("value")));
                        }
                    }
                }

                if (NationDecks == null)
                {
                    NationDecks = new Dictionary<string, int>();
                    command.CommandText = "select * from overdress_decks;";

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            NationDecks.Add(reader.GetFieldValue<string>("nation"), Convert.ToInt32(reader.GetValue("value")));
                        }
                    }
                }

                connection.Close();
            }
        }

        public static CFDBLoader Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new CFDBLoader();
                }

                return _instance;
            }
        }

        public static CFCardArt? GetCard(string cardID, string artID)
        {
            return CardData.FirstOrDefault(crd => cardID == crd.CardID && crd.ArtID == artID);
        }

        public static CFCardArt? GetCard(DeckBuilderCard card)
        {
            return GetCard(card.CardID, card.ArtID);
        }

        public static IEnumerable<CFCardArt> CardData { get; private set; } = Enumerable.Empty<CFCardArt>();

        public static Dictionary<string, int>? ClanDecks { get; private set; }

        public static Dictionary<string, int>? NationDecks { get; private set; }
    }
}
