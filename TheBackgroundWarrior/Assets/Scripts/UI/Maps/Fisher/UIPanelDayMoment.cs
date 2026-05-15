using TMPro;
using UnityEngine;

public class UIPanelDayMoment : MonoBehaviour
{
    [SerializeField] float cooldownCheckMoment;
    [SerializeField] TMP_Text textDayMoment;

    private float timerCheckMoment;

    private void OnEnable()
    {
        UpdateTextUI();
    }

    private void Update()
    {
        if(timerCheckMoment <= 0)
        {
            UpdateTextUI();
            timerCheckMoment = cooldownCheckMoment;
        }
        else
        {
            timerCheckMoment -= Time.unscaledDeltaTime;
        }
    }

    private void UpdateTextUI()
    {
        textDayMoment.text = UtilsGeneral.GetDayMomentName(UtilsGeneral.GetDayMoment());
    }
}
