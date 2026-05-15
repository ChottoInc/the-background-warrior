using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanelShopItems : MonoBehaviour
{
    [SerializeField] GameObject shopCardPackPrefab;
    [SerializeField] GameObject shopJobPrefab;
    [SerializeField] Transform container;

    private List<GameObject> itemObjs;

    private int currentFilter;

    [Space(10)]
    [SerializeField] UIShopPanelInfo panelInfo;

    public void Setup(int filter)
    {
        ShowPanelInfo(false);

        currentFilter = filter;

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
        List<ShopItemSO> shopItems;

        switch (currentFilter)
        {
            default:
            case UtilsShop.ID_SHOP_FILTER_CARDPACKS: shopItems = UtilsShop.GetAllCardPacks(); break;
            case UtilsShop.ID_SHOP_FILTER_JOBS: shopItems = UtilsShop.GetAllShopJobs(); break;
        }

        for (int i = 0; i < shopItems.Count; i++)
        {
            CreateSinglePrefab(shopItems[i]);
        }
    }

    private void CreateSinglePrefab(ShopItemSO item)
    {
        // Select correct prefab to show
        GameObject chosenPrefab;

        switch (currentFilter)
        {
            default:
            case UtilsShop.ID_SHOP_FILTER_CARDPACKS: chosenPrefab = shopCardPackPrefab; break;
            case UtilsShop.ID_SHOP_FILTER_JOBS: chosenPrefab = shopJobPrefab; break;
        }

        // The attached object willhave a script inheriting from abstract class uishopitem, and use a setup function
        GameObject prefab = Instantiate(chosenPrefab, transform.position, Quaternion.identity);
        prefab.transform.SetParent(container);

        prefab.transform.localScale = new Vector3(1, 1, 1);
        prefab.SetActive(true);

        if (prefab.TryGetComponent(out UIShopItem obj))
        {
            obj.Setup(this, item, currentFilter);
        }
        itemObjs.Add(prefab);
    }


    public void ShowPanelInfo(bool show)
    {
        panelInfo.Show(show);
    }

    public void ShowDetails(ShopItemSO itemSO, int currentFilter)
    {
        panelInfo.Show(true);
        panelInfo.Setup(itemSO, currentFilter);
    }
}
