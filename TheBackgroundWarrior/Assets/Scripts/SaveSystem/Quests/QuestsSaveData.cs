using System.Collections.Generic;

public class QuestsSaveData
{
    public List<QuestStorySaveData> storySaveDatas;
    public List<QuestBountySaveData> bountySaveDatas;
    public List<QuestDailySaveData> dailySaveDatas;

    public bool hasInitBountiesRefresh;
    public long lastCheckBountiesRefreshDate;
    public List<string> currentPulledBounties;
    public List<string> acceptedPulledBounties;

    public long lastDailyCreationDate;

    public QuestsSaveData() { }

    public QuestsSaveData(QuestManager manager)
    {
        hasInitBountiesRefresh = manager.HasInitBountiesRefresh;
        lastCheckBountiesRefreshDate = manager.LastCheckBountiesRefreshDate;
        lastDailyCreationDate = manager.LastDailyCreationDate;

        currentPulledBounties = new List<string>();
        currentPulledBounties.AddRange(manager.CurrentPulledBounties);

        acceptedPulledBounties = new List<string>();
        acceptedPulledBounties.AddRange(manager.AcceptedPulledBounties);

        SaveStoryQuests(manager.DictQuestsStoryProgress);
        SaveBountyQuests(manager.ActiveBountyQuests, manager.DictQuestsBountyProgress);
        SaveDailyQuests(manager.DictQuestsDailyProgress);
    }

    private void SaveStoryQuests(Dictionary<string, UtilsQuest.QuestDataProgress> dict)
    {
        storySaveDatas = new List<QuestStorySaveData>();

        // first save for each story quest
        foreach (var pair in dict)
        {
            // save single progress fro every quest
            QuestStorySaveData questStoryData = new QuestStorySaveData(pair.Key, pair.Value);
            storySaveDatas.Add(questStoryData);
        }
    }

    private void SaveBountyQuests(Dictionary<int, string> activeBounties, Dictionary<string, UtilsQuest.QuestDataProgress> dict)
    {
        bountySaveDatas = new List<QuestBountySaveData>();

        // first save for each story quest

        foreach (var pair in activeBounties)
        {
            int slot = pair.Key;
            string id = pair.Value;
            UtilsQuest.QuestDataProgress progress = dict[id];

            QuestBountySaveData questBountyData = new QuestBountySaveData(slot, id, progress);
            bountySaveDatas.Add(questBountyData);
        }
    }

    private void SaveDailyQuests(Dictionary<string, UtilsQuest.QuestDataProgress> dict)
    {
        dailySaveDatas = new List<QuestDailySaveData>();

        // first save for each story quest
        foreach (var pair in dict)
        {
            // save single progress fro every quest
            QuestDailySaveData questDailyData = new QuestDailySaveData(pair.Key, pair.Value);
            dailySaveDatas.Add(questDailyData);
        }
    }
}
