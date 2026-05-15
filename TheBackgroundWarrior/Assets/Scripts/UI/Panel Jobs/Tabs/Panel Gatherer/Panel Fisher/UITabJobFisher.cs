using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UITabJobFisher : UITabWindow
{
    [SerializeField] UITabPlayerJob panelJob;

    [Space(10)]
    [SerializeField] TMP_Text textAvailableFishes;

    [Space(10)]
    [SerializeField] GameObject fishGroupPrefab;
    [SerializeField] Transform container;
    [SerializeField] ScrollRect scrollRectFishGroups;

    private List<GameObject> groupsObjs;

    [Space(10)]
    //[SerializeField] GameObject panelFishGroup;
    //[SerializeField] GameObject panelLog;
    //[SerializeField] TMP_Text textButtonLog;
    [SerializeField] TMP_Text textLog;


    //private bool isLogShow;



    private PlayerFisher player;


    private bool isInitialized;


    public override void Open()
    {
        base.Open();
        
        if (player == null)
        {
            player = FindFirstObjectByType<PlayerFisher>();
        }
        
        InitializeIfNeeded();

        panelJob.ChangeCurrentTab(this, UITabPlayerJob.ID_FISHER_TAB);

        // resets
        //panelFishGroup.SetActive(true);
        //panelLog.SetActive(false);

        //textButtonLog.text = "Log";

        //isLogShow = false;

        // refreshes
        RefreshGroups();

        FillLog();

        FillAvailables();
    }

    private void InitializeIfNeeded()
    {
        if (isInitialized) return;

        InitializeGroups();

        isInitialized = true;
    }

    public void OnButtonBack()
    {
        AudioManager.Instance.PlayClickUI();

        Close();
        panelJob.ChangeCurrentTab(null, -1);
    }

    private void InitializeGroups()
    {
        // clear list and refresh groups
        groupsObjs = ClearList(groupsObjs);
        FillGroups();
    }

    private void FillAvailables()
    {
        string result = "";

        // Get day moment
        UtilsGeneral.DayMoment currentMoment = UtilsGeneral.GetDayMoment();

        // Get available from day moment
        var availables = UtilsItem.GetFishByDayMoment(currentMoment);
        //Debug.Log("availables: " + availables.Count);

        // fill log with name and rarity of each fish
        foreach (var fish in availables)
        {
            string name = fish.ItemName;

            if (PlayerManager.Instance.Inventory.HasItem(fish.Id))
            {
                result += string.Format("<color=#FFFFFF>{0}</color>\n", name);
            }
            else
            {
                result += string.Format("<color=#878787>{0}</color>\n", name);
            }
        }

        textAvailableFishes.text = result;
    }

    private void RefreshGroups()
    {
        foreach (var obj in groupsObjs)
        {
            UIFisherGroupPrefab group = obj.GetComponent<UIFisherGroupPrefab>();
            group.RefreshGroup();
        }
    }

    private List<GameObject> ClearList(List<GameObject> list)
    {
        if (list == null)
            list = new List<GameObject>();

        foreach (var item in list)
        {
            Destroy(item);
        }

        list.Clear();
        return list;
    }

    private void FillGroups()
    {
        groupsObjs = new List<GameObject>();

        FishGroupSO[] groups = UtilsGather.GetAllFishGroups();

        for (int i = 0; i < groups.Length; i++)
        {
            GameObject prefab = Instantiate(fishGroupPrefab, transform.position, Quaternion.identity);
            prefab.transform.SetParent(container);

            prefab.transform.localScale = new Vector3(1, 1, 1);
            prefab.SetActive(true);

            if (prefab.TryGetComponent(out UIFisherGroupPrefab obj))
            {
                obj.Setup(scrollRectFishGroups, groups[i]);
            }
            groupsObjs.Add(prefab);
        }
    }

    public void OnButtonFish()
    {
        if (player != null)
        {
            panelJob.OnButtonClose();
        }

        LastSceneSettings settings = new LastSceneSettings();
        settings.lastSceneName = "FisherScene";
        settings.lastSceneType = SceneLoaderManager.SceneType.Fisher;

        SceneLoaderManager.Instance.LoadScene(settings);
    }
    /*
    public void OnButtonLog()
    {
        AudioManager.Instance.PlayClickUI();

        if (!isLogShow)
        {
            panelFishGroup.SetActive(false);
            panelLog.SetActive(true);

            textButtonLog.text = "Groups";

            FillLog();

            isLogShow = true;
        }
        else
        {
            panelFishGroup.SetActive(true);
            panelLog.SetActive(false);

            textButtonLog.text = "Log";

            isLogShow = false;
        }
    }
    */

    private void FillLog()
    {
        if (FishSpawnManager.Instance == null)
        {
            textLog.text = "";
        }
        else
        {
            string result = "";

            // from last caught to first
            List<FishSO> sessionFishes = new List<FishSO>();
            sessionFishes.AddRange(FishSpawnManager.Instance.CaughtFishesSession);
            sessionFishes.Reverse();

            // fill log with name and rarity of each fish
            foreach (var fish in sessionFishes)
            {
                string singleLine = string.Format(
                "<color=#{0}>{1}</color>",      // name and rarity

                UtilsItem.GetFishRarityColor(fish.FishRarity),
                fish.ItemName
                );
                result += singleLine + "\n";
            }

            textLog.text = result;
        }
    }
}
