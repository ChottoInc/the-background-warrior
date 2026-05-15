using UnityEngine;

[CreateAssetMenu(menuName = "Data/Inventory/Card Data", fileName = "CardData_")]
public class CardSO : ItemSO
{
    [Space(10)]
    [SerializeField] UtilsItem.CardRarity cardRarity;

    [Space(10)]
    [SerializeField] Sprite backgroundSprite;

    [Space(10)]
    [SerializeField] int cardNumber;

    [TextArea]
    [SerializeField] string cardDescription;


    public UtilsItem.CardRarity CardRarity => cardRarity;

    public Sprite BackgoundSprite => backgroundSprite;

    public int CardNumber => cardNumber;

    public string CardDescription => cardDescription;
}
