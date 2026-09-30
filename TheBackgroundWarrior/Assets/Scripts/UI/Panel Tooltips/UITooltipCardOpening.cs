using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UITooltipCardOpening : UITooltipBase
{
    [Space(10)]
    [SerializeField] GameObject cardOpeningPrefab;
    [SerializeField] Transform container;

    private GridLayoutGroup _gridGroup;

    private List<CardSO> _cards;

    private List<GameObject> cardObjs;

    [Header("Texts")]
    [SerializeField] TMP_Text textButtonrevealAll;

    private bool _isRevealAllPressed;

    /*
     * >10 120x180 sp x80 y30
     * >5 168x252 sp x80 y30
     * >0 192x288 sp x80 y30
     * */

    private void Awake()
    {
        _gridGroup = container.GetComponent<GridLayoutGroup>();
    }

    public void Show(TooltipManagerData data, Vector2 position, bool fade = false)
    {
        if (Time.timeScale == 1f) Time.timeScale = 0f;

        // clear previuos list
        if (_cards != null) _cards.Clear();

        _cards = data.openingCards;

        Appear(data, fade, position);

        // populate list cards
        Setup();

        RefreshTexts();
    }

    private void RefreshTexts()
    {
        textButtonrevealAll.text = UtilsText.AllText[UtilsText.text_button_revealall];
    }

    public void Hide(bool fade = false)
    {
        if (Time.timeScale == 0f) Time.timeScale = 1f;

        Disappear(fade);
    }

    public void Setup()
    {
        cardObjs = ClearList(cardObjs);

        if(_cards.Count < 6)
        {
            _gridGroup.cellSize = new Vector2(192f, 288f);
        }
        else if(_cards.Count < 11)
        {
            _gridGroup.cellSize = new Vector2(162f, 252f);
        }
        else
        {
            _gridGroup.cellSize = new Vector2(120f, 180f);
        }

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
        for (int i = 0; i < _cards.Count; i++)
        {
            CreateSinglePrefab(_cards[i]);
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
        if (_isRevealAllPressed) return;

        _isRevealAllPressed = true;

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
