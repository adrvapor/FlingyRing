using UnityEngine.Localization.Settings;

public static class RingNameLocalizer
{
    private const string TABLE_NAME = "RingNames";

    public static string GetNameByKey(string key)
    {
        return LocalizationSettings.StringDatabase.GetLocalizedStringAsync(TABLE_NAME, key).Result;
        //switch (key)
        //{

        //    default:
        //    case "Default": 
        //        return LocalizationSettings.StringDatabase.GetLocalizedStringAsync(TABLE_NAME, "Default").Result;
        //    case "Gear":
        //        return LocalizationSettings.StringDatabase.GetLocalizedStringAsync(TABLE_NAME, "Gear").Result;
        //    case "Donut":
        //        return LocalizationSettings.StringDatabase.GetLocalizedStringAsync(TABLE_NAME, "Donut").Result;
        //    case "Tyre":
        //        return LocalizationSettings.StringDatabase.GetLocalizedStringAsync(TABLE_NAME, "Tyre").Result;
        //}
    }
}
