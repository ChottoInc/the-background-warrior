using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UtilsSave
{

    public const string ROOT_FOLDER = "Data";

    public const string SETTINGS_FOLDER = "Settings";
    public const string SETTINGS_FILE = "settings.json";

    public const string PLAYER_FOLDER = "Player";
    public const string PLAYER_INVENTORY_FILE = "player_inventory.json";

    public const string PLAYER_JOBS_FILE = "player_jobs.json";
    public const string PLAYER_FIGHT_FILE = "player_fight.json";
    public const string PLAYER_MINER_FILE = "player_miner.json";
    public const string PLAYER_BLACKSMITH_FILE = "player_blacksmith.json";
    public const string PLAYER_FISHER_FILE = "player_fisher.json";
    public const string PLAYER_FARMER_FILE = "player_farmer.json";

    public const string COMBATMAPS_FOLDER = "CombatMaps";
    public const string COMBATMAPS_EXT = ".json";

    public const string QUESTS_FOLDER = "Quests";
    public const string QUESTS_FILE = "quests.json";

    public const string SHOP_FOLDER = "Shop";
    public const string SHOP_FILE = "shop.json";

    // ----- SETTINGS

    public static string GetSettingsFolder()
    {
        return ROOT_FOLDER + "/" + SETTINGS_FOLDER;
    }

    public static string GetSettingsFile()
    {
        return GetSettingsFolder() + "/" + SETTINGS_FILE;
    }


    // ----- PLAYER

    public static string GetPlayerFolder()
    {
        return ROOT_FOLDER + "/" + PLAYER_FOLDER;
    }

    public static string GetPlayerInventoryFile()
    {
        return GetPlayerFolder() + "/" + PLAYER_INVENTORY_FILE;
    }

    public static string GetPlayerJobsFile()
    {
        return GetPlayerFolder() + "/" + PLAYER_JOBS_FILE;
    }

    public static string GetPlayerFightFile()
    {
        return GetPlayerFolder() + "/" + PLAYER_FIGHT_FILE;
    }

    public static string GetPlayerMinerFile()
    {
        return GetPlayerFolder() + "/" + PLAYER_MINER_FILE;
    }

    public static string GetPlayerBlacksmithFile()
    {
        return GetPlayerFolder() + "/" + PLAYER_BLACKSMITH_FILE;
    }

    public static string GetPlayerFisherFile()
    {
        return GetPlayerFolder() + "/" + PLAYER_FISHER_FILE;
    }

    public static string GetPlayerFarmerFile()
    {
        return GetPlayerFolder() + "/" + PLAYER_FARMER_FILE;
    }

    // ----- MAPS

    public static string GetCombatMapsFolder()
    {
        return ROOT_FOLDER + "/" + COMBATMAPS_FOLDER;
    }

    public static string GetCombatMapFile(string firstPart)
    {
        return GetCombatMapsFolder() + "/" + firstPart + COMBATMAPS_EXT;
    }


    // ----- QUESTS

    public static string GetQuestsFolder()
    {
        return ROOT_FOLDER + "/" + QUESTS_FOLDER;
    }

    public static string GetQuestFile()
    {
        return GetQuestsFolder() + "/" + QUESTS_FILE;
    }

    // ----- SHOP

    public static string GetShopFolder()
    {
        return ROOT_FOLDER + "/" + SHOP_FOLDER;
    }

    public static string GetShopFile()
    {
        return GetShopFolder() + "/" + SHOP_FILE;
    }
}
