using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace CFVanguard.Data
{
    public enum CFType
    {
        [Display(Name = "Normal Unit")]
        NormalUnit = 0,
        [Display(Name = "Trigger Unit")]
        TriggerUnit = 1,
        [Display(Name = "G Unit")]
        GUnit = 2,
        [Display(Name = "Normal Order")]
        NormalOrder = 3,
        [Display(Name = "Blitz Order")]
        BlitzOrder = 4,
        [Display(Name = "Set Order")]
        SetOrder = 5,
        [Display(Name = "Trigger Order")]
        TriggerOrder = 6,
    }

    public static class CFTypeExtensionMethods
    {
        //This is a extension class of enum
        public static string GetEnumDisplayName(this CFType enumType)
        {
            return enumType.GetType().GetMember(enumType.ToString())
                           .First()
                           .GetCustomAttribute<DisplayAttribute>()!
                           .Name ?? "";
        }

        public static bool IsUnit(this CFType cfType)
        {
            return cfType == CFType.NormalUnit || cfType == CFType.TriggerUnit || cfType == CFType.GUnit;
        }

        public static bool IsMainUnit(this CFType cfType)
        {
            return cfType == CFType.NormalUnit || cfType == CFType.TriggerUnit;
        }

        public static bool IsMainNonTrigger(this CFType cfType)
        {
            return cfType != CFType.GUnit && !cfType.IsTrigger();
        }

        public static bool IsTrigger(this CFType cfType)
        {
            return cfType == CFType.TriggerUnit || cfType == CFType.TriggerOrder;
        }

        public static bool IsOrder(this CFType cfType)
        {
            return cfType == CFType.NormalOrder || cfType == CFType.BlitzOrder || cfType == CFType.SetOrder || cfType == CFType.TriggerOrder;
        }
    }
}
