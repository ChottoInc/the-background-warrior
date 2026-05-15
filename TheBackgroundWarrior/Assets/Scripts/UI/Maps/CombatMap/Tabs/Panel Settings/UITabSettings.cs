using UnityEngine;

public class UITabSettings : UITabWindow
{
    [SerializeField] TabManager tabManager;

    public override void Open()
    {
        base.Open();

        tabManager.SelectFirstTab();
    }

    public void OnButtonClose()
    {
        AudioManager.Instance.PlayClickUI();

        Close();
    }
}
