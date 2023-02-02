using System.ComponentModel.DataAnnotations;
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
