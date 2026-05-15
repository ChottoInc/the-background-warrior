using DG.Tweening;
using System.Collections;
using UnityEngine;

public class UIPanelLevelUp : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] GameObject panelText;

    [Space(10)]
    [SerializeField] bool isMovementEnabled = true;

    [Space(10)]
    [SerializeField] float showFor = 2f;

    [Space(10)]
    [SerializeField] float moveDuration = 1f;
    [SerializeField] Transform movePos;

    private bool isShowing;

    private Tween tweenMove;
    private Vector2 startPos;


    private void OnDestroy()
    {
        player.OnLevelUp -= ShowPanel;

        tweenMove?.Kill();
    }

    private void Awake()
    {
        player.OnLevelUp += ShowPanel;

        startPos = panelText.transform.localPosition;
    }

    private void ShowPanel()
    {
        if (isShowing) return;

        isShowing = true;

        panelText.SetActive(true);

        if(isMovementEnabled)
            tweenMove = panelText.transform.DOMove(movePos.position, moveDuration).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);

        StartCoroutine(CoHidePanel());
    }

    private IEnumerator CoHidePanel()
    {
        yield return new WaitForSeconds(showFor);

        isShowing = false;

        panelText.SetActive(false);

        ResetTween();
    }

    private void ResetTween()
    {
        tweenMove.Kill();

        panelText.transform.localPosition = startPos;
    }
}
