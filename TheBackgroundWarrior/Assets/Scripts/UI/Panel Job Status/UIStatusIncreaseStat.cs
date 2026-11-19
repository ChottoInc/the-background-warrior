using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIStatusIncreaseStat : MonoBehaviour
{
    [SerializeField] UITabPlayerStatus panelStatus;

    [Space(10)]
    [SerializeField] int idStat;

    [Space(10)]
    [SerializeField] TMP_Text textStatLevel;

    [Space(10)]
    [SerializeField] TMP_Text textStatName;
    [SerializeField] HorizontalLayoutGroup _scrollingStatGroup;

    [Space(10)]
    [SerializeField] Button buttonDecrease;
    [SerializeField] Button buttonIncrease;

    [Space(10)]
    [SerializeField] float showTooltipAfter = 1.5f;
    [SerializeField] Transform tooltipPosition;

    private bool isShowingTooltip;



    private int currentLevel;
    private int tempLevel;



    private void OnDestroy()
    {
        panelStatus.OnStatusSave -= OnStatusSave;

        panelStatus.OnToggleAdvancedStats -= RefreshTexts;
        panelStatus.OnStatusSave -= RefreshTexts;
    }

    private void Awake()
    {
        panelStatus.OnStatusSave += OnStatusSave;

        panelStatus.OnToggleAdvancedStats += RefreshTexts;
        panelStatus.OnStatusSave += RefreshTexts;
    }

    private void OnEnable()
    {
        // should be called when player is already initialized

        currentLevel = panelStatus.GetStatLevel(idStat);
        tempLevel = currentLevel;

        CheckEnableButton();

        UpdateLevelUI();

        RefreshTexts();
    }

    public void RefreshTexts()
    {
        if (SettingsManager.Instance.IsShowAdvancedStatOn)
        {
            string name = UtilsPlayer.GetStatNameById(idStat);

            string stat = UtilsPlayer.GetAdvancedStatText(UtilsPlayer.GetCurrentStatById(idStat), idStat);

            textStatName.text = name + " - " + stat;
            /*
            _scrollingStatGroup.childForceExpandWidth = false;
            var textRect = textStatName.GetComponent<RectTransform>();
            textRect.sizeDelta = new Vector2(400f, textRect.sizeDelta.y);
            _scrollingStatGroup.childForceExpandWidth = true;*/
        }
        else
        {
            textStatName.text = UtilsPlayer.GetStatNameById(idStat);
        }
    }


    private void UpdateLevelUI()
    {
        if(tempLevel != currentLevel)
        {
            // show old and new level if there have been changes
            textStatLevel.text = string.Format(UtilsText.AllText[UtilsText.text_job_change_stat_level], currentLevel, tempLevel);
        }
        else
        {
            textStatLevel.text = string.Format(UtilsText.AllText[UtilsText.text_job_current_stat_level], currentLevel);
        }
    }

    private void OnStatusSave()
    {
        currentLevel = tempLevel;

        CheckEnableButton();

        UpdateLevelUI();
    }

    private void CheckEnableButton()
    {
        if (tempLevel != -1)
        {
            int maxLevel = UtilsPlayer.GetStatMaxLevelById(idStat);
            buttonIncrease.interactable = tempLevel < maxLevel;

            buttonDecrease.interactable = tempLevel != currentLevel;
        }
    }

    public void OnButtonIncrease()
    {
        if (panelStatus.IncreaseStatLevel(idStat))
        {
            AudioManager.Instance.PlayClickUI();

            tempLevel++;

            UpdateLevelUI();

            CheckEnableButton();
        }
    }

    public void OnButtonDecrease()
    {
        if (panelStatus.DecreaseStatLevel(idStat))
        {
            AudioManager.Instance.PlayClickUI();

            tempLevel--;

            UpdateLevelUI();

            CheckEnableButton();
        }
    }



    public void OnPointerEnter()
    {
        StartCoroutine(CoShowTooltip());
    }

    public void OnPointerExit()
    {
        if (isShowingTooltip)
        {
            isShowingTooltip = false;
            UITooltipManager.Instance.Hide(UITooltipManager.ID_SHOW_TEXT, true);
        }
        else
        {
            StopAllCoroutines();
        }
    }

    private IEnumerator CoShowTooltip()
    {
        yield return new WaitForSecondsRealtime(showTooltipAfter);

        isShowingTooltip = true;

        TooltipManagerData tooltipData = new TooltipManagerData();
        tooltipData.idTooltip = UITooltipManager.ID_SHOW_TEXT;
        tooltipData.text = UtilsPlayer.GetStatTooltipById(idStat);
        UITooltipManager.Instance.Show(tooltipData, tooltipPosition.position, true);
    }
}
