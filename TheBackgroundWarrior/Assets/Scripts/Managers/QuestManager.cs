using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using static UtilsQuest;

public class QuestManager : MonoBehaviour
{
    private const int TOT_DAILY_QUEST = 3;

    private IDataService saveService;

    private QuestEventHandler questEventHandler;

    // --- STORY QUESTS

    // Store every quest progress ever made
    public Dictionary<string, QuestDataProgress> DictQuestsStoryProgress { get; private set; }

    // List of all active story quests
    public List<string> ActiveStoryQuests { get; private set; }







    // --- BOUNTY QUESTS


    // Store slot and its active bounty quests
    public Dictionary<string, QuestDataProgress> DictQuestsBountyProgress { get; private set; }

    public Dictionary<int, string> ActiveBountyQuests { get; private set; }



    public bool HasInitBountiesRefresh { get; private set; }

    public long LastCheckBountiesRefreshDate { get; private set; }

    public List<string> CurrentPulledBounties { get; private set; }
    public List<string> AcceptedPulledBounties { get; private set; }


    public bool CanRefreshBounties
    {
        get
        {
            // only works when game is launched for the first time
            if(!HasInitBountiesRefresh)
            {
                //Debug.Log("refresh first time");
                return true;
            }
            else
            {
                // get current ticks
                long currentCheckBounties = DateTime.UtcNow.Ticks;

                // make the difference
                TimeSpan difference = new TimeSpan(currentCheckBounties - LastCheckBountiesRefreshDate);
                //Debug.Log("difference (m): " + difference.Minutes);

                // check for time
                if (difference.Minutes >= 20)
                {
                    // update last check, only in the moment when you can actually refresh
                    LastCheckBountiesRefreshDate = currentCheckBounties;

                    //Debug.Log("refresh second time");
                    return true;
                }
                else
                {
                    //Debug.Log("can't refresh");
                    return false;
                }
            }
        }
    }



    // --- DAILY QUESTS

    // Store every quest progress for daily
    public Dictionary<string, QuestDataProgress> DictQuestsDailyProgress { get; private set; }

    // List of all active daily quests
    public List<string> ActiveDailyQuests { get; private set; }


    public long LastDailyCreationDate { get; private set; }






    // check if player manager has been setup
    private bool isPlayerObserverInit;

    // check if quest manager is done setupping
    private bool isSetup;


    private PlayerFight playerFight;
    private PlayerMiner playerMiner;
    private PlayerBlacksmith playerBlacksmith;
    private PlayerFisher playerFisher;
    private PlayerFarmer playerFarmer;



    public event Action OnNeedNotification;



    public static QuestManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        if (Instance != this) return;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        if (Instance != this) return;

        SceneManager.sceneLoaded -= OnSceneLoaded;

        //Called here because is destroyd on scene switch

        if (CombatManager.Instance != null)
            CombatManager.Instance.OnEnemyKill -= questEventHandler.OnEnemyKilled;

        if (playerFight != null)
        {
            playerFight.OnStatChange -= questEventHandler.OnStatUp;
            playerFight.OnAddMap -= questEventHandler.OnAddMap;
        }

        if (playerMiner != null)
            playerMiner.OnStatChange -= questEventHandler.OnStatUp;

        if (playerBlacksmith != null)
            playerBlacksmith.OnStatChange -= questEventHandler.OnStatUp;

        if (playerFisher != null)
            playerFisher.OnStatChange -= questEventHandler.OnStatUp;

        if (playerFarmer != null)
            playerFarmer.OnStatChange -= questEventHandler.OnStatUp;
    }

    private void OnDestroy()
    {
        if (Instance != this) return;

        // Called here because player manager is non destroyable
        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.OnItemAdd -= questEventHandler.OnItemObtain;
            PlayerManager.Instance.OnCompanionBefriended -= questEventHandler.OnBefriend;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // if home only
        if (scene.name == "HomeScene") return;


        LastSceneSettings settings = SettingsManager.Instance.LastSceneSettings;

        switch (settings.lastSceneType)
        {
            case SceneLoaderManager.SceneType.CombatMap:
                playerFight = FindFirstObjectByType<PlayerFight>();
                playerFight.OnStatChange += questEventHandler.OnStatUp;
                playerFight.OnAddMap += questEventHandler.OnAddMap;

                CombatManager.Instance.OnEnemyKill += questEventHandler.OnEnemyKilled;
                break;

            case SceneLoaderManager.SceneType.Miner:
                playerMiner = FindFirstObjectByType<PlayerMiner>();
                playerMiner.OnStatChange += questEventHandler.OnStatUp;
                break;

            case SceneLoaderManager.SceneType.Blacksmith:
                playerBlacksmith = FindFirstObjectByType<PlayerBlacksmith>();
                playerBlacksmith.OnStatChange += questEventHandler.OnStatUp;
                break;

            case SceneLoaderManager.SceneType.Fisher:
                playerFisher = FindFirstObjectByType<PlayerFisher>();
                playerFisher.OnStatChange += questEventHandler.OnStatUp;
                break;

            case SceneLoaderManager.SceneType.Farmer:
                playerFarmer = FindFirstObjectByType<PlayerFarmer>();
                playerFarmer.OnStatChange += questEventHandler.OnStatUp;
                break;
        }
    }

    private void Update()
    {
        if (isPlayerObserverInit) return;

        if(PlayerManager.Instance != null && isSetup)
        {
            PlayerManager.Instance.OnItemAdd += questEventHandler.OnItemObtain;
            PlayerManager.Instance.OnCompanionBefriended += questEventHandler.OnBefriend;
            isPlayerObserverInit = true;
        }
    }

    //Called after player manager
    public void Setup(IDataService service)
    {
        saveService = service;

        try
        {
            QuestsSaveData saveData = saveService.LoadData<QuestsSaveData>(UtilsSave.GetQuestFile(), SettingsManager.Instance.FileEncryption);
            SetupFromFile(saveData);
        }
        catch
        {
            SetupFromDefault();
            SaveQuestsData();

            //Debug.Log("Datas quest: " + DictQuestsStoryProgress.Count);
            //Debug.Log("Datas quest active: " + ActiveStoryQuests.Count);
        }

        questEventHandler = new QuestEventHandler();

        isSetup = true;
    }

    #region DEFAULT

    private void SetupFromDefault()
    {
        HasInitBountiesRefresh = false;
        LastDailyCreationDate = DateTime.UtcNow.Ticks;

        CurrentPulledBounties = new List<string>();
        AcceptedPulledBounties = new List<string>();

        InitializeStoryQuests();
        InitializeBountyQuests();
        InitializeDailyQuests();
    }

    /// <summary>
    /// Initialize all story quests, call even when loading from file to check new added quests
    /// </summary>
    private void InitializeStoryQuests()
    {
        // initialize dict and first actives quests

        if(ActiveStoryQuests == null)
            ActiveStoryQuests = new List<string>();

        if(DictQuestsStoryProgress == null)
            DictQuestsStoryProgress = new Dictionary<string, QuestDataProgress>();

        // create default for every story
        QuestStorySO[] storyQuests = GetAllStoryQuests();
        for (int i = 0; i < storyQuests.Length; i++)
        {
            // get so
            QuestStorySO so = storyQuests[i];

            // check if already in dictionary
            if(!DictQuestsStoryProgress.ContainsKey(so.UniqueId))
            {
                // create progress
                QuestDataProgress questProgress = new QuestDataProgress();

                questProgress.isActive = so.IsActiveFromStart;

                // add to active if from start SO
                if (so.IsActiveFromStart)
                {
                    ActiveStoryQuests.Add(so.UniqueId);
                }

                questProgress.progressCounter = 0;
                questProgress.progressCompleted = false;

                questProgress.isCleared = false;

                // save in dictionary
                DictQuestsStoryProgress.Add(so.UniqueId, questProgress);
            }
        }
    }

    private void InitializeBountyQuests()
    {
        // initialize dict and first actives quests
        ActiveBountyQuests = new Dictionary<int, string>();
        DictQuestsBountyProgress = new Dictionary<string, QuestDataProgress>();
    }

    /// <summary>
    /// Called when file is empty, or when comparing dates and day has changed
    /// </summary>
    private void InitializeDailyQuests()
    {
        // initialize dict and first actives quests
        ActiveDailyQuests = new List<string>();
        DictQuestsDailyProgress = new Dictionary<string, QuestDataProgress>();

        //TODO:  change 3 with const value or random one between values
        for (int i = 0; i < TOT_DAILY_QUEST; i++)
        {
            int tries = 0;
            int MAX_TRIES = 1000;
            bool valid;

            QuestDailySO daily;

            do
            {
                valid = true;

                daily = GetRandomDailyQuest();

                // check if daily quest has already been pulled and added to dailies
                if (ActiveDailyQuests.Contains(daily.UniqueId))
                    valid = false;

                if (valid)
                {
                    // check if player has the job for the quest
                    if (!daily.AvailableFor.SharesAnyValueWith(PlayerManager.Instance.PlayerJobsData.AvailableJobs))
                        valid = false;

                    // check if daily need to increase a stat level
                    if(daily.QuestData.questObjectiveType == QuestObjectiveType.LevelUp)
                    {
                        if (daily.QuestData.questLevelUpSpecific)
                        {
                            // if stat is already at max, discard
                            int statId = daily.QuestData.statId;

                            int currentLevel = UtilsPlayer.GetStatCurrentLevelById(statId);
                            int maxLevel = UtilsPlayer.GetStatMaxLevelById(statId);

                            if (currentLevel >= maxLevel)
                                valid = false;
                        }
                        else
                        {
                            // if stat is not specific, check for all stat maxed out
                            if (UtilsPlayer.AreStatsMaxedOut())
                                valid = false;
                        }
                    }
                    
                    // check if enemy is currently available in your maps
                    if(!IsMonsterAvailable(daily.QuestData, PlayerManager.Instance.PlayerFightData.AvailableMaps))
                        valid = false;
                }

                tries++;
            } while (!valid && tries < MAX_TRIES);

            if (valid)
            {
                // add to active and create progress
                ActiveDailyQuests.Add(daily.UniqueId);

                QuestDataProgress questProgress = new QuestDataProgress();
                questProgress.isActive = true;

                questProgress.progressCounter = 0;
                questProgress.progressCompleted = false;
                
                questProgress.isCleared = false;

                // save in dictionary
                DictQuestsDailyProgress.Add(daily.UniqueId, questProgress);
            }
        }
    }

    #endregion

    #region FROM FILE

    private void SetupFromFile(QuestsSaveData saveData)
    {
        HasInitBountiesRefresh = saveData.hasInitBountiesRefresh;
        LastCheckBountiesRefreshDate = saveData.lastCheckBountiesRefreshDate;
        LastDailyCreationDate = saveData.lastDailyCreationDate;

        // bounties pulled list
        CurrentPulledBounties = new List<string>();
        CurrentPulledBounties.AddRange(saveData.currentPulledBounties);

        // bounties pulled list
        AcceptedPulledBounties = new List<string>();
        AcceptedPulledBounties.AddRange(saveData.acceptedPulledBounties);

        // refresh story quests if new ones are added
        InitializeStoryQuests();

        LoadStoryQuests(saveData.storySaveDatas);

        LoadBountyQuests(saveData.bountySaveDatas);

        // check for daily using date
        DateTime lastDailyDate = new DateTime(LastDailyCreationDate, DateTimeKind.Utc);
        if(DateTime.UtcNow.Date != lastDailyDate.Date)
        {
            // save new date
            LastDailyCreationDate = DateTime.UtcNow.Ticks;

            // refresh dailies
            InitializeDailyQuests();
            SaveQuestsData();
        }
        else
        {
            LoadDailyQuests(saveData.dailySaveDatas);
        }
    }

    private void LoadStoryQuests(List<QuestStorySaveData> datas)
    {
        if (ActiveStoryQuests == null)
            ActiveStoryQuests = new List<string>();

        if (DictQuestsStoryProgress == null)
            DictQuestsStoryProgress = new Dictionary<string, QuestDataProgress>();

        // used for debug infos
        int exceptionIndex = 0;

        try
        {
            // for every found save, update dictionary
            for (int i = 0; i < datas.Count; i++)
            {
                exceptionIndex = i;

                // save in dictionary
                QuestDataProgress dataProgress = new QuestDataProgress(datas[i]);

                DictQuestsStoryProgress[datas[i].questId] = dataProgress;

                // if save is inactive but active by default, remove from list
                if (!dataProgress.isActive && ActiveStoryQuests.Contains(datas[i].questId))
                {
                    ActiveStoryQuests.Remove(datas[i].questId);
                }

                // instead add to active if active from save and not active by default
                if (dataProgress.isActive && !ActiveStoryQuests.Contains(datas[i].questId))
                {
                    ActiveStoryQuests.Add(datas[i].questId);

                    // check on load if unlock map quest is completed
                    if (IsReachedMapQuest(datas[i].questId))
                    {
                        UpdateQuestAlreadyReachedMap(datas[i].questId);
                    }
                }
            }
        }
        catch
        {
            Debug.LogError("Can't load quest data id: " + datas[exceptionIndex].questId);
        }

        //Debug.Log("Dictionary quests counter: " + DictQuestsStoryProgress.Count);
    }

    private void LoadBountyQuests(List<QuestBountySaveData> datas)
    {
        ActiveBountyQuests = new Dictionary<int, string>();
        DictQuestsBountyProgress = new Dictionary<string, QuestDataProgress>();

        // used for debug infos
        int exceptionIndex = 0;

        try
        {
            for (int i = 0; i < datas.Count; i++)
            {
                exceptionIndex = i;

                bool valid = true;
                QuestBountySO so = GetBountyQuestById(datas[i].questId);

                // check if player can do it
                if (!so.AvailableFor.SharesAnyValueWith(PlayerManager.Instance.PlayerJobsData.AvailableJobs))
                    valid = false;

                // check if enemy is currently available in your maps
                if (!IsMonsterAvailable(so.QuestData, PlayerManager.Instance.PlayerFightData.AvailableMaps))
                    valid = false;

                // only if the bounty clears the check is added to actives from save
                if (valid)
                {
                    // save in dictionary
                    QuestDataProgress dataProgress = new QuestDataProgress(datas[i]);
                    DictQuestsBountyProgress.Add(datas[i].questId, dataProgress);

                    ActiveBountyQuests.Add(datas[i].slotTab, datas[i].questId);
                }
            }
        }
        catch
        {
            Debug.LogError("Can't load quest data id: " + datas[exceptionIndex].questId);
        }

        //Debug.Log("Dictionary quests counter: " + DictQuestsBountyProgress.Count);
    }

    public bool IsBountyActiveById(string id)
    {
        foreach (var activeId in ActiveBountyQuests.Values)
        {
            if (activeId == id)
                return true;
        }
        return false;
    }

    public void FillPossibleBountiesList(List<string> list)
    {
        if (CurrentPulledBounties == null)
            CurrentPulledBounties = new List<string>();

        CurrentPulledBounties.Clear();
        CurrentPulledBounties.AddRange(list);
    }

    public void FillAcceptedBountiesList(List<string> list)
    {
        if (AcceptedPulledBounties == null)
            AcceptedPulledBounties = new List<string>();

        AcceptedPulledBounties.Clear();
        AcceptedPulledBounties.AddRange(list);
    }

    private void LoadDailyQuests(List<QuestDailySaveData> datas)
    {
        ActiveDailyQuests = new List<string>();
        DictQuestsDailyProgress = new Dictionary<string, QuestDataProgress>();

        // used for debug infos
        int exceptionIndex = 0;

        try
        {
            for (int i = 0; i < datas.Count; i++)
            {
                exceptionIndex = i;

                // save in dictionary
                QuestDataProgress dataProgress = new QuestDataProgress(datas[i]);
                DictQuestsDailyProgress.Add(datas[i].questId, dataProgress);

                // set active from reading
                if (dataProgress.isActive)
                {
                    ActiveDailyQuests.Add(datas[i].questId);
                }
            }
        }
        catch
        {
            Debug.LogError("Can't load quest data id: " + datas[exceptionIndex].questId);
        }

        //Debug.Log("Dictionary quests counter: " + DictQuestsStoryProgress.Count);
    }

    #endregion

    public void AddActiveBountyQuest(int slot, string id)
    {
        // add to active
        ActiveBountyQuests.Add(slot, id);

        // add to dictionary
        QuestDataProgress progress = new QuestDataProgress();
        progress.isActive = true;
        DictQuestsBountyProgress.Add(id, progress);
    }

    #region STORY

    public void SetStoryQuestCleared(string id)
    {
        QuestDataProgress progressData = DictQuestsStoryProgress[id];
        progressData.isCleared = true;
        DictQuestsStoryProgress[id] = progressData;
    }

    public void UpdateStoryQuests()
    {
        // copy of dictionary with new progress info
        Dictionary<string, QuestDataProgress> copyDict = new Dictionary<string, QuestDataProgress>();

        // list of unlock quests that need to be updated after the new quests are added
        List<string> unlockMapUpdates = new List<string>();

        foreach (var pair in DictQuestsStoryProgress)
        {
            if (pair.Value.isCleared && pair.Value.isActive)
            {
                QuestStorySO so = GetStoryQuestById(pair.Key);

                // get next quests
                var nexts = so.Nexts;

                // set quest progress as cleared
                QuestDataProgress copyProgress = pair.Value;
                copyProgress.isActive = false;
                
                // new progress into copy dictionary
                copyDict.Add(pair.Key, copyProgress);

                // update list of actives
                if(nexts != null)
                {
                    foreach(var next in nexts)
                    {
                        ActiveStoryQuests.Add(next.UniqueId);

                        if (IsReachedMapQuest(next.UniqueId))
                        {
                            unlockMapUpdates.Add(next.UniqueId);
                        }
                    }
                }

                ActiveStoryQuests.Remove(pair.Key);
                //Debug.Log("removed story quest: " + pair.Key);
            }
            else
            {
                copyDict.Add(pair.Key, pair.Value);
            }
        }

        DictQuestsStoryProgress = copyDict;

        // update progress after list update
        foreach (var active in ActiveStoryQuests)
        {
            QuestDataProgress copyProgress = DictQuestsStoryProgress[active];
            copyProgress.isActive = true;
            DictQuestsStoryProgress[active] = copyProgress;
        }

        // add to dict updated unlock maps
        foreach (var map in unlockMapUpdates)
        {
            UpdateQuestAlreadyReachedMap(map);
        }

        SaveQuestsData();
    }

    private bool IsReachedMapQuest(string questId)
    {
        QuestStorySO nextSO = GetStoryQuestById(questId);
        if (nextSO.QuestData.questObjectiveType == QuestObjectiveType.UnlockMap)
        {
            if (PlayerManager.Instance.PlayerFightData.AvailableMaps.Contains(nextSO.QuestData.mapId))
            {
                return true;
            }
        }
        return false;
    }

    private void UpdateQuestAlreadyReachedMap(string questId)
    {
        QuestStorySO so = GetStoryQuestById(questId);
        questEventHandler.OnAddMap(so.QuestData.mapId);
    }

    #endregion

    #region BOUNTY

    public void SetBountyQuestCleared(string id)
    {
        QuestDataProgress progressData = DictQuestsBountyProgress[id];
        progressData.isCleared = true;
        DictQuestsBountyProgress[id] = progressData;
    }

    public void UpdateBountyQuests()
    {
        // copy of dictionary active bounties
        Dictionary<int, string> copyDict = new Dictionary<int, string>();

        // cycle active bounties
        foreach (var pair in ActiveBountyQuests)
        {
            // check progress
            QuestDataProgress progress = DictQuestsBountyProgress[pair.Value];
            if (progress.isCleared)
            {
                // remove progerss from dictionary
                DictQuestsBountyProgress.Remove(pair.Value);
            }
            else
            {
                copyDict.Add(pair.Key, pair.Value);
            }
        }

        ActiveBountyQuests = copyDict;

        SaveQuestsData();
    }

    public void SetHasInitBountyFirstTime()
    {
        HasInitBountiesRefresh = true;
        LastCheckBountiesRefreshDate = DateTime.UtcNow.Ticks;
    }

    #endregion

    #region DAILY

    public void ClearDailyQuests()
    {
        ActiveDailyQuests.Clear();
        DictQuestsDailyProgress.Clear();
    }

    #endregion

    public void TriggerNotification()
    {
        OnNeedNotification?.Invoke();
    }


    #region EVENT ACTIONS

    /// <summary>
    /// Check the progress and returns if the quest can be claimed
    /// </summary>
    public bool CheckNotifications(QuestData data, QuestType questType, string questId)
    {
        QuestDataProgress progress;

        switch (questType)
        {
            default:
            case QuestType.Story:
                progress = DictQuestsStoryProgress[questId];
                break;

            case QuestType.Bounties:
                progress = DictQuestsBountyProgress[questId];
                break;

            case QuestType.Daily:
                progress = DictQuestsDailyProgress[questId];
                break;
        }

        return CanClaim(data, progress);
    }

    #endregion



    public void SaveQuestsData()
    {
        QuestsSaveData data = new QuestsSaveData(this);
        saveService.SaveData(UtilsSave.GetQuestFile(), data, SettingsManager.Instance.FileEncryption);
    }
}
