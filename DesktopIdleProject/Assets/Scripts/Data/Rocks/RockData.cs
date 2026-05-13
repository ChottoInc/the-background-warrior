using System;
using UnityEngine;

public class RockData
{
    private RockSO rockSO;

    private float maxDurability;
    private float currentDurability;




    public RockSO RockSO => rockSO;

    public float MaxDurability => maxDurability;
    public float CurrentDurability => currentDurability;


    public event Action OnTakeDamage;


    public RockData(RockSO rockSO)
    {
        this.rockSO = rockSO;

        currentDurability = UtilsGather.GetRockDurabilityByType(rockSO.RockType);
        maxDurability = currentDurability;
    }

    public void TakeDamage(PlayerMinerData data)
    {
        // can't take less than 0 or it will cure

        float baseDamage = data.CurrentPower;
        float total;

        total = Mathf.Max(0f, baseDamage);

        // subtract total to hp
        currentDurability -= total;

        if (currentDurability <= 0f)
        {
            currentDurability = 0;
        }

        OnTakeDamage?.Invoke();
    }

    public void TakeDamage(float damage)
    {
        // can't take less than 0 or it will cure

        float total;

        total = Mathf.Max(0f, damage);

        // subtract total to hp
        currentDurability -= total;

        if (currentDurability <= 0f)
        {
            currentDurability = 0;
        }

        OnTakeDamage?.Invoke();
    }

    public void SetSmashed()
    {
        currentDurability = 0;
    }
}
