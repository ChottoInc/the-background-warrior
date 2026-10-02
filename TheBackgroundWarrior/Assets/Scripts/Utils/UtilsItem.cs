using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class UtilsItem
{
    /*
     * Ores ids start from 0
     * Cards ids start from 50
     * Metals ids start from 150
     * Fishes ids start from 200
     * Crops ids start from 400
     * Baits ids start from 450
     * Concoctions ids start from 500
     * */

    public const int MAX_ITEM_IN_INVENTORY = 999999;

    public const int ID_GOLD_ORE = 4;

    public const int ID_CARD_ANCIENTTOME = 80;


    private static Dictionary<int, ListableGameDataSO> dictItems;

    public enum ItemType { Ore, Card, Metal, Fish, Crop, Bait, Concoction }

    public enum CardRarity { Common, Uncommon, Rare }

    public enum FishRarity { Riverfolk, Deepwater, Tideborn, Ancient, Mythic }


    public static void Initialize()
    {
        LoadItems();
    }

    private static void LoadItems()
    {
        var container = Resources.Load<ContainerGameDataSO>("Data/Items/ContainerGameData_Items");
        dictItems = container.Entries.ToDictionary(e => e.Id);
    }
    
    public static List<ItemSO> GetAllItems()
    {
        return dictItems.OfType<ItemSO>().ToList();
    }

    public static ItemSO GetItemById(int id)
    {
        return UtilsGeneral.GetGameDataSO<ItemSO>(id, dictItems);
    }

    public static T[] GetAllTypeItem<T>() where T : ItemSO
    {
        return dictItems.Values.OfType<T>().ToArray();
    }

    #region METALS

    /// <summary>
    /// Exp given by the metals
    /// </summary>
    public static long GetMetalExp(MetalSO metalSO)
    {
        int multiplier = 2;
        int result;
        switch (metalSO.RockType)
        {
            default:
            case UtilsMiner.RockType.Copper: result = 200; break;
            case UtilsMiner.RockType.Iron: result = 600; break;
            case UtilsMiner.RockType.Bronze: result = 1400; break;
            case UtilsMiner.RockType.Silver: result = 2400; break;
            case UtilsMiner.RockType.Gold: result = 4000; break;
        }
        return result * metalSO.RequiredOres * multiplier;
    }

    #endregion

    #region FISHES

    public static List<FishSO> GetFishByDayMoment(UtilsGeneral.DayMoment moment)
    {
        return GetAllTypeItem<FishSO>()
            .Where(f => f.SpawnDayMoment == moment)
            .ToList();
    }

    public static FishSO GetRandomFish(List<FishSO> list)
    {
        bool found = false;
        FishSO fish = null;

        int tries = 0;
        int maxTries = 1000;

        while (!found && tries < maxTries)
        {
            found = false;
            int rand = UnityEngine.Random.Range(0, list.Count);

            fish = list[rand];
            if (fish != null)
                found = true;

            tries++;
        }

        if (found)
            return fish;
        return null;
    }

    public static FishSO GetRandomFishByDayMomentAndRarity(UtilsGeneral.DayMoment dayMoment, FishRarity rarity)
    {
        FishSO result = null;

        var fishes = GetFishByDayMoment(dayMoment);

        bool found = false;

        int tries = 0;
        int maxTries = 1000;

        while (!found && tries < maxTries)
        {
            found = false;
            result = GetRandomFish(fishes);

            if (result.FishRarity == rarity)
                found = true;


            // check if the fish can actually get caught, there are some SOs extra, so the fish wouldn't be in any of the groups
            if (UtilsFisher.GetFishGroupByFish(result) == null)
                found = false;

            tries++;
        }

        if (found)
            return result;
        return GetRandomFish(fishes);
    }

    public static int DismantleFish(FishRarity rarity)
    {
        switch(rarity)
        {
            default:
            case FishRarity.Riverfolk: return 1;
            case FishRarity.Deepwater: return 2;
            case FishRarity.Tideborn: return 3;
            case FishRarity.Ancient: return 5;
            case FishRarity.Mythic: return 8;
        }
    }

    /// <summary>
    /// Exp given by the caught fishes
    /// </summary>
    public static long GetFishExp(FishRarity rarity)
    {
        switch (rarity)
        {
            default:
            case FishRarity.Riverfolk: return 2000;
            case FishRarity.Deepwater: return 2500;
            case FishRarity.Tideborn: return 3500;
            case FishRarity.Ancient: return 5000;
            case FishRarity.Mythic: return 7000;
        }
    }

    public static string GetFishRarityName(FishRarity rarity)
    {
        switch (rarity)
        {
            default:
            case FishRarity.Riverfolk: return UtilsText.AllText[UtilsText.text_name_fish_rarity_riverfolk];
            case FishRarity.Deepwater: return UtilsText.AllText[UtilsText.text_name_fish_rarity_deepwater];
            case FishRarity.Tideborn: return UtilsText.AllText[UtilsText.text_name_fish_rarity_tideborn];
            case FishRarity.Ancient: return UtilsText.AllText[UtilsText.text_name_fish_rarity_ancient];
            case FishRarity.Mythic: return UtilsText.AllText[UtilsText.text_name_fish_rarity_mythic];
        }
    }

    public static string GetFishRarityColor(FishRarity rarity)
    {
        switch (rarity)
        {
            default:
            case FishRarity.Riverfolk: return "D9D9D9";
            case FishRarity.Deepwater: return "27B95B";
            case FishRarity.Tideborn: return "273FB9";
            case FishRarity.Ancient: return "7928BA";
            case FishRarity.Mythic: return "E0D315";
        }
    }

    #endregion

    #region CROPS

    public static CropSO[] GetCropsByAgronomyLevel(int agronmyLevel)
    {
        var crops = GetAllTypeItem<CropSO>();
        return crops.Where(c => c.UnlocksWithAgronomyLevel <= agronmyLevel).ToArray();
    }

    #endregion



    private static readonly Dictionary<ItemType, IConsumableHandler> _consumableHandlers = new()
    {
        { ItemType.Bait, new ConsumableBaitHandler() },
        { ItemType.Concoction, new ConsumableConcoctionHandler() },
    };


    public static bool UseConsumable(ItemSO item)
    {
        if (_consumableHandlers.TryGetValue(item.ItemType, out var handler))
           return handler.Use(item);
        else
        {
            Debug.LogWarning($"No handler for item type {item.ItemType}");
            return false;
        }
    }
}
