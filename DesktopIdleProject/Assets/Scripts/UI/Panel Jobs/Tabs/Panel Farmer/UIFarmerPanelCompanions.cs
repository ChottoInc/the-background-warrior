using System.Collections.Generic;
using UnityEngine;

public class UIFarmerPanelCompanions : MonoBehaviour
{
    [SerializeField] UITabJobFarmer tabFarmer;

    [Header("Companions")]
    [SerializeField] GameObject companionInfoPrefab;
    [SerializeField] Transform containerCompanions;

    private List<GameObject> companionObjs;

    [Header("Crops")]
    [SerializeField] GameObject cropInfoPrefab;
    [SerializeField] Transform containerCrops;

    private List<GameObject> cropsObjs;

    public void Setup()
    {
        gameObject.SetActive(true);

        companionObjs = ClearList(companionObjs);
        FillCompanions();

        //Debug.Log("Companions: " + PlayerManager.Instance.PlayerFarmerData.Companions.Count);
        //Debug.Log("Companions objs: " + companionObjs.Count);

        cropsObjs = ClearList(cropsObjs);
        FillCrops();
    }

    private List<GameObject> ClearList(List<GameObject> list)
    {
        if (list == null)
            list = new List<GameObject>();

        foreach (var item in list)
        {
            Destroy(item);
        }

        list.Clear();
        return list;
    }

    private void FillCompanions()
    {
        var companions = PlayerManager.Instance.PlayerFarmerData.Companions;
        //Debug.Log("Companions: " + PlayerManager.Instance.PlayerFarmerData.Companions.Count);
        //Debug.Log("Companions 2: " + companions.Count);
        foreach (var companion in companions)
        {
            CreateSingleCompanionPrefab(companion);
        }
    }

    private void FillCrops()
    {
        if(PlayerManager.Instance.PlayerFarmerData.Slot1CropData != null)
        {
            CreateSingleCropPrefab(PlayerManager.Instance.PlayerFarmerData.Slot1CropData);
        }
        else
        {
            //Debug.Log("slot 1 null: ");
        }

        if (PlayerManager.Instance.PlayerFarmerData.Slot2CropData != null)
        {
            CreateSingleCropPrefab(PlayerManager.Instance.PlayerFarmerData.Slot2CropData);
        }
        else
        {
            //Debug.Log("slot 2 null: ");
        }

        if (PlayerManager.Instance.PlayerFarmerData.Slot3CropData != null)
        {
            CreateSingleCropPrefab(PlayerManager.Instance.PlayerFarmerData.Slot3CropData);
        }
        else
        {
            //Debug.Log("slot 3 null: ");
        }

        if (PlayerManager.Instance.PlayerFarmerData.Slot4CropData != null)
        {
            CreateSingleCropPrefab(PlayerManager.Instance.PlayerFarmerData.Slot4CropData);
        }
        else
        {
            //Debug.Log("slot 4 null: ");
        }
    }

    private void CreateSingleCompanionPrefab(CompanionData companionData)
    {
        GameObject prefab = Instantiate(companionInfoPrefab, transform.position, Quaternion.identity);
        prefab.transform.SetParent(containerCompanions);

        prefab.transform.localScale = new Vector3(1, 1, 1);
        prefab.SetActive(true);

        if (prefab.TryGetComponent(out UIFarmerPanelCompanionInfoPrefab obj))
        {
            obj.Setup(this, companionData);
        }
        companionObjs.Add(prefab);
    }

    private void CreateSingleCropPrefab(CropData cropData)
    {
        GameObject prefab = Instantiate(cropInfoPrefab, transform.position, Quaternion.identity);
        prefab.transform.SetParent(containerCrops);

        prefab.transform.localScale = new Vector3(1, 1, 1);
        prefab.SetActive(true);

        if (prefab.TryGetComponent(out UIFarmerSideCropInfo obj))
        {
            obj.Setup(cropData);
        }
        cropsObjs.Add(prefab);
    }

    public void OnButtonEquip(CompanionData companionData)
    {
        int firstEmptySlot = PlayerManager.Instance.PlayerFarmerData.GetFirstEmptyCompanionSlot();

        if (firstEmptySlot != -1)
        {
            PlayerManager.Instance.PlayerFarmerData.SetCompanionToSlot(companionData, firstEmptySlot);
            //PlayerManager.Instance.UpdateFarmerData(tabFarmer.Player.PlayerData);
            PlayerManager.Instance.SaveFarmerData();

            // refresh
            Setup();
        }
        else
        {
            Debug.Log("All slots are full but equip button is interactable");
        }
    }

    public void OnButtonUnequip(CompanionData companionData)
    {
        int index = PlayerManager.Instance.PlayerFarmerData.GetCompanionIndexList(companionData.CompanionSO);

        if (index != -1)
        {
            PlayerManager.Instance.PlayerFarmerData.SetCompanionToSlot(companionData, -1);
            //PlayerManager.Instance.UpdateFarmerData(tabFarmer.Player.PlayerData);
            PlayerManager.Instance.SaveFarmerData();

            // refresh
            Setup();
        }
        else
        {
            Debug.Log("All slots are full but equip button is interactable");
        }
    }

    public void OnButtonCrops()
    {
        tabFarmer.OnButtonCrops();
    }
}
