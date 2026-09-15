
using System;
using System.IO;
using UnityEngine;

public static class UtilsSave
{

    public const string ROOT_FOLDER = "Data";

    public const string SETTINGS_FOLDER = "Settings";
    public const string SETTINGS_FILE = "settings.json";

    public const string PLAYER_FOLDER = "Player";
    public const string PLAYER_INVENTORY_FILE = "player_inventory.json";

    public const string PLAYER_JOBS_FILE = "player_jobs.json";
    public const string PLAYER_BUFFS_FILE = "player_buffs.json";
    public const string PLAYER_FIGHT_FILE = "player_fight.json";
    public const string PLAYER_MINER_FILE = "player_miner.json";
    public const string PLAYER_BLACKSMITH_FILE = "player_blacksmith.json";
    public const string PLAYER_FISHER_FILE = "player_fisher.json";
    public const string PLAYER_FARMER_FILE = "player_farmer.json";
    public const string PLAYER_MAGE_FILE = "player_mage.json";
    public const string PLAYER_ALCHEMIST_FILE = "player_alchemist.json";
    public const string PLAYER_NECROMANCER_FILE = "player_necromancer.json";
    public const string PLAYER_BARD_FILE = "player_bard.json";

    public const string COMBATMAPS_FOLDER = "CombatMaps";
    public const string COMBATMAPS_EXT = ".json";

    public const string QUESTS_FOLDER = "Quests";
    public const string QUESTS_FILE = "quests.json";

    public const string SHOP_FOLDER = "Shop";
    public const string SHOP_FILE = "shop.json";

    public const string BACKUP_FOLDER = "Backups";
    public const string TEMP_FOLDER = "Temps";

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

    public static string GetPlayerBuffsFile()
    {
        return GetPlayerFolder() + "/" + PLAYER_BUFFS_FILE;
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

    public static string GetPlayerMageFile()
    {
        return GetPlayerFolder() + "/" + PLAYER_MAGE_FILE;
    }

    public static string GetPlayerAlchemistFile()
    {
        return GetPlayerFolder() + "/" + PLAYER_ALCHEMIST_FILE;
    }

    public static string GetPlayerNecromancerFile()
    {
        return GetPlayerFolder() + "/" + PLAYER_NECROMANCER_FILE;
    }

    public static string GetPlayerBardFile()
    {
        return GetPlayerFolder() + "/" + PLAYER_BARD_FILE;
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



    // ----- BACKUPS NA TEMPS
    public static string GetBackupFolder()
    {
        return ROOT_FOLDER + "/" + BACKUP_FOLDER;
    }

    public static string GetTempsFolder()
    {
        return ROOT_FOLDER + "/" + TEMP_FOLDER;
    }


    // ----- CREATE AND CHECK FILES

    public static void CreateAllFolders()
    {
        string persistent = Application.persistentDataPath + "/";

        Directory.CreateDirectory(persistent + ROOT_FOLDER);

        Directory.CreateDirectory(persistent + GetPlayerFolder());
        Directory.CreateDirectory(persistent + GetSettingsFolder());
        Directory.CreateDirectory(persistent + GetCombatMapsFolder());
        Directory.CreateDirectory(persistent + GetQuestsFolder());
        Directory.CreateDirectory(persistent + GetShopFolder());
        Directory.CreateDirectory(persistent + GetBackupFolder());
        Directory.CreateDirectory(persistent + GetTempsFolder());
    }

    public static bool CheckAllFolders()
    {
        string persistent = Application.persistentDataPath + "/";

        try
        {

            CheckAndCreateFolder(persistent + ROOT_FOLDER);

            CheckAndCreateFolder(persistent + GetPlayerFolder());
            CheckAndCreateFolder(persistent + GetSettingsFolder());
            CheckAndCreateFolder(persistent + GetCombatMapsFolder());
            CheckAndCreateFolder(persistent + GetQuestsFolder());
            CheckAndCreateFolder(persistent + GetShopFolder());
            CheckAndCreateFolder(persistent + GetBackupFolder());
            CheckAndCreateFolder(persistent + GetTempsFolder());
            return true;
        }
        catch (Exception e)
        {
            throw e;
        }
    }



    public static bool CheckAndCreateFolder(string path)
    {
        if(!Directory.Exists(path))
        {
            try
            {
                Directory.CreateDirectory(path);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"Unable to create folder due to: {e.Message} {e.StackTrace}");
                throw e;
            }
        }
        return true;
    } 
}
