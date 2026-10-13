using TMPro;
using UnityEngine;

public class UIShopPanelAchievements : MonoBehaviour
{
    [SerializeField] TMP_InputField _inputCode;

    public void OnButtonRemove()
    {
        AudioManager.Instance.PlayClickUI();

        UtilsAchievement.RemoveAchievement(_inputCode.text);
    }
}
