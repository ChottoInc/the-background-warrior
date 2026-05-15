using System.Collections.Generic;
using UnityEngine;

public class UIPanelBountiesList : MonoBehaviour
{
    [SerializeField] UITabQuestsBounties panelQuest;

    [Space(10)]
    [SerializeField] GameObject bountyPrefab;
    [SerializeField] Transform container;

    private List<GameObject> questObjs;

    [Space(10)]
    [SerializeField] int maxBounties = 7;

    private List<string> currentPossibleBountiesList;
    private List<string> acceptedPossibleBountiesList;


    public void Open()
    {
        if(currentPossibleBountiesList == null)
        {
            currentPossibleBountiesList = new List<string>();
            currentPossibleBountiesList.AddRange(QuestManager.Instance.CurrentPulledBounties);
        }

        if (acceptedPossibleBountiesList == null)
        {
            acceptedPossibleBountiesList = new List<string>();
            acceptedPossibleBountiesList.AddRange(QuestManager.Instance.AcceptedPulledBounties);
        }


        gameObject.SetActive(true);

        FillBounties();
    }

    private void FillBounties()
    {
        // every time it opens refresh
        questObjs = ClearList(questObjs);

        if (QuestManager.Instance.CanRefreshBounties)
        {
            // clear current bounties and accepted bounties on refresh
            currentPossibleBountiesList.Clear();
            acceptedPossibleBountiesList.Clear();

            // clear also the quest manager save
            QuestManager.Instance.FillAcceptedBountiesList(acceptedPossibleBountiesList);

            for (int i = 0; i < maxBounties; i++)
            {
                bool valid;
                int tries = 0;
                int maxTries = 1000;

                QuestBountySO randBountySO;

                do
                {
                    valid = true;

                    randBountySO = UtilsQuest.GetRandomBountyQuest();

                    // check if already in list
                    if (currentPossibleBountiesList.Contains(randBountySO.UniqueId))
                        valid = false;

                    // check if player can do it
                    if (!randBountySO.AvailableFor.SharesAnyValueWith(PlayerManager.Instance.PlayerJobsData.AvailableJobs))
                        valid = false;

                    // check if is already an active bounty
                    if (QuestManager.Instance.IsBountyActiveById(randBountySO.UniqueId))
                        valid = false;

                    // check if enemy is currently available in your maps
                    if (!UtilsQuest.IsMonsterAvailable(randBountySO.QuestData, PlayerManager.Instance.PlayerFightData.AvailableMaps))
                        valid = false;

                    tries++;
                } while (!valid && tries < maxTries);

                if (valid)
                {
                    CreateSinglePrefab(randBountySO);
                    //Debug.Log("valid, create prefab");

                    currentPossibleBountiesList.Add(randBountySO.UniqueId);
                    //Debug.Log("added: " + randBountySO.UniqueId);
                }
            }

            QuestManager.Instance.FillPossibleBountiesList(currentPossibleBountiesList);
            //Debug.Log("possible list count: " + currentPossibleBountiesList.Count);

            // Set first time bounties
            if (!QuestManager.Instance.HasInitBountiesRefresh)
            {
                QuestManager.Instance.SetHasInitBountyFirstTime();
            }

            QuestManager.Instance.SaveQuestsData();
        }
        else
        {
            // if can't refresh, so you need the save
            currentPossibleBountiesList.Clear();
            currentPossibleBountiesList.AddRange(QuestManager.Instance.CurrentPulledBounties);

            // also get again the accepted list
            acceptedPossibleBountiesList.Clear();
            acceptedPossibleBountiesList.AddRange(QuestManager.Instance.AcceptedPulledBounties);

            // if can't refresh, show again what has already been pulled from the list
            foreach (var id in currentPossibleBountiesList)
            {
                QuestBountySO bountySO = UtilsQuest.GetBountyQuestById(id);

                // check if is already an active bounty, and it's not accepted yet
                if (!QuestManager.Instance.IsBountyActiveById(bountySO.UniqueId) &&
                    !acceptedPossibleBountiesList.Contains(id))
                {
                    CreateSinglePrefab(bountySO);
                }
            }
        }
    }

    private void CreateSinglePrefab(QuestBountySO bountySO)
    {
        GameObject prefab = Instantiate(bountyPrefab, transform.position, Quaternion.identity);
        prefab.transform.SetParent(container);

        prefab.transform.localScale = new Vector3(1, 1, 1);

        if (prefab.TryGetComponent(out UIBountyPrefab obj))
        {
            obj.Setup(this, bountySO.UniqueId, bountySO.QuestData);
        }

        // add to ui objects
        questObjs.Add(prefab);
    }

    private List<GameObject> ClearList(List<GameObject> list)
    {
        if (list == null)
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

    public void OnBountyAccepted(string id)
    {
        // tell tab bounties which one is accepted
        panelQuest.OnBountyAccepted(id);

        // save the new accepted into the list
        //acceptedPossibleBountiesList.Add(id);
        QuestManager.Instance.FillAcceptedBountiesList(acceptedPossibleBountiesList);
        QuestManager.Instance.SaveQuestsData();

        // hide bounty list
        gameObject.SetActive(false);
    }

    public void OnButtonClose()
    {
        AudioManager.Instance.PlayClickUI();

        // back to previous menu
        panelQuest.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}
