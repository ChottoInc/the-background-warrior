using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIFisherGroupPrefab : MonoBehaviour
{
    [SerializeField] GameObject panelFishPrefab;
    [SerializeField] Transform container;

    private List<GameObject> panelFishesObjs;

    [Space(10)]
    [SerializeField] TMP_Text textGroupName;
    [SerializeField] TMP_Text textDescription;


    private ScrollRect scroll;

    private FishGroupSO fishGroupSO;


    private bool isInitialized;


    public void Setup(ScrollRect scroll, FishGroupSO fishGroupSO)
    {
        this.scroll = scroll;
        this.fishGroupSO = fishGroupSO;

        InitializeIfNeeded();

        RefreshGroup();
    }

    private void InitializeIfNeeded()
    {
        if (isInitialized) return;

        textGroupName.text = fishGroupSO.GroupName;
        textDescription.text = fishGroupSO.GroupDesc;

        isInitialized = true;
    }

    public void RefreshGroup()
    {
        // clear list and refresh to see if new fishes are caught
        panelFishesObjs = ClearList(panelFishesObjs);
        RefreshFishes();
    }

    private List<GameObject> ClearList(List<GameObject> list)
    {
        if (list == null)
            list = new List<GameObject>();

        foreach (var item in list)
        {
            Destroy(item);
        }

        list.Clear();
        return list;
    }

    private void RefreshFishes()
    {
        panelFishesObjs = new List<GameObject>();

        for (int i = 0; i < fishGroupSO.Fishes.Length; i++)
        {
            GameObject prefab = Instantiate(panelFishPrefab, transform.position, Quaternion.identity);
            prefab.transform.SetParent(container);

            prefab.transform.localScale = new Vector3(1, 1, 1);
            prefab.SetActive(true);

            if (prefab.TryGetComponent(out UIFisherPanelFishPrefab obj))
            {
                obj.Setup(scroll, fishGroupSO.Fishes[i]);
            }
            panelFishesObjs.Add(prefab);
        }
    }
}
