using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIFarmerPlantedCropInfo : MonoBehaviour
{
    [SerializeField] UIFarmerPanelCrops panelCrops;
    [SerializeField] int slot;

    [Space(10)]
    [SerializeField] Sprite spriteAdd;
    [SerializeField] Image imageCrop;

    [Space(10)]
    [SerializeField] TMP_Text textName;
    [SerializeField] TMP_Text textGrowth;

    [Space(10)]
    [SerializeField] GenericBar barGrowth;

    private CropData _cropData;


    public void Setup(CropData cropData)
    {
        _cropData = cropData;

        if(cropData == null)
        {
            imageCrop.sprite = spriteAdd;

            textName.text = string.Format("Add a new crop to grow");
            textGrowth.gameObject.SetActive(false);

            barGrowth.gameObject.SetActive(false);
        }
        else
        {
            // show last sprite of the crop, full growth
            imageCrop.sprite = cropData.CropSO.SpriteCrop[cropData.CropSO.SpriteCrop.Length - 1];

            textName.text = cropData.CropSO.ItemName;

            textGrowth.gameObject.SetActive(true);

            float percGrowth = (cropData.CurrentGrowth / cropData.GrowthTime) * 100f;
            percGrowth = Mathf.Min(percGrowth, 100f); // ensure it doesn't exceed 100%
            textGrowth.text = string.Format("Growth: {0:0}%", percGrowth);

            barGrowth.gameObject.SetActive(true);
            barGrowth.Setup(cropData.GrowthTime, cropData.CurrentGrowth);
        }
    }

    public void OnButtonAddCrop()
    {
        panelCrops.OpenPanelSelectionCrops(slot);
    }
}
