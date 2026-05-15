using UnityEngine;

public class UITabJobGatherer : UITabWindow
{
    public const int ID_MINER_TAB = 0;

    [SerializeField] UITabPlayerJob panelJob;

    [Header("Tabs")]
    [SerializeField] UITab tabMiner;

    private int currentTab = -1;

    public override void Open()
    {
        base.Open();

        panelJob.ChangeCurrentTab(this, UITabPlayerJob.ID_MINER_TAB);

        switch (currentTab)
        {
            default:
            case ID_MINER_TAB: tabMiner.Select(); break;
        }
    }

    public void ChangeCurrentTab(int id)
    {
        currentTab = id;
    }
}
