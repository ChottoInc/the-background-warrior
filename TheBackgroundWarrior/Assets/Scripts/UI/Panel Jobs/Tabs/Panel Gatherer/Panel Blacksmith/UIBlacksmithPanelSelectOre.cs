using System.Collections.Generic;
using UnityEngine;

public class UIBlacksmithPanelSelectOre : MonoBehaviour
{
    [SerializeField] UITabJobBlacksmith tabBlacksmith;

    [Space(10)]
    [SerializeField] GameObject oreSelectionPrefab;
    [SerializeField] Transform container;

    private List<GameObject> oresObjs;

    private UIBlacksmithOreSelectionPrefab selectedPrefab;
    private ItemSO selectedOre;


    public bool IsOpen { get; private set; }


    public void Open()
    {
        // reset
        selectedPrefab = null;
        selectedOre = null;

        FillList();

        gameObject.SetActive(true);

        IsOpen = true;
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

    private void FillList()
    {
        oresObjs = ClearList(oresObjs);

        var ores = UtilsItem.GetAllOres();

        for (int i = 0; i < ores.Length; i++)
        {
            // check if the player has the ore to refine
            if (PlayerManager.Instance.Inventory.HasItem(ores[i].Id))
            {
                GameObject prefab = Instantiate(oreSelectionPrefab, transform.position, Quaternion.identity);
                prefab.transform.SetParent(container);

                prefab.transform.localScale = new Vector3(1, 1, 1);
                prefab.SetActive(true);

                if (prefab.TryGetComponent(out UIBlacksmithOreSelectionPrefab obj))
                {
                    int groupIndex = PlayerManager.Instance.Inventory.GetGroupIndex(ores[i].Id);
                    ItemGroup itemGroup = PlayerManager.Instance.Inventory.ItemGroups[groupIndex];
                    obj.Setup(this, itemGroup);
                }

                oresObjs.Add(prefab);
            }
        }
    }

    public void OnSelectOre(UIBlacksmithOreSelectionPrefab prefab, ItemSO oreSO)
    {
        if(selectedPrefab != null)
        {
            selectedPrefab.Deselect();
        }

        selectedPrefab = prefab;
        selectedOre = oreSO;

        if (selectedPrefab != null)
        {
            selectedPrefab.Select();
        }
    }

    public void Close()
    {
        gameObject.SetActive(false);
        IsOpen = false;
    }

    public void OnButtonCancel()
    {
        Close();
    }

    public void OnButtonConfirm()
    {
        if (selectedOre != null)
            tabBlacksmith.OnSelectedOre(selectedOre);

        Close();
    }
}
