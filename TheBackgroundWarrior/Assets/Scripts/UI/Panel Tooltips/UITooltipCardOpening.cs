using System.Collections.Generic;
using UnityEngine;

public class UITooltipCardOpening : UITooltipBase
{
    [Space(10)]
    [SerializeField] GameObject cardOpeningPrefab;
    [SerializeField] Transform container;

    private List<CardSO> cards;

    private List<GameObject> cardObjs;

    

    public void Show(List<CardSO> cards, bool fade = false)
    {
        if (Time.timeScale == 1f) Time.timeScale = 0f;

        // clear previuos list
        if (this.cards != null) this.cards.Clear();

        this.cards = cards;

        // populate list cards
        Setup();

        Appear(fade);
    }

    public void Hide(bool fade = false)
    {
        if (Time.timeScale == 0f) Time.timeScale = 1f;

        Disappear(fade);
    }

    public void Setup()
    {
        cardObjs = ClearList(cardObjs);

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
        for (int i = 0; i < cards.Count; i++)
        {
            CreateSinglePrefab(cards[i]);
        }
    }

    private void CreateSinglePrefab(CardSO card)
    {
        GameObject prefab = Instantiate(cardOpeningPrefab, transform.position, Quaternion.identity);
        prefab.transform.SetParent(container);

        prefab.transform.localScale = new Vector3(1, 1, 1);
        prefab.SetActive(true);

        if (prefab.TryGetComponent(out UICardReveal obj))
        {
            obj.Setup(card);
        }
        cardObjs.Add(prefab);
    }

    public void OnButtonRevealAll()
    {
        AudioManager.Instance.PlayClickUI();
        foreach (var item in cardObjs)
        {
            if (item.TryGetComponent(out UICardReveal obj))
            {
                obj.Flip();
            }
        }
    }

    public void OnButtonClose()
    {
        AudioManager.Instance.PlayClickUI();
        Hide(true);
    }
}
