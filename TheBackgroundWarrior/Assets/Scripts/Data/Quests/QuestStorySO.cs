using UnityEngine;

[CreateAssetMenu(menuName = "Data/Quest/Story Data", fileName = "QuestStoryData_")]
public class QuestStorySO : ScriptableObject, IQuestScriptable
{
    [SerializeField] string uniqueId;

    [SerializeField] bool isActiveFromStart;

    [SerializeField] UtilsPlayer.PlayerJob[] availableFor;

    [SerializeField] UtilsQuest.QuestData questData;

    [SerializeField] QuestStorySO[] nexts;


    public string UniqueId => uniqueId;
    public bool IsActiveFromStart => isActiveFromStart;
    public UtilsPlayer.PlayerJob[] AvailableFor => availableFor;
    public UtilsQuest.QuestData QuestData => questData;
    public QuestStorySO[] Nexts => nexts;

    public UtilsQuest.QuestData GetQuestaData()
    {
        return questData;
    }
}
