using System.Collections;
using UnityEngine;
using DG.Tweening;

public class UIShopPanelAnimation : MonoBehaviour
{
    [SerializeField] UIShopPanelDebug _panelDebug;

    [Header("Elements")]
    [SerializeField] Transform _centerPoint;
    [SerializeField] GameObject _imageAlchemist;
    [SerializeField] GameObject _textUnlocked;

    [Header("Animation")]
    [SerializeField] float _timerBeforeRotation = 1f;
    [SerializeField] float _timerSelfRotation = 1.5f;
    [SerializeField] float _timerAppearAlchemist = 2f;
    [SerializeField] float _timerEndAnimation = 3f;

    [Space(10)]
    [SerializeField] float _scaleText = 1.1f;
    [SerializeField] float _timerScale = 0.75f;


    public void Open()
    {
        gameObject.SetActive(true);
        StartAnimation();
    }

    private void StartAnimation()
    {
        StartCoroutine(CoAnimation());
    }

    private void EndAnimation()
    {
        gameObject.SetActive(false);
        _panelDebug.EndAnimation();
    }

    private IEnumerator CoAnimation()
    {
        yield return new WaitForSecondsRealtime(_timerBeforeRotation);

        _centerPoint.DORotate(new Vector3(0f, 0f, 360f), _timerSelfRotation, RotateMode.FastBeyond360).SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart).SetUpdate(true)
            .SetLink(gameObject, LinkBehaviour.KillOnDestroy);

        yield return new WaitForSecondsRealtime(_timerAppearAlchemist);

        _imageAlchemist.SetActive(true);
        _textUnlocked.SetActive(true);

        _textUnlocked.transform.DOScale(_textUnlocked.transform.localScale * _scaleText, _timerScale).SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo).SetUpdate(true)
            .SetLink(gameObject, LinkBehaviour.KillOnDestroy);

        yield return new WaitForSecondsRealtime(_timerEndAnimation);

        EndAnimation();
    }
}
