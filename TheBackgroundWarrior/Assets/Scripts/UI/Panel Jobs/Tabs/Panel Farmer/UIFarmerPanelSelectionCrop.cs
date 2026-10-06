using System.Collections.Generic;
using UnityEngine;

public class UIFarmerPanelSelectionCrop : MonoBehaviour
{
    [SerializeField] UITabJobFarmer tabFarmer;
    [SerializeField] UIFarmerPanelCrops panelCrops;

    [Space(10)]
    [SerializeField] GameObject cropSelectionPrefab;
    [SerializeField] Transform container;

    private List<GameObject> _cropObjs;

    public void Setup()
    {
        gameObject.SetActive(true);

        _cropObjs = ClearList(_cropObjs);
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
        var crops = UtilsItem.GetCropsByAgronomyLevel(Mathf.FloorToInt(PlayerManager.Instance.PlayerFarmerData.CurrentAgronomy));

        foreach (var crop in crops)
        {
            if (crop != null)
            {
                CreateSingleCropPrefab(crop);
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
        _cropObjs.Add(prefab);
    }

    public void OnCropSelected(CropSO cropSO)
    {
        panelCrops.OnCropSelected(cropSO);
        gameObject.SetActive(false);
    }

    public void OnButtonClose()
    {
        panelCrops.Setup();
    }
}
