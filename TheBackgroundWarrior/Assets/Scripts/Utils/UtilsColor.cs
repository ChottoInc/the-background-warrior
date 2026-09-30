using UnityEngine;

public static class UtilsColor
{
    public const string SELECTED_GREEN = "#42bd41";

    public const string BACKGROUND_BAR_LEARN_MAGE = "FFE4BA";
    public const string FILL_BAR_LEARN_MAGE = "E29B2E";

    public const string CARD_COMMON_RARITY = "#f36c60";
    public const string CARD_UNCOMMON_RARITY = "#4fc3f7";
    public const string CARD_RARE_RARITY = "#ffca28";
    //ab47bc


    public const string EQUIPPED_COMPANION = "#8d6e63";


    public static Color GetColorByRarity(UtilsItem.CardRarity rarity)
    {
        switch (rarity)
        {
            default:
            case UtilsItem.CardRarity.Common: return GetColorHex(CARD_COMMON_RARITY);
            case UtilsItem.CardRarity.Uncommon: return GetColorHex(CARD_UNCOMMON_RARITY);
            case UtilsItem.CardRarity.Rare: return GetColorHex(CARD_RARE_RARITY);
        }
    }

    public static Color GetColorHex(string hex)
    {
        if (ColorUtility.TryParseHtmlString(hex, out Color color)) 
        {
            return color;
        }
        return Color.white;
    }

    public static Color GetSelectedGreen()
    {
        return GetColorHex(SELECTED_GREEN);
    }
}
