using UnityEngine;
using UnityEngine.EventSystems;

public class UIHoverBaitIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] UITimerBait _timer;

    public void OnPointerEnter(PointerEventData eventData)
    {
        UpdateTimerUI();
        _timer.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _timer.gameObject.SetActive(false);
    }

    private void UpdateTimerUI()
    {
        // get active bait
        var activeBait = PlayerManager.Instance.PlayerFisherData.ActiveBait;
        if (activeBait == null) return;

        // get buff type based on attracting moment
        UtilsBuffs.BuffType buffType;
        if (activeBait.AttractsMoment == UtilsGeneral.DayMoment.Morning)
        {
            buffType = UtilsBuffs.BuffType.MorningAngler;
        }
        else if (activeBait.AttractsMoment == UtilsGeneral.DayMoment.Afternoon)
        {
            buffType = UtilsBuffs.BuffType.AfternoonAngler;
        }
        else
        {
            buffType = UtilsBuffs.BuffType.NightAngler;
        }

        // get buff data
        var buff = PlayerManager.Instance.PlayerBuffsData.GetBuffByType(buffType);

        if (buff == null) return;

        float totalSeconds = buff.RemainingTime;

        int hours = (int)(totalSeconds / 3600);
        int minutes = (int)(totalSeconds % 3600 / 60);
        int seconds = (int)(totalSeconds % 60);

        string timerText = string.Empty;
        if (hours > 0)
            timerText = $"{hours:00}:{minutes:00}:{seconds:00}";
        else
            timerText = $"{minutes:00}:{seconds:00}";

        _timer.SetTimer(timerText);
    }
}
