using UnityEngine;
using UnityEngine.UI;

public class UIButtonJobTab : MonoBehaviour
{
    [SerializeField] UtilsPlayer.PlayerJob job;

    [Space(10)]
    [SerializeField] GameObject[] links;

    [Space(10)]
    [SerializeField] GameObject barrier;
    [SerializeField] Button button;

    [Space(10)]
    [SerializeField] Transform showTooltipPosition;

    private PlayerJobSO jobSO;

    private bool isActive;
    private bool isShow;
    
    public void Refresh()
    {
        // Initialize
        if(jobSO == null)
        {
            jobSO = UtilsPlayer.GetJobByType(job);
        }

        // Check if all required jobs are available, if so show the job
        isShow = true;

        foreach (var requiredJob in jobSO.RequiredJobs)
        {
            if (!PlayerManager.Instance.PlayerJobsData.AvailableJobs.Contains(requiredJob))
            {
                isShow = false;
            }
        }

        gameObject.SetActive(isShow);

        foreach (var link in links)
        {
            link.SetActive(isShow);
        }

        // Check if the player can actually do the job, if so activate the button
        isActive = PlayerManager.Instance.PlayerJobsData.AvailableJobs.Contains(job);

        button.interactable = isActive;
        barrier.SetActive(!isActive);
    }

    public void OnPointerEnter()
    {
        if (!isShow) return;

        if (isActive) return;

        TooltipManagerData data = new TooltipManagerData();
        data.idTooltip = UITooltipManager.ID_SHOW_TEXT;
        data.text = jobSO.JobUnlockConditions;

        UITooltipManager.Instance.Show(data, showTooltipPosition.position, true);
    }

    public void OnPointerExit()
    {
        if (!isShow) return;

        if (isActive) return;

        UITooltipManager.Instance.Hide(UITooltipManager.ID_SHOW_TEXT, true);
    }
}
