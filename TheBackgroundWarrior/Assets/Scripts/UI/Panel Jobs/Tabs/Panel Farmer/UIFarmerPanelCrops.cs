using UnityEngine;

public class UIFarmerPanelCrops : MonoBehaviour
{
    [SerializeField] UITabJobFarmer tabFarmer;

    [Header("Crops")]
    [SerializeField] UIFarmerPlantedCropInfo crop1Info;
    [SerializeField] UIFarmerPlantedCropInfo crop2Info;
    [SerializeField] UIFarmerPlantedCropInfo crop3Info;
    [SerializeField] UIFarmerPlantedCropInfo crop4Info;

    private int lastSelectedSlotInfo;

    [Space(10)]
    [SerializeField] GameObject panelList;
    [SerializeField] UIFarmerPanelSelectionCrop panelSelectionCrops;

    [Header("Companions")]
    [SerializeField] UIFarmerSideCompanionInfo equipped1Info;
    [SerializeField] UIFarmerSideCompanionInfo equipped2Info;
    [SerializeField] UIFarmerSideCompanionInfo equipped3Info;

    public void Setup()
    {
        gameObject.SetActive(true);
        panelList.SetActive(true);
        panelSelectionCrops.gameObject.SetActive(false);

        lastSelectedSlotInfo = 1;

        crop1Info.Setup(PlayerManager.Instance.PlayerFarmerData.Slot1CropData);
        crop2Info.Setup(PlayerManager.Instance.PlayerFarmerData.Slot2CropData);
        crop3Info.Setup(PlayerManager.Instance.PlayerFarmerData.Slot3CropData);
        crop4Info.Setup(PlayerManager.Instance.PlayerFarmerData.Slot4CropData);

        equipped1Info.Setup(null);
        equipped2Info.Setup(null);
        equipped3Info.Setup(null);

        var equippedCompanions = PlayerManager.Instance.PlayerFarmerData.GetEquippedCompanions();
        foreach (var equipped in equippedCompanions)
        {
            switch (equipped.slot)
            {
                case 0: equipped1Info.Setup(equipped.companionData); break;
                case 1: equipped2Info.Setup(equipped.companionData); break;
                case 2: equipped3Info.Setup(equipped.companionData); break;
            }
        }
    }

    public void OpenPanelSelectionCrops(int slot)
    {
        lastSelectedSlotInfo = slot;

        panelList.SetActive(false);

        panelSelectionCrops.Setup();
    }

    public void OnCropSelected(CropSO cropSO)
    {
        CropData cropData = null;

        switch (lastSelectedSlotInfo)
        {
            default: Debug.Log("Crop slot not selected"); break;
            case 0:
                cropData = PlayerManager.Instance.PlayerFarmerData.SetCropToSlot(cropSO, 0);
                //crop1Info.Setup(cropData);
                break;

            case 1:
                cropData = PlayerManager.Instance.PlayerFarmerData.SetCropToSlot(cropSO, 1);
                //crop2Info.Setup(cropData);
                break;

            case 2:
                cropData = PlayerManager.Instance.PlayerFarmerData.SetCropToSlot(cropSO, 2);
                //crop3Info.Setup(cropData);
                break;

            case 3:
                cropData = PlayerManager.Instance.PlayerFarmerData.SetCropToSlot(cropSO, 3);
                //crop4Info.Setup(cropData);
                break;
        }

        if(cropData != null)
        {
            PlayerManager.Instance.SaveFarmerData();

            if(CropsPlantManager.Instance != null)
            {
                CropsPlantManager.Instance.SetCrop(lastSelectedSlotInfo, cropData, true);
            }
                

            //Debug.Log("setted slot " + lastSelectedSlotInfo + ", crop: " + cropData.CropSO.CropName);
        }

        // refresh
        Setup();
    }

    public void OnButtonCompanions()
    {
        tabFarmer.OnButtonCompanions();
    }
}
