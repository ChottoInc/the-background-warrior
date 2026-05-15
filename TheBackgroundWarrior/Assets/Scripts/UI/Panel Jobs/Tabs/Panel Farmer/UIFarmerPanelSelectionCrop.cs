using System.Collections.Generic;
using UnityEngine;

public class UIFarmerPanelSelectionCrop : MonoBehaviour
{
    [SerializeField] UITabJobFarmer tabFarmer;
    [SerializeField] UIFarmerPanelCrops panelCrops;

    [Space(10)]
    [SerializeField] GameObject cropSelectionPrefab;
    [SerializeField] Transform container;

    private List<GameObject> cropObjs;

    public void Setup()
    {
        gameObject.SetActive(true);

        cropObjs = ClearList(cropObjs);
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

    private void FillCrops()
    {
        int lastIndex = Mathf.FloorToInt(PlayerManager.Instance.PlayerFarmerData.CurrentAgronomy) + 1; // +1 because the first is always available

        for (int i = 0; i < lastIndex; i++)
        {
            CropSO cropSO = UtilsGather.GetCropById(i);
            if(cropSO != null)
            {
                CreateSingleCropPrefab(cropSO);
            }
        }
    }

    private void CreateSingleCropPrefab(CropSO cropSO)
    {
        GameObject prefab = Instantiate(cropSelectionPrefab, transform.position, Quaternion.identity);
        prefab.transform.SetParent(container);

        prefab.transform.localScale = new Vector3(1, 1, 1);
        prefab.SetActive(true);

        if (prefab.TryGetComponent(out UIFarmerCropSelectionPrefab obj))
        {
            obj.Setup(this, cropSO);
        }
        cropObjs.Add(prefab);
    }

    public void OnCropSelected(CropSO cropSO)
    {
        panelCrops.OnCropSelected(cropSO);
        gameObject.SetActive(false);
    }

    public void OnButtonClose()
    {
        AudioManager.Instance.PlayClickUI();
        panelCrops.Setup();
    }
}
