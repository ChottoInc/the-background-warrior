using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UtilsQuest;

public class QuestEventHandler
{
    #region ENEMY KILLED EVENT

    public void OnEnemyKilled(EnemySO enemySO)
    {
        bool needSave = false;

        bool needNotification = false;

        // Story quest checks
        foreach (var quest in QuestManager.Instance.ActiveStoryQuests)
        {
            // get so
            QuestStorySO so = GetStoryQuestById(quest);

            HandleEventResult enemyKilledResult = HandleEnemyKilled(quest, so.QuestData, QuestType.Story, enemySO, needNotification);
            needSave = enemyKilledResult.needSave;

            if (!needNotification)
                needNotification = enemyKilledResult.needNotification;
        }

        // Bounties quest checks
        foreach (var quest in QuestManager.Instance.ActiveBountyQuests.Values)
        {
            // get so
            QuestBountySO so = GetBountyQuestById(quest);

            HandleEventResult enemyKilledResult = HandleEnemyKilled(quest, so.QuestData, QuestType.Bounties, enemySO, needNotification);
            needSave = enemyKilledResult.needSave;

            if (!needNotification)
                needNotification = enemyKilledResult.needNotification;
        }

        int counterNotificationDailies = 0;

        // Daily quest checks
        foreach (var quest in QuestManager.Instance.ActiveDailyQuests)
        {
            // get so
            QuestDailySO so = GetDailyQuestById(quest);

            HandleEventResult enemyKilledResult = HandleEnemyKilled(quest, so.QuestData, QuestType.Daily, enemySO, needNotification);
            needSave = enemyKilledResult.needSave;

            if (!needNotification)
                counterNotificationDailies++;
        }

        if (!needNotification && counterNotificationDailies == QuestManager.Instance.ActiveDailyQuests.Count && QuestManager.Instance.ActiveDailyQuests.Count > 0)
        {
            needNotification = true;
        }

        if (needNotification)
        {
            QuestManager.Instance.TriggerNotification();
        }

        if (needSave)
        {
            QuestManager.Instance.SaveQuestsData();
        }
    }

    private HandleEventResult HandleEnemyKilled(string questId, QuestData data, QuestType questType, EnemySO enemySO, bool needNotification)
    {
        HandleEventResult result = new HandleEventResult()
        {
            needSave = false,
            needNotification = false
        };

        if (NeedUpdateKillProgress(data, enemySO))
        {
            UpdateKillProgress(questType, questId);
            result.needSave = true;

            // even if one notification needs display, do not check again
            if (!needNotification)
            {
                result.needNotification = QuestManager.Instance.CheckNotifications(data, questType, questId);
            }
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
        bool needSave = false;

        bool needNotification = false;

        // Story quest checks
        foreach (var quest in QuestManager.Instance.ActiveStoryQuests)
        {
            // get so
            QuestStorySO so = GetStoryQuestById(quest);
            HandleEventResult itemObtainResult = HandleItemObtain(quest, so.QuestData, QuestType.Story, id, needNotification);
            needSave = itemObtainResult.needSave;

            if (!needNotification)
                needNotification = itemObtainResult.needNotification;
        }

        // Bounties quest checks
        foreach (var quest in QuestManager.Instance.ActiveBountyQuests.Values)
        {
            // get so
            QuestBountySO so = GetBountyQuestById(quest);
            HandleEventResult itemObtainResult = HandleItemObtain(quest, so.QuestData, QuestType.Bounties, id, needNotification);
            needSave = itemObtainResult.needSave;

            if (!needNotification)
                needNotification = itemObtainResult.needNotification;
        }

        int counterNotificationDailies = 0;

        // Daily quest checks
        foreach (var quest in QuestManager.Instance.ActiveDailyQuests)
        {
            // get so
            QuestDailySO so = GetDailyQuestById(quest);
            HandleEventResult itemObtainResult = HandleItemObtain(quest, so.QuestData, QuestType.Daily, id, needNotification);
            needSave = itemObtainResult.needSave;

            if (!needNotification)
                counterNotificationDailies++;
        }

        if (!needNotification && counterNotificationDailies == QuestManager.Instance.ActiveDailyQuests.Count)
        {
            needNotification = true;
        }

        if (needNotification)
        {
            QuestManager.Instance.TriggerNotification();
        }

        if (needSave)
        {
            QuestManager.Instance.SaveQuestsData();
        }
    }

    private HandleEventResult HandleItemObtain(string questId, QuestData data, QuestType questType, int itemId, bool needNotification)
    {
        HandleEventResult result = new HandleEventResult()
        {
            needSave = false,
            needNotification = false
        };

        if (NeedUpdateObtainProgress(data, itemId))
        {
            UpdateObtainProgress(questType, questId);
            result.needSave = true;

            // even if one notification needs display, do not check again
            if (!needNotification)
            {
                result.needNotification = QuestManager.Instance.CheckNotifications(data, questType, questId);
            }
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
        bool needSave = false;

        bool needNotification = false;

        // Story quest checks
        foreach (var quest in QuestManager.Instance.ActiveStoryQuests)
        {
            // get so
            QuestStorySO so = GetStoryQuestById(quest);
            HandleEventResult statUpResult = HandleStatUp(quest, so.QuestData, QuestType.Story, id, amount, needNotification);
            needSave = statUpResult.needSave;

            if (!needNotification)
                needNotification = statUpResult.needNotification;
        }

        // Bounties quest checks
        foreach (var quest in QuestManager.Instance.ActiveBountyQuests.Values)
        {
            // get so
            QuestBountySO so = GetBountyQuestById(quest);
            HandleEventResult statUpResult = HandleStatUp(quest, so.QuestData, QuestType.Bounties, id, amount, needNotification);
            needSave = statUpResult.needSave;

            if (!needNotification)
                needNotification = statUpResult.needNotification;
        }

        int counterNotificationDailies = 0;

        // Daily quest checks
        foreach (var quest in QuestManager.Instance.ActiveDailyQuests)
        {
            // get so
            QuestDailySO so = GetDailyQuestById(quest);
            HandleEventResult statUpResult = HandleStatUp(quest, so.QuestData, QuestType.Story, id, amount, needNotification);
            needSave = statUpResult.needSave;

            if (!needNotification)
                counterNotificationDailies++;
        }

        if (!needNotification && counterNotificationDailies == QuestManager.Instance.ActiveDailyQuests.Count)
        {
            needNotification = true;
        }

        if (needNotification)
        {
            QuestManager.Instance.TriggerNotification();
        }

        if (needSave)
        {
            QuestManager.Instance.SaveQuestsData();
        }
    }

    private HandleEventResult HandleStatUp(string questId, QuestData data, QuestType questType, int statId, int amountStat, bool needNotification)
    {
        HandleEventResult result = new HandleEventResult()
        {
            needSave = false,
            needNotification = false
        };

        if (NeedUpdateStatUpProgress(data, statId))
        {
            UpdateStatLevelUpProgress(questType, questId, amountStat);
            result.needSave = true;

            // even if one notification needs display, do not check again
            if (!needNotification)
            {
                result.needNotification = QuestManager.Instance.CheckNotifications(data, questType, questId);
            }
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
        bool needSave = false;

        bool needNotification = false;

        // Story quest checks
        foreach (var quest in QuestManager.Instance.ActiveStoryQuests)
        {
            // get so
            QuestStorySO so = GetStoryQuestById(quest);
            HandleEventResult addMapResult = HandleAddMap(quest, so.QuestData, QuestType.Story, id, needNotification);
            needSave = addMapResult.needSave;

            if (!needNotification)
                needNotification = addMapResult.needNotification;
        }

        // Bounties quest checks
        foreach (var quest in QuestManager.Instance.ActiveBountyQuests.Values)
        {
            // get so
            QuestBountySO so = GetBountyQuestById(quest);
            HandleEventResult addMapResult = HandleAddMap(quest, so.QuestData, QuestType.Bounties, id, needNotification);
            needSave = addMapResult.needSave;

            if (!needNotification)
                needNotification = addMapResult.needNotification;
        }

        int counterNotificationDailies = 0;

        // Daily quest checks
        foreach (var quest in QuestManager.Instance.ActiveDailyQuests)
        {
            // get so
            QuestDailySO so = GetDailyQuestById(quest);
            HandleEventResult addMapResult = HandleAddMap(quest, so.QuestData, QuestType.Daily, id, needNotification);
            needSave = addMapResult.needSave;

            if (!needNotification)
                counterNotificationDailies++;
        }

        if (!needNotification && counterNotificationDailies == QuestManager.Instance.ActiveDailyQuests.Count)
        {
            needNotification = true;
        }

        if (needNotification)
        {
            QuestManager.Instance.TriggerNotification();
        }

        if (needSave)
        {
            QuestManager.Instance.SaveQuestsData();
        }
    }

    private HandleEventResult HandleAddMap(string questId, QuestData data, QuestType questType, int mapId, bool needNotification)
    {
        HandleEventResult result = new HandleEventResult()
        {
            needSave = false,
            needNotification = false
        };

        if (NeedUpdateUnlockMapProgress(data, mapId))
        {
            UpdateUnlockMapProgress(questType, questId);
            result.needSave = true;

            // even if one notification needs display, do not check again
            if (!needNotification)
            {
                result.needNotification = QuestManager.Instance.CheckNotifications(data, questType, questId);
            }
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
        bool needSave = false;

        bool needNotification = false;

        // Story quest checks
        foreach (var quest in QuestManager.Instance.ActiveStoryQuests)
        {
            // get so
            QuestStorySO so = GetStoryQuestById(quest);
            HandleEventResult befriendedCompanionResult = HandleBefriendCompanion(quest, so.QuestData, QuestType.Story, id, needNotification);
            needSave = befriendedCompanionResult.needSave;

            if (!needNotification)
                needNotification = befriendedCompanionResult.needNotification;
        }

        // Bounties quest checks
        foreach (var quest in QuestManager.Instance.ActiveBountyQuests.Values)
        {
            // get so
            QuestBountySO so = GetBountyQuestById(quest);
            HandleEventResult befriendedCompanionResult = HandleBefriendCompanion(quest, so.QuestData, QuestType.Bounties, id, needNotification);
            needSave = befriendedCompanionResult.needSave;

            if (!needNotification)
                needNotification = befriendedCompanionResult.needNotification;
        }

        int counterNotificationDailies = 0;

        // Daily quest checks
        foreach (var quest in QuestManager.Instance.ActiveDailyQuests)
        {
            // get so
            QuestDailySO so = GetDailyQuestById(quest);
            HandleEventResult befriendedCompanionResult = HandleBefriendCompanion(quest, so.QuestData, QuestType.Daily, id, needNotification);
            needSave = befriendedCompanionResult.needSave;

            if (!needNotification)
                counterNotificationDailies++;
        }

        if (!needNotification && counterNotificationDailies == QuestManager.Instance.ActiveDailyQuests.Count)
        {
            needNotification = true;
        }

        if (needNotification)
        {
            QuestManager.Instance.TriggerNotification();
        }

        if (needSave)
        {
            QuestManager.Instance.SaveQuestsData();
        }
    }

    private HandleEventResult HandleBefriendCompanion(string questId, QuestData data, QuestType questType, int companionId, bool needNotification)
    {
        HandleEventResult result = new HandleEventResult()
        {
            needSave = false,
            needNotification = false
        };

        if (NeedUpdateBefriendProgress(data, companionId))
        {
            UpdateBefriendProgress(questType, questId);
            result.needSave = true;

            // even if one notification needs display, do not check again
            if (!needNotification)
            {
                result.needNotification = QuestManager.Instance.CheckNotifications(data, questType, questId);
            }
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
}
