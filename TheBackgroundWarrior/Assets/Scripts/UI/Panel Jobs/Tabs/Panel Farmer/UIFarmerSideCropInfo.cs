using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIFarmerSideCropInfo : MonoBehaviour
{
    [SerializeField] Image imageCrop;
    [SerializeField] TMP_Text textName;
    [SerializeField] GenericBar barGrowth;

    public void Setup(CropData cropData)
    {
        // show last sprite of the crop, full growth
        imageCrop.sprite = cropData.CropSO.SpriteCrop[cropData.CropSO.SpriteCrop.Length - 1];

        textName.text = cropData.CropSO.CropName;

        barGrowth.Setup(cropData.GrowthTime, cropData.CurrentGrowth);
    }
}
