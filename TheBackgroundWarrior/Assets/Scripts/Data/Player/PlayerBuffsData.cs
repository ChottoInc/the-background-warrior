using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UtilsBuffs;

public class PlayerBuffsData
{
    public List<Buff> ActiveBuffs { get; private set; }

    public event Action<BuffType> OnAddBuff;
    public event Action<BuffType> OnRemoveBuff;

    public PlayerBuffsData()
    {
        GenerateBaseStats();
    }
    
    public PlayerBuffsData(PlayerBuffsSaveData saveData)
    {
        GenerateBaseStats();

        // assign event trigger to expire and add to list
        foreach (var saveBuff in saveData.buffs)
        {
            Buff buff = new Buff(saveBuff);
            buff.OnBuffExpired += HandlerExpiredBuff;
            ActiveBuffs.Add(buff);
        }
    }

    private void GenerateBaseStats()
    {
        ActiveBuffs = new List<Buff>();
    }

    public bool HasBuff(Buff buff)
    {
        return ActiveBuffs.Where(b => b.BuffType == buff.BuffType).Any();
    }

    public bool HasBuff(BuffType buffType)
    {
        return ActiveBuffs.Where(b => b.BuffType == buffType).Any();
    }

    public Buff GetBuffByType(BuffType buffType)
    {
        return ActiveBuffs.Where(buff => buff.BuffType == buffType).FirstOrDefault();
    }

    public void DecreaseBuffs(List<BuffType> buffTypes, float val)
    {
        foreach (var item in buffTypes)
        {
            DecreaseBuff(item, val);
        }
    }

    public void DecreaseBuff(BuffType buffType, float val)
    {
        var buff = GetBuffByType(buffType);

        if(buff != null)
            buff.DecreaseTimer(val);
    }

    public void AddBuff(Buff buff)
    {
        if (HasBuff(buff))
        {
            var buffInList = GetBuffByType(buff.BuffType);

            // custom add if buff is ispiration
            if(buff.BuffType == BuffType.Inspiration)
            {
                buffInList.AddTimer(buff.RemainingTime);
                //Debug.Log("remaining: " + buffInList.RemainingTime);
            }
            else
            {
                buffInList.AddTimer(buff.StartDuration);
            }
        }
        else
        {
            // add expired handler
            buff.OnBuffExpired += HandlerExpiredBuff;

            // add to list
            ActiveBuffs.Add(buff);

            // invoke event add
            OnAddBuff?.Invoke(buff.BuffType);
        }
    }

    public void RemoveBuff(Buff buff)
    {
        // get index
        int index = ActiveBuffs.FindIndex(b => b.BuffType == buff.BuffType);
        if(index >= 0)
        {
            // remove expired handler
            ActiveBuffs[index].OnBuffExpired -= HandlerExpiredBuff;

            // remove from list
            ActiveBuffs.RemoveAt(index);

            // invoke event removed
            OnRemoveBuff?.Invoke(buff.BuffType);
        }
    }

    /// <summary>
    /// Handles expiration buff
    /// </summary>
    private void HandlerExpiredBuff(BuffType buffType)
    {
        // get buff from the list
        var buff = GetBuffByType(buffType);

        if(buffType != BuffType.Inspiration)
        {
            // remove from list
            RemoveBuff(buff);
        }
        else
        {
            // just don't remove it, let it idle if it's Inspiration buff
        }
    }

    public bool IsInspirationBuffActive()
    {
        if (HasBuff(BuffType.Inspiration))
        {
            var inspirationBuff = GetBuffByType(BuffType.Inspiration);
            return inspirationBuff.RemainingTime > 0 ? true : false;
        }
        return false;
    }
}
