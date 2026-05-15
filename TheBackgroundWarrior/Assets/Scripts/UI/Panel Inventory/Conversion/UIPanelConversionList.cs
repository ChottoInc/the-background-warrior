using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanelConversionList : MonoBehaviour
{
    [SerializeField] UIPanelConversion panelConversion;

    [Space(10)]
    [SerializeField] GameObject convertCardPrefab;

    [Space(10)]
    [SerializeField] Transform container;

    private List<GameObject> itemObjs;

    private List<ItemGroup> cardGroups;

    public void Setup()
    {
        itemObjs = ClearList(itemObjs);

        FillWindow();

        gameObject.SetActive(true);
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
        if (cardGroups != null) cardGroups.Clear();

        // get updated list
        cardGroups = new List<ItemGroup>(PlayerManager.Instance.Inventory.GetAllCards());

        for (int i = 0; i < cardGroups.Count; i++)
        {
            ItemSO itemSO = UtilsItem.GetItemById(cardGroups[i].IdItem);

            // cycle for quantity, show all for each group
            for (int j = 0; j < cardGroups[i].Quantity; j++)
            {
                CreateSinglePrefab(itemSO as CardSO, j);
            }
        }
    }

    private void CreateSinglePrefab(CardSO cardSO, int index)
    {
        GameObject prefab = Instantiate(convertCardPrefab, transform.position, Quaternion.identity);
        prefab.transform.SetParent(container);

        prefab.transform.localScale = new Vector3(1, 1, 1);
        prefab.SetActive(true);

        if (prefab.TryGetComponent(out UIConvertCardPrefab obj))
        {
            obj.Setup(this, cardSO, index);
        }
        itemObjs.Add(prefab);
    }


    public void OnButtonQuickSelection()
    {
        UIConversionSlot slot = null;
        bool valid;

        do
        {
            valid = true;
            slot = panelConversion.GetFirstEmptySlot();

            if(slot == null)
            {
                // there are no more slots available
                valid = false;
            }
            else
            {
                // select first non-selected card and assign to slot
                UIConvertCardPrefab firstNonSelected = GetFirstNonSelectedCard();

                // set as selected the card
                firstNonSelected.SetAsSelected(slot);

                // assign to conversion slot the card
                slot.Setup(firstNonSelected);
            }

        } while (valid);
    }

    private UIConvertCardPrefab GetFirstNonSelectedCard()
    {
        UIConvertCardPrefab result = null;
        foreach (var item in itemObjs)
        {
            UIConvertCardPrefab card = item.GetComponent<UIConvertCardPrefab>();
            if (!card.IsSelected)
            {
                result = card;
                break;
            }
        }
        return result;
    }

    public void OnCardSelected(UIConvertCardPrefab cardPrefab)
    {
        UIConversionSlot firstEmpty = panelConversion.GetFirstEmptySlot();

        // Check if there's empty slots
        if(firstEmpty == null)
        {
            Debug.Log("All slots are already filled");
            return;
        }

        // Set as selected the card
        cardPrefab.SetAsSelected(firstEmpty);

        // Assign to conversion slot the card
        firstEmpty.Setup(cardPrefab);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
