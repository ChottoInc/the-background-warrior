using UnityEngine;

public class UITabWarriorStatus : UITabPlayerStatus
{
    [Header("Player")]
    [SerializeField] PlayerFight player;

    private int distributedPointsOnMaxHp;
    private int distributedPointsOnAtk;
    private int distributedPointsOnDef;
    private int distributedPointsOnAtkSpd;
    private int distributedPointsOnCritRate;
    private int distributedPointsOnCritDmg;
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

        distributedPointsOnMaxHp = 0;
        distributedPointsOnAtk = 0;
        distributedPointsOnDef = 0;
        distributedPointsOnAtkSpd = 0;
        distributedPointsOnCritRate = 0;
        distributedPointsOnCritDmg = 0;
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

        if (distributedPointsOnMaxHp > 0)
        {
            player.PlayerData.IncreaseLevelStat(UtilsPlayer.ID_WARRIOR_MAXHP, distributedPointsOnMaxHp);
        }

        if (distributedPointsOnAtk > 0)
        {
            player.PlayerData.IncreaseLevelStat(UtilsPlayer.ID_WARRIOR_ATK, distributedPointsOnAtk);
        }

        if (distributedPointsOnDef > 0)
        {
            player.PlayerData.IncreaseLevelStat(UtilsPlayer.ID_WARRIOR_DEF, distributedPointsOnDef);
        }

        if (distributedPointsOnAtkSpd > 0)
        {
            player.PlayerData.IncreaseLevelStat(UtilsPlayer.ID_WARRIOR_ATKSPD, distributedPointsOnAtkSpd);
        }

        if (distributedPointsOnCritRate > 0)
        {
            player.PlayerData.IncreaseLevelStat(UtilsPlayer.ID_WARRIOR_CRITRATE, distributedPointsOnCritRate);
        }

        if (distributedPointsOnCritDmg > 0)
        {
            player.PlayerData.IncreaseLevelStat(UtilsPlayer.ID_WARRIOR_CRITDMG, distributedPointsOnCritDmg);
        }

        if (distributedPointsOnLuck > 0)
        {
            player.PlayerData.IncreaseLevelStat(UtilsPlayer.ID_WARRIOR_LUCK, distributedPointsOnLuck);
        }



        player.PlayerData.RemoveStatPoints(totalDistributedPoints);

        availablePoints -= totalDistributedPoints;

        Resets();

        // calls event in base class
        SaveChanges();


        player.SaveFightData();
    }

    protected override void HandleIncreaseJobStat(int id)
    {
        switch (id)
        {
            default: Debug.Log("Increased stat id not correct. " + id); break;
            case UtilsPlayer.ID_WARRIOR_MAXHP: distributedPointsOnMaxHp++; break;
            case UtilsPlayer.ID_WARRIOR_ATK: distributedPointsOnAtk++; break;
            case UtilsPlayer.ID_WARRIOR_DEF: distributedPointsOnDef++; break;
            case UtilsPlayer.ID_WARRIOR_ATKSPD: distributedPointsOnAtkSpd++; break;
            case UtilsPlayer.ID_WARRIOR_CRITRATE: distributedPointsOnCritRate++; break;
            case UtilsPlayer.ID_WARRIOR_CRITDMG: distributedPointsOnCritDmg++; break;
            case UtilsPlayer.ID_WARRIOR_LUCK: distributedPointsOnLuck++; break;
        }
    }

    protected override void HandleDecreaseJobStat(int id)
    {
        switch (id)
        {
            default: Debug.Log("Increased stat id not correct. " + id); break;
            case UtilsPlayer.ID_WARRIOR_MAXHP: distributedPointsOnMaxHp--; break;
            case UtilsPlayer.ID_WARRIOR_ATK: distributedPointsOnAtk--; break;
            case UtilsPlayer.ID_WARRIOR_DEF: distributedPointsOnDef--; break;
            case UtilsPlayer.ID_WARRIOR_ATKSPD: distributedPointsOnAtkSpd--; break;
            case UtilsPlayer.ID_WARRIOR_CRITRATE: distributedPointsOnCritRate--; break;
            case UtilsPlayer.ID_WARRIOR_CRITDMG: distributedPointsOnCritDmg--; break;
            case UtilsPlayer.ID_WARRIOR_LUCK: distributedPointsOnLuck--; break;
        }
    }

    protected override int HandleGetJobStatLevel(int id)
    {
        switch (id)
        {
            default: Debug.Log("Increased stat id not correct. " + id); return -1;
            case UtilsPlayer.ID_WARRIOR_MAXHP: return player.PlayerData.LevelStatMaxHp;
            case UtilsPlayer.ID_WARRIOR_ATK: return player.PlayerData.LevelStatAtk;
            case UtilsPlayer.ID_WARRIOR_DEF: return player.PlayerData.LevelStatDef;
            case UtilsPlayer.ID_WARRIOR_ATKSPD: return player.PlayerData.LevelStatAtkSpd;
            case UtilsPlayer.ID_WARRIOR_CRITRATE: return player.PlayerData.LevelStatCritRate;
            case UtilsPlayer.ID_WARRIOR_CRITDMG: return player.PlayerData.LevelStatCritDmg;
            case UtilsPlayer.ID_WARRIOR_LUCK: return player.PlayerData.LevelStatLuck;
        }
    }
}
