using IGamePlugInBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFVanguard
{
    public enum CardType
    {
        NormalUnit = 0,
        TriggerUnit = 1,
        GUnit = 2,
        NormalOrder = 3,
        BlitzOrder = 4,
        SetOrder = 5,
        TriggerOrder = 6,
    }

    public enum Ability
    {
        None,
        Boost,
        Intercept,
        TwinDrive,
        TripleDrive
    }

    public enum Format
    {
        Premium = 0x1,
        VPremium = 0x2,
        DStandard = 0x4,
    }

    public class CFCard
    {
        private string id;

        private string cardname;

        private CardType cardtype;

        private string? subtype;

        private int grade;

        private Ability ability;

        private int? power;

        private int? critical;

        private string[] nations;

        private string[] clans;

        private string[] races;

        private string effectText;

        private Format formats;

        private List<CFArt> alternateArts;

        public CFCard(string id, string cardname, CardType cardtype, string? subtype, int grade, Ability ability, int? power, int? critical, string[] nations, string[] clans, string[] races, string effectText, Format formats, List<CFArt> alternateArts)
        {
            this.id = id;
            this.cardname = cardname;
            this.cardtype = cardtype;
            this.subtype = subtype;
            this.grade = grade;
            this.ability = ability;
            this.power = power;
            this.critical = critical;
            this.nations = nations;
            this.clans = clans;
            this.races = races;
            this.effectText = effectText;
            this.formats = formats;
            this.alternateArts = alternateArts;
        }

        public string ID => this.id;

        public string Name { get => this.cardname; }

        public Dictionary<string, CFArt> AltArts
        {
            get
            {
                return this.alternateArts.ToDictionary(keySelector: m => m.Id, elementSelector: m => m);
            }
        }

        public string ViewDetails
        {
            get
            {
                return "";
            }
        }
    }
}