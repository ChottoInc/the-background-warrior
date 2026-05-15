using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UICardReveal : MonoBehaviour
{
    [SerializeField] Image imageRarity;

    [SerializeField] GameObject panelBack;

    [Space(10)]
    [SerializeField] GameObject panelFront;
    [SerializeField] Image imageBackground;
    [SerializeField] Image imageCard;

    [Space(10)]
    [SerializeField] float flip90Time = 0.5f;

    private bool isFlipped;



    private CardSO cardSO;



    private Tween tweenRot1;
    private Tween tweenRot2;


    private void OnDestroy()
    {
        tweenRot1?.Kill();
        tweenRot2?.Kill();
    }

    public void Setup(CardSO cardSO)
    {
        this.cardSO = cardSO;

        imageRarity.gameObject.SetActive(false);

        panelFront.SetActive(false);

        panelBack.SetActive(true);

        imageBackground.sprite = cardSO.BackgoundSprite;
        imageCard.sprite = cardSO.Sprite;
    }



    public void OnPointerEnter()
    {
        if (isFlipped) return;

        imageRarity.gameObject.SetActive(true);
    }

    public void OnPointerExit()
    {
        if (isFlipped) return;

        imageRarity.gameObject.SetActive(false);
    }



    public void Flip()
    {
        isFlipped = true;

        tweenRot1 = transform.DORotate(new Vector3(0, 90f, 0), flip90Time).SetEase(Ease.InOutSine).SetUpdate(true).OnComplete(() =>
        {
            panelBack.SetActive(false);
            panelFront.SetActive(true);

            tweenRot2 = transform.DORotate(new Vector3(0, 0, 0), flip90Time).SetEase(Ease.InOutSine).SetUpdate(true);
        });
    }
}
