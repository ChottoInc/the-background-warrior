using System;
using TMPro;
using UnityEngine;

public class UITabPlayerStatus : UITabWindow
{
    [Header("Level")]
    [SerializeField] protected TMP_Text textCurrentLevel;

    [Header("Points")]
    [SerializeField] protected TMP_Text textAvailablePoints;

    protected int availablePoints;

    protected int tempAvailablePoints;
    protected int totalDistributedPoints;



    public event Action OnStatusSave;


    public override void Open()
    {
        base.Open();

        Setup();
    }

    public override void Close()
    {
        base.Close();

        AudioManager.Instance.PlayClickUI();

        Resets();
    }

    protected virtual void Setup()
    {
        AssignAvailablePoints();

        tempAvailablePoints = availablePoints;

        UpdateCurrentLevelUI();

        UpdateAvailablePointsUI();
    }

    protected virtual void AssignAvailablePoints()
    {
        availablePoints = 0;
    }

    protected void OnPlayerLevelUp()
    {
        if (!isOpen) return;

        availablePoints++;

        tempAvailablePoints++;

        UpdateCurrentLevelUI();

        UpdateAvailablePointsUI();
    }

    protected virtual void Resets()
    {
        totalDistributedPoints = 0;
    }


    protected virtual void UpdateCurrentLevelUI()
    {
        textCurrentLevel.text = $"Lv. : 0";
    }

    private void UpdateAvailablePointsUI()
    {
        textAvailablePoints.text = $"Available points: {tempAvailablePoints}";
    }


    #region STAT HANDLES



    public bool IncreaseStatLevel(int id)
    {
        if (tempAvailablePoints <= 0) return false;

        totalDistributedPoints++;
        tempAvailablePoints--;

        HandleIncreaseJobStat(id);

        UpdateAvailablePointsUI();

        return true;
    }

    protected virtual void HandleIncreaseJobStat(int id)
    {
        // handle every stat here
    }

    public bool DecreaseStatLevel(int id)
    {
        if (tempAvailablePoints >= availablePoints) return false;

        totalDistributedPoints--;
        tempAvailablePoints++;

        HandleDecreaseJobStat(id);

        UpdateAvailablePointsUI();

        return true;
    }

    protected virtual void HandleDecreaseJobStat(int id)
    {
        // handle every stat here
    }


    /// <summary>
    /// Used by single stat ui to know its level by id
    /// </summary>
    public int GetStatLevel(int id)
    {
        return HandleGetJobStatLevel(id);
    }

    protected virtual int HandleGetJobStatLevel(int id)
    {
        // handle every stat here
        return -1;
    }

    #endregion


    protected virtual void SaveChanges()
    {
        OnStatusSave?.Invoke();
    }



    public void OnButtonClose()
    {
        AudioManager.Instance.PlayClickUI();
        Close();
    }
}
