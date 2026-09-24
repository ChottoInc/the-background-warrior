using UnityEngine;

public class UIHoverTextId : UIBaseTooltipName
{
    [SerializeField] string _textId;

    public override string GetText()
    {
        return UtilsText.AllText[_textId];
    }
}
