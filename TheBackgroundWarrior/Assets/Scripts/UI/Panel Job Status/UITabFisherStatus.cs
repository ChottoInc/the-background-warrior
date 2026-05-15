using UnityEngine;

public class UITabFisherStatus : UITabPlayerStatus
{
    [Header("Player")]
    [SerializeField] PlayerFisher player;

    private int distributedPointsOnCalmness;
    private int distributedPointsOnReflex;
    private int distributedPointsOnKnowledge;
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

        distributedPointsOnCalmness = 0;
        distributedPointsOnReflex = 0;
        distributedPointsOnKnowledge = 0;
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

        if (distributedPointsOnCalmness > 0)
        {
            player.PlayerData.IncreaseLevelStat(UtilsPlayer.ID_FISHER_CALMNESS, distributedPointsOnCalmness);
        }

        if (distributedPointsOnReflex > 0)
        {
            player.PlayerData.IncreaseLevelStat(UtilsPlayer.ID_FISHER_REFLEX, distributedPointsOnReflex);
        }

        if (distributedPointsOnKnowledge > 0)
        {
            player.PlayerData.IncreaseLevelStat(UtilsPlayer.ID_FISHER_KNOWLEDGE, distributedPointsOnKnowledge);
        }

        if (distributedPointsOnLuck > 0)
        {
            player.PlayerData.IncreaseLevelStat(UtilsPlayer.ID_FISHER_LUCK, distributedPointsOnLuck);
        }



        player.PlayerData.RemoveStatPoints(totalDistributedPoints);

        availablePoints -= totalDistributedPoints;

        Resets();

        // calls event in base class
        SaveChanges();


        player.SaveFisherData();
    }

    protected override void HandleIncreaseJobStat(int id)
    {
        switch (id)
        {
            default: Debug.Log("Increased stat id not correct. " + id); break;
            case UtilsPlayer.ID_FISHER_CALMNESS: distributedPointsOnCalmness++; break;
            case UtilsPlayer.ID_FISHER_REFLEX: distributedPointsOnReflex++; break;
            case UtilsPlayer.ID_FISHER_KNOWLEDGE: distributedPointsOnKnowledge++; break;
            case UtilsPlayer.ID_FISHER_LUCK: distributedPointsOnLuck++; break;
        }
    }

    protected override void HandleDecreaseJobStat(int id)
    {
        switch (id)
        {
            default: Debug.Log("Increased stat id not correct. " + id); break;
            case UtilsPlayer.ID_FISHER_CALMNESS: distributedPointsOnCalmness--; break;
            case UtilsPlayer.ID_FISHER_REFLEX: distributedPointsOnReflex--; break;
            case UtilsPlayer.ID_FISHER_KNOWLEDGE: distributedPointsOnKnowledge--; break;
            case UtilsPlayer.ID_FISHER_LUCK: distributedPointsOnLuck--; break;
        }
    }

    protected override int HandleGetJobStatLevel(int id)
    {
        switch (id)
        {
            default: Debug.Log("Increased stat id not correct. " + id); return -1;
            case UtilsPlayer.ID_FISHER_CALMNESS: return player.PlayerData.LevelStatCalmness;
            case UtilsPlayer.ID_FISHER_REFLEX: return player.PlayerData.LevelStatReflex;
            case UtilsPlayer.ID_FISHER_KNOWLEDGE: return player.PlayerData.LevelStatKnowledge;
            case UtilsPlayer.ID_FISHER_LUCK: return player.PlayerData.LevelStatLuck;
        }
    }
}
