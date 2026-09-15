using UnityEngine;

using static UtilsText;

public static class UtilsPlayer
{
    public enum AdvanceStatType { None, Flat, Multiplier }

    public enum PlayerJob { None, Warrior, Miner, Blacksmith, Fisher, Farmer, Mage, Alchemist, Necromancer, Bard }

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

    public const int ID_MAGE_INSIGHT = 60;              // reduce times a spell needs to be casted to increase the level
    public const int ID_MAGE_CASTSPEED = 61;            // increase cast speed
    public const int ID_MAGE_SCHOLAR = 62;              // every x levels unlock new spell
    public const int ID_MAGE_PROFICIENCY = 63;          // every x levels unlock spell slot

    public const int ID_ALCHEMIST_ROUTINE = 70;         // increase crafting speed
    public const int ID_ALCHEMIST_YIELD = 71;           // craft extra materials
    public const int ID_ALCHEMIST_RESEARCH = 72;        // every x levels unlock new recipe
    public const int ID_ALCHEMIST_STABILITY = 73;       // reduce failed crafts

    public const int ID_NECROMANCER_APTITUDE = 80;      // every x levels increase couple fighting necromancer
    public const int ID_NECROMANCER_SUMMON = 81;        // increase spawn speed
    public const int ID_NECROMANCER_MIGHT = 82;         // increase minions strength
    public const int ID_NECROMANCER_LIFESPAN = 83;      // increase life duration
    public const int ID_NECROMANCER_HORDE = 84;         // every x levels increase horde limit by 1
    public const int ID_NECROMANCER_LUCK = 85;          // increase spawn chance of big minion - increase experience gain necromancer


    public static void Initialize()
    {
        jobs = LoadJobs();

        // load database of jobs
        jobDatabaseSO = LoadJobDatabase();
        jobDatabaseSO.Initialize();


        // Call every static helper for jobs after the load of SOs

        UtilsBuffs.Initialize();

        UtilsWarrior.Initialize();
        UtilsMiner.Initialize();
        UtilsBlacksmith.Initialize();
        UtilsFisher.Initialize();
        UtilsFarmer.Initialize();
        UtilsMage.Initialize();
        UtilsAlchemist.Initialize();
        UtilsNecromancer.Initialize();

        
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

            // MAGE DATA
            case ID_MAGE_INSIGHT: return UtilsMage.PER_LEVEL_MAGE_MAX_INSIGHT;
            case ID_MAGE_CASTSPEED: return UtilsMage.PER_LEVEL_MAGE_MAX_CASTSPEED;
            case ID_MAGE_SCHOLAR: return UtilsMage.PER_LEVEL_MAGE_MAX_SCHOLAR;
            case ID_MAGE_PROFICIENCY: return UtilsMage.PER_LEVEL_MAGE_MAX_PROFICIENCY;

            // ALCHEMIST DATA
            case ID_ALCHEMIST_ROUTINE: return UtilsAlchemist.PER_LEVEL_ALCHEMIST_MAX_ROUTINE;
            case ID_ALCHEMIST_YIELD: return UtilsAlchemist.PER_LEVEL_ALCHEMIST_MAX_YIELD;
            case ID_ALCHEMIST_RESEARCH: return UtilsAlchemist.PER_LEVEL_ALCHEMIST_MAX_RESEARCH;
            case ID_ALCHEMIST_STABILITY: return UtilsAlchemist.PER_LEVEL_ALCHEMIST_MAX_STABILITY;

            // NECROMANCER DATA
            case ID_NECROMANCER_APTITUDE: return UtilsNecromancer.PER_LEVEL_NECROMANCER_MAX_APTITUDE;
            case ID_NECROMANCER_SUMMON: return UtilsNecromancer.PER_LEVEL_NECROMANCER_MAX_SUMMON;
            case ID_NECROMANCER_MIGHT: return UtilsNecromancer.PER_LEVEL_NECROMANCER_MAX_MIGHT;
            case ID_NECROMANCER_LIFESPAN: return UtilsNecromancer.PER_LEVEL_NECROMANCER_MAX_LIFESPAN;
            case ID_NECROMANCER_HORDE: return UtilsNecromancer.PER_LEVEL_NECROMANCER_MAX_HORDE;
            case ID_NECROMANCER_LUCK: return UtilsNecromancer.PER_LEVEL_NECROMANCER_MAX_LUCK;
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

            // MAGE DATA
            case ID_MAGE_INSIGHT: return PlayerManager.Instance.PlayerMageData.LevelStatInsight;
            case ID_MAGE_CASTSPEED: return PlayerManager.Instance.PlayerMageData.LevelStatCastSpeed;
            case ID_MAGE_SCHOLAR: return PlayerManager.Instance.PlayerMageData.LevelStatScholar;
            case ID_MAGE_PROFICIENCY: return PlayerManager.Instance.PlayerMageData.LevelStatProficiency;

            // ALCHEMIST DATA
            case ID_ALCHEMIST_ROUTINE: return PlayerManager.Instance.PlayerAlchemistData.LevelStatRoutine;
            case ID_ALCHEMIST_YIELD: return PlayerManager.Instance.PlayerAlchemistData.LevelStatYield;
            case ID_ALCHEMIST_RESEARCH: return PlayerManager.Instance.PlayerAlchemistData.LevelStatResearch;
            case ID_ALCHEMIST_STABILITY: return PlayerManager.Instance.PlayerAlchemistData.LevelStatStability;

            // NECROMANCER DATA
            case ID_NECROMANCER_APTITUDE: return PlayerManager.Instance.PlayerNecromancerData.LevelStatAptitude;
            case ID_NECROMANCER_SUMMON: return PlayerManager.Instance.PlayerNecromancerData.LevelStatSummon;
            case ID_NECROMANCER_MIGHT: return PlayerManager.Instance.PlayerNecromancerData.LevelStatMight;
            case ID_NECROMANCER_LIFESPAN: return PlayerManager.Instance.PlayerNecromancerData.LevelStatLifespan;
            case ID_NECROMANCER_HORDE: return PlayerManager.Instance.PlayerNecromancerData.LevelStatHorde;
            case ID_NECROMANCER_LUCK: return PlayerManager.Instance.PlayerNecromancerData.LevelStatLuck;
        }
    }

    public static float GetCurrentStatById(int id)
    {
        switch (id)
        {
            default: Debug.Log("stat id not correct. " + id); return -1;

            // FIGHT DATA
            case ID_WARRIOR_MAXHP: return PlayerManager.Instance.PlayerFightData.MaxHp;
            case ID_WARRIOR_ATK: return PlayerManager.Instance.PlayerFightData.CurrentAtk;
            case ID_WARRIOR_DEF: return PlayerManager.Instance.PlayerFightData.CurrentDef;
            case ID_WARRIOR_ATKSPD: return PlayerManager.Instance.PlayerFightData.CurrentAtkSpd;
            case ID_WARRIOR_CRITRATE: return PlayerManager.Instance.PlayerFightData.CurrentCritRate;
            case ID_WARRIOR_CRITDMG: return PlayerManager.Instance.PlayerFightData.CurrentCritDmg;
            case ID_WARRIOR_LUCK: return PlayerManager.Instance.PlayerFightData.CurrentLuck;

            // MINER DATA
            case ID_MINER_POWER: return PlayerManager.Instance.PlayerMinerData.CurrentPower;
            case ID_MINER_SMASHSPEED: return PlayerManager.Instance.PlayerMinerData.CurrentSmashSpeed;
            case ID_MINER_SHOCKWAVE: return PlayerManager.Instance.PlayerMinerData.CurrentShockwave;
            case ID_MINER_LUCK: return PlayerManager.Instance.PlayerMinerData.CurrentLuck;

            // BLACKSMITH DATA
            case ID_BLACKSMITH_CRAFTSPEED: return PlayerManager.Instance.PlayerBlacksmithData.CurrentCraftSpeed;
            case ID_BLACKSMITH_EFFICIENCY: return PlayerManager.Instance.PlayerBlacksmithData.CurrentEfficiency;
            case ID_BLACKSMITH_LUCK: return PlayerManager.Instance.PlayerBlacksmithData.CurrentLuck;
            case ID_BLACKSMITH_METALLURGY: return PlayerManager.Instance.PlayerBlacksmithData.CurrentMetallurgy;

            // FISHER DATA
            case ID_FISHER_CALMNESS: return PlayerManager.Instance.PlayerFisherData.CurrentCalmness;
            case ID_FISHER_REFLEX: return PlayerManager.Instance.PlayerFisherData.CurrentReflex;
            case ID_FISHER_KNOWLEDGE: return PlayerManager.Instance.PlayerFisherData.CurrentKnowledge;
            case ID_FISHER_LUCK: return PlayerManager.Instance.PlayerFisherData.CurrentLuck;

            // FARMER DATA
            case ID_FARMER_GREENTHUMB: return PlayerManager.Instance.PlayerFarmerData.CurrentGreenthumb;
            case ID_FARMER_AGRONOMY: return PlayerManager.Instance.PlayerFarmerData.CurrentAgronomy;
            case ID_FARMER_KINDNESS: return PlayerManager.Instance.PlayerFarmerData.CurrentKindness;
            case ID_FARMER_LUCK: return PlayerManager.Instance.PlayerFarmerData.CurrentLuck;

            // MAGE DATA
            case ID_MAGE_INSIGHT: return PlayerManager.Instance.PlayerMageData.CurrentInsight;
            case ID_MAGE_CASTSPEED: return PlayerManager.Instance.PlayerMageData.CurrentCastSpeed;
            case ID_MAGE_SCHOLAR: return PlayerManager.Instance.PlayerMageData.CurrentScholar;
            case ID_MAGE_PROFICIENCY: return PlayerManager.Instance.PlayerMageData.CurrentProficiency;

            // ALCHEMIST DATA
            case ID_ALCHEMIST_ROUTINE: return PlayerManager.Instance.PlayerAlchemistData.CurrentRoutine;
            case ID_ALCHEMIST_YIELD: return PlayerManager.Instance.PlayerAlchemistData.CurrentYield;
            case ID_ALCHEMIST_RESEARCH: return PlayerManager.Instance.PlayerAlchemistData.CurrentResearch;
            case ID_ALCHEMIST_STABILITY: return PlayerManager.Instance.PlayerAlchemistData.CurrentStability;

            // NECROMANCER DATA
            case ID_NECROMANCER_APTITUDE: return PlayerManager.Instance.PlayerNecromancerData.CurrentAptitude;
            case ID_NECROMANCER_SUMMON: return PlayerManager.Instance.PlayerNecromancerData.CurrentSummon;
            case ID_NECROMANCER_MIGHT: return PlayerManager.Instance.PlayerNecromancerData.CurrentMight;
            case ID_NECROMANCER_LIFESPAN: return PlayerManager.Instance.PlayerNecromancerData.CurrentLifespan;
            case ID_NECROMANCER_HORDE: return PlayerManager.Instance.PlayerNecromancerData.CurrentHorde;
            case ID_NECROMANCER_LUCK: return PlayerManager.Instance.PlayerNecromancerData.CurrentLuck;
        }
    }

    public static string GetStatTooltipById(int id)
    {
        switch (id)
        {
            default: return "N/A";

            // FIGHT DATA
            case ID_WARRIOR_MAXHP: return AllText[text_tooltip_stat_warrior_maxhp];
            case ID_WARRIOR_ATK: return AllText[text_tooltip_stat_warrior_atk];
            case ID_WARRIOR_DEF: return AllText[text_tooltip_stat_warrior_def];
            case ID_WARRIOR_ATKSPD: return AllText[text_tooltip_stat_warrior_atkspd];
            case ID_WARRIOR_CRITRATE: return AllText[text_tooltip_stat_warrior_critrate];
            case ID_WARRIOR_CRITDMG: return AllText[text_tooltip_stat_warrior_critdmg];
            case ID_WARRIOR_LUCK: return AllText[text_tooltip_stat_warrior_luck];

            // MINER DATA
            case ID_MINER_POWER: return AllText[text_tooltip_stat_miner_power];
            case ID_MINER_SMASHSPEED: return AllText[text_tooltip_stat_miner_smashspeed];
            case ID_MINER_SHOCKWAVE: return AllText[text_tooltip_stat_miner_shockwave];
            case ID_MINER_LUCK: return AllText[text_tooltip_stat_miner_luck];

            // BLACKSMITH DATA
            case ID_BLACKSMITH_CRAFTSPEED: return AllText[text_tooltip_stat_blacksmith_craftspeed];
            case ID_BLACKSMITH_EFFICIENCY: return AllText[text_tooltip_stat_blacksmith_efficiency];
            case ID_BLACKSMITH_LUCK: return AllText[text_tooltip_stat_blacksmith_luck];
            case ID_BLACKSMITH_METALLURGY: return AllText[text_tooltip_stat_blacksmith_metallurgy];

            // FISHER DATA
            case ID_FISHER_CALMNESS: return AllText[text_tooltip_stat_fisher_calmness];
            case ID_FISHER_REFLEX: return AllText[text_tooltip_stat_fisher_reflex];
            case ID_FISHER_KNOWLEDGE: return AllText[text_tooltip_stat_fisher_knowledge];
            case ID_FISHER_LUCK: return AllText[text_tooltip_stat_fisher_luck];

            // FARMER DATA
            case ID_FARMER_GREENTHUMB: return AllText[text_tooltip_stat_farmer_greenthumb];
            case ID_FARMER_AGRONOMY: return AllText[text_tooltip_stat_farmer_agronomy];
            case ID_FARMER_KINDNESS: return AllText[text_tooltip_stat_farmer_kindness];
            case ID_FARMER_LUCK: return AllText[text_tooltip_stat_farmer_luck];

            // MAGE DATA
            case ID_MAGE_INSIGHT: return AllText[text_tooltip_stat_mage_insight];
            case ID_MAGE_CASTSPEED: return AllText[text_tooltip_stat_mage_castspeed];
            case ID_MAGE_SCHOLAR: return AllText[text_tooltip_stat_mage_scholar];
            case ID_MAGE_PROFICIENCY: return AllText[text_tooltip_stat_mage_proficiency];

            // ALCHEMIST DATA
            case ID_ALCHEMIST_ROUTINE: return AllText[text_tooltip_stat_alchemist_routine];
            case ID_ALCHEMIST_YIELD: return AllText[text_tooltip_stat_alchemist_yield];
            case ID_ALCHEMIST_RESEARCH: return AllText[text_tooltip_stat_alchemist_research];
            case ID_ALCHEMIST_STABILITY: return AllText[text_tooltip_stat_alchemist_stability];

            // NECROMANCER DATA
            case ID_NECROMANCER_APTITUDE: return AllText[text_tooltip_stat_necromancer_aptitude];
            case ID_NECROMANCER_SUMMON: return AllText[text_tooltip_stat_necromancer_summon];
            case ID_NECROMANCER_MIGHT: return AllText[text_tooltip_stat_necromancer_might];
            case ID_NECROMANCER_LIFESPAN: return AllText[text_tooltip_stat_necromancer_lifespan];
            case ID_NECROMANCER_HORDE: return AllText[text_tooltip_stat_necromancer_horde];
            case ID_NECROMANCER_LUCK: return AllText[text_tooltip_stat_necromancer_luck];
        }
    }

    public static bool AreStatsMaxedOut()
    {
        if( PlayerManager.Instance.PlayerFightData.CurrentLevel > UtilsWarrior.MAX_LEVEL_WARRIOR &&
            PlayerManager.Instance.PlayerMinerData.CurrentLevel > UtilsMiner.MAX_LEVEL_MINER &&
            PlayerManager.Instance.PlayerBlacksmithData.CurrentLevel > UtilsBlacksmith.MAX_LEVEL_BLACKSMITH &&
            PlayerManager.Instance.PlayerFisherData.CurrentLevel > UtilsFisher.MAX_LEVEL_FISHER &&
            PlayerManager.Instance.PlayerFarmerData.CurrentLevel > UtilsFarmer.MAX_LEVEL_FARMER &&
            PlayerManager.Instance.PlayerMageData.CurrentLevel > UtilsMage.MAX_LEVEL_MAGE &&
            PlayerManager.Instance.PlayerAlchemistData.CurrentLevel > UtilsAlchemist.MAX_LEVEL_ALCHEMIST &&
            PlayerManager.Instance.PlayerNecromancerData.CurrentLevel > UtilsNecromancer.MAX_LEVEL_NECROMANCER)
        {
            return true;
        }

        return false;
    }

    public static string GetQuestStatNameById(int id)
    {
        switch (id)
        {
            default: return "Error";
            case ID_WARRIOR_MAXHP:
            case ID_WARRIOR_ATK:
            case ID_WARRIOR_DEF:
            case ID_WARRIOR_ATKSPD:
            case ID_WARRIOR_CRITRATE: 
            case ID_WARRIOR_CRITDMG:
            case ID_WARRIOR_LUCK: return string.Format("{0} ({1})", GetStatNameById(id), AllText[text_name_class_warrior]);

            case ID_MINER_POWER:
            case ID_MINER_SMASHSPEED:
            case ID_MINER_SHOCKWAVE: 
            case ID_MINER_LUCK: return string.Format("{0} ({1})", GetStatNameById(id), AllText[text_name_class_miner]);

            case ID_BLACKSMITH_CRAFTSPEED:
            case ID_BLACKSMITH_EFFICIENCY:
            case ID_BLACKSMITH_LUCK:
            case ID_BLACKSMITH_METALLURGY: return string.Format("{0} ({1})", GetStatNameById(id), AllText[text_name_class_blacksmith]);

            case ID_FISHER_CALMNESS:
            case ID_FISHER_REFLEX:
            case ID_FISHER_KNOWLEDGE:
            case ID_FISHER_LUCK: return string.Format("{0} ({1})", GetStatNameById(id), AllText[text_name_class_fisher]);

            case ID_FARMER_GREENTHUMB:
            case ID_FARMER_AGRONOMY:
            case ID_FARMER_KINDNESS:
            case ID_FARMER_LUCK: return string.Format("{0} ({1})", GetStatNameById(id), AllText[text_name_class_farmer]);

            case ID_MAGE_INSIGHT:
            case ID_MAGE_CASTSPEED:
            case ID_MAGE_SCHOLAR:
            case ID_MAGE_PROFICIENCY: return string.Format("{0} ({1})", GetStatNameById(id), AllText[text_name_class_mage]);

            case ID_ALCHEMIST_ROUTINE:
            case ID_ALCHEMIST_YIELD:
            case ID_ALCHEMIST_RESEARCH:
            case ID_ALCHEMIST_STABILITY: return string.Format("{0} ({1})", GetStatNameById(id), AllText[text_name_class_alchemist]);

            case ID_NECROMANCER_APTITUDE:
            case ID_NECROMANCER_SUMMON:
            case ID_NECROMANCER_MIGHT:
            case ID_NECROMANCER_LIFESPAN:
            case ID_NECROMANCER_HORDE:
            case ID_NECROMANCER_LUCK: return string.Format("{0} ({1})", GetStatNameById(id), AllText[text_name_class_necromancer]);
        }
    }
    
    public static string GetStatNameById(int id)
    {
        switch (id)
        {
            default: return "Error";
            case ID_WARRIOR_MAXHP: return AllText[text_name_warrior_stat_maxhp];
            case ID_WARRIOR_ATK: return AllText[text_name_warrior_stat_atk];
            case ID_WARRIOR_DEF: return AllText[text_name_warrior_stat_def];
            case ID_WARRIOR_ATKSPD: return AllText[text_name_warrior_stat_atkspd];
            case ID_WARRIOR_CRITRATE: return AllText[text_name_warrior_stat_critrate];
            case ID_WARRIOR_CRITDMG: return AllText[text_name_warrior_stat_critdmg];
            case ID_WARRIOR_LUCK: return AllText[text_name_warrior_stat_luck];

            case ID_MINER_POWER: return AllText[text_name_miner_stat_power];
            case ID_MINER_SMASHSPEED: return AllText[text_name_miner_stat_smashspeed];
            case ID_MINER_SHOCKWAVE: return AllText[text_name_miner_stat_shockwave];
            case ID_MINER_LUCK: return AllText[text_name_miner_stat_luck];

            case ID_BLACKSMITH_CRAFTSPEED: return AllText[text_name_blacksmith_stat_craftspeed];
            case ID_BLACKSMITH_EFFICIENCY: return AllText[text_name_blacksmith_stat_efficiency];
            case ID_BLACKSMITH_LUCK: return AllText[text_name_blacksmith_stat_luck];
            case ID_BLACKSMITH_METALLURGY: return AllText[text_name_blacksmith_stat_metallurgy];

            case ID_FISHER_CALMNESS: return AllText[text_name_fisher_stat_calmness];
            case ID_FISHER_REFLEX: return AllText[text_name_fisher_stat_reflex];
            case ID_FISHER_KNOWLEDGE: return AllText[text_name_fisher_stat_knowledge];
            case ID_FISHER_LUCK: return AllText[text_name_fisher_stat_luck];

            case ID_FARMER_GREENTHUMB: return AllText[text_name_farmer_stat_greenthumb];
            case ID_FARMER_AGRONOMY: return AllText[text_name_farmer_stat_agronomy];
            case ID_FARMER_KINDNESS: return AllText[text_name_farmer_stat_kindness];
            case ID_FARMER_LUCK: return AllText[text_name_farmer_stat_luck];

            case ID_MAGE_INSIGHT: return AllText[text_name_mage_stat_insight];
            case ID_MAGE_CASTSPEED: return AllText[text_name_mage_stat_castspeed];
            case ID_MAGE_SCHOLAR: return AllText[text_name_mage_stat_scholar];
            case ID_MAGE_PROFICIENCY: return AllText[text_name_mage_stat_proficiency];

            case ID_ALCHEMIST_ROUTINE: return AllText[text_name_alchemist_stat_routine];
            case ID_ALCHEMIST_YIELD: return AllText[text_name_alchemist_stat_yield];
            case ID_ALCHEMIST_RESEARCH: return AllText[text_name_alchemist_stat_research];
            case ID_ALCHEMIST_STABILITY: return AllText[text_name_alchemist_stat_stability];

            case ID_NECROMANCER_APTITUDE: return AllText[text_name_necromancer_stat_aptitude];
            case ID_NECROMANCER_SUMMON: return AllText[text_name_necromancer_stat_summon];
            case ID_NECROMANCER_MIGHT: return AllText[text_name_necromancer_stat_might];
            case ID_NECROMANCER_LIFESPAN: return AllText[text_name_necromancer_stat_lifespan];
            case ID_NECROMANCER_HORDE: return AllText[text_name_necromancer_stat_horde];
            case ID_NECROMANCER_LUCK: return AllText[text_name_necromancer_stat_luck];
        }
    }

    public static AdvanceStatType GetAdvanceStatType(int id)
    {
        switch (id)
        {
            default: Debug.Log("Set advance type for " + id); return AdvanceStatType.None;
            case ID_WARRIOR_MAXHP: return AdvanceStatType.Flat;
            case ID_WARRIOR_ATK: return AdvanceStatType.Flat;
            case ID_WARRIOR_DEF: return AdvanceStatType.Flat;
            case ID_WARRIOR_ATKSPD: return AdvanceStatType.Flat;
            case ID_WARRIOR_CRITRATE: return AdvanceStatType.Multiplier;
            case ID_WARRIOR_CRITDMG: return AdvanceStatType.Multiplier;
            case ID_WARRIOR_LUCK: return AdvanceStatType.Multiplier;

            case ID_MINER_POWER: return AdvanceStatType.Flat;
            case ID_MINER_SMASHSPEED: return AdvanceStatType.Flat;
            case ID_MINER_SHOCKWAVE: return AdvanceStatType.Multiplier;
            case ID_MINER_LUCK: return AdvanceStatType.Multiplier;

            case ID_BLACKSMITH_CRAFTSPEED: return AdvanceStatType.Flat;
            case ID_BLACKSMITH_EFFICIENCY: return AdvanceStatType.Multiplier;
            case ID_BLACKSMITH_LUCK: return AdvanceStatType.Multiplier;
            case ID_BLACKSMITH_METALLURGY: return AdvanceStatType.Multiplier;

            case ID_FISHER_CALMNESS: return AdvanceStatType.Multiplier;
            case ID_FISHER_REFLEX: return AdvanceStatType.Multiplier;
            case ID_FISHER_KNOWLEDGE: return AdvanceStatType.Multiplier;
            case ID_FISHER_LUCK: return AdvanceStatType.Multiplier;

            case ID_FARMER_GREENTHUMB: return AdvanceStatType.Multiplier;
            case ID_FARMER_AGRONOMY: return AdvanceStatType.Flat;
            case ID_FARMER_KINDNESS: return AdvanceStatType.Multiplier;
            case ID_FARMER_LUCK: return AdvanceStatType.Multiplier;

            case ID_MAGE_INSIGHT: return AdvanceStatType.Multiplier;
            case ID_MAGE_CASTSPEED: return AdvanceStatType.Flat;
            case ID_MAGE_SCHOLAR: return AdvanceStatType.Flat;
            case ID_MAGE_PROFICIENCY: return AdvanceStatType.Flat;

            case ID_ALCHEMIST_ROUTINE: return AdvanceStatType.Multiplier;
            case ID_ALCHEMIST_YIELD: return AdvanceStatType.Multiplier;
            case ID_ALCHEMIST_RESEARCH: return AdvanceStatType.Flat;
            case ID_ALCHEMIST_STABILITY: return AdvanceStatType.Multiplier;

            case ID_NECROMANCER_APTITUDE: return AdvanceStatType.Flat;
            case ID_NECROMANCER_SUMMON: return AdvanceStatType.Multiplier;
            case ID_NECROMANCER_MIGHT: return AdvanceStatType.Multiplier;
            case ID_NECROMANCER_LIFESPAN: return AdvanceStatType.Multiplier;
            case ID_NECROMANCER_HORDE: return AdvanceStatType.Flat;
            case ID_NECROMANCER_LUCK: return AdvanceStatType.Multiplier;
        }
    }

    public static string GetAdvancedStatText(float stat, int id)
    {
        string res = string.Empty;

        var type = GetAdvanceStatType(id);

        switch(type)
        {
            case AdvanceStatType.None: return res;
            case AdvanceStatType.Flat: res = stat.ToString(); break;
            case AdvanceStatType.Multiplier: res = (stat * 100f).ToString() + "%"; break;
        }

        return res;
    }
}
