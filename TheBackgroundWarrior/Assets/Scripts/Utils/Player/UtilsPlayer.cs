using UnityEngine;

public static class UtilsPlayer
{
    public enum PlayerJob { None, Warrior, Miner, Blacksmith, Fisher, Farmer }

    private static PlayerJobSO[] jobs;

    private static JobDatabaseSO jobDatabaseSO;


    // ----- IDS -------

    public const int ID_WARRIOR_MAXHP = 0;
    public const int ID_WARRIOR_ATK = 1;
    public const int ID_WARRIOR_DEF = 2;
    public const int ID_WARRIOR_ATKSPD = 3;
    public const int ID_WARRIOR_CRITRATE = 4;
    public const int ID_WARRIOR_CRITDMG = 5;
    public const int ID_WARRIOR_LUCK = 6;

    public const int ID_MINER_POWER = 20;               // damage to rocks
    public const int ID_MINER_SMASHSPEED = 21;          // atk spd for rocks
    public const int ID_MINER_SHOCKWAVE = 22;           // increase damage on nearby rocks
    public const int ID_MINER_LUCK = 23;                // increase loot chance from rocks
        
    public const int ID_BLACKSMITH_CRAFTSPEED = 30;     // craft material spd
    public const int ID_BLACKSMITH_EFFICIENCY = 31;     // spare material
    public const int ID_BLACKSMITH_LUCK = 32;           // extra metals if procs
    public const int ID_BLACKSMITH_METALLURGY = 33;     // amount extra metals if procs

    public const int ID_FISHER_CALMNESS = 40;           // reduce max time to wait for hook
    public const int ID_FISHER_REFLEX = 41;             // increase success on catch
    public const int ID_FISHER_KNOWLEDGE = 42;          // increase chance fishes in the pool are different from the ones already caught
    public const int ID_FISHER_LUCK = 43;               // increase the fish rarity any time it procs

    public const int ID_FARMER_GREENTHUMB = 50;         // increase crop growth
    public const int ID_FARMER_AGRONOMY = 51;           // increase available seeds every N levels
    public const int ID_FARMER_KINDNESS = 52;           // increase probability to encounter a companion
    public const int ID_FARMER_LUCK = 53;               // increase the probability the companion is befriended


    public static void Initialize()
    {
        jobs = LoadJobs();

        // load database of jobs
        jobDatabaseSO = LoadJobDatabase();
        jobDatabaseSO.Initialize();


        // Call every static helper for jobs after the load of SOs
        UtilsWarrior.Initialize();
        UtilsMiner.Initialize();
        UtilsBlacksmith.Initialize();
        UtilsFisher.Initialize();
        UtilsFarmer.Initialize();
    }

    private static PlayerJobSO[] LoadJobs()
    {
        return Resources.LoadAll<PlayerJobSO>("Data/Player/Jobs");
    }


    public static PlayerJobSO[] GetAllJobs()
    {
        return jobs;
    }

    public static PlayerJobSO GetJobByType(PlayerJob job)
    {
        foreach (var playerJob in jobs)
        {
            if (playerJob.Job == job)
                return playerJob;
        }
        return null;
    }



    private static JobDatabaseSO LoadJobDatabase()
    {
        return Resources.Load<JobDatabaseSO>("Data/Player/JobDatabase/DatabaseJobData");
    }


    public static AbstractPlayerJobData GetJobFromDatabase(PlayerJob job)
    {
        return jobDatabaseSO.Get<AbstractPlayerJobData>((int)job);
    }


    public static int GetStatMaxLevelById(int id)
    {
        switch (id)
        {
            default: Debug.Log("stat id not correct. " + id); return -1;

            // FIGHT DATA
            case ID_WARRIOR_MAXHP: return UtilsWarrior.PER_LEVEL_WARRIOR_MAX_MAXHP;
            case ID_WARRIOR_ATK:  return UtilsWarrior.PER_LEVEL_WARRIOR_MAX_ATK;
            case ID_WARRIOR_DEF: return UtilsWarrior.PER_LEVEL_WARRIOR_MAX_DEF;
            case ID_WARRIOR_ATKSPD: return UtilsWarrior.PER_LEVEL_WARRIOR_MAX_ATK_SPEED;
            case ID_WARRIOR_CRITRATE: return UtilsWarrior.PER_LEVEL_WARRIOR_MAX_CRIT_RATE;
            case ID_WARRIOR_CRITDMG: return UtilsWarrior.PER_LEVEL_WARRIOR_MAX_CRIT_DMG;
            case ID_WARRIOR_LUCK: return UtilsWarrior.PER_LEVEL_WARRIOR_MAX_LUCK;

            // MINER DATA
            case ID_MINER_POWER: return UtilsMiner.PER_LEVEL_MINER_MAX_POWER;
            case ID_MINER_SMASHSPEED: return UtilsMiner.PER_LEVEL_MINER_MAX_SMASHSPEED;
            case ID_MINER_SHOCKWAVE: return UtilsMiner.PER_LEVEL_MINER_MAX_SHOCKWAVE;
            case ID_MINER_LUCK: return UtilsMiner.PER_LEVEL_MINER_MAX_LUCK;

            // BLACKSMITH DATA
            case ID_BLACKSMITH_CRAFTSPEED: return UtilsBlacksmith.PER_LEVEL_BLACKSMITH_MAX_CRAFTSPEED;
            case ID_BLACKSMITH_EFFICIENCY: return UtilsBlacksmith.PER_LEVEL_BLACKSMITH_MAX_EFFICIENCY;
            case ID_BLACKSMITH_LUCK: return UtilsBlacksmith.PER_LEVEL_BLACKSMITH_MAX_LUCK;
            case ID_BLACKSMITH_METALLURGY: return UtilsBlacksmith.PER_LEVEL_BLACKSMITH_MAX_METALLURGY;

            // FISHER DATA
            case ID_FISHER_CALMNESS: return UtilsFisher.PER_LEVEL_FISHER_MAX_CALMNESS;
            case ID_FISHER_REFLEX: return UtilsFisher.PER_LEVEL_FISHER_MAX_REFLEX;
            case ID_FISHER_KNOWLEDGE: return UtilsFisher.PER_LEVEL_FISHER_MAX_KNOWLEDGE;
            case ID_FISHER_LUCK: return UtilsFisher.PER_LEVEL_FISHER_MAX_LUCK;

            // FARMER DATA
            case ID_FARMER_GREENTHUMB: return UtilsFarmer.PER_LEVEL_FARMER_MAX_GREENTHUMB;
            case ID_FARMER_AGRONOMY: return UtilsFarmer.PER_LEVEL_FARMER_MAX_AGRONOMY;
            case ID_FARMER_KINDNESS: return UtilsFarmer.PER_LEVEL_FARMER_MAX_KINDNESS;
            case ID_FARMER_LUCK: return UtilsFarmer.PER_LEVEL_FARMER_MAX_LUCK;
        }
    }

    public static int GetStatCurrentLevelById(int id)
    {
        switch (id)
        {
            default: Debug.Log("stat id not correct. " + id); return -1;

            // FIGHT DATA
            case ID_WARRIOR_MAXHP: return PlayerManager.Instance.PlayerFightData.LevelStatMaxHp;
            case ID_WARRIOR_ATK: return PlayerManager.Instance.PlayerFightData.LevelStatAtk;
            case ID_WARRIOR_DEF: return PlayerManager.Instance.PlayerFightData.LevelStatDef;
            case ID_WARRIOR_ATKSPD: return PlayerManager.Instance.PlayerFightData.LevelStatAtkSpd;
            case ID_WARRIOR_CRITRATE: return PlayerManager.Instance.PlayerFightData.LevelStatCritRate;
            case ID_WARRIOR_CRITDMG: return PlayerManager.Instance.PlayerFightData.LevelStatCritDmg;
            case ID_WARRIOR_LUCK: return PlayerManager.Instance.PlayerFightData.LevelStatLuck;

            // MINER DATA
            case ID_MINER_POWER: return PlayerManager.Instance.PlayerMinerData.LevelStatPower;
            case ID_MINER_SMASHSPEED: return PlayerManager.Instance.PlayerMinerData.LevelStatSmashSpeed;
            case ID_MINER_SHOCKWAVE: return PlayerManager.Instance.PlayerMinerData.LevelStatShockwave;
            case ID_MINER_LUCK: return PlayerManager.Instance.PlayerMinerData.LevelStatLuck;

            // BLACKSMITH DATA
            case ID_BLACKSMITH_CRAFTSPEED: return PlayerManager.Instance.PlayerBlacksmithData.LevelStatCraftSpeed;
            case ID_BLACKSMITH_EFFICIENCY: return PlayerManager.Instance.PlayerBlacksmithData.LevelEfficiency;
            case ID_BLACKSMITH_LUCK: return PlayerManager.Instance.PlayerBlacksmithData.LevelStatLuck;
            case ID_BLACKSMITH_METALLURGY: return PlayerManager.Instance.PlayerBlacksmithData.LevelStatMetallurgy;

            // FISHER DATA
            case ID_FISHER_CALMNESS: return PlayerManager.Instance.PlayerFisherData.LevelStatCalmness;
            case ID_FISHER_REFLEX: return PlayerManager.Instance.PlayerFisherData.LevelStatReflex;
            case ID_FISHER_KNOWLEDGE: return PlayerManager.Instance.PlayerFisherData.LevelStatKnowledge;
            case ID_FISHER_LUCK: return PlayerManager.Instance.PlayerFisherData.LevelStatLuck;

            // FARMER DATA
            case ID_FARMER_GREENTHUMB: return PlayerManager.Instance.PlayerFarmerData.LevelStatGreenthumb;
            case ID_FARMER_AGRONOMY: return PlayerManager.Instance.PlayerFarmerData.LevelStatAgronomy;
            case ID_FARMER_KINDNESS: return PlayerManager.Instance.PlayerFarmerData.LevelStatKindness;
            case ID_FARMER_LUCK: return PlayerManager.Instance.PlayerFarmerData.LevelStatLuck;
        }
    }

    public static string GetStatTooltipById(int id)
    {
        switch (id)
        {
            default: return "N/A";

            // FIGHT DATA
            case ID_WARRIOR_MAXHP: return UtilsText.AllTextDictionary[UtilsText.text_tooltip_stat_warrior_maxhp];
            case ID_WARRIOR_ATK: return UtilsText.AllTextDictionary[UtilsText.text_tooltip_stat_warrior_atk];
            case ID_WARRIOR_DEF: return UtilsText.AllTextDictionary[UtilsText.text_tooltip_stat_warrior_def];
            case ID_WARRIOR_ATKSPD: return UtilsText.AllTextDictionary[UtilsText.text_tooltip_stat_warrior_atkspd];
            case ID_WARRIOR_CRITRATE: return UtilsText.AllTextDictionary[UtilsText.text_tooltip_stat_warrior_critrate];
            case ID_WARRIOR_CRITDMG: return UtilsText.AllTextDictionary[UtilsText.text_tooltip_stat_warrior_critdmg];
            case ID_WARRIOR_LUCK: return UtilsText.AllTextDictionary[UtilsText.text_tooltip_stat_warrior_luck];

            // MINER DATA
            case ID_MINER_POWER: return UtilsText.AllTextDictionary[UtilsText.text_tooltip_stat_miner_power];
            case ID_MINER_SMASHSPEED: return UtilsText.AllTextDictionary[UtilsText.text_tooltip_stat_miner_smashspeed];
            case ID_MINER_SHOCKWAVE: return UtilsText.AllTextDictionary[UtilsText.text_tooltip_stat_miner_shockwave];
            case ID_MINER_LUCK: return UtilsText.AllTextDictionary[UtilsText.text_tooltip_stat_miner_luck];

            // BLACKSMITH DATA
            case ID_BLACKSMITH_CRAFTSPEED: return UtilsText.AllTextDictionary[UtilsText.text_tooltip_stat_blacksmith_craftspeed];
            case ID_BLACKSMITH_EFFICIENCY: return UtilsText.AllTextDictionary[UtilsText.text_tooltip_stat_blacksmith_efficiency];
            case ID_BLACKSMITH_LUCK: return UtilsText.AllTextDictionary[UtilsText.text_tooltip_stat_blacksmith_luck];
            case ID_BLACKSMITH_METALLURGY: return UtilsText.AllTextDictionary[UtilsText.text_tooltip_stat_blacksmith_metallurgy];

            // FISHER DATA
            case ID_FISHER_CALMNESS: return UtilsText.AllTextDictionary[UtilsText.text_tooltip_stat_fisher_calmness];
            case ID_FISHER_REFLEX: return UtilsText.AllTextDictionary[UtilsText.text_tooltip_stat_fisher_reflex];
            case ID_FISHER_KNOWLEDGE: return UtilsText.AllTextDictionary[UtilsText.text_tooltip_stat_fisher_knowledge];
            case ID_FISHER_LUCK: return UtilsText.AllTextDictionary[UtilsText.text_tooltip_stat_fisher_luck];

            // FARMER DATA
            case ID_FARMER_GREENTHUMB: return UtilsText.AllTextDictionary[UtilsText.text_tooltip_stat_farmer_greenthumb];
            case ID_FARMER_AGRONOMY: return UtilsText.AllTextDictionary[UtilsText.text_tooltip_stat_farmer_agronomy];
            case ID_FARMER_KINDNESS: return UtilsText.AllTextDictionary[UtilsText.text_tooltip_stat_farmer_kindness];
            case ID_FARMER_LUCK: return UtilsText.AllTextDictionary[UtilsText.text_tooltip_stat_farmer_luck];
        }
    }

    public static bool AreStatsMaxedOut()
    {
        if( PlayerManager.Instance.PlayerFightData.CurrentLevel > UtilsWarrior.MAX_LEVEL_WARRIOR &&
            PlayerManager.Instance.PlayerMinerData.CurrentLevel > UtilsMiner.MAX_LEVEL_MINER &&
            PlayerManager.Instance.PlayerBlacksmithData.CurrentLevel > UtilsBlacksmith.MAX_LEVEL_BLACKSMITH &&
            PlayerManager.Instance.PlayerFisherData.CurrentLevel > UtilsFisher.MAX_LEVEL_FISHER &&
            PlayerManager.Instance.PlayerFarmerData.CurrentLevel > UtilsFarmer.MAX_LEVEL_FARMER)
        {
            return true;
        }

        return false;
    }

    public static string GetStatNameById(int id)
    {
        switch (id)
        {
            default: return "Error";
            case ID_WARRIOR_MAXHP: return "Max HP (Warrior)";
            case ID_WARRIOR_ATK: return "Atk (Warrior)";
            case ID_WARRIOR_DEF: return "Def (Warrior)";
            case ID_WARRIOR_ATKSPD: return "Atk Speed (Warrior)";
            case ID_WARRIOR_CRITRATE: return "Crit Rate (Warrior)";
            case ID_WARRIOR_CRITDMG: return "Crit Dmg (Warrior)";
            case ID_WARRIOR_LUCK: return "Luck (Warrior)";

            case ID_MINER_POWER: return "Power (Miner)";
            case ID_MINER_SMASHSPEED: return "Smash Spd (Miner)";
            case ID_MINER_SHOCKWAVE: return "Shockwave (Miner)";
            case ID_MINER_LUCK: return "Luck (Miner)";

            case ID_BLACKSMITH_CRAFTSPEED: return "Craft Speed (Blacksmith)";
            case ID_BLACKSMITH_EFFICIENCY: return "Efficiency (Blacksmith)";
            case ID_BLACKSMITH_LUCK: return "Luck (Blacksmith)";
            case ID_BLACKSMITH_METALLURGY: return "Metallurgy (Blacksmith)";

            case ID_FISHER_CALMNESS: return "Calmness (Fisher)";
            case ID_FISHER_REFLEX: return "Reflex (Fisher)";
            case ID_FISHER_KNOWLEDGE: return "Knowledge (Fisher)";
            case ID_FISHER_LUCK: return "Luck (Fisher)";

            case ID_FARMER_GREENTHUMB: return "Green Thumb (Farmer)";
            case ID_FARMER_AGRONOMY: return "Agronomy (Farmer)";
            case ID_FARMER_KINDNESS: return "Kindness (Farmer)";
            case ID_FARMER_LUCK: return "Luck (Farmer)";
        }
    }
}
