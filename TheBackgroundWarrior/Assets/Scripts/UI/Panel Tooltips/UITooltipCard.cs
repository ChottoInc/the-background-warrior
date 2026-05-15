using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UITooltipCard : MonoBehaviour
{
    [SerializeField] CanvasGroup canvasGroup;

    [Space(10)]
    [SerializeField] float timeToFade = 1f;

    [Space(10)]
    [SerializeField] Image imageBackground;
    [SerializeField] Image imageCard;

    [Space(10)]
    [SerializeField] Image imageRarity;
    [SerializeField] TMP_Text textRarity;
    [SerializeField] TMP_Text textNumber;
    [SerializeField] TMP_Text textName;

    [Space(10)]
    [SerializeField] TMP_Text textDesc;

    private CardSO cardSO;

    private Tween tweenFade;

    private void OnDestroy()
    {
        tweenFade?.Kill();
    }

    public void Show(CardSO cardSO, bool fade = false)
    {
        this.cardSO = cardSO;

        // set card
        imageBackground.sprite = cardSO.BackgoundSprite;
        imageCard.sprite = cardSO.Sprite;

        imageRarity.color = UtilsGeneral.GetColorByRarity(cardSO.CardRarity);
        textRarity.text = $"{cardSO.CardRarity}";

        textNumber.text = $"{cardSO.CardNumber}";

        textName.text = cardSO.ItemName;

        textDesc.text = cardSO.CardDescription;

        gameObject.SetActive(true);

        if (!fade)
        {
            canvasGroup.alpha = 1f;
        }
        else
        {
            // handles fade
            if (tweenFade == null)
            {
                canvasGroup.alpha = 0f;
            }
            else
            {
                tweenFade.Kill();
            }

            // scale with unscaled delta time
            tweenFade = canvasGroup.DOFade(1f, timeToFade).SetEase(Ease.InOutSine).SetUpdate(true);
        }
    }

    public void Hide(bool fade = false)
    {
        if (!fade)
        {
            canvasGroup.alpha = 0f;

            gameObject.SetActive(false);
        }
        else
        {
            // handles fade
            if (tweenFade == null)
            {
                canvasGroup.alpha = 1f;
            }
            else
            {
                tweenFade.Kill();
            }

            // scale with unscaled delta time
            tweenFade = canvasGroup.DOFade(0f, timeToFade).SetEase(Ease.InOutSine).SetUpdate(true).OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
        }
    }
}
