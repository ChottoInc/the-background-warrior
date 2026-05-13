using UnityEngine;

public class UITabBlacksmithStatus : UITabPlayerStatus
{
    [Header("Player")]
    [SerializeField] PlayerBlacksmith player;

    private int distributedPointsOnCraftSpeed;
    private int distributedPointsOnEfficiency;
    private int distributedPointsOnLuck;
    private int distributedPointsOnMetallurgy;

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

        distributedPointsOnCraftSpeed = 0;
        distributedPointsOnEfficiency = 0;
        distributedPointsOnLuck = 0;
        distributedPointsOnMetallurgy = 0;
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

        if (distributedPointsOnCraftSpeed > 0)
        {
            player.PlayerData.IncreaseLevelStat(UtilsPlayer.ID_BLACKSMITH_CRAFTSPEED, distributedPointsOnCraftSpeed);
        }

        if (distributedPointsOnEfficiency > 0)
        {
            player.PlayerData.IncreaseLevelStat(UtilsPlayer.ID_BLACKSMITH_EFFICIENCY, distributedPointsOnEfficiency);
        }

        if (distributedPointsOnLuck > 0)
        {
            player.PlayerData.IncreaseLevelStat(UtilsPlayer.ID_BLACKSMITH_LUCK, distributedPointsOnLuck);
        }

        if (distributedPointsOnMetallurgy > 0)
        {
            player.PlayerData.IncreaseLevelStat(UtilsPlayer.ID_BLACKSMITH_METALLURGY, distributedPointsOnMetallurgy);
        }



        player.PlayerData.RemoveStatPoints(totalDistributedPoints);

        availablePoints -= totalDistributedPoints;

        Resets();

        // calls event in base class
        SaveChanges();


        player.SaveBlacksmithData();
    }

    protected override void HandleIncreaseJobStat(int id)
    {
        switch (id)
        {
            default: Debug.Log("Increased stat id not correct. " + id); break;
            case UtilsPlayer.ID_BLACKSMITH_CRAFTSPEED: distributedPointsOnCraftSpeed++; break;
            case UtilsPlayer.ID_BLACKSMITH_EFFICIENCY: distributedPointsOnEfficiency++; break;
            case UtilsPlayer.ID_BLACKSMITH_LUCK: distributedPointsOnLuck++; break;
            case UtilsPlayer.ID_BLACKSMITH_METALLURGY: distributedPointsOnMetallurgy++; break;
        }
    }

    protected override void HandleDecreaseJobStat(int id)
    {
        switch (id)
        {
            default: Debug.Log("Increased stat id not correct. " + id); break;
            case UtilsPlayer.ID_BLACKSMITH_CRAFTSPEED: distributedPointsOnCraftSpeed--; break;
            case UtilsPlayer.ID_BLACKSMITH_EFFICIENCY: distributedPointsOnEfficiency--; break;
            case UtilsPlayer.ID_BLACKSMITH_LUCK: distributedPointsOnLuck--; break;
            case UtilsPlayer.ID_BLACKSMITH_METALLURGY: distributedPointsOnMetallurgy--; break;
        }
    }

    protected override int HandleGetJobStatLevel(int id)
    {
        switch (id)
        {
            default: Debug.Log("Increased stat id not correct. " + id); return -1;
            case UtilsPlayer.ID_BLACKSMITH_CRAFTSPEED: return player.PlayerData.LevelStatCraftSpeed;
            case UtilsPlayer.ID_BLACKSMITH_EFFICIENCY: return player.PlayerData.LevelEfficiency;
            case UtilsPlayer.ID_BLACKSMITH_LUCK: return player.PlayerData.LevelStatLuck;
            case UtilsPlayer.ID_BLACKSMITH_METALLURGY: return player.PlayerData.LevelStatMetallurgy;
        }
    }
}
