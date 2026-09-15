using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class UtilsQuest
{
    public enum QuestType { Story, Daily, Bounties }

    public enum QuestObjectiveType { Kill, Obtain, LevelUp, UnlockMap, Befriend, SpellRank, Craft, Summon }


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

    public static bool IsQuestNextToAnother(string id)
    {
        return storySOs.Any(so => so.Nexts.Any(next => next.UniqueId == id));
    }

    public static QuestStorySO GetPreviousQuest(string id)
    {
        if (IsQuestNextToAnother(id))
        {
            return storySOs.FirstOrDefault(so => so.Nexts.Any(next => next.UniqueId == id));
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
        string result = UtilsQuestDescriptions.GetQuestDescription(data);

        switch (data.questObjectiveType)
        {
            case QuestObjectiveType.Kill:
                result += string.Format("\n{0}/{1}", progress.progressCounter, data.amountKill);
                break;

            case QuestObjectiveType.Obtain:
            case QuestObjectiveType.Craft:
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

            case QuestObjectiveType.SpellRank:
                result += string.Format("\n{0}/{1}", progress.progressCounter, data.amountRank);
                break;

            case QuestObjectiveType.Summon:
                result += string.Format("\n{0}/{1}", progress.progressCounter, data.amountSummon);
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
            case QuestObjectiveType.Craft:
                return HandleCounterQuestCheck(data.amountObtain, progress.progressCounter);

            case QuestObjectiveType.Befriend:
                return HandleCounterQuestCheck(data.amountBefriend, progress.progressCounter);

            case QuestObjectiveType.SpellRank:
                return HandleCounterQuestCheck(data.amountRank, progress.progressCounter);

            case QuestObjectiveType.LevelUp:
                return HandleCounterQuestCheck(data.amountStat, progress.progressCounter);

            case QuestObjectiveType.UnlockMap:
                return HandleCompletedQuestCheck(progress.progressCompleted);

            case QuestObjectiveType.Summon:
                return HandleCounterQuestCheck(data.amountSummon, progress.progressCounter);
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

        // --------- Quest Obtain / Craft ---------
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

        // --------- Quest Learn Spells ---------
        public bool questSpellSpecific;

        // --- Specific
        public SpellSO spellSO;

        public int amountRank;

        // --------- Quest Summon ---------

        public int amountSummon;


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

        // on rank spell level up
        public int spellId;

        // on summon
        public int summonAmount;
    }

    #endregion
}
