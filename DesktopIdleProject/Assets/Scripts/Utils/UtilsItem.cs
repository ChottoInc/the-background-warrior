using System;
using System.Collections.Generic;
using UnityEngine;

public static class UtilsItem
{
    /*
     * Ores ids start from 0
     * Cards ids start from 50
     * Metals ids start from 150
     * Fishes ids start from 200
     * */

    public enum ItemType { Ore, Card, Metal, Fish }

    public enum CardRarity { Common, Uncommon, Rare }

    public enum FishRarity { Riverfolk, Deepwater, Tideborn, Ancient, Mythic }


    private static List<ItemSO> ores;
    private static List<ItemSO> cards;
    private static List<ItemSO> metals;

    private static List<ItemSO> fishes;
    private static List<ItemSO> fishesMorning;
    private static List<ItemSO> fishesAfternoon;
    private static List<ItemSO> fishesEvening;
    private static List<ItemSO> fishesNight;


    private static List<ItemSO> otherItems;

    public static void Initialize()
    {
        LoadAllItems();
    }

    private static void LoadAllItems()
    {
        ItemSO[] items = Resources.LoadAll<ItemSO>("Data/Items");

        ores = new List<ItemSO>();
        cards = new List<ItemSO>();
        metals = new List<ItemSO>();

        fishes = new List<ItemSO>();
        fishesMorning = new List<ItemSO>();
        fishesAfternoon = new List<ItemSO>();
        fishesEvening = new List<ItemSO>();
        fishesNight = new List<ItemSO>();

        otherItems = new List<ItemSO>();

        foreach (ItemSO item in items) 
        {
            switch(item.ItemType)
            {
                default: otherItems.Add(item); break;
                case ItemType.Ore: ores.Add(item); break;
                case ItemType.Card: cards.Add(item); break;
                case ItemType.Metal: metals.Add(item); break;

                case ItemType.Fish: 
                    fishes.Add(item); 
                    FishSO fish = item as FishSO;
                    switch (fish.SpawnDayMoment)
                    {
                        case UtilsGeneral.DayMoment.Morning: fishesMorning.Add(item); break;
                        case UtilsGeneral.DayMoment.Afternoon: fishesAfternoon.Add(item); break;
                        case UtilsGeneral.DayMoment.Evening: fishesEvening.Add(item); break;
                        case UtilsGeneral.DayMoment.Night: fishesNight.Add(item); break;
                    }
                    break;
            }
        }
    }

    public static List<ItemSO> GetAllItems()
    {
        List<ItemSO> result = new List<ItemSO>();
        result.AddRange(ores);
        result.AddRange(cards);
        result.AddRange(metals);
        result.AddRange(fishes);
        result.AddRange(otherItems);
        return result;
    }

    public static ItemSO GetItemById(int id)
    {
        List<ItemSO> items = GetAllItems();

        foreach (ItemSO item in items)
        {
            if(item.Id == id)
                return item;
        }
        return null;
    }

    #region ORES

    public static ItemSO[] GetAllOres()
    {
        return ores.ToArray();
    }

    #endregion

    #region METALS

    public static ItemSO[] GetAllMetals()
    {
        return metals.ToArray();
    }

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
            case UtilsGather.RockType.Copper: result = 200; break;
            case UtilsGather.RockType.Iron: result = 600; break;
            case UtilsGather.RockType.Bronze: result = 1400; break;
            case UtilsGather.RockType.Silver: result = 2400; break;
            case UtilsGather.RockType.Gold: result = 4000; break;
        }
        return result * metalSO.RequiredOres * multiplier;
    }

    #endregion

    #region CARDS

    public static ItemSO GetCardById(int id)
    {
        foreach (ItemSO card in cards)
        {
            if (card.Id == id)
                return card;
        }
        return null;
    }

    public static ItemSO[] GetAllCards()
    {
        return cards.ToArray();
    }

    public static CardSO GetRandomCardByRarity(CardRarity rarity)
    {
        bool found = false;
        CardSO card = null;

        int tries = 0;
        int maxTries = 1000;

        while (!found && tries < maxTries)
        {
            found = false;
            int rand = UnityEngine.Random.Range(0, cards.Count);

            card = cards[rand] as CardSO;

            if (card.CardRarity == rarity)
                found = true;

            tries++;
        }

        if (found)
            return card;
        return null;
    }

    public static bool DoesCardListContainRarity(List<CardSO> cards, CardRarity rarity)
    {
        foreach (var card in cards)
        {
            if (card.CardRarity == rarity)
                return true;
        }
        return false;
    }

    public static int GetRandomIndexLowestRarityCard(List<CardSO> cards)
    {
        int cardRaritiesCount = Enum.GetNames(typeof(CardRarity)).Length;
        List<int> indexes = new List<int>();

        for (int i = 0; i < cardRaritiesCount; i++)
        {
            for (int j = 0; j < cards.Count; j++)
            {
                if ((int)cards[j].CardRarity == i)
                    indexes.Add(j);
            }

            // check only the lowest rarity in list
            if (indexes.Count > 0)
                break;
        }

        return indexes[UnityEngine.Random.Range(0, indexes.Count)];
    }

    public static CardSO GetConvertedCard(List<CardSO> converted)
    {
        CardSO result = null;

        float commonPerc = 0.90f;
        float uncommonPerc = 0.07f;
        float rarePerc = 0.01f;

        foreach (var card in converted)
        {
            // If uncommon, +2% to uncommon, and 1% to rare
            if(card.CardRarity == CardRarity.Uncommon)
            {
                commonPerc -= 0.03f;
                uncommonPerc += 0.02f;
                rarePerc += 0.01f;
            }
            // If rare, +4% uncommon and +2% rare
            else if(card.CardRarity == CardRarity.Rare)
            {
                commonPerc -= 0.06f;
                uncommonPerc += 0.04f;
                rarePerc += 0.02f;
            }
        }

        UtilsGeneral.GeneralChances<CardRarity>[] balancedArray = new UtilsGeneral.GeneralChances<CardRarity>[3];

        balancedArray[0] = new UtilsGeneral.GeneralChances<CardRarity>
        {
            chanches = Mathf.RoundToInt(commonPerc * 100f),
            value = CardRarity.Common
        };

        balancedArray[1] = new UtilsGeneral.GeneralChances<CardRarity>
        {
            chanches = Mathf.RoundToInt(uncommonPerc * 100f),
            value = CardRarity.Uncommon
        };

        balancedArray[2] = new UtilsGeneral.GeneralChances<CardRarity>
        {
            chanches = Mathf.RoundToInt(rarePerc * 100f),
            value = CardRarity.Rare
        };

        CardRarity selectedRarity = UtilsGeneral.GetRandomValueFromGeneralChanches(balancedArray);

        result = GetRandomCardByRarity(selectedRarity);

        return result;
    }


    public static int GetDismantleValueFromCard(CardSO card)
    {
        int result = 0;

        switch (card.CardRarity)
        {
            case CardRarity.Common: result = 1; break;
            case CardRarity.Uncommon: result = 2; break;
            case CardRarity.Rare: result = 5; break;
        }

        return result;
    }
    #endregion

    #region FISHES

    public static ItemSO[] GetAllFishes()
    {
        return fishes.ToArray();
    }

    public static List<FishSO> GetFishByDayMoment(UtilsGeneral.DayMoment moment)
    {
        List<FishSO> result = new List<FishSO>();

        foreach (var fish in fishes)
        {
            if ((fish as FishSO).SpawnDayMoment == moment)
                result.Add(fish as FishSO);
        }
        return result;
    }

    public static FishSO GetRandomFish(List<ItemSO> list)
    {
        bool found = false;
        FishSO fish = null;

        int tries = 0;
        int maxTries = 1000;

        while (!found && tries < maxTries)
        {
            found = false;
            int rand = UnityEngine.Random.Range(0, list.Count);

            fish = list[rand] as FishSO;
            if (fish != null)
                found = true;

            tries++;
        }

        if (found)
            return fish;
        return null;
    }

    public static FishSO GetRandomFishByRarity(FishRarity rarity)
    {
        bool found = false;
        FishSO fish = null;

        int tries = 0;
        int maxTries = 1000;

        while (!found && tries < maxTries)
        {
            found = false;
            fish = GetRandomFish(fishes);

            if (fish.FishRarity == rarity)
                found = true;

            tries++;
        }

        if (found)
            return fish;
        return null;
    }

    

    public static FishSO GetRandomFishByDayMoment(UtilsGeneral.DayMoment dayMoment)
    {
        FishSO result = null;

        switch (dayMoment)
        {
            case UtilsGeneral.DayMoment.Morning: result = GetRandomFish(fishesMorning); break;
            case UtilsGeneral.DayMoment.Afternoon: result = GetRandomFish(fishesAfternoon); break;
            case UtilsGeneral.DayMoment.Evening: result = GetRandomFish(fishesEvening); break;
            case UtilsGeneral.DayMoment.Night: result = GetRandomFish(fishesNight); break;
        }

        return result;
    }

    public static FishSO GetRandomFishByDayMomentAndRarity(UtilsGeneral.DayMoment dayMoment, FishRarity rarity)
    {
        FishSO result = null;

        List<ItemSO> selectedFishes;

        switch (dayMoment)
        {
            default:
            case UtilsGeneral.DayMoment.Morning: selectedFishes = fishesMorning; break;
            case UtilsGeneral.DayMoment.Afternoon: selectedFishes = fishesAfternoon; break;
            case UtilsGeneral.DayMoment.Evening: selectedFishes = fishesEvening; break;
            case UtilsGeneral.DayMoment.Night: selectedFishes = fishesNight; break;
        }

        bool found = false;

        int tries = 0;
        int maxTries = 1000;

        while (!found && tries < maxTries)
        {
            found = false;
            result = GetRandomFish(selectedFishes);

            if (result.FishRarity == rarity)
                found = true;


            // check if the fish can actually get caught, there are some SOs extra, so the fish wouldn't be in any of the groups
            if (UtilsGather.GetFishGroupByFish(result) == null)
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
            case FishRarity.Riverfolk: return 1500;
            case FishRarity.Deepwater: return 2500;
            case FishRarity.Tideborn: return 4000;
            case FishRarity.Ancient: return 6000;
            case FishRarity.Mythic: return 8500;
        }
    }

    public static string GetFishRarityName(FishRarity rarity)
    {
        switch (rarity)
        {
            default:
            case FishRarity.Riverfolk: return "Riverfolk";
            case FishRarity.Deepwater: return "Deepwater";
            case FishRarity.Tideborn: return "Tideborn";
            case FishRarity.Ancient: return "Ancient";
            case FishRarity.Mythic: return "Mythic";
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
}
