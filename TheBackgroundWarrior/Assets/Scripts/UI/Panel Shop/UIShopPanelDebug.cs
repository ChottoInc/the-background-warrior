using TMPro;
using UnityEngine;

public class UIShopPanelDebug : MonoBehaviour
{
    [SerializeField] TMP_InputField inputCode;

    [Space(10)]
    [SerializeField] GameObject _contentDebug;
    [SerializeField] UIShopPanelAnimation _panelAnimation;

    private bool _isAnimating;

    public void OnButtonRedeem()
    {
        AudioManager.Instance.PlayClickUI();

        if (ShopManager.Instance.HasRedeemedErisCode) return;
        if (_isAnimating) return;

        bool redeemSuccess = false;

        switch (inputCode.text)
        {
            default: Debug.Log("Redeem denied"); break;
        }

        if(!redeemSuccess)
        {
            Debug.Log("Redeem denied");
            // little ui animation of button shaking if not success?
        }
    }

    public void EndAnimation()
    {
        _contentDebug.SetActive(true);
        _isAnimating = false;
    }
}
