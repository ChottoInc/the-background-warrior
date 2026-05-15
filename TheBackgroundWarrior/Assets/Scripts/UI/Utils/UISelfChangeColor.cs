using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UISelfChangeColor : MonoBehaviour
{
    private enum AnimationType { Switch, Pulse }

    [SerializeField] AnimationType animationType;

    [SerializeField] bool fromStart;

    [Space(10)]
    [SerializeField] Image imageToChange;

    [Space(10)]
    [SerializeField] Color secondColor;
    [SerializeField] float timerChangeColor = 1.5f;

    [Header("Pulse")]
    [SerializeField] float timerPulse = 1.5f;



    private Color startColor;


    private void Awake()
    {
        startColor = imageToChange.color;

        if (fromStart)
        {
            switch (animationType)
            {
                case AnimationType.Switch: StartSwitch(); break;
                case AnimationType.Pulse: StartPulse(); break;
            }
        }
    }

    public void StartSwitch()
    {
        imageToChange.DOColor(secondColor, timerChangeColor).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo).SetUpdate(true).SetLink(gameObject, LinkBehaviour.KillOnDestroy);
    }

    public void StartPulse()
    {
        imageToChange.color = secondColor;

        imageToChange.DOColor(startColor, timerPulse).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Restart).SetUpdate(true).SetLink(gameObject, LinkBehaviour.KillOnDestroy).OnComplete(() =>
        {
            imageToChange.color = secondColor;
        });
    }
}
