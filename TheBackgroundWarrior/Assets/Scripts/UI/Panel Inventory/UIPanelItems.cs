using System.Collections.Generic;
using UnityEngine;

public class UIPanelItems : MonoBehaviour
{
    [SerializeField] GameObject invItemPrefab;
    [SerializeField] GameObject invCardPrefab;

    [Space(10)]
    [SerializeField] Transform container;

    private List<GameObject> itemObjs;

    private List<ItemGroup> itemGroups;

    private int currentInvFilter;

    [Space(10)]
    [SerializeField] UIInventoryPanelInfo panelInfo;
    [SerializeField] UIPanelDismantle panelDismantle;

    public void Setup(int filter)
    {
        currentInvFilter = filter;

        itemObjs = ClearList(itemObjs);

        FillWindow();
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

    private void FillWindow()
    {
        // clear previuos list
        if (itemGroups != null) itemGroups.Clear();

        // get updated list
        itemGroups = new List<ItemGroup>(PlayerManager.Instance.Inventory.ItemGroups);

        for (int i = 0; i < itemGroups.Count; i++)
        {
            ItemSO itemSO = UtilsItem.GetItemById(itemGroups[i].IdItem);

            switch (currentInvFilter)
            {
                case UITabInventory.ID_INVENTORY_FILTER_ALL: CreateSinglePrefab(itemGroups[i], itemSO); break;

                case UITabInventory.ID_INVENTORY_FILTER_ORES: 
                    if(itemSO.ItemType == UtilsItem.ItemType.Ore)
                    {
                        CreateSinglePrefab(itemGroups[i], itemSO); 
                    }
                    break;

                case UITabInventory.ID_INVENTORY_FILTER_METALS:
                    if (itemSO.ItemType == UtilsItem.ItemType.Metal)
                    {
                        CreateSinglePrefab(itemGroups[i], itemSO);
                    }
                    break;

                case UITabInventory.ID_INVENTORY_FILTER_FISHES:
                    if (itemSO.ItemType == UtilsItem.ItemType.Fish)
                    {
                        CreateSinglePrefab(itemGroups[i], itemSO);
                    }
                    break;

                case UITabInventory.ID_INVENTORY_FILTER_CARDS:
                    if (itemSO.ItemType == UtilsItem.ItemType.Card)
                    {
                        CreateSinglePrefab(itemGroups[i], itemSO);
                    }
                    break;
            }
        }
    }

    private void CreateSinglePrefab(ItemGroup group, ItemSO itemSO)
    {
        GameObject selected = null;

        switch (itemSO.ItemType)
        {
            default: selected = invItemPrefab; break;
            case UtilsItem.ItemType.Card: selected = invCardPrefab; break;
        }

        GameObject prefab = Instantiate(selected, transform.position, Quaternion.identity);
        prefab.transform.SetParent(container);

        prefab.transform.localScale = new Vector3(1, 1, 1);
        prefab.SetActive(true);

        if (prefab.TryGetComponent(out UIInventoryItem obj))
        {
            obj.Setup(this, group, itemSO);
        }
        itemObjs.Add(prefab);
    }


    public void ShowPanelInfo(bool show)
    {
        panelInfo.Show(show);
    }

    public void ShowDetails(ItemGroup group)
    {
        // If already dismantling, update panel dismantle, else open info item
        if (!panelDismantle.IsOpen)
        {
            panelInfo.Show(true);
            panelInfo.Setup(group);
        }
        else
        {
            ItemSO itemSO = UtilsItem.GetItemById(group.IdItem);

            // check if item click is card or not
            if(itemSO.ItemType == UtilsItem.ItemType.Card)
            {
                panelDismantle.Show(true);
                panelDismantle.Setup(group);
            }
            else
            {
                panelDismantle.Show(false);
                panelInfo.Show(true);
                panelInfo.Setup(group);
            }
        }
    }
}
