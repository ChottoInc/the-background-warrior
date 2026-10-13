using UnityEngine;

public static class UtilsAchievement
{
    public const string UNLOCK_BLACKSMITH_JOB = "UNLOCK_BLACKSMITH_JOB";
    public const string UNLOCK_FARMER_JOB = "UNLOCK_FARMER_JOB";
    public const string UNLOCK_MAGE_JOB = "UNLOCK_MAGE_JOB";
    public const string UNLOCK_ALCHEMIST_JOB = "UNLOCK_ALCHEMIST_JOB";
    public const string UNLOCK_NECROMANCER_JOB = "UNLOCK_NECROMANCER_JOB";
    public const string UNLOCK_BARD_JOB = "UNLOCK_BARD_JOB";
    public const string CLEAR_WOODS_MAP = "CLEAR_WOODS_MAP";
    public const string CLEAR_SWAMPS_MAP = "CLEAR_SWAMPS_MAP";
    public const string CLEAR_RUINEDCITY_MAP = "CLEAR_RUINEDCITY_MAP";
    public const string COLLECT_CARD_MONSTERS_GROUP = "COLLECT_CARD_MONSTERS_GROUP";
    public const string COLLECT_CARD_PEOPLE_GROUP = "COLLECT_CARD_PEOPLE_GROUP";
    public const string COLLECT_CARD_OBJECTS_GROUP = "COLLECT_CARD_OBJECTS_GROUP";
    public const string COLLECT_CARD_FIRST_RARE = "COLLECT_CARD_FIRST_RARE";
    public const string COLLECT_CARD_ALL = "COLLECT_CARD_ALL";
    public const string OBTAIN_FISH_GOLDEN_GROUP = "OBTAIN_FISH_GOLDEN_GROUP";
    public const string OBTAIN_FISH_ALL_GROUP = "OBTAIN_FISH_ALL_GROUP";
    public const string CONSUME_BAITS_5 = "CONSUME_BAITS_5";
    public const string OBTAIN_GOLD_ORE_100 = "OBTAIN_GOLD_ORE_100";
    public const string UPGRADE_SWORD_5 = "UPGRADE_SWORD_5";
    public const string UPGRADE_SWORD_10 = "UPGRADE_SWORD_10";
    public const string CRAFT_GOLD_100 = "CRAFT_GOLD_100";
    public const string UPGRADE_GEARS_5 = "UPGRADE_GEARS_5";
    public const string UPGRADE_GEARS_10 = "UPGRADE_GEARS_10";
    public const string BEFRIEND_COMPANIONS_FIRST = "BEFRIEND_COMPANIONS_FIRST";
    public const string BEFRIEND_COMPANIONS_50 = "BEFRIEND_COMPANIONS_50";
    public const string GROW_CROP_FIRST = "GROW_CROP_FIRST";
    public const string GROW_CROP_50 = "GROW_CROP_50";
    public const string MAX_UPGRADE_COMPANIONS_1 = "MAX_UPGRADE_COMPANIONS_1";
    public const string MAX_UPGRADE_COMPANIONS_5 = "MAX_UPGRADE_COMPANIONS_5";
    public const string MAX_UPGRADE_MAGE_SPELLS_1 = "MAX_UPGRADE_MAGE_SPELLS_1";
    public const string MAX_UPGRADE_MAGE_SPELLS_5 = "MAX_UPGRADE_MAGE_SPELLS_5";
    public const string UNLOCK_ALL_MAGE_SPELLS = "UNLOCK_ALL_MAGE_SPELLS";
    public const string UNLOCK_ALL_MAGE_SLOTS = "UNLOCK_ALL_MAGE_SLOTS";
    public const string KILL_MAGE_SPELL_50 = "KILL_MAGE_SPELL_50";
    public const string CRAFT_CONCOCTIONS_1 = "CRAFT_CONCOCTIONS_1";
    public const string CRAFT_CONCOCTIONS_50 = "CRAFT_CONCOCTIONS_50";
    public const string UNLOCK_ALL_CONCOCTIONS_RECIPES = "UNLOCK_ALL_CONCOCTIONS_RECIPES";
    public const string UNLOCK_NECROMANCER_ARISE_RITUAL = "UNLOCK_NECROMANCER_ARISE_RITUAL";
    public const string UNLOCK_NECROMANCER_AFTERLIFE_RITUAL = "UNLOCK_NECROMANCER_AFTERLIFE_RITUAL";
    public const string UNLOCK_NECROMANCER_ADE_RITUAL = "UNLOCK_NECROMANCER_ADE_RITUAL";
    public const string UNLOCK_NECROMANCER_ALL_COUPLES = "UNLOCK_NECROMANCER_ALL_COUPLES";
    public const string SUMMON_UNDEAD_100 = "SUMMON_UNDEAD_100";
    public const string SUMMON_UNDEAD_POWERFUL_FIRST = "SUMMON_UNDEAD_POWERFUL_FIRST";
    public const string LISTEN_BARD_5 = "LISTEN_BARD_5";



    public const string STAT_BAITS_CONSUMED = "STAT_BAITS_CONSUMED";
    public const int STAT_BAITS_CONSUMED_MAX = 5;

    public const string STAT_GOLD_OBTAINED = "STAT_GOLD_OBTAINED";
    public const int STAT_GOLD_OBTAINED_MAX = 100;

    public const string STAT_GOLD_FORGED = "STAT_GOLD_FORGED";
    public const int STAT_GOLD_FORGED_MAX = 100;

    public const string STAT_BEFRIEND_COMPANIONS_COUNT = "STAT_BEFRIEND_COMPANIONS_COUNT";
    public const int STAT_BEFRIEND_COMPANIONS_COUNT_MAX = 50;

    public const string STAT_GROW_CROP_COUNT = "STAT_GROW_CROP_COUNT";
    public const int STAT_GROW_CROP_COUNT_MAX = 50;

    public const string STAT_KILL_MAGE_COUNT = "STAT_KILL_MAGE_COUNT";
    public const int STAT_KILL_MAGE_COUNT_MAX = 50;

    public const string STAT_CRAFT_CONCOCTIONS_COUNT = "STAT_CRAFT_CONCOCTIONS_COUNT";
    public const int STAT_CRAFT_CONCOCTIONS_COUNT_MAX = 50;

    public const string STAT_SUMMON_UNDEADS_COUNT = "STAT_SUMMON_UNDEADS_COUNT";
    public const int STAT_SUMMON_UNDEADS_COUNT_MAX = 100;

    


    public static bool IsAchievementUnlocked(string achievementId)
    {
        if (SteamManager.Initialized)
        {
            Steamworks.SteamUserStats.GetAchievement(achievementId, out bool achieved);
            return achieved;
        }
        else
        {
            Debug.LogWarning("SteamManager is not initialized. Cannot check achievement status.");
            return false;
        }
    }

    public static void UnlockAchievement(string achievementId)
    {
        if (SteamManager.Initialized)
        {
            Steamworks.SteamUserStats.SetAchievement(achievementId);
            Steamworks.SteamUserStats.StoreStats();
        }
        else
        {
            Debug.LogWarning("SteamManager is not initialized. Cannot unlock achievement.");
        }
    }


    public static void AddStat(string statName, int increment, int clampMaxVal)
    {
        if (SteamManager.Initialized)
        {
            Steamworks.SteamUserStats.GetStat(statName, out int currentValue);

            currentValue += increment;
            if (currentValue > clampMaxVal)
            {
                currentValue = clampMaxVal;
            }

            Steamworks.SteamUserStats.SetStat(statName, currentValue);
            Steamworks.SteamUserStats.StoreStats();
        }
        else
        {
            Debug.LogWarning("SteamManager is not initialized. Cannot add to stat.");
        }
    }

    public static bool RemoveAchievement(string achievementId)
    {
        if (SteamManager.Initialized)
        {
            if (Steamworks.SteamUserStats.ClearAchievement(achievementId))
            {
                Steamworks.SteamUserStats.StoreStats();
                return true;
            }
            else
            {
                Debug.LogWarning("Achievement Id not found: " + achievementId);
                return false;
            }
        }
        else
        {
            Debug.LogWarning("SteamManager is not initialized. Cannot remove achievement.");
            return false;
        }
    }
}
