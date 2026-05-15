using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITabQuestsStory : UITabWindow
{
    [SerializeField] GameObject questPrefab;
    [SerializeField] Transform container;

    private List<GameObject> questObjs;

    public override void Open()
    {
        base.Open();

        FillQuests();
    }

    public void FillQuests()
    {
        questObjs = ClearList(questObjs);

        var activeQuests = QuestManager.Instance.ActiveStoryQuests;

        for (int i = 0; i < activeQuests.Count; i++)
        {
            string currentId = activeQuests[i];
            //Debug.Log("showing story quest: " + currentId);

            QuestStorySO so = UtilsQuest.GetStoryQuestById(currentId);

            bool valid = true;

            if (!so.AvailableFor.SharesAnyValueWith(PlayerManager.Instance.PlayerJobsData.AvailableJobs))
                valid = false;

            if (valid)
            {
                GameObject prefab = Instantiate(questPrefab, transform.position, Quaternion.identity);
                prefab.transform.SetParent(container);

                prefab.transform.localScale = new Vector3(1, 1, 1);

                if (prefab.TryGetComponent(out UIQuestPrefab obj))
                {
                    UtilsQuest.QuestData questData = so.QuestData;

                    UtilsQuest.QuestDataProgress questDataProgress = QuestManager.Instance.DictQuestsStoryProgress[currentId];

                    obj.Setup(this, UtilsQuest.QuestType.Story, currentId, questData, questDataProgress);
                }
                questObjs.Add(prefab);
            }
           
        }
    }

    private List<GameObject> ClearList(List<GameObject> list)
    {
        if(list == null)
        {
            list = new List<GameObject>();
            return list;
        }

        foreach (var item in list)
        {
            Destroy(item);
        }
        list.Clear();
        return list;
    }
}
