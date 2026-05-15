using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITabFarmerStatus : UITabPlayerStatus
{
    [Header("Player")]
    [SerializeField] PlayerFarmer player;

    private int distributedPointsOnGreenthumb;
    private int distributedPointsOnAgronomy;
    private int distributedPointsOnKindness;
    private int distributedPointsOnLuck;

    private void OnDestroy()
    {
        player.PlayerData.OnLevelUp -= OnPlayerLevelUp;
    }

    private void Awake()
    {
        player.PlayerData.OnLevelUp += OnPlayerLevelUp;
    }

    protected override void Resets()
    {
        base.Resets();

        distributedPointsOnGreenthumb = 0;
        distributedPointsOnAgronomy = 0;
        distributedPointsOnKindness = 0;
        distributedPointsOnLuck = 0;
    }

    protected override void AssignAvailablePoints()
    {
        availablePoints = player.PlayerData.AvailableStatPoints;
    }

    protected override void UpdateCurrentLevelUI()
    {
        textCurrentLevel.text = $"Lv. : {player.PlayerData.CurrentLevel}";
    }

    public void OnButtonSaveChanges()
    {
        // set changes

        if (distributedPointsOnGreenthumb > 0)
        {
            player.PlayerData.IncreaseLevelStat(UtilsPlayer.ID_FARMER_GREENTHUMB, distributedPointsOnGreenthumb);
        }

        if (distributedPointsOnAgronomy > 0)
        {
            player.PlayerData.IncreaseLevelStat(UtilsPlayer.ID_FARMER_AGRONOMY, distributedPointsOnAgronomy);
        }

        if (distributedPointsOnKindness > 0)
        {
            player.PlayerData.IncreaseLevelStat(UtilsPlayer.ID_FARMER_KINDNESS, distributedPointsOnKindness);
        }

        if (distributedPointsOnLuck > 0)
        {
            player.PlayerData.IncreaseLevelStat(UtilsPlayer.ID_FARMER_LUCK, distributedPointsOnLuck);
        }



        player.PlayerData.RemoveStatPoints(totalDistributedPoints);

        availablePoints -= totalDistributedPoints;

        Resets();

        // calls event in base class
        SaveChanges();


        player.SaveFarmerData();
    }

    protected override void HandleIncreaseJobStat(int id)
    {
        switch (id)
        {
            default: Debug.Log("Increased stat id not correct. " + id); break;
            case UtilsPlayer.ID_FARMER_GREENTHUMB: distributedPointsOnGreenthumb++; break;
            case UtilsPlayer.ID_FARMER_AGRONOMY: distributedPointsOnAgronomy++; break;
            case UtilsPlayer.ID_FARMER_KINDNESS: distributedPointsOnKindness++; break;
            case UtilsPlayer.ID_FARMER_LUCK: distributedPointsOnLuck++; break;
        }
    }

    protected override void HandleDecreaseJobStat(int id)
    {
        switch (id)
        {
            default: Debug.Log("Increased stat id not correct. " + id); break;
            case UtilsPlayer.ID_FARMER_GREENTHUMB: distributedPointsOnGreenthumb--; break;
            case UtilsPlayer.ID_FARMER_AGRONOMY: distributedPointsOnAgronomy--; break;
            case UtilsPlayer.ID_FARMER_KINDNESS: distributedPointsOnKindness--; break;
            case UtilsPlayer.ID_FARMER_LUCK: distributedPointsOnLuck--; break;
        }
    }

    protected override int HandleGetJobStatLevel(int id)
    {
        switch (id)
        {
            default: Debug.Log("Increased stat id not correct. " + id); return -1;
            case UtilsPlayer.ID_FARMER_GREENTHUMB: return player.PlayerData.LevelStatGreenthumb;
            case UtilsPlayer.ID_FARMER_AGRONOMY: return player.PlayerData.LevelStatAgronomy;
            case UtilsPlayer.ID_FARMER_KINDNESS: return player.PlayerData.LevelStatKindness;
            case UtilsPlayer.ID_FARMER_LUCK: return player.PlayerData.LevelStatLuck;
        }
    }
}
