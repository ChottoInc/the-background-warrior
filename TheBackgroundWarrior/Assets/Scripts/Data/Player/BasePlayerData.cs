using System;

public class BasePlayerData : IBasePlayerData
{
    public int CurrentLevel { get; protected set; }
    public long CurrentExp { get; protected set; }


    public int AvailableStatPoints { get; protected set; }


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
