
using static UtilsQuest;
using static UtilsText;

public static class UtilsQuestDescriptions
{
    public static string GetQuestDescription(QuestData data)
    {
        switch (data.questObjectiveType)
        {
            default: return "";
            case QuestObjectiveType.Kill: return HandleKillDescription(data);
            case QuestObjectiveType.Obtain: return HandleObtainItemDescription(data);
            case QuestObjectiveType.Craft: return HandleCraftItemDescription(data);
            case QuestObjectiveType.LevelUp: return HandleLevelUpStatDescription(data);
            case QuestObjectiveType.UnlockMap: return HandleUnlockMapDescription(data);
            case QuestObjectiveType.Befriend: return HandleBefriendDescription(data);
            case QuestObjectiveType.SpellRank: return HandleSpellRankUpDescription(data);
            case QuestObjectiveType.Summon: return HandleSummonDescription(data);
        }
    }

    private static string HandleKillDescription(QuestData data)
    {
        bool plural = data.amountKill > 1;
        if (data.questKillSpecific)
        {
            EnemySO enemySO = UtilsEnemy.GetEnemyById(data.monsterId);
            string name = plural ? enemySO.EnemyNamePlural : enemySO.EnemyName;
            return string.Format(AllText[text_quest_desc_kill_specific], data.amountKill, name);
        }
        else
        {
            return plural 
                ? string.Format(AllText[text_quest_desc_kill_nonspecific_plural], data.amountKill)
                : string.Format(AllText[text_quest_desc_kill_nonspecific], data.amountKill);
        }
    }

    private static string HandleObtainItemDescription(QuestData data)
    {
        bool plural = data.amountObtain > 1;
        if (data.questObtainSpecific)
        {
            ItemSO itemSO = UtilsItem.GetItemById(data.itemId);
            string name = plural ? itemSO.ItemNamePlural : itemSO.ItemName;
            return string.Format(AllText[text_quest_desc_obtain_specific], data.amountObtain, name);
        }
        else
        {
            string itemType = string.Empty;

            switch (data.itemType)
            {
                case UtilsItem.ItemType.Ore: 
                    itemType = plural ? AllText[text_quest_desc_obtain_item_category_ores_plural] :
                        AllText[text_quest_desc_obtain_item_category_ores];
                    break;

                case UtilsItem.ItemType.Card:
                    itemType = plural ? AllText[text_quest_desc_obtain_item_category_cards_plural] :
                        AllText[text_quest_desc_obtain_item_category_cards];
                    break;

                case UtilsItem.ItemType.Metal:
                    itemType = plural ? AllText[text_quest_desc_obtain_item_category_metals_plural] :
                        AllText[text_quest_desc_obtain_item_category_metals];
                    break;

                case UtilsItem.ItemType.Fish:
                    itemType = plural ? AllText[text_quest_desc_obtain_item_category_fishes_plural] :
                        AllText[text_quest_desc_obtain_item_category_fishes];
                    break;
            }

            return string.Format(AllText[text_quest_desc_obtain_nonspecific], data.amountObtain, itemType);
        }
    }

    private static string HandleCraftItemDescription(QuestData data)
    {
        bool plural = data.amountObtain > 1;
        if (data.questObtainSpecific)
        {
            ItemSO itemSO = UtilsItem.GetItemById(data.itemId);
            string name = plural ? itemSO.ItemNamePlural : itemSO.ItemName;
            return string.Format(AllText[text_quest_desc_craft_specific], data.amountObtain, name);
        }
        else
        {
            return string.Format(AllText[text_quest_desc_craft_nonspecific], data.amountObtain);
        }
    }

    private static string HandleLevelUpStatDescription(QuestData data)
    {
        if (data.questLevelUpSpecific)
        {
            string statName = UtilsPlayer.GetQuestStatNameById(data.statId);

            return data.amountStat > 1
                ? string.Format(AllText[text_quest_desc_levelup_specific_multiple], statName, data.amountStat)
                : string.Format(AllText[text_quest_desc_levelup_specific_once], statName, data.amountStat);
        }
        else
        {
            return data.amountStat > 1
                ? string.Format(AllText[text_quest_desc_levelup_nonspecific_multiple], data.amountStat)
                : string.Format(AllText[text_quest_desc_levelup_nonspecific_once], data.amountStat);
        }
    }

    private static string HandleUnlockMapDescription(QuestData data)
    {
        CombatMapSO mapSO = UtilsCombatMap.GetMapById(data.mapId);
        string mapName = mapSO.MapName;

        return string.Format(AllText[text_quest_desc_unlockmap], mapName);
    }

    private static string HandleBefriendDescription(QuestData data)
    {
        if (data.questBefriendSpecific)
        {
            return string.Format(AllText[text_quest_desc_befriend_specific], data.amountBefriend, data.companionSO.CompanionName);
        }
        else
        {
            return string.Format(AllText[text_quest_desc_befriend_nonspecific], data.amountBefriend);
        }
    }

    private static string HandleSpellRankUpDescription(QuestData data)
    {
        if (data.questSpellSpecific)
        {
            return data.amountRank > 1
                ? string.Format(AllText[text_quest_desc_spellrankup_specific_multiple], data.spellSO.SpellName, data.amountRank)
                : string.Format(AllText[text_quest_desc_spellrankup_specific_once], data.spellSO.SpellName, data.amountRank);
        }
        else
        {
            return data.amountRank > 1
                ? string.Format(AllText[text_quest_desc_spellrankup_nonspecific_multiple], data.amountRank  )
                : string.Format(AllText[text_quest_desc_spellrankup_nonspecific_once], data.amountStat);
        }
    }

    private static string HandleSummonDescription(QuestData data)
    {
        return string.Format(AllText[text_quest_desc_summon_plural], data.amountSummon);
    }
}
