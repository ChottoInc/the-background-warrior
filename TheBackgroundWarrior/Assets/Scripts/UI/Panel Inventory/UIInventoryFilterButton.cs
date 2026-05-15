using UnityEngine;
using UnityEngine.UI;

public class UIInventoryFilterButton : MonoBehaviour
{
    [SerializeField] UITabInventory tabInventory;
    [SerializeField] int filterId;
    [SerializeField] UtilsPlayer.PlayerJob[] showIfAvailableJobs;

    [Header("Highlight")]
    [SerializeField] Image imageSelected;
    [SerializeField] Color selectedColor;

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
        tabInventory.OpenInventory(this, filterId);
    }

    public void SelectButton(bool selected)
    {
        if (selected)
            imageSelected.color = selectedColor;
        else
            imageSelected.color = Color.white;
    }
}
