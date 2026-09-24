using UnityEngine;

public class UITabQuestsBounties : UITabWindow
{
    [Header("Window Bounties List")]
    [SerializeField] UIPanelBountiesList panelBountiesList;

    [Space(10)]
    [SerializeField] GameObject questPrefab;

    [Header("Slot 1")]
    [SerializeField] Transform slot1Container;
    [SerializeField] UIBountyRequestPrefab bountyRequest1;
    [SerializeField] GameObject _slot1Border;

    private GameObject bountyQuest1;

    [Header("Slot 1")]
    [SerializeField] Transform slot2Container;
    [SerializeField] UIBountyRequestPrefab bountyRequest2;
    [SerializeField] GameObject _slot2Border;

    private GameObject bountyQuest2;

    [Header("Slot 1")]
    [SerializeField] Transform slot3Container;
    [SerializeField] UIBountyRequestPrefab bountyRequest3;
    [SerializeField] GameObject _slot3Border;

    private GameObject bountyQuest3;


    private int lastSlotSelected;


    public override void Open()
    {
        base.Open();

        FillQuests();
    }

    public override void Close()
    {
        // close list if was open
        panelBountiesList.gameObject.SetActive(false);

        base.Close();
    }

    public void FillQuests()
    {
        _slot1Border.SetActive(true);
        _slot2Border.SetActive(true);
        _slot3Border.SetActive(true);

        if (bountyQuest1 != null)
        {
            Destroy(bountyQuest1);
            bountyQuest1 = null;
        }

        if (bountyQuest2 != null)
        {
            Destroy(bountyQuest2);
            bountyQuest2 = null;
        }

        if (bountyQuest3 != null)
        {
            Destroy(bountyQuest3);
            bountyQuest3 = null;
        }

        foreach (var pair in QuestManager.Instance.ActiveBountyQuests)
        {
            // vars to store right slot settings
            Transform slotContainer;
            UIBountyRequestPrefab requestPrefab;
            GameObject borderToHide;

            UtilsQuest.QuestData questData = UtilsQuest.GetBountyQuestById(pair.Value).QuestData;
            UtilsQuest.QuestDataProgress questProgress = QuestManager.Instance.DictQuestsBountyProgress[pair.Value];

            // 0: slot 1, 1: slot 2, 2: slot 3
            switch(pair.Key)
            {
                default:
                case 0:
                    slotContainer = slot1Container;
                    requestPrefab = bountyRequest1;
                    borderToHide = _slot1Border;

                    bountyQuest1 = CreateQuestPrefab(pair.Value, questData, questProgress, slotContainer);

                    break;

                case 1:
                    slotContainer = slot2Container;
                    requestPrefab = bountyRequest2;
                    borderToHide = _slot2Border;

                    bountyQuest2 = CreateQuestPrefab(pair.Value, questData, questProgress, slotContainer);

                    break;

                case 2:
                    slotContainer = slot3Container;
                    requestPrefab = bountyRequest3;
                    borderToHide = _slot3Border;

                    bountyQuest3 = CreateQuestPrefab(pair.Value, questData, questProgress, slotContainer);

                    break;
            }

            // disable request prefab
            requestPrefab.gameObject.SetActive(false);
            borderToHide.SetActive(false);
        }

        // Set active bounty choose if not active
        if(bountyQuest1 == null)
        {
            bountyRequest1.gameObject.SetActive(true);
        }

        if (bountyQuest2 == null)
        {
            bountyRequest2.gameObject.SetActive(true);
        }

        if (bountyQuest3 == null)
        {
            bountyRequest3.gameObject.SetActive(true);
        }
    }

    private GameObject CreateQuestPrefab(string id, UtilsQuest.QuestData questData, UtilsQuest.QuestDataProgress questProgress, Transform slotContainer)
    {
        GameObject prefab;

        prefab = Instantiate(questPrefab, transform.position, Quaternion.identity);
        prefab.transform.SetParent(slotContainer);

        prefab.transform.localScale = new Vector3(1, 1, 1);

        if (prefab.TryGetComponent(out UIQuestPrefab obj))
        {
            obj.Setup(this, UtilsQuest.QuestType.Bounties, id, questData, questProgress);
        }

        return prefab;
    }


    public void OpenBountiesList(int slot)
    {
        // save which slot to occupy
        lastSlotSelected = slot;

        gameObject.SetActive(false);

        panelBountiesList.Open();
    }

    public void OnBountyAccepted(string id)
    {
        QuestManager.Instance.AddActiveBountyQuest(lastSlotSelected, id);
        QuestManager.Instance.SaveQuestsData();

        FillQuests();
        gameObject.SetActive(true);
    }
}
