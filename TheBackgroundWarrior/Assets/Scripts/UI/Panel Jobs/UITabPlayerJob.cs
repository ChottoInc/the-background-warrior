using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UITabPlayerJob : UITabWindow
{
    public const int ID_WARRIOR_TAB = 0;
    public const int ID_MINER_TAB = 1;
    public const int ID_BLACKSMITH_TAB = 2;
    public const int ID_FISHER_TAB = 3;
    public const int ID_FARMER_TAB = 4;

    [Header("Title")]
    [SerializeField] TMP_Text textJob;

    [Header("Job Tree")]
    [SerializeField] ScrollRect panelScroll;
    //[SerializeField] UIButtonJobTab[] jobTabs;

    [Header("Windows")]
    [SerializeField] UITab tabWarrior;
    [SerializeField] UITab tabMiner;
    [SerializeField] UITab tabBlacksmith;
    [SerializeField] UITab tabFisher;
    [SerializeField] UITab tabFarmer;

    private List<UIButtonJobTab> jobTabs;

    private UITabWindow currentTabWindow;
    private int currentTab = -1;

    public override void Open()
    {
        base.Open();

        InitTabs();

        // Refresh to check if the job is available now
        foreach (var tab in jobTabs)
        {
            tab.Refresh();
        }

        if(currentTab == -1)
        {
            switch (SettingsManager.Instance.LastSceneSettings.lastSceneType)
            {
                case SceneLoaderManager.SceneType.CombatMap: currentTab = ID_WARRIOR_TAB; break;
                case SceneLoaderManager.SceneType.Miner: currentTab = ID_MINER_TAB; break;
                case SceneLoaderManager.SceneType.Blacksmith: currentTab = ID_BLACKSMITH_TAB; break;
                case SceneLoaderManager.SceneType.Fisher: currentTab = ID_FISHER_TAB; break;
                case SceneLoaderManager.SceneType.Farmer: currentTab = ID_FARMER_TAB; break;
            }
            ChangeCurrentTab(currentTab);
        }
        else
        {
            ChangeCurrentTab(currentTab);
        }
    }

    private void InitTabs()
    {
        if (jobTabs == null)
        {
            jobTabs = new List<UIButtonJobTab>
            {
                tabWarrior.GetComponent<UIButtonJobTab>(),
                tabMiner.GetComponent<UIButtonJobTab>(),
                tabBlacksmith.GetComponent<UIButtonJobTab>(),
                tabFisher.GetComponent<UIButtonJobTab>(),
                tabFarmer.GetComponent<UIButtonJobTab>()
            };
        }
    }

    public void ChangeCurrentTab(int tab)
    {
        switch (tab)
        {
            default: ResetScrollUI(); break; // show job tree

            case ID_WARRIOR_TAB: tabWarrior.Select(); break;
            case ID_MINER_TAB: tabMiner.Select(); break;
            case ID_BLACKSMITH_TAB: tabBlacksmith.Select(); break;
            case ID_FISHER_TAB: tabFisher.Select(); break;
            case ID_FARMER_TAB: tabFarmer.Select(); break;
        }

        ChangeTitleText(tab);
    }

    public void ChangeCurrentTab(UITabWindow window, int id)
    {
        if(id != -1)
        {
            panelScroll.gameObject.SetActive(false);
            currentTabWindow = window;
        }
        else
        {
            currentTabWindow = null;
            ResetScrollUI();
        }

        ChangeTitleText(id);
        currentTab = id;
    }

    private void ChangeTitleText(int idTab)
    {
        switch (idTab)
        {
            default:
                textJob.text = "Jobs";
                break;

            case ID_WARRIOR_TAB:
                textJob.text = "Warrior";
                break;

            case ID_MINER_TAB:
                textJob.text = "Miner";
                break;

            case ID_BLACKSMITH_TAB:
                textJob.text = "Blacksmith";
                break;

            case ID_FISHER_TAB:
                textJob.text = "Fisher";
                break;

            case ID_FARMER_TAB:
                textJob.text = "Farmer";
                break;
        }
    }


    private void ResetScrollUI()
    {
        panelScroll.normalizedPosition = new Vector2(0.5f, 0.5f);
        panelScroll.gameObject.SetActive(true);
    }



    public void OnButtonClose()
    {
        if(currentTabWindow == null)
        {
            AudioManager.Instance.PlayClickUI();
            Close();
        }
        else
        {
            if (currentTabWindow.CanClose())
            {
                AudioManager.Instance.PlayClickUI();
                Close();
            }
        }
    }
}
