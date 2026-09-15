using System.Collections.Generic;
using UnityEngine;

public class UITabBuffs : UITabWindow
{
    [SerializeField] GameObject buffPrefab;

    [Space(10)]
    [SerializeField] Transform container;

    private List<GameObject> itemObjs;

    public override void Open()
    {
        base.Open();

        itemObjs = ClearList(itemObjs);

        RefreshBuffs();
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

    private void RefreshBuffs()
    {
        var buffs = PlayerManager.Instance.PlayerBuffsData.ActiveBuffs;

        foreach (var buff in buffs)
        {
            CreateSinglePrefab(buff);
        }
    }

    private void CreateSinglePrefab(Buff buff)
    {
        if(buff.BuffType == UtilsBuffs.BuffType.Inspiration)
        {
            if (buff.RemainingTime <= 0) return;
        }

        GameObject prefab = Instantiate(buffPrefab, transform.position, Quaternion.identity);
        prefab.transform.SetParent(container);

        prefab.transform.localScale = new Vector3(1, 1, 1);
        prefab.SetActive(true);

        if (prefab.TryGetComponent(out UIBuffPrefab obj))
        {
            obj.Setup(buff);
        }
        itemObjs.Add(prefab);
    }
}
