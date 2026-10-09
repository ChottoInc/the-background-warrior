using UnityEngine;
using UnityEngine.UI;

public class UIFarmerCropSelectionPrefab : MonoBehaviour
{
    [SerializeField] Image imageCrop;
    [SerializeField] Transform tooltipPosition;

    private bool _isShowingTooltip;

    private UIFarmerPanelSelectionCrop _panelSelection;
    private CropSO _cropSO;

    public void Setup(UIFarmerPanelSelectionCrop panelSelection, CropSO cropSO)
    {
        _panelSelection = panelSelection;
        _cropSO = cropSO;

        imageCrop.sprite = cropSO.SpriteSeed;
    }

    public void OnPointerEnter()
    {
        if (_isShowingTooltip) return;

        _isShowingTooltip = true;

        string possibleCompanions = string.Empty;

        for (int i = 0; i < _cropSO.AttractedCompanions.Length; i++)
        {
            possibleCompanions += _cropSO.AttractedCompanions[i].CompanionName;

            // add new line only when not last possible
            if(i < _cropSO.AttractedCompanions.Length - 1)
            {
                possibleCompanions += "\n";
            }
        }

        float cropGrowthTime = _cropSO.BaseGrowthTime - (_cropSO.BaseGrowthTime * PlayerManager.Instance.PlayerFarmerData.CurrentGreenthumb);

        string text = string.Format(
            "{0}\n" +
            UtilsText.AllText[UtilsText.text_job_farmer_crop_basegrowthtime] +
            UtilsText.AllText[UtilsText.text_job_farmer_crop_attracts] +
            "{3}",
            _cropSO.ItemName,
            Mathf.FloorToInt(cropGrowthTime / 60f),
            Mathf.FloorToInt(cropGrowthTime % 60f),
            possibleCompanions);


        TooltipManagerData tooltipData = new TooltipManagerData();
        tooltipData.idTooltip = UITooltipManager.ID_SHOW_TEXT;
        tooltipData.text = text;
        UITooltipManager.Instance.Show(tooltipData, tooltipPosition.position, true, 35f);
    }

    public void OnPointerExit()
    {
        if (!_isShowingTooltip) return;

        _isShowingTooltip = false;

        UITooltipManager.Instance.Hide(UITooltipManager.ID_SHOW_TEXT, true);
    }

    public void OnCropSelected()
    {
        if (_isShowingTooltip)
        {
            UITooltipManager.Instance.Hide(UITooltipManager.ID_SHOW_TEXT, true);
        }

        AudioManager.Instance.PlayClickUI();
        _panelSelection.OnCropSelected(_cropSO);
    }
}
