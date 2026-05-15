using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UtilsQuest;

public class QuestEventHandler
{
    #region ENEMY KILLED EVENT

    public void OnEnemyKilled(EnemySO enemySO)
    {
        // create custom event basic data
        CustomEventData customEventData = new CustomEventData();
        customEventData.enemySO = enemySO;

        HandleEvent(customEventData, HandleEnemyKilled);
    }

    private HandleEventResult HandleEnemyKilled(CustomEventData eventData)
    {
        HandleEventResult result = new HandleEventResult()
        {
            needSave = false,
            needNotification = false
        };

        if (NeedUpdateKillProgress(eventData.questData, eventData.enemySO))
        {
            UpdateKillProgress(eventData.questType, eventData.questId);
            result.needSave = true;

            result.needNotification = QuestManager.Instance.CheckNotifications(eventData.questData, eventData.questType, eventData.questId);
        }

        return result;
    }

    private bool NeedUpdateKillProgress(QuestData data, EnemySO enemySO)
    {
        if (data.questObjectiveType == QuestObjectiveType.Kill)
        {
            // check specific
            if (data.questKillSpecific)
            {
                // Check actual pooling name, since the prefabs are identical
                // If the check is on the id, different monsters data would be compared instead of monster type
                string enemyName = UtilsEnemy.GetEnemySOById(data.monsterId).EnemyPoolName;
                if (enemyName == enemySO.EnemyPoolName)
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }
        return false;
    }

    private void UpdateKillProgress(QuestType questType, string questId)
    {
        QuestDataProgress progress;

        switch (questType)
        {
            default:
            case QuestType.Story:
                progress = QuestManager.Instance.DictQuestsStoryProgress[questId];

                progress.progressCounter++;
                QuestManager.Instance.DictQuestsStoryProgress[questId] = progress;
                break;

            case QuestType.Bounties:
                progress = QuestManager.Instance.DictQuestsBountyProgress[questId];

                progress.progressCounter++;
                QuestManager.Instance.DictQuestsBountyProgress[questId] = progress;
                break;

            case QuestType.Daily:
                progress = QuestManager.Instance.DictQuestsDailyProgress[questId];

                progress.progressCounter++;
                QuestManager.Instance.DictQuestsDailyProgress[questId] = progress;
                break;
        }
    }

    #endregion

    #region ITEM OBTAIN EVENT

    public void OnItemObtain(int id)
    {
        // create custom event basic data
        CustomEventData customEventData = new CustomEventData();
        customEventData.itemId = id;

        HandleEvent(customEventData, HandleItemObtain);
    }

    private HandleEventResult HandleItemObtain(CustomEventData eventData)
    {
        HandleEventResult result = new HandleEventResult()
        {
            needSave = false,
            needNotification = false
        };

        if (NeedUpdateObtainProgress(eventData.questData, eventData.itemId))
        {
            UpdateObtainProgress(eventData.questType, eventData.questId);
            result.needSave = true;

            result.needNotification = QuestManager.Instance.CheckNotifications(eventData.questData, eventData.questType, eventData.questId);
        }

        return result;
    }

    private bool NeedUpdateObtainProgress(QuestData data, int itemId)
    {
        if (data.questObjectiveType == QuestObjectiveType.Obtain)
        {
            // check specific
            if (data.questObtainSpecific)
            {
                if (data.itemId == itemId)
                {
                    return true;
                }
            }
            else
            {
                ItemSO so = UtilsItem.GetItemById(itemId);
                if (data.itemType == so.ItemType)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void UpdateObtainProgress(QuestType questType, string questId)
    {
        QuestDataProgress progress;

        switch (questType)
        {
            default:
            case QuestType.Story:
                progress = QuestManager.Instance.DictQuestsStoryProgress[questId];

                progress.progressCounter++;
                QuestManager.Instance.DictQuestsStoryProgress[questId] = progress;
                break;

            case QuestType.Bounties:
                progress = QuestManager.Instance.DictQuestsBountyProgress[questId];

                progress.progressCounter++;
                QuestManager.Instance.DictQuestsBountyProgress[questId] = progress;
                break;

            case QuestType.Daily:
                progress = QuestManager.Instance.DictQuestsDailyProgress[questId];

                progress.progressCounter++;
                QuestManager.Instance.DictQuestsDailyProgress[questId] = progress;
                break;
        }
    }

    #endregion

    #region STAT UP EVENT

    public void OnStatUp(int id, int amount)
    {
        // create custom event basic data
        CustomEventData customEventData = new CustomEventData();
        customEventData.statId = id;
        customEventData.statAmount = amount;

        HandleEvent(customEventData, HandleStatUp);
    }

    private HandleEventResult HandleStatUp(CustomEventData eventData)
    {
        HandleEventResult result = new HandleEventResult()
        {
            needSave = false,
            needNotification = false
        };

        if (NeedUpdateStatUpProgress(eventData.questData, eventData.statId))
        {
            UpdateStatLevelUpProgress(eventData.questType, eventData.questId, eventData.statAmount);
            result.needSave = true;
            result.needNotification = QuestManager.Instance.CheckNotifications(eventData.questData, eventData.questType, eventData.questId);
        }

        return result;
    }

    private bool NeedUpdateStatUpProgress(QuestData data, int statId)
    {
        if (data.questObjectiveType == QuestObjectiveType.LevelUp)
        {
            // check specific
            if (data.questLevelUpSpecific)
            {
                if (data.statId == statId)
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }
        return false;
    }

    private void UpdateStatLevelUpProgress(QuestType questType, string questId, int amount)
    {
        QuestDataProgress progress;

        switch (questType)
        {
            default:
            case QuestType.Story:
                progress = QuestManager.Instance.DictQuestsStoryProgress[questId];

                progress.progressCounter += amount;
                QuestManager.Instance.DictQuestsStoryProgress[questId] = progress;
                break;

            case QuestType.Bounties:
                progress = QuestManager.Instance.DictQuestsBountyProgress[questId];

                progress.progressCounter += amount;
                QuestManager.Instance.DictQuestsBountyProgress[questId] = progress;
                break;

            case QuestType.Daily:
                progress = QuestManager.Instance.DictQuestsDailyProgress[questId];

                progress.progressCounter += amount;
                QuestManager.Instance.DictQuestsDailyProgress[questId] = progress;
                break;
        }
    }

    #endregion

    #region ADD MAP EVENT

    public void OnAddMap(int id)
    {
        // create custom event basic data
        CustomEventData customEventData = new CustomEventData();
        customEventData.mapId = id;

        HandleEvent(customEventData, HandleAddMap);
    }

    private HandleEventResult HandleAddMap(CustomEventData eventData)
    {
        HandleEventResult result = new HandleEventResult()
        {
            needSave = false,
            needNotification = false
        };

        if (NeedUpdateUnlockMapProgress(eventData.questData, eventData.mapId))
        {
            UpdateUnlockMapProgress(eventData.questType, eventData.questId);
            result.needSave = true;
            result.needNotification = QuestManager.Instance.CheckNotifications(eventData.questData, eventData.questType, eventData.questId);
        }

        return result;
    }

    private bool NeedUpdateUnlockMapProgress(QuestData data, int mapId)
    {
        if (data.questObjectiveType == QuestObjectiveType.UnlockMap)
        {
            if (data.mapId == mapId)
                return true;
        }
        return false;
    }

    private void UpdateUnlockMapProgress(QuestType questType, string questId)
    {
        QuestDataProgress progress;

        switch (questType)
        {
            default:
            case QuestType.Story:
                progress = QuestManager.Instance.DictQuestsStoryProgress[questId];

                progress.progressCompleted = true;
                QuestManager.Instance.DictQuestsStoryProgress[questId] = progress;
                break;

            case QuestType.Bounties:
                progress = QuestManager.Instance.DictQuestsBountyProgress[questId];

                progress.progressCompleted = true;
                QuestManager.Instance.DictQuestsBountyProgress[questId] = progress;
                break;

            case QuestType.Daily:
                progress = QuestManager.Instance.DictQuestsDailyProgress[questId];

                progress.progressCompleted = true;
                QuestManager.Instance.DictQuestsDailyProgress[questId] = progress;
                break;
        }
    }

    #endregion

    #region BEFRIEND COMPANION EVENT

    public void OnBefriend(int id)
    {
        // create custom event basic data
        CustomEventData customEventData = new CustomEventData();
        customEventData.companionId = id;

        HandleEvent(customEventData, HandleBefriendCompanion);
    }

    private HandleEventResult HandleBefriendCompanion(CustomEventData eventData)
    {
        HandleEventResult result = new HandleEventResult()
        {
            needSave = false,
            needNotification = false
        };

        if (NeedUpdateBefriendProgress(eventData.questData, eventData.companionId))
        {
            UpdateBefriendProgress(eventData.questType, eventData.questId);
            result.needSave = true;
            result.needNotification = QuestManager.Instance.CheckNotifications(eventData.questData, eventData.questType, eventData.questId);
        }

        return result;
    }

    private bool NeedUpdateBefriendProgress(QuestData data, int companionId)
    {
        if (data.questObjectiveType == QuestObjectiveType.Befriend)
        {
            // check specific
            if (data.questBefriendSpecific)
            {
                if (data.companionSO.Id == companionId)
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }
        return false;
    }

    private void UpdateBefriendProgress(QuestType questType, string questId)
    {
        QuestDataProgress progress;

        switch (questType)
        {
            default:
            case QuestType.Story:
                progress = QuestManager.Instance.DictQuestsStoryProgress[questId];

                progress.progressCounter++;
                QuestManager.Instance.DictQuestsStoryProgress[questId] = progress;
                break;

            case QuestType.Bounties:
                progress = QuestManager.Instance.DictQuestsBountyProgress[questId];

                progress.progressCounter++;
                QuestManager.Instance.DictQuestsBountyProgress[questId] = progress;
                break;

            case QuestType.Daily:
                progress = QuestManager.Instance.DictQuestsDailyProgress[questId];

                progress.progressCounter++;
                QuestManager.Instance.DictQuestsDailyProgress[questId] = progress;
                break;
        }
    }

    #endregion

    /// <summary>
    /// Iterate through list and returns if needs save or notification
    /// </summary>
    /// <param name="questList">List to iterate</param>
    /// <param name="questType">Type of quests</param>
    /// <param name="eventData">Custom event data</param>
    /// <param name="eventFunction">Function for the event raised</param>
    private HandleEventResult HandleQuestList(List<string> questList, QuestType questType, CustomEventData eventData, Func<CustomEventData, HandleEventResult> eventFunction)
    {
        // initialize result
        HandleEventResult result = new()
        {
            needSave = false,
            needNotification = false
        };

        // iterate list
        foreach (var quest in questList)
        {
            IQuestScriptable so;

            // get so
            switch (questType)
            {
                default:
                case QuestType.Story: so = GetStoryQuestById(quest); break;
                case QuestType.Bounties: so = GetBountyQuestById(quest); break;
                case QuestType.Daily: so = GetDailyQuestById(quest); break;
            }

            // populate with missing informations
            eventData.questId = quest;
            eventData.questData = so.GetQuestaData();
            eventData.questType = questType;

            // get result
            HandleEventResult eventResult = eventFunction(eventData);
            result.needSave = eventResult.needSave;
            result.needNotification = eventResult.needNotification;

            if (result.needNotification)
            {
                result.counterNotification++;
            }
        }

        return result;
    }

    private void HandleEvent(CustomEventData eventData, Func<CustomEventData, HandleEventResult> eventFunction)
    {
        bool needNotification = false;

        // get results from story quests
        HandleEventResult storyResult = HandleQuestList(QuestManager.Instance.ActiveStoryQuests, QuestType.Story, eventData, eventFunction);

        if (!needNotification)
            needNotification = storyResult.needNotification;

        // get results from bounty quests
        HandleEventResult bountyResult = HandleQuestList(QuestManager.Instance.ActiveBountyQuests.Values.ToList(), QuestType.Bounties, eventData, eventFunction);

        if (!needNotification)
            needNotification = bountyResult.needNotification;

        // get results from daily quests
        HandleEventResult dailyResult = HandleQuestList(QuestManager.Instance.ActiveDailyQuests, QuestType.Daily, eventData, eventFunction);

        if (!needNotification && dailyResult.counterNotification == QuestManager.Instance.ActiveDailyQuests.Count && QuestManager.Instance.ActiveDailyQuests.Count > 0)
        {
            needNotification = true;
        }

        if (needNotification)
        {
            QuestManager.Instance.TriggerNotification();
        }

        if (storyResult.needSave || bountyResult.needSave || dailyResult.needSave)
        {
            QuestManager.Instance.SaveQuestsData();
        }
    }
}
