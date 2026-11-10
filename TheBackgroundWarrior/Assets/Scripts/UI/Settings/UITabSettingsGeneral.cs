using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UITabSettingsGeneral : UITabWindow
{
    [SerializeField] Slider sliderMaster;
    [SerializeField] TMP_Dropdown dropdownLanguage;

    public override void Open()
    {
        base.Open();

        Setup();
    }

    private void Setup()
    {
        sliderMaster.SetValueWithoutNotify(SettingsManager.Instance.MasterVolume);

        dropdownLanguage.ClearOptions();
        List<TMP_Dropdown.OptionData> listLanguage = new List<TMP_Dropdown.OptionData>()
        {
            new TMP_Dropdown.OptionData(UtilsText.AllText[UtilsText.text_settings_general_lang_english]),
            new TMP_Dropdown.OptionData(UtilsText.AllText[UtilsText.text_settings_general_lang_italian])
        };

        dropdownLanguage.AddOptions(listLanguage);

        dropdownLanguage.SetValueWithoutNotify((int)SettingsManager.Instance.CurrentLanguage);
    }



    public void OnMasterChange(float value)
    {
        SettingsManager.Instance.SetMasterVolume(value);
    }

    public void OnLanguageChange(int index)
    {
        AudioManager.Instance.PlayClickUI();
        SettingsManager.Instance.SetLanguage((UtilsGeneral.Language)index);
    }




    public async void OnButtonTitleScreen()
    {
        string question = UtilsText.AllText[UtilsText.text_yesno_question_titlescreen];

        TooltipManagerData tooltipData = new TooltipManagerData();
        tooltipData.idTooltip = UITooltipManager.ID_SHOW_YESNO;
        tooltipData.text = question;

        bool confirm = await UITooltipManager.Instance.ShowPanelYesNoCallback(tooltipData, UITooltipManager.Instance.CenterPoint.position, true);

        if(confirm)
        {

            SceneLoaderManager.Instance.LoadHome();
        }
    }

    public async void OnButtonQuit()
    {
        string question = UtilsText.AllText[UtilsText.text_yesno_question_quitgame];

        TooltipManagerData tooltipData = new TooltipManagerData();
        tooltipData.idTooltip = UITooltipManager.ID_SHOW_YESNO;
        tooltipData.text = question;

        bool confirm = await UITooltipManager.Instance.ShowPanelYesNoCallback(tooltipData, UITooltipManager.Instance.CenterPoint.position, true);

        if (confirm)
        {
            Application.Quit();
        }
    }
}
