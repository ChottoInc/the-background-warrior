using UnityEngine;

[CreateAssetMenu(menuName = "Data/Inventory/Card Data", fileName = "CardData_")]
public class CardSO : ItemSO
{
    [Space(10)]
    [SerializeField] UtilsItem.CardRarity cardRarity;

    [Space(10)]
    [SerializeField] UtilsCard.CardType _cardType;
    [SerializeField] Sprite backgroundSprite;

    [Space(10)]
    [SerializeField] int cardNumber;


    public UtilsItem.CardRarity CardRarity => cardRarity;
    public string CardRarityName
    {
        get
        {
            string res = string.Empty;
            switch (cardRarity)
            {
                case UtilsItem.CardRarity.Common: return UtilsText.AllText[UtilsText.text_name_card_rarity_common];
                case UtilsItem.CardRarity.Uncommon: return UtilsText.AllText[UtilsText.text_name_card_rarity_uncommon];
                case UtilsItem.CardRarity.Rare: return UtilsText.AllText[UtilsText.text_name_card_rarity_rare];
            }

            if (res != string.Empty) return res; else return cardRarity.ToString();
        }
    }

    public Sprite BackgoundSprite => backgroundSprite;
    public UtilsCard.CardType CardType => _cardType;

    public int CardNumber => cardNumber;
}
