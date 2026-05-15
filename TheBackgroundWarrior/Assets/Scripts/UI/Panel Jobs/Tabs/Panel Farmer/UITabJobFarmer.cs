using UnityEngine;

public class UITabJobFarmer : UITabWindow
{
    [SerializeField] UITabPlayerJob panelJob;

    [Space(10)]
    [SerializeField] UIFarmerPanelCrops panelCrops;
    [SerializeField] UIFarmerPanelCompanions panelCompanions;



    private PlayerFarmer player;



    public PlayerFarmer Player => player;


    public override void Open()
    {
        base.Open();

        if (player == null)
        {
            player = FindFirstObjectByType<PlayerFarmer>();
        }

        panelJob.ChangeCurrentTab(this, UITabPlayerJob.ID_FARMER_TAB);

        panelCrops.Setup();
        panelCompanions.gameObject.SetActive(false);
    }

    public void OnButtonBack()
    {
        AudioManager.Instance.PlayClickUI();

        Close();
        panelJob.ChangeCurrentTab(null, -1);
    }

    public void OnButtonFarm()
    {
        if (player != null)
        {
            panelJob.OnButtonClose();
        }

        LastSceneSettings settings = new LastSceneSettings();
        settings.lastSceneName = "FarmerScene";
        settings.lastSceneType = SceneLoaderManager.SceneType.Farmer;

        SceneLoaderManager.Instance.LoadScene(settings);
    }

    public void OnButtonCompanions()
    {
        panelCrops.gameObject.SetActive(false);
        panelCompanions.Setup();
    }

    public void OnButtonCrops()
    {
        panelCompanions.gameObject.SetActive(false);
        panelCrops.Setup();
    }
}
