using UnityEngine;
using UnityEngine.UI;

public class UIConversionSlot : MonoBehaviour
{
    [SerializeField] GameObject panelEmpty;

    [Space(10)]
    [SerializeField] GameObject panelFill;
    [SerializeField] Image imageBackground;
    [SerializeField] Image imageCard;

    private UIConvertCardPrefab cardPrefab;
    private CardSO cardSO;

    private bool isFilled;


    public CardSO CardSO => cardSO;
    public bool IsFilled => isFilled;


    public void Setup(UIConvertCardPrefab cardPrefab)
    {
        isFilled = false;
        this.cardPrefab = cardPrefab;

        if (cardPrefab == null) 
        { 
            panelEmpty.SetActive(true);
            panelFill.SetActive(false);
        }
        else
        {
            isFilled = true;

            cardSO = cardPrefab.CardSO;

            imageBackground.sprite = cardSO.BackgoundSprite;
            imageCard.sprite = cardSO.Sprite;

            panelFill.SetActive(true);
            panelEmpty.SetActive(false);
        }
    }

    /// <summary>
    /// Grants the possibility to remove a card from the slot itself for QoL
    /// </summary>
    public void OnButtonClick()
    {
        if (!isFilled) return;

        AudioManager.Instance.PlayClickUI();

        DeselectCardPrefab();
    }

    /// <summary>
    /// Deselct the card prefab from the list, also the slot is reset from that prefab
    /// </summary>
    public void DeselectCardPrefab()
    {
        isFilled = false;
        cardPrefab.Deselect();
    }
}
