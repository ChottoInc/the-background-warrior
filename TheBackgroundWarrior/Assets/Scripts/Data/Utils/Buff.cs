
using System;
using System.Linq;
using static UtilsBuffs;

[System.Serializable]
public class Buff
{

    public float StartDuration { get; private set; }


    public BuffType BuffType { get; private set; }
    public float RemainingTime { get; private set; }


    public bool IsExpired => RemainingTime <= 0;


    public event Action<BuffType> OnBuffExpired;

    public Buff(BuffType buffType, float remainingTime)
    {
        BuffType = buffType;
        RemainingTime = remainingTime;

        StartDuration = remainingTime;
    }

    public Buff(BuffSaveData saveData)
    {
        BuffType = (BuffType)saveData.buffType;
        RemainingTime = saveData.remainingTime;

        ConcoctionSO so = UtilsItem.GetAllTypeItem<ConcoctionSO>().Where(c => c.Buff == BuffType).FirstOrDefault();
        if(so != null)
        {
            StartDuration = so.Duration;
        }
    }

    public void AddTimer(float val)
    {
        RemainingTime += val;
    }

    public void DecreaseTimer(float val)
    {
        RemainingTime -= val;

        if (IsExpired)
        {
            RemainingTime = 0;
            OnBuffExpired?.Invoke(BuffType);
        }
        
    }
}
