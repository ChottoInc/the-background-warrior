using DG.Tweening;
using UnityEngine;

public class UITooltipBase : MonoBehaviour
{
    [SerializeField] protected CanvasGroup canvasGroup;

    [Space(10)]
    [SerializeField] protected float timeToFade = 1f;

    protected Tween tweenFade;


    public bool IsShowing { get; private set; }

    protected virtual void OnDestroy()
    {
        tweenFade?.Kill();
    }

    public virtual void Appear(bool fade)
    {
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

        IsShowing = true;
    }

    public virtual void Disappear(bool fade)
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

        IsShowing = false;
    }
}
