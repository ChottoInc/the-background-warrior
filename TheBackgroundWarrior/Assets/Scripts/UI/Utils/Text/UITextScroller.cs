using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class UITextScroller : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IScrollHandler
{
    [Header("Objects")]
    [SerializeField] private RectTransform _content;
    [SerializeField] private RectTransform _viewport;
    [SerializeField] private ContentSizeFitter _contentSizeFitter;

    [Header("Scroll")]
    [SerializeField] private float _scrollSpeed = 0.2f;
    [SerializeField] private float _resumeDelay = 2f;
    [SerializeField] private float _wheelSensitivity = 0.2f;

    [Header("Text")]
    [SerializeField] TMP_Text _text;
    [SerializeField] TextAlignmentOptions _alignment = TextAlignmentOptions.Center;


    private ScrollRect _scrollRect;

    private bool _isUserInteracting;

    private float _resumeTimer;

    private bool _isAutoScrolling = true;
    private bool _canScroll;

    private bool _upwards = true;

    private bool _isInitialized;

    private void Awake()
    {
        _scrollRect = GetComponent<ScrollRect>();
    }

    private void LateUpdate()
    {
        UpdateScrollState();

        if (!_canScroll)
        {
            return;
        }

        if (_isUserInteracting) return;

        if (!_isAutoScrolling)
        {
            _resumeTimer -= Time.unscaledDeltaTime;
            if (_resumeTimer <= 0f)
            {
                _isAutoScrolling = true;
            }
            return;
        }

        float pos = _scrollRect.horizontalNormalizedPosition;

        if(_upwards)
            pos += _scrollSpeed * Time.unscaledDeltaTime;
        else
            pos -= _scrollSpeed * Time.unscaledDeltaTime;

        if (pos > 1f)
        {
            //pos = 0f;

            _upwards = false;

            _isAutoScrolling = false;
            _resumeTimer = _resumeDelay;
        }
        else if(pos <= 0f)
        {
            _upwards = true;

            _isAutoScrolling = false;
            _resumeTimer = _resumeDelay;
        }

        _scrollRect.horizontalNormalizedPosition = pos;
    }

    private void UpdateScrollState()
    {
        float contentWidth = _content.rect.width;
        float viewportWidth = _viewport.rect.width;

        bool shouldScroll = contentWidth > viewportWidth;

        if (_isInitialized && shouldScroll == _canScroll) return;

        _isInitialized = true;
        _canScroll = shouldScroll;
        _scrollRect.horizontal = _canScroll;

        if (_canScroll)
        {
            SetOverflowMode();
            _scrollRect.horizontalNormalizedPosition = 0f;
            _isAutoScrolling = true;
            _isUserInteracting = false;
        }
        else
        {
            SetCenteredMode();
        }
    }

    private void SetOverflowMode()
    {
        // Non-stretched, left-aligned anchors — free-floating box positioned by ScrollRect itself
        //content.anchorMin = new Vector2(0f, content.anchorMin.y);
        //content.anchorMax = new Vector2(0f, content.anchorMax.y);
        //content.pivot = new Vector2(0f, content.pivot.y);

        _contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
    }

    private void SetCenteredMode()
    {
        _contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;

        // Stretch anchors full width (same as Alt+click "stretch" preset)
        _content.anchorMin = new Vector2(0f, _content.anchorMin.y);
        _content.anchorMax = new Vector2(1f, _content.anchorMax.y);
        _content.pivot = new Vector2(0.5f, _content.pivot.y);

        
        // Stretched anchors drive size via offsets, not sizeDelta directly —
        // reset offsets so it actually spans the full viewport width
        _content.offsetMin = new Vector2(0f, _content.offsetMin.y);
        _content.offsetMax = new Vector2(0f, _content.offsetMax.y);

        if(_text != null)
        {
            _text.alignment = _alignment;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!_canScroll) return;
        _isUserInteracting = true;
        _isAutoScrolling = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!_canScroll) return;
        _isUserInteracting = false;
        _resumeTimer = _resumeDelay;
    }

    public void OnScroll(PointerEventData eventData)
    {
        if (!_canScroll) return;

        _isAutoScrolling = false;
        _resumeTimer = _resumeDelay;

        float delta = eventData.scrollDelta.y * _wheelSensitivity;
        float pos = _scrollRect.horizontalNormalizedPosition - delta;
        _scrollRect.horizontalNormalizedPosition = Mathf.Clamp01(pos);
    }



}
