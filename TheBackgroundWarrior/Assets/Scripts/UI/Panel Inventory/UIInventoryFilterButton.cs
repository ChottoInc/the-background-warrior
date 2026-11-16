using UnityEngine;

public class UIInventoryFilterButton : UIBaseTooltipName
{
    [SerializeField] string _textId;

    [Header("Inventory")]
    [SerializeField] UITabInventory tabInventory;
    [SerializeField] UITabInventory.InventoryItemType _filter;
    [SerializeField] UtilsPlayer.PlayerJob[] showIfAvailableJobs;

    public void Refresh()
    {
        bool canShow = true;

        foreach (var job in showIfAvailableJobs)
        {
            if(!PlayerManager.Instance.PlayerJobsData.AvailableJobs.Contains(job))
            {
                canShow = false;
                break;
            }
        }

        gameObject.SetActive(canShow);
    }

    public void OnButtonClick()
    {
        tabInventory.OpenInventory(_filter);
    }

    public override string GetText()
    {
        return UtilsText.AllText[_textId];
    }
}
