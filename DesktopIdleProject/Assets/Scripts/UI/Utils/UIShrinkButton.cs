using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

public class UIShrinkButton : Button
{
    [SerializeField] float multiplier = 0.9f;
    [SerializeField] float timerShrink = 0.1f;

    private Vector3 startScale;

    private Tween tweenScale;

    private bool isClicked;

    protected override void OnDestroy()
    {
        tweenScale?.Kill();
    }

    protected override void Awake()
    {
        startScale = transform.localScale;
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (isClicked) return;

        if(transform.TryGetComponent(out UISelfIncreaseObject incrObj))
        {
            incrObj.Stop();
        }

        tweenScale?.Kill();

        tweenScale = transform.DOScale(startScale * multiplier, timerShrink).SetEase(Ease.InOutSine).SetUpdate(true);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (isClicked) return;

        if (!interactable) return;

        tweenScale?.Kill();

        AudioManager.Instance.PlayClickUI();

        isClicked = true;

        transform.localScale = startScale;

        tweenScale = transform.DOScale(startScale * multiplier, timerShrink).SetEase(Ease.InOutSine).SetUpdate(true);

        tweenScale.OnComplete(() =>
        {
            base.OnPointerClick(eventData);
            isClicked = false;
        });
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        if (isClicked) return;

        tweenScale?.Kill();

        tweenScale = transform.DOScale(startScale, timerShrink).SetEase(Ease.InOutSine).SetUpdate(true).OnComplete(() =>
        {
            if (transform.TryGetComponent(out UISelfIncreaseObject incrObj))
            {
                incrObj.Resize();
            }
        });
    }
}
