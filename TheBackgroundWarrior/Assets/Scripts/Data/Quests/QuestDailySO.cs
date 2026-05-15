using UnityEngine;

[CreateAssetMenu(menuName = "Data/Quest/Daily Data", fileName = "QuestDailyData_")]
public class QuestDailySO : ScriptableObject, IQuestScriptable
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
