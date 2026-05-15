using System;
using UnityEngine;
using UnityEngine.UI;

public class UIPanelAutoBattle : MonoBehaviour
{
    [SerializeField] Transform tooltipPosition;
    [SerializeField] float cooldownTooltip = 1.5f;

    [Space(10)]
    [SerializeField] Toggle toggleAutoBattle;

    private bool hasStartedTimer;
    private float timerTooltip;
    private bool isShowingTooltip;


    public event Action<bool> OnSet;


    public void Start()
    {
        toggleAutoBattle.SetIsOnWithoutNotify(SettingsManager.Instance.IsAutoBattleOn);
    }

    private void Update()
    {
        if (!hasStartedTimer) return;

        // works only if the pointer is on the text
        if (timerTooltip <= 0)
        {
            if (isShowingTooltip) return;

            // set showing
            isShowingTooltip = true;

            // show
            string text = "If this option is enabled, automatically advance to the next stage when all enemies are slayed, or retry the current stage in case of death";

            TooltipManagerData tooltipData = new TooltipManagerData();
            tooltipData.idTooltip = UITooltipManager.ID_SHOW_TEXT;
            tooltipData.text = text;
            UITooltipManager.Instance.Show(tooltipData, tooltipPosition.position, true);
        }
        else
        {
            timerTooltip -= Time.deltaTime;
        }
    }


    public void OnToggleAutoBattle(bool isOn)
    {
        AudioManager.Instance.PlayClickUI();

        SettingsManager.Instance.SetIsAutoBattle(isOn);

        OnSet?.Invoke(isOn);
    }


    public void Setup()
    {
        toggleAutoBattle.SetIsOnWithoutNotify(SettingsManager.Instance.IsAutoBattleOn);
    }


    /// <summary>
    /// Used to set isOn if toggled anywhere
    /// </summary>
    public void SetToggleWithoutNotify(bool isOn)
    {
        toggleAutoBattle.SetIsOnWithoutNotify(isOn);
    }







    public void OnPointerEnter()
    {
        // start timer
        timerTooltip = cooldownTooltip;

        hasStartedTimer = true;
    }

    public void OnPointerExit()
    {
        if (isShowingTooltip)
        {
            UITooltipManager.Instance.Hide(UITooltipManager.ID_SHOW_TEXT, true);
        }

        // resets
        hasStartedTimer = false;
        timerTooltip = 0;
        isShowingTooltip = false;
    }
}
