using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class UIScrollHelper : MonoBehaviour
{
    [Header("Vertical")]
    [SerializeField] Scrollbar _barVertical;
    [SerializeField] UIScrollTriangle _triangleUp;
    [SerializeField] UIScrollTriangle _triangleDown;

    [Header("Horizontal")]
    [SerializeField] Scrollbar _barHorizontal;
    [SerializeField] UIScrollTriangle _triangleRight;
    [SerializeField] UIScrollTriangle _triangleLeft;

    [Space(10)]
    [SerializeField] float _offsetCheck = 0.1f;

    private float _offsetStart;
    private float _offsetEnd;

    private ScrollRect _scroll;

    [Header("Centering")]
    [SerializeField] VerticalLayoutGroup _verticalLayoutGroup;

    private int _startingPaddingLeft;
    private int _startingPaddingRight;

    private int _lastChildrenCount;

    private event Action OnChildrenCountDiffers;



    private bool _isInit;


    private void Awake()
    {
        _scroll = GetComponent<ScrollRect>();

        OnChildrenCountDiffers += CheckVerticalBar;
    }

    private void OnDestroy()
    {
        OnChildrenCountDiffers -= CheckVerticalBar;
    }

    private void Start()
    {
        _offsetStart = 1f - _offsetCheck;
        _offsetEnd = 0f + _offsetCheck;

        if(_verticalLayoutGroup != null)
        {
            _startingPaddingLeft = _verticalLayoutGroup.padding.left;
            _startingPaddingRight = _verticalLayoutGroup.padding.right;
        }
    }

    private void LateUpdate()
    {
        if (!_isInit)
        {
            _isInit = true;

            CheckVerticalBar();
        }
        else
        {
            HandleVertical();
            HandleHorizontal();

            CheckChildrenCount();
        }
    }

    private void HandleHorizontal()
    {
        if (_barHorizontal == null) return;

        if (!IsHorizontalScrollbarActive()) return;

        // Call resume if not at the bottom
        if (_barHorizontal.value < _offsetStart)
        {
            _triangleRight.Resume();
        }
        else
        {
            _triangleRight.Pause();
        }

        // Call resume if not at the top
        if (_barHorizontal.value > _offsetEnd)
        {
            _triangleLeft.Resume();
        }
        else
        {
            _triangleLeft.Pause();
        }
    }

    private void HandleVertical()
    {
        if (_barVertical == null) return;

        if (!IsVerticalScrollbarActive()) return;

        // Call resume if not at the bottom
        if (_barVertical.value < _offsetStart)
        {
            _triangleUp.Resume();
        }
        else
        {
            _triangleUp.Pause();
        }

        // Call resume if not at the top
        if (_barVertical.value > _offsetEnd)
        {
            _triangleDown.Resume();
        }
        else
        {
            _triangleDown.Pause();
        }
    }

    private void CheckChildrenCount()
    {
        if (_verticalLayoutGroup == null) return;

        int currentChildrenCount = _verticalLayoutGroup.transform.childCount;

        if(currentChildrenCount != _lastChildrenCount)
        {
            OnChildrenCountDiffers?.Invoke();
            _lastChildrenCount = currentChildrenCount;
        }
    }

    private void CheckVerticalBar()
    {
        if (_barVertical == null) return;

        if(!IsVerticalScrollbarActive())
        {
            _triangleUp.Hide();
            _triangleDown.Hide();
        }
        else
        {
            _triangleUp.Show();
            _triangleDown.Show();
        }

        if (_verticalLayoutGroup == null) return;
        
        if(IsVerticalScrollbarActive())
        {
            _verticalLayoutGroup.padding.left = _startingPaddingLeft;
            _verticalLayoutGroup.padding.right = _startingPaddingRight;
        }
        else
        {
            _verticalLayoutGroup.padding.left = 0;
            _verticalLayoutGroup.padding.right = 0;
        }
    }

    private bool IsVerticalScrollbarActive()
    {
        return _scroll.content.rect.height > _scroll.viewport.rect.height + 0.01f; // small epsilon for float precision
    }

    private bool IsHorizontalScrollbarActive()
    {
        return _scroll.content.rect.width > _scroll.viewport.rect.width + 0.01f; // small epsilon for float precision
    }
}
