using UnityEngine;

[CreateAssetMenu(menuName = "Data/Quest/Bounty Data", fileName = "QuestBountyData_")]
public class QuestBountySO : ScriptableObject, IQuestScriptable
{
    [SerializeField] string uniqueId;

    [SerializeField] UtilsPlayer.PlayerJob[] availableFor;

    [SerializeField] UtilsQuest.QuestData questData;


    public string UniqueId => uniqueId;
    public UtilsPlayer.PlayerJob[] AvailableFor => availableFor;
    public UtilsQuest.QuestData QuestData => questData;

    public UtilsQuest.QuestData GetQuestaData()
    {
        return questData;   
    }
}
