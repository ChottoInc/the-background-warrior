using System.Collections.Generic;
using UnityEngine;

public static class UtilsQuest
{
    public enum QuestType { Story, Daily, Bounties }

    public enum QuestObjectiveType { Kill, Obtain, LevelUp, UnlockMap, Befriend }


    private static QuestStorySO[] storySOs;
    private static QuestBountySO[] bountySOs;
    private static QuestDailySO[] dailySOs;


    public static void Initialize()
    {
        storySOs = LoadStoryQuests();
        bountySOs = LoadBountyQuests();
        dailySOs = LoadDailyQuests();
    }

    #region STORY

    private static QuestStorySO[] LoadStoryQuests()
    {
        return Resources.LoadAll<QuestStorySO>("Data/Quests/Story");
    }


    public static QuestStorySO[] GetAllStoryQuests()
    {
        return storySOs;
    }

    public static QuestStorySO GetStoryQuestById(string id)
    {
        foreach (var quest in storySOs)
        {
            if (quest.UniqueId == id)
                return quest;
        }
        return null;
    }

    #endregion

    #region BOUNTY

    private static QuestBountySO[] LoadBountyQuests()
    {
        return Resources.LoadAll<QuestBountySO>("Data/Quests/Bounty");
    }


    public static QuestBountySO[] GetAllBountyQuests()
    {
        return bountySOs;
    }

    public static QuestBountySO GetBountyQuestById(string id)
    {
        foreach (var quest in bountySOs)
        {
            if (quest.UniqueId == id)
                return quest;
        }
        return null;
    }

    public static QuestBountySO GetRandomBountyQuest()
    {
        int rand = Random.Range(0, bountySOs.Length);
        return bountySOs[rand];
    }

    #endregion

    #region DAILY

    public const int DAILY_BITS_REWARD = 20;

    private static QuestDailySO[] LoadDailyQuests()
    {
        return Resources.LoadAll<QuestDailySO>("Data/Quests/Daily");
    }


    public static QuestDailySO[] GetAllDailyQuests()
    {
        return dailySOs;
    }

    public static QuestDailySO GetDailyQuestById(string id)
    {
        foreach (var quest in dailySOs)
        {
            if (quest.UniqueId == id)
                return quest;
        }
        return null;
    }

    public static QuestDailySO GetRandomDailyQuest()
    {
        int rand = Random.Range(0, dailySOs.Length);
        return dailySOs[rand];
    }


    #endregion



    public static string GetQuestProgress(QuestData data, QuestDataProgress progress)
    {
        string result = GetQuestDescription(data);

        switch (data.questObjectiveType)
        {
            case QuestObjectiveType.Kill:
                result += string.Format("\n{0}/{1}", progress.progressCounter, data.amountKill);
                break;

            case QuestObjectiveType.Obtain:
                result += string.Format("\n{0}/{1}", progress.progressCounter, data.amountObtain);
                break;

            case QuestObjectiveType.LevelUp:
                result += string.Format("\n{0}/{1}", progress.progressCounter, data.amountStat);
                break;

            case QuestObjectiveType.UnlockMap:
                //result += string.Format("\n{0}/{1}", progress.progressCounter, data.amountStat);
                break;

            case QuestObjectiveType.Befriend:
                result += string.Format("\n{0}/{1}", progress.progressCounter, data.amountBefriend);
                break;
        }

        return result;
    }

    public static string GetQuestDescription(QuestData data)
    {
        string result = "";

        switch (data.questObjectiveType)
        {
            case QuestObjectiveType.Kill:
                if (data.questKillSpecific)
                {
                    EnemySO enemySO = UtilsEnemy.GetEnemySOById(data.monsterId);
                    result = string.Format("Kill {0} {1}", data.amountKill, enemySO.EnemyName);
                }
                else
                {
                    result = string.Format("Kill {0} monsters", data.amountKill);
                }
                break;

            case QuestObjectiveType.Obtain:
                if (data.questObtainSpecific)
                {
                    ItemSO itemSO = UtilsItem.GetItemById(data.itemId);
                    result = string.Format("Obtain {0} {1}", data.amountObtain, itemSO.ItemName);
                }
                else
                {
                    string itemType = string.Empty;

                    switch (data.itemType)
                    {
                        case UtilsItem.ItemType.Ore: itemType = "ores"; break;
                        case UtilsItem.ItemType.Card: itemType = "cards"; break;
                        case UtilsItem.ItemType.Metal: itemType = "metals"; break;
                        case UtilsItem.ItemType.Fish: itemType = "fishes"; break;
                    }

                    result = string.Format("Obtain {0} {1}", data.amountObtain, itemType);
                }
                break;

            case QuestObjectiveType.LevelUp:
                string timesString = "times";

                if (data.questLevelUpSpecific)
                {
                    string statName = UtilsPlayer.GetStatNameById(data.statId);

                    if (data.amountStat < 2)
                        timesString = "time";

                    result = string.Format("Level up {0} {1} {2}", statName, data.amountStat, timesString);
                }
                else
                {
                    if (data.amountStat < 2)
                        timesString = "time";

                    result = string.Format("Level up any stat {0} {1}", data.amountStat, timesString);
                }
                break;

            case QuestObjectiveType.UnlockMap:

                CombatMapSO mapSO = UtilsCombatMap.GetMapById(data.mapId);
                string mapName = mapSO.MapName;

                result = string.Format("Unlock {0} map", mapName);

                break;

            case QuestObjectiveType.Befriend:
                if (data.questBefriendSpecific)
                {
                    result = string.Format("Obtain {0} {1}", data.amountBefriend, data.companionSO.CompanionName);
                }
                else
                {
                    result = string.Format("Obtain {0} companions", data.amountBefriend);
                }
                break;
        }


        return result;
    }


    public static bool CanClaim(QuestData data, QuestDataProgress progress)
    {
        switch (data.questObjectiveType)
        {
            default:
            case QuestObjectiveType.Kill:
                return HandleCounterQuestCheck(data.amountKill, progress.progressCounter);

            case QuestObjectiveType.Obtain:
                return HandleCounterQuestCheck(data.amountObtain, progress.progressCounter);

            case QuestObjectiveType.Befriend:
                return HandleCounterQuestCheck(data.amountBefriend, progress.progressCounter);

            case QuestObjectiveType.LevelUp:
                return HandleCounterQuestCheck(data.amountStat, progress.progressCounter);

            case QuestObjectiveType.UnlockMap:
                return HandleCompletedQuestCheck(progress.progressCompleted);
        }
    }

    private static bool HandleCounterQuestCheck(int counter, int progress)
    {
        if (progress >= counter) return true;
        return false;
    }

    private static bool HandleCompletedQuestCheck(bool completed)
    {
        if (completed) return true;
        return false;
    }





    public static bool IsMonsterAvailable(QuestData data, List<int> availableMaps)
    {
        if (data.questKillSpecific)
        {
            // get map monsters
            List<MapToEnemiesSO> maps = new List<MapToEnemiesSO>();
            foreach (var idMap in availableMaps)
            {
                maps.Add(UtilsCombatMap.GetEnemiesByMap(idMap));
            }

            foreach (var map in maps)
            {
                bool found = UtilsCombatMap.IsEnemyInMap(data.monsterId, map);

                if (found) 
                    return true;
            }

            return false;
        }
        else
        {
            // if not specific every monster counts, so true
            return true;
        }
    }



    #region DATA

    [System.Serializable]
    public struct QuestData
    {
        public QuestObjectiveType questObjectiveType;

        // --------- Quest Kill ---------
        public bool questKillSpecific;

        // --- Specific
        public int monsterId;

        public int amountKill;

        // --------- Quest Obtain ---------
        public UtilsItem.ItemType itemType;
        public bool questObtainSpecific;

        // --- Specific
        public int itemId;

        public int amountObtain;

        // --------- Quest Level Up ---------
        public bool questLevelUpSpecific;

        // --- Specific
        public int statId;

        public int amountStat;

        // --------- Quest Unlock Map ---------

        // --- Specific
        public int mapId;


        // --------- Quest Befriend ---------
        public bool questBefriendSpecific;

        // --- Specific
        public CompanionSO companionSO;

        public int amountBefriend;


        // --------- Reward ---------
        public int rewardAmount;
    }

    [System.Serializable]
    public struct QuestDataProgress
    {
        public QuestDataProgress(QuestStorySaveData saveData)
        {
            isActive = saveData.isActive;

            progressCounter = saveData.progressCounter;
            progressCompleted = saveData.progressCompleted;

            isCleared = saveData.isCleared;
        }

        public QuestDataProgress(QuestBountySaveData saveData)
        {
            isActive = true;

            progressCounter = saveData.progressCounter;
            progressCompleted = saveData.progressCompleted;

            isCleared = saveData.isCleared;
        }

        public QuestDataProgress(QuestDailySaveData saveData)
        {
            isActive = saveData.isActive;

            progressCounter = saveData.progressCounter;
            progressCompleted = saveData.progressCompleted;

            isCleared = saveData.isCleared;
        }

        public bool isActive;

        // if need to count something
        public int progressCounter;

        // if need to check if something is completed
        public bool progressCompleted;

        public bool isCleared;
    }


    public struct HandleEventResult
    {
        public bool needSave;
        public bool needNotification;
        public int counterNotification;
    }

    public struct CustomEventData
    {
        public string questId;
        public QuestData questData;
        public QuestType questType;

        public bool needNotification;

        // on enemy killed
        public EnemySO enemySO;

        // on item obtain
        public int itemId;

        // on stat up
        public int statId;
        public int statAmount;

        // on map unlock
        public int mapId;

        // on befriend companion
        public int companionId;
    }

    #endregion
}
