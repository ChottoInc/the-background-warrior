using UnityEngine;
using UnityEngine.UI;

public class UITabSettingsGeneral : UITabWindow
{
    [SerializeField] Slider sliderMaster;

    public override void Open()
    {
        base.Open();

        Setup();
    }

    private void Setup()
    {
        sliderMaster.SetValueWithoutNotify(SettingsManager.Instance.MasterVolume);
    }




    public void OnMasterChange(float value)
    {
        SettingsManager.Instance.SetMasterVolume(value);
    }


    public async void OnButtonTitleScreen()
    {
        string question = $"Return to title screen?";

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
        string question = $"Close the game?";

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
