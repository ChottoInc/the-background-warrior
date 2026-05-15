using TMPro;
using UnityEngine;

public class UIShopPanelRedeem : MonoBehaviour
{
    [SerializeField] TMP_InputField inputCode;

    public void OnButtonRedeem()
    {
        AudioManager.Instance.PlayClickUI();

        bool redeemSuccess = false;

        switch (inputCode.text)
        {
            default: Debug.Log("Redeem denied"); break;

            case UtilsShop.REDEEM_ERIS_CODE:

                if (!ShopManager.Instance.HasRedeemedErisCode)
                {
                    redeemSuccess = true;
                    ShopManager.Instance.SetRedeemCode(UtilsShop.ID_REDEEM_ERIS_CODE);
                    ShopManager.Instance.SaveShopData();
                    Debug.Log("Redeem success: " + UtilsShop.REDEEM_ERIS_CODE);
                }
                break;
        }

        if(!redeemSuccess)
        {
            Debug.Log("Redeem denied");
            // little ui animation of button shaking if not success?
        }
    }
}
