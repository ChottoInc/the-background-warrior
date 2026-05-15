using UnityEngine;

public interface ITooltipWindow
{
    public void Show(string text, Vector2 position, bool fade = false);
    public void Hide(bool fade = false);
}
