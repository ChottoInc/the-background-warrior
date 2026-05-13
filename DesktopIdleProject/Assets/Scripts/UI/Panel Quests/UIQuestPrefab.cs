using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIQuestPrefab : MonoBehaviour
{
    [SerializeField] TMP_Text textQuest;
    [SerializeField] GenericBar progressBar;

    [Space(10)]
    [SerializeField] GameObject panelButtonClaim;
    [SerializeField] Button buttonClaim;

    [Space(10)]
    [SerializeField] GameObject panelReward;
    [SerializeField] TMP_Text textReward;


    private UITabQuestsStory questStoryWindow;
    private UITabQuestsBounties questBountiesWindow;
    private UITabQuestsDaily questDailyWindow;

    private UtilsQuest.QuestType questType;

    private string storyQuestId;

    private string bountyQuestId;

    private string dailyQuestId;

    // Used to check if the daily quest is complete
    public bool IsCleared => buttonClaim.interactable;

    private int rewardAmount;



    public void Setup(UITabQuestsStory questWindow, UtilsQuest.QuestType questType, string storyQuestId, UtilsQuest.QuestData questData, UtilsQuest.QuestDataProgress questDataProgress)
    {
        questStoryWindow = questWindow;

        SetupQuest(questType, storyQuestId, questData, questDataProgress);

        UpdateProgressBarUI(questData, questDataProgress);
    }

    public void Setup(UITabQuestsBounties questWindow, UtilsQuest.QuestType questType, string bountyQuestId, UtilsQuest.QuestData questData, UtilsQuest.QuestDataProgress questDataProgress)
    {
        questBountiesWindow = questWindow;

        SetupQuest(questType, bountyQuestId, questData, questDataProgress);

        UpdateProgressBarUI(questData, questDataProgress);
    }

    public void Setup(UITabQuestsDaily questWindow, UtilsQuest.QuestType questType, string storyQuestId, UtilsQuest.QuestData questData, UtilsQuest.QuestDataProgress questDataProgress)
    {
        questDailyWindow = questWindow;

        SetupQuest(questType, storyQuestId, questData, questDataProgress);

        UpdateProgressBarUI(questData, questDataProgress);
    }

    private void SetupQuest(UtilsQuest.QuestType questType, string questId, UtilsQuest.QuestData questData, UtilsQuest.QuestDataProgress questDataProgress)
    {
        this.questType = questType;

        switch (questType)
        {
            case UtilsQuest.QuestType.Story: storyQuestId = questId; break;
            case UtilsQuest.QuestType.Bounties: bountyQuestId = questId; break;
            case UtilsQuest.QuestType.Daily: 
                dailyQuestId = questId; 
                panelButtonClaim.SetActive(false);
                panelReward.SetActive(false);
                break;
        }

        // set description
        string questDesc = UtilsQuest.GetQuestProgress(questData, questDataProgress);
        textQuest.text = questDesc;

        // set claimable
        buttonClaim.interactable = UtilsQuest.CanClaim(questData, questDataProgress);

        // set reward
        rewardAmount = questData.rewardAmount;

        textReward.text = rewardAmount.ToString();
    }

    private void UpdateProgressBarUI(UtilsQuest.QuestData data, UtilsQuest.QuestDataProgress progress)
    {
        switch (data.questObjectiveType)
        {
            case UtilsQuest.QuestObjectiveType.Kill:
                progressBar.Setup(data.amountKill, progress.progressCounter);
                break;

            case UtilsQuest.QuestObjectiveType.Obtain:
                progressBar.Setup(data.amountObtain, progress.progressCounter);
                break;

            case UtilsQuest.QuestObjectiveType.LevelUp:
                progressBar.Setup(data.amountStat, progress.progressCounter);
                break;

            case UtilsQuest.QuestObjectiveType.UnlockMap:
                progressBar.gameObject.SetActive(false);
                break;

            case UtilsQuest.QuestObjectiveType.Befriend:
                progressBar.Setup(data.amountObtain, progress.progressCounter);
                break;
        }
        
    }


    public void OnButtonClaim()
    {
        // add reward to player
        PlayerManager.Instance.Inventory.AddBits(rewardAmount);

        // switch behaviour for quest type
        switch (questType)
        {
            case UtilsQuest.QuestType.Story:

                //Debug.Log("claimed story quest: " + storyQuestId);

                // set clear and update new paths
                QuestManager.Instance.SetStoryQuestCleared(storyQuestId);
                QuestManager.Instance.UpdateStoryQuests();

                // update quest window
                questStoryWindow.FillQuests();

                break;

            case UtilsQuest.QuestType.Bounties:
                // set clear bounty
                QuestManager.Instance.SetBountyQuestCleared(bountyQuestId);
                QuestManager.Instance.UpdateBountyQuests();

                // update quest window
                questBountiesWindow.FillQuests();
                break;
        }
        
        // save
        PlayerManager.Instance.SaveInventoryData();
        QuestManager.Instance.SaveQuestsData();
    }
}
