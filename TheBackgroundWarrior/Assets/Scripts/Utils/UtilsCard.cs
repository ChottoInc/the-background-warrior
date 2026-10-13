using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using static UtilsItem;

public static class UtilsCard
{
    public enum CardType
    {
        Monsters,
        People,
        Objects
    }

    public static CardSO GetRandomCardByRarity(CardRarity rarity)
    {
        var cards = GetAllTypeItem<CardSO>().ToList();
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
            if (card.CardRarity == CardRarity.Uncommon)
            {
                commonPerc -= 0.03f;
                uncommonPerc += 0.02f;
                rarePerc += 0.01f;
            }
            // If rare, +4% uncommon and +2% rare
            else if (card.CardRarity == CardRarity.Rare)
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

    public static bool AreAllCardTypeCollected(CardType cardType)
    {
        var allCards = GetAllTypeItem<CardSO>().ToList();
        var allTypeCards = allCards.Where(c => c.CardType == cardType).ToList();
        foreach (var card in allTypeCards)
        {
            if (!PlayerManager.Instance.Inventory.HasItem(card.Id))
                return false;
        }
        return true;
    }

    public static bool AreAllCardCollected()
    {
        var allCards = GetAllTypeItem<CardSO>().ToList();
        foreach (var card in allCards)
        {
            if (!PlayerManager.Instance.Inventory.HasItem(card.Id))
                return false;
        }
        return true;
    }
}
