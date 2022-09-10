using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
namespace CFVanguard.Data
{
    public enum Format
    {
        [Display(Name = "Premium")]
        Premium = 0x1,
        [Display(Name = "V Premium")]
        VPremium = 0x2,
        [Display(Name = "Standard")]
        DStandard = 0x4,
    }

    public static class CFFormatExtensionMethods
    {
    }
}
