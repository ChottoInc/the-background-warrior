using UnityEngine;
using UnityEngine.EventSystems;

public class UIHoverBuffs : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] UITab _tabBuff;

    private bool _isOpen;
    private bool _isTabbed;

    private void Awake()
    {
        _tabBuff.OnSelected += Selected;
        _tabBuff.OnDeselected += Deselected;
    }

    private void OnDestroy()
    {
        _tabBuff.OnSelected -= Selected;
        _tabBuff.OnDeselected -= Deselected;
    }

    private void OnEnable()
    {
        if (!PlayerManager.Instance.PlayerJobsData.AvailableJobs.Contains(UtilsPlayer.PlayerJob.Alchemist) &&
            !PlayerManager.Instance.PlayerJobsData.AvailableJobs.Contains(UtilsPlayer.PlayerJob.Bard))
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }

    private void Selected()
    {
        _isTabbed = true;

        if(_isOpen)
        {
            OnExit();
        }
    }

    private void Deselected()
    {
        _isTabbed = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        var buffDatas = PlayerManager.Instance.PlayerBuffsData;

        // check if on buffs or if the only one is inspiration with no remaining time, don't show in that case
        if (buffDatas.ActiveBuffs.Count < 1) 
            return;
        else if(buffDatas.ActiveBuffs.Count == 1)
        {
            if (buffDatas.ActiveBuffs[0].BuffType == UtilsBuffs.BuffType.Inspiration)
            {
                if (buffDatas.ActiveBuffs[0].RemainingTime <= 0) return;
            }
        }

        if (_isTabbed) return;

        TooltipManagerData tooltipData = new TooltipManagerData();
        tooltipData.idTooltip = UITooltipManager.ID_SHOW_BUFFS;
        UITooltipManager.Instance.Show(tooltipData, transform.position, true);

        _isOpen = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!_isOpen) return;

        OnExit();
    }

    private void OnExit()
    {
        UITooltipManager.Instance.Hide(UITooltipManager.ID_SHOW_BUFFS, true);

        _isOpen = false;
    }
}
