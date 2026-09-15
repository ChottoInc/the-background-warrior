using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UITabPlayerJob : UITabWindow
{
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
    [SerializeField] UITab tabMage;
    [SerializeField] UITab tabAlchemist;
    [SerializeField] UITab tabNecromancer;
    [SerializeField] UITab tabBard;

    private List<UIButtonJobTab> jobTabs;

    private UITabWindow currentTabWindow;
    private UtilsPlayer.PlayerJob currentTab;

    public override void Open()
    {
        base.Open();

        InitTabs();

        // Refresh to check if the job is available now
        foreach (var tab in jobTabs)
        {
            tab.Refresh();
        }

        ResetScrollUI();

        if(currentTab == UtilsPlayer.PlayerJob.None)
        {
            switch (SettingsManager.Instance.LastSceneSettings.lastSceneType)
            {
                case SceneLoaderManager.SceneType.CombatMap: currentTab = UtilsPlayer.PlayerJob.Warrior; break;
                case SceneLoaderManager.SceneType.Miner: currentTab = UtilsPlayer.PlayerJob.Miner; break;
                case SceneLoaderManager.SceneType.Blacksmith: currentTab = UtilsPlayer.PlayerJob.Blacksmith; break;
                case SceneLoaderManager.SceneType.Fisher: currentTab = UtilsPlayer.PlayerJob.Fisher; break;
                case SceneLoaderManager.SceneType.Farmer: currentTab = UtilsPlayer.PlayerJob.Farmer; break;
                case SceneLoaderManager.SceneType.Mage: currentTab = UtilsPlayer.PlayerJob.Mage; break;
                case SceneLoaderManager.SceneType.Alchemist: currentTab = UtilsPlayer.PlayerJob.Alchemist; break;
                case SceneLoaderManager.SceneType.Necromancer: currentTab = UtilsPlayer.PlayerJob.Necromancer; break;
                case SceneLoaderManager.SceneType.Bard: currentTab = UtilsPlayer.PlayerJob.Bard; break;
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
                tabFarmer.GetComponent<UIButtonJobTab>(),
                tabMage.GetComponent<UIButtonJobTab>(),
                tabAlchemist.GetComponent<UIButtonJobTab>(),
                tabNecromancer.GetComponent<UIButtonJobTab>(),
                tabBard.GetComponent<UIButtonJobTab>()
            };
        }
    }

    public void ChangeCurrentTab(UtilsPlayer.PlayerJob tab)
    {
        switch (tab)
        {
            default: ResetScrollUI(); break; // show job tree

            case UtilsPlayer.PlayerJob.Warrior: tabWarrior.Select(); break;
            case UtilsPlayer.PlayerJob.Miner: tabMiner.Select(); break;
            case UtilsPlayer.PlayerJob.Blacksmith: tabBlacksmith.Select(); break;
            case UtilsPlayer.PlayerJob.Fisher: tabFisher.Select(); break;
            case UtilsPlayer.PlayerJob.Farmer: tabFarmer.Select(); break;
            case UtilsPlayer.PlayerJob.Mage: tabMage.Select(); break;
            case UtilsPlayer.PlayerJob.Alchemist: tabAlchemist.Select(); break;
            case UtilsPlayer.PlayerJob.Necromancer: tabNecromancer.Select(); break;
            case UtilsPlayer.PlayerJob.Bard: tabBard.Select(); break;
        }

        ChangeTitleText(tab);
    }

    public void ChangeCurrentTab(UITabWindow window, UtilsPlayer.PlayerJob tab)
    {
        if(tab != UtilsPlayer.PlayerJob.None)
        {
            panelScroll.gameObject.SetActive(false);
            currentTabWindow = window;
        }
        else
        {
            currentTabWindow = null;
            ResetScrollUI();
        }

        ChangeTitleText(tab);
        currentTab = tab;
    }

    private void ChangeTitleText(UtilsPlayer.PlayerJob tab)
    {
        switch (tab)
        {
            default: textJob.text = UtilsText.AllText[UtilsText.text_title_jobs]; break;
            case UtilsPlayer.PlayerJob.Warrior: textJob.text = UtilsText.AllText[UtilsText.text_button_help_filter_warrior]; break;
            case UtilsPlayer.PlayerJob.Miner: textJob.text = UtilsText.AllText[UtilsText.text_button_help_filter_miner]; break;
            case UtilsPlayer.PlayerJob.Blacksmith: textJob.text = UtilsText.AllText[UtilsText.text_button_help_filter_blacksmith]; break;
            case UtilsPlayer.PlayerJob.Fisher: textJob.text = UtilsText.AllText[UtilsText.text_button_help_filter_fisher]; break;
            case UtilsPlayer.PlayerJob.Farmer: textJob.text = UtilsText.AllText[UtilsText.text_button_help_filter_farmer]; break;
            case UtilsPlayer.PlayerJob.Mage: textJob.text = UtilsText.AllText[UtilsText.text_button_help_filter_mage]; break;
            case UtilsPlayer.PlayerJob.Alchemist: textJob.text = UtilsText.AllText[UtilsText.text_button_help_filter_alchemist]; break;
            case UtilsPlayer.PlayerJob.Necromancer: textJob.text = UtilsText.AllText[UtilsText.text_button_help_filter_necromancer]; break;
            case UtilsPlayer.PlayerJob.Bard: textJob.text = UtilsText.AllText[UtilsText.text_button_help_filter_bard]; break;
        }
    }


    private void ResetScrollUI()
    {
        panelScroll.normalizedPosition = new Vector2(0.5f, 0.5f);
        panelScroll.gameObject.SetActive(true);
    }



    public void OnButtonClose(bool makeSound = true)
    {
        if(currentTabWindow == null)
        {
            if(makeSound)
                AudioManager.Instance.PlayClickUI();

            Close();
        }
        else
        {
            if (currentTabWindow.CanClose())
            {
                if (makeSound)
                    AudioManager.Instance.PlayClickUI();

                Close();
            }
        }
    }
}
