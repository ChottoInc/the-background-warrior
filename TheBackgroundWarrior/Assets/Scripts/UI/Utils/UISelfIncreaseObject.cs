using UnityEngine;
using DG.Tweening;

public class UISelfIncreaseObject : MonoBehaviour
{
    [SerializeField] Transform objectToResize;

    [Space(10)]
    [SerializeField] float multiplier;
    [SerializeField] float timer = 1f;

    [Space(10)]
    [SerializeField] bool fromStart;
    [SerializeField] bool synchUpWithOthers = false;

    private bool requestedSynchUp;
    private float timerSynchUp;

    private Vector3 startSize;

    private Tween tweenScale;

    private void Awake()
    {
        startSize = objectToResize.localScale;

        timerSynchUp = timer * 2f;

        if (fromStart)
        {
            Resize();
        }
    }

    private void Update()
    {
        if(timerSynchUp <= 0)
        {
            if (requestedSynchUp)
            {
                requestedSynchUp = false;

                DOResize();
            }

            timerSynchUp = timer * 2f;
        }
        else
        {
            timerSynchUp -= Time.unscaledDeltaTime;
        }
    }

    public void Resize()
    {
        objectToResize.localScale = startSize;

        if (synchUpWithOthers)
        {
            requestedSynchUp = true;
        }
        else
        {
            DOResize();
        }
    }

    private void DOResize()
    {
        tweenScale = objectToResize.DOScale(startSize * multiplier, timer).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo).SetUpdate(true).SetLink(gameObject, LinkBehaviour.KillOnDestroy);
    }

    public void Stop()
    {
        tweenScale?.Kill();

        objectToResize.localScale = startSize;
    }
}
