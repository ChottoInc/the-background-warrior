using UnityEngine;

[CreateAssetMenu(menuName = "Data/Shop/Job Data", fileName = "JobData_")]
public class ShopJobSO : ShopItemSO
{
    [Space(10)]
    [SerializeField] UtilsPlayer.PlayerJob shoppingJob;

    public UtilsPlayer.PlayerJob ShoppingJob => shoppingJob;
}
