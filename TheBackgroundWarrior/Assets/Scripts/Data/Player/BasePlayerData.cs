using System;
using UnityEngine;

public class BasePlayerData : IBasePlayerData
{
    public int CurrentLevel { get; protected set; }
    public long CurrentExp { get; protected set; }


    public int AvailableStatPoints { get; protected set; }


    protected float _inspirationModifier
    {
        get
        {
            float inspirationModifier = 1f;
            if (PlayerManager.Instance.PlayerBuffsData.IsInspirationBuffActive())
                inspirationModifier = 1.1f;

            return inspirationModifier;
        }
    }


    public event Action OnAddedExp;
    public event Action OnLevelUp;
    public event Action<int, int> OnStatChange;

    public virtual void AddExp(long amount, Func<int, bool> isMaxLevel, Func<long> expToNextLevel)
    {
        // check max level
        if (isMaxLevel(CurrentLevel))
        {
            // set current exp to 0
            CurrentExp = 0;
            return;
        }

        // check for veteran buff and adds 20%
        if (PlayerManager.Instance.PlayerBuffsData.HasBuff(UtilsBuffs.BuffType.Veteran))
        {
            amount = Mathf.RoundToInt((float)amount * 1.2f);
        }

        CurrentExp += amount;

        // looping for every level gained
        while (CurrentExp >= expToNextLevel())
        {
            // recalculate current exp
            CurrentExp -= expToNextLevel();

            // give level and stat point
            CurrentLevel++;
            AddStatPoints(1);

            OnLevelUp?.Invoke();
        }

        OnAddedExp?.Invoke();
    }

    public void AddLevel(int amount, int maxLevel)
    {
        if (CurrentLevel + amount > maxLevel)
        {
            amount = maxLevel - CurrentLevel;
        }
        CurrentLevel += amount;
        AvailableStatPoints += amount;
    }

    public void AddStatPoints(int amount)
    {
        AvailableStatPoints += amount;
    }

    public void RemoveStatPoints(int amount)
    {
        AvailableStatPoints -= amount;
    }

    protected void InvokeStatChange(int id, int amount)
    {
        OnStatChange?.Invoke(id, amount);
    }
}
