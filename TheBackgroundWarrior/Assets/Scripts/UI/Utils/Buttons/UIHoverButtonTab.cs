using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIHoverButtonTab : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] UIHoverButtonTabManager _manager;

    [SerializeField] GameObject _border;

    private Image _imageBorder;

    [Header("Highlight")]
    [SerializeField] bool _highlight;
    [SerializeField] Color _selectedColor;

    public bool IsSelected { get; private set; }

    private void Start()
    {
        if(_manager == null)
        {
            _manager = GetComponentInParent<UIHoverButtonTabManager>();
        }

        if(_border != null)
        {
            _imageBorder = _border.GetComponent<Image>();
        }
    }

    public void Hover()
    {
        _border.SetActive(true);
    }

    public void Unhover()
    {
        _border.SetActive(false);
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        _manager.ChangeHovered(this);
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        if (IsSelected) return;

        _manager.ChangeHovered(null);
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        // prevent the border to disappear
        if(!IsSelected)
            _manager.ChangeSelected(this);

        IsSelected = true;

        if (_highlight)
        {
            if(_imageBorder != null)
            {
                _imageBorder.color = _selectedColor;
            }
        }
    }

    public void Deselect()
    {
        IsSelected = false;

        if (_highlight)
        {
            if (_imageBorder != null)
            {
                _imageBorder.color = Color.white;
            }
        }
    }
}
