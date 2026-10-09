using System;
using UnityEngine;

public class CropData
{
    private CropSO cropSO;


    private float baseGrowthTime;

    private int plantedSlot;


    public CropSO CropSO => cropSO;

    public float GrowthTime => 
        baseGrowthTime -
        (baseGrowthTime * PlayerManager.Instance.PlayerFarmerData.CurrentGreenthumb);

    public float CurrentGrowth { get; private set; }
    public int PlantedSlot => plantedSlot;


    public bool IsFullyGrown => CurrentGrowth >= GrowthTime;



    public CropData(CropSO cropSO, int plantedSlot)
    {
        this.cropSO = cropSO;

        baseGrowthTime = cropSO.BaseGrowthTime;
        CurrentGrowth = 0;

        this.plantedSlot = plantedSlot;
    }

    public CropData(CropSaveData saveData)
    {
        cropSO = UtilsItem.GetItemById(saveData.cropId) as CropSO;

        baseGrowthTime = cropSO.BaseGrowthTime;
        CurrentGrowth = saveData.currentGrowth;

        CurrentGrowth = saveData.currentGrowth;

        plantedSlot = saveData.plantedSlot;
    }

    public Sprite GetCurrentSprite()
    {
        int maxSprites = CropSO.SpriteCrop.Length;
        float percGrowth = CurrentGrowth / GrowthTime;

        // set to max - 1, so when the growth is not 100%, the right sprite will be shown
        int spriteIndex;

        if(percGrowth >= 1f)
        {
            spriteIndex = maxSprites - 1;
            return CropSO.SpriteCrop[spriteIndex];
        }
        else
        {
            spriteIndex = Mathf.FloorToInt(percGrowth * (maxSprites - 1));
            return CropSO.SpriteCrop[spriteIndex];
        }
    }

    public void AddGrowth(float t)
    {
        CurrentGrowth += t;

        // set max as growth time
        CurrentGrowth = Mathf.Min(GrowthTime, CurrentGrowth);

        // reward exp if growth reaches max
        if(IsFullyGrown)
        {
            PlayerManager.Instance.PlayerFarmerData.AddExp(cropSO.RewardedExp);
            PlayerManager.Instance.SaveFarmerData();
        }
    }

    public void ResetGrowth()
    {
        CurrentGrowth = 0;

        PlayerManager.Instance.SaveFarmerData();
    }
}