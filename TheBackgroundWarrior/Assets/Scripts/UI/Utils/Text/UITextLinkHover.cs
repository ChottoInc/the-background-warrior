using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(TMP_Text))]
public class UITextLinkHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Transform _tooltipPosition;


    private TMP_Text _text;
    private Camera _cam;
    private bool _pointerOver;
    private string _currentLinkId;

    private void Awake()
    {
        _text = GetComponent<TMP_Text>();

        Canvas canvas = _text.canvas;

        // Overlay canvases need a null camera, the others need the canvas camera
        _cam = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;
    }

    public void OnPointerEnter(PointerEventData eventData) => _pointerOver = true;

    public void OnPointerExit(PointerEventData eventData)
    {
        _pointerOver = false;
        SetLink(null);
    }

    private void OnDisable()
    {
        _pointerOver = false;
        SetLink(null);
    }

    private void Update()
    {
        if (!_pointerOver) return;

        int index = TMP_TextUtilities.FindIntersectingLink(_text, Input.mousePosition, _cam);

        if (index == -1)
        {
            SetLink(null);
        }
        else
        {
            SetLink(_text.textInfo.linkInfo[index].GetLinkID());
        }
    }

    private void SetLink(string linkId)
    {
        if (linkId == _currentLinkId) return;
        _currentLinkId = linkId;

        if (linkId == null)
            HideTooltip();
        else
            ShowTooltip(linkId);
    }

    private void ShowTooltip(string linkId)
    {
        string linkString = UtilsGeneral.GetTextByLink(linkId);

        TooltipManagerData tooltipData = new TooltipManagerData();
        tooltipData.idTooltip = UITooltipManager.ID_SHOW_TEXT;
        tooltipData.text = linkString;
        UITooltipManager.Instance.Show(tooltipData, _tooltipPosition.position, true);
    }

    private void HideTooltip()
    {
        UITooltipManager.Instance.Hide(UITooltipManager.ID_SHOW_TEXT, true);
    }
}
