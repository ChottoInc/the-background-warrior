using UnityEngine;

[CreateAssetMenu(menuName = "Data/Shop/Card Pack Data", fileName = "CardPackData_")]
public class ShopCardPackSO : ShopItemSO
{
    [Space(10)]
    [Tooltip("How many cards by default")]
    [SerializeField] int size;

    [Space(10)]
    [SerializeField]UtilsGeneral.GeneralChances<UtilsItem.CardRarity>[] rarityChances;

    [Space(10)]
    [SerializeField] bool isGuaranteed;
    [SerializeField] UtilsItem.CardRarity guaranteedRarity;

    public int Size => size;

    public UtilsGeneral.GeneralChances<UtilsItem.CardRarity>[] RarityChances => rarityChances;

    public bool IsGuaranteed => isGuaranteed;
    public UtilsItem.CardRarity GuaranteedRarity => guaranteedRarity;
}
