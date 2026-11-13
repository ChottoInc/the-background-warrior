using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITooltipBuff : MonoBehaviour
{
    [SerializeField] CanvasGroup canvasGroup;

    [Space(10)]
    [SerializeField] float timeToFade = 1f;

    [Space(10)]
    [SerializeField] RectTransform _root;
    [SerializeField] GridLayoutGroup _gridLayout;

    [Header("Prefab")]
    [SerializeField] GameObject _buffPrefab;
    [SerializeField] Transform _container;

    private List<GameObject> _buffObjects;

    [Header("Width")]
    [SerializeField] float minWidth = 80f;
    [SerializeField] float maxWidth = 2000f;

    private Tween tweenFade;

    private void OnDestroy()
    {
        tweenFade?.Kill();
    }

    private void Resize()
    {
        int elementCount = PlayerManager.Instance.PlayerBuffsData.ActiveBuffs.Count;
        int columns = Mathf.CeilToInt((float)elementCount / _gridLayout.constraintCount);

        float totalWidth = columns * (_gridLayout.cellSize.x + _gridLayout.spacing.x)
                           - _gridLayout.spacing.x
                           + _gridLayout.padding.left
                           + _gridLayout.padding.right;

        totalWidth = Mathf.Clamp(totalWidth, minWidth, maxWidth);
        _root.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, totalWidth);

        //Debug.Log("conctions buff tooltip columns: " + columns);
        //Debug.Log("conctions buff tooltip size: " + totalWidth);
    }

    public void Show(bool fade = false)
    {
        _buffObjects = ClearList(_buffObjects);

        FillBuff();

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

        Resize();
    }

    private List<GameObject> ClearList(List<GameObject> list)
    {
        if (list == null)
            list = new List<GameObject>();

        foreach (var item in list)
        {
            Destroy(item);
        }

        list.Clear();
        return list;
    }

    private void FillBuff()
    {
        var buffList = PlayerManager.Instance.PlayerBuffsData.ActiveBuffs;

        for (int i = 0; i < buffList.Count; i++)
        {
            var buff = buffList[i];

            // skip inspiration buff if it's over
            if(buff.BuffType == UtilsBuffs.BuffType.Inspiration && buff.RemainingTime <= 0)
            {
                continue;
            }

            GameObject prefab = Instantiate(_buffPrefab, transform.position, Quaternion.identity);
            prefab.transform.SetParent(_container);

            prefab.transform.localScale = new Vector3(1, 1, 1);
            prefab.SetActive(true);

            if (prefab.TryGetComponent(out UITooltipBuffPrefab obj))
            {
                obj.Setup(buff);
            }
            _buffObjects.Add(prefab);
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
