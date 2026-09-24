using UnityEngine;
using UnityEngine.EventSystems;

public class UIBaseTooltipName : MonoBehaviour, ITooltipNameable
{
    [Header("Tooltip")]
    [SerializeField] Transform _tooltipPosition;

    private void OnDisable()
    {
        OnPointerExit(null);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipManagerData tooltipData = new TooltipManagerData();
        tooltipData.idTooltip = UITooltipManager.ID_SHOW_TEXT;
        tooltipData.text = GetText();

        if(_tooltipPosition != null)
            UITooltipManager.Instance.Show(tooltipData, _tooltipPosition.position, true);
        else
            UITooltipManager.Instance.Show(tooltipData, transform.position, true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UITooltipManager.Instance.Hide(UITooltipManager.ID_SHOW_TEXT, true);
    }

    public virtual string GetText()
    {
        Debug.Log("Subclass need to override");
        return "N/A";
    }

}
