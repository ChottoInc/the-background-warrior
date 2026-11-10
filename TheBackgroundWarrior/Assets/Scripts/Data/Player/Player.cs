using System;
using UnityEngine;

public class Player : MonoBehaviour, IPlayerDataProvider
{
    protected float timer1Second = UtilsGeneral.TIMER_1SECONDS;


    public event Action<ItemSO> OnItemAdd;

    public event Action OnLevelUp;


    protected virtual void Awake()
    {
        
    }

    protected virtual void OnDestroy()
    {
        
    }


    protected virtual void Update()
    {
        CheckSpeedMult();
    }

    protected void CheckSpeedMult()
    {
        if (!SettingsManager.Instance.AreCheatsEnabled) return;

        if (Input.GetKeyDown(KeyCode.X))
        {
            if (Time.timeScale == 1f)
            {
                Time.timeScale = 2f;
            }
            else if (Time.timeScale == 2f)
            {
                Time.timeScale = 4f;
            }
            else if (Time.timeScale == 4f)
            {
                Time.timeScale = 1f;
            }
        }
    }

    
    public virtual void AddItem(int id, int quantity)
    {
        PlayerManager.Instance.Inventory.AddItem(id, quantity);
        PlayerManager.Instance.SaveInventoryData();

        ItemSO itemSO = UtilsItem.GetItemById(id);
        AddItemEvent(itemSO);
    }

    public virtual void AddItemEvent(ItemSO itemSO)
    {
        OnItemAdd?.Invoke(itemSO);
    }

    protected virtual void LevelUp()
    {
        OnLevelUp?.Invoke();
    }

    /// <summary>
    /// Get the player data interface
    /// </summary>
    public virtual IBasePlayerData GetPlayerData()
    {
        throw new NotImplementedException("Class needs to override get player data function");
    }

    public virtual long GetCurrenExp()
    {
        throw new NotImplementedException("Class needs to override get current exp function");
    }

    public virtual long GetExpToNextLevel()
    {
        throw new NotImplementedException("Class needs to override get exp next level function");
    }
}
