using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITabJobFisher : UITabWindow
{
    [SerializeField] UITabPlayerJob panelJob;

    [Header("Available Log")]
    [SerializeField] GameObject _availableFishPrefab;
    [SerializeField] Transform _containerAvailable;

    private List<GameObject> _availableFishes;

    [Header("Groups")]
    [SerializeField] GameObject fishGroupPrefab;
    [SerializeField] Transform container;
    [SerializeField] ScrollRect scrollRectFishGroups;

    private List<GameObject> groupsObjs;

    [Header("Session Log")]
    [SerializeField] GameObject _sessionFishPrefab;
    [SerializeField] Transform _containerSession;

    private List<GameObject> _sessionFishes;


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

        panelJob.ChangeCurrentTab(this, UtilsPlayer.PlayerJob.Fisher);

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
        panelJob.ChangeCurrentTab(null, UtilsPlayer.PlayerJob.None);
    }

    private void InitializeGroups()
    {
        // clear list and refresh groups
        groupsObjs = ClearList(groupsObjs);
        FillGroups();
    }

    private void FillAvailables()
    {
        _availableFishes = ClearList(_availableFishes);

        //string result = "";

        // Get day moment
        UtilsGeneral.DayMoment currentMoment = UtilsGeneral.GetDayMoment();

        // Get available from day moment
        var availables = UtilsItem.GetFishByDayMoment(currentMoment);
        //Debug.Log("availables: " + availables.Count);

        // fill log with name and rarity of each fish
        foreach (var fish in availables)
        {
            string name = fish.ItemName;
            string coloredName = string.Format(
                "<color=#{0}>{1}</color>\n",      // name and rarity

                UtilsItem.GetFishRarityColor(fish.FishRarity),
                name
            );

            GameObject prefab = Instantiate(_availableFishPrefab, transform.position, Quaternion.identity);
            prefab.transform.SetParent(_containerAvailable);

            prefab.transform.localScale = new Vector3(1, 1, 1);
            prefab.SetActive(true);

            if (prefab.TryGetComponent(out UIFisherAvailableFishPrefab obj))
            {
                obj.Setup(coloredName, PlayerManager.Instance.Inventory.HasItem(fish.Id));
            }
            _availableFishes.Add(prefab);
        }
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

        FishGroupSO[] groups = UtilsFisher.GetAllFishGroups();

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
        if (player != null) return;
        /*
        if (player != null)
        {
            panelJob.OnButtonClose(false);
        }*/

        LastSceneSettings settings = new LastSceneSettings();
        settings.lastSceneName = "FisherScene";
        settings.lastSceneType = SceneLoaderManager.SceneType.Fisher;

        SceneLoaderManager.Instance.LoadScene(settings);
    }

    private void FillLog()
    {
        _sessionFishes = ClearList(_sessionFishes);

        if (FishSpawnManager.Instance == null)
        {
            return;
        }
        else
        {
            // from last caught to first
            List<(FishSO, bool)> sessionFishes = new List<(FishSO, bool)>();
            sessionFishes.AddRange(FishSpawnManager.Instance.HookedFishesSession);
            sessionFishes.Reverse();

            // fill log with name and rarity of each fish
            foreach (var fish in sessionFishes)
            {
                string name = fish.Item1.ItemName;
                string coloredName = string.Format(
                    "<color=#{0}>{1}</color>\n",      // name and rarity

                    UtilsItem.GetFishRarityColor(fish.Item1.FishRarity),
                    name
                );

                GameObject prefab = Instantiate(_sessionFishPrefab, transform.position, Quaternion.identity);
                prefab.transform.SetParent(_containerSession);

                prefab.transform.localScale = new Vector3(1, 1, 1);
                prefab.SetActive(true);

                if (prefab.TryGetComponent(out UIFisherSessionFishPrefab obj))
                {
                    obj.Setup(coloredName, fish.Item2);
                }
                _sessionFishes.Add(prefab);
            }
        }
    }
}
