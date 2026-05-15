using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIPanelAdd : MonoBehaviour
{
    [SerializeField] TMP_InputField inputField;

    public void OnButtonClose()
    {
        gameObject.SetActive(false);
    }

    public void OnButtonAdd()
    {
        string all = inputField.text;
        Debug.Log("all: " + all);

        List<string> list = new List<string>();
        list.AddRange(all.Split(','));

        Debug.Log("list count: " + list.Count);

        foreach (var item in list)
        {
            Debug.Log("item: " + item);
            string[] parts = item.Split(':');
            int id = int.Parse(parts[0]);
            int amount = int.Parse(parts[1]);

            PlayerManager.Instance.Inventory.AddItem(id, amount);
        }
        PlayerManager.Instance.SaveInventoryData();
    }
}
