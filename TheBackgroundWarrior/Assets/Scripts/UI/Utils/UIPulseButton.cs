using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIPulseButton : MonoBehaviour
{
    [SerializeField] bool fromStart;

    [Space(10)]
    [SerializeField] Image objectToChange;

    [Space(10)]
    [SerializeField] float timerPulse;
    [SerializeField] float multiplier;

    private Vector3 startScale;

    private Tween tweenScale;

    private bool isPulsing;


    public bool IsPulsing => isPulsing;

    private void Awake()
    {
        startScale = objectToChange.transform.localScale;

        if (fromStart)
        {
            StartPulse();
        }
    }

    public void StartPulse()
    {
        isPulsing = true;

        objectToChange.transform.localScale = startScale * multiplier;

        tweenScale = objectToChange.transform.DOScale(startScale, timerPulse).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Restart).SetUpdate(true).SetLink(gameObject, LinkBehaviour.KillOnDestroy).OnComplete(() =>
        {
            objectToChange.transform.localScale = startScale * multiplier;
        });
    }

    public void Stop()
    {
        tweenScale?.Kill();

        objectToChange.transform.localScale = startScale;
    }
}
