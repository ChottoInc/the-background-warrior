using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class UITooltipManager : MonoBehaviour
{
    public const int ID_SHOW_TEXT = 0;
    public const int ID_SHOW_CARD = 1;
    public const int ID_SHOW_YESNO = 2;
    public const int ID_SHOW_CARDOPENING = 3;


    [SerializeField] Transform centerPoint;

    [Space(10)]
    [SerializeField] UITooltipName tooltipName;
    [SerializeField] UITooltipCard tooltipCard;
    [SerializeField] UITooltipYesNo tooltipYesNo;
    [SerializeField] UITooltipCardOpening tooltipCardOpening;



    public Transform CenterPoint => centerPoint;


    public bool IsCallbackOpen
    {
        get
        {
            return  IsYesNoCallbackShowing ||
                    IsCardOpeningShowing;
        }
    }

    public bool IsYesNoCallbackShowing => tooltipYesNo.IsShowing;
    public bool IsCardOpeningShowing => tooltipCardOpening.IsShowing;


    public static UITooltipManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Show(TooltipManagerData tooltipData, Vector2 position, bool fade = false, float fontMaxSize = 50f)
    {
        switch(tooltipData.idTooltip)
        {
            case ID_SHOW_TEXT: tooltipName.Show(tooltipData.text, position, fade, fontMaxSize); break;
            case ID_SHOW_CARD: tooltipCard.Show(tooltipData.cardSO, fade); break;
            case ID_SHOW_CARDOPENING: tooltipCardOpening.Show(tooltipData.openingCards, fade); break;
        }
    }

    public Task<bool> ShowPanelYesNoCallback(TooltipManagerData tooltipData, Vector2 position, bool fade = false)
    {
        return tooltipYesNo.Show(tooltipData.text, position, fade);
    }

    public void Hide(int idTooltip,  bool fade = false)
    {
        switch (idTooltip)
        {
            default:
            case ID_SHOW_TEXT: tooltipName.Hide(fade); break;
            case ID_SHOW_CARD: tooltipCard.Hide(fade); break;
            case ID_SHOW_CARDOPENING: tooltipCardOpening.Hide(fade); break;
        }
    }
}

public struct TooltipManagerData
{
    public int idTooltip;

    // tooltip generic text
    public string text;

    // tooltip card
    public CardSO cardSO;

    // tooltip card opening
    public List<CardSO> openingCards;
}