using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UtilsText
{
    public static Dictionary<string, string> AllTextDictionary;

    public const string text_tooltip_stat_warrior_maxhp = "text_tooltip_stat_warrior_maxhp";
    public const string text_tooltip_stat_warrior_atk = "text_tooltip_stat_warrior_atk";
    public const string text_tooltip_stat_warrior_def = "text_tooltip_stat_warrior_def";
    public const string text_tooltip_stat_warrior_atkspd = "text_tooltip_stat_warrior_atkspd";
    public const string text_tooltip_stat_warrior_critrate = "text_tooltip_stat_warrior_critrate";
    public const string text_tooltip_stat_warrior_critdmg = "text_tooltip_stat_warrior_critdmg";
    public const string text_tooltip_stat_warrior_luck = "text_tooltip_stat_warrior_luck";
           
    public const string text_tooltip_stat_miner_power = "text_tooltip_stat_miner_power";
    public const string text_tooltip_stat_miner_smashspeed = "text_tooltip_stat_miner_smashspeed";
    public const string text_tooltip_stat_miner_shockwave = "text_tooltip_stat_miner_shockwave";
    public const string text_tooltip_stat_miner_luck = "text_tooltip_stat_miner_luck";
           
    public const string text_tooltip_stat_blacksmith_craftspeed = "text_tooltip_stat_blacksmith_craftspeed";
    public const string text_tooltip_stat_blacksmith_efficiency = "text_tooltip_stat_blacksmith_efficiency";
    public const string text_tooltip_stat_blacksmith_luck = "text_tooltip_stat_blacksmith_luck";
    public const string text_tooltip_stat_blacksmith_metallurgy = "text_tooltip_stat_blacksmith_metallurgy";
           
    public const string text_tooltip_stat_fisher_calmness = "text_tooltip_stat_fisher_calmness";
    public const string text_tooltip_stat_fisher_reflex = "text_tooltip_stat_fisher_reflex";
    public const string text_tooltip_stat_fisher_knowledge = "text_tooltip_stat_fisher_knowledge";
    public const string text_tooltip_stat_fisher_luck = "text_tooltip_stat_fisher_luck";
           
    public const string text_tooltip_stat_farmer_greenthumb = "text_tooltip_stat_farmer_greenthumb";
    public const string text_tooltip_stat_farmer_agronomy = "text_tooltip_stat_farmer_agronomy";
    public const string text_tooltip_stat_farmer_kindness = "text_tooltip_stat_farmer_kindness";
    public const string text_tooltip_stat_farmer_luck = "text_tooltip_stat_farmer_luck";

    public static void Initialize()
    {
        PopulateDefaultValues();
    }

    private static void PopulateDefaultValues()
    {
        AllTextDictionary = new Dictionary<string, string>()
        {
           { text_tooltip_stat_warrior_maxhp, "Increase Warrior max health" },
           { text_tooltip_stat_warrior_atk, "Increase Warrior damage" },
           { text_tooltip_stat_warrior_def, "Increase Warrior defense" },
           { text_tooltip_stat_warrior_atkspd, "Increase Warrior attack speed" },
           { text_tooltip_stat_warrior_critrate, "Increase Warrior critical rate" },
           { text_tooltip_stat_warrior_critdmg, "Increase Warrior critical damage" },
           { text_tooltip_stat_warrior_luck, "Increase Warrior critial rate AND chance of dropping cards from monsters" },


           { text_tooltip_stat_miner_power, "Increase Miner damage to rocks" },
           { text_tooltip_stat_miner_smashspeed, "Increase Miner smash speed" },
           { text_tooltip_stat_miner_shockwave, "Increase Miner damage to nearby rocks" },
           { text_tooltip_stat_miner_luck, "Increase Miner chance to drop loot from rocks" },


           { text_tooltip_stat_blacksmith_craftspeed, "Increase Blacksmith crafting speed" },
           { text_tooltip_stat_blacksmith_efficiency, "Increase Blacksmith chance to not consume materials when crafting" },
           { text_tooltip_stat_blacksmith_luck, "Increase Blacksmith chance to craft extra materials" },
           { text_tooltip_stat_blacksmith_metallurgy, "Increase by 1 the multiplier of extra materials crafted by the Blacksmith every 10 levels" },


           { text_tooltip_stat_fisher_calmness, "Decrease Fisher waiting time for the next hook" },
           { text_tooltip_stat_fisher_reflex, "Increase Fisher chance to catch the hooked fish" },
           { text_tooltip_stat_fisher_knowledge, "Increase Fisher chance to hook a never caught fish" },
           { text_tooltip_stat_fisher_luck, "Increase Fisher chance a rare fish comes into the lake" },


           { text_tooltip_stat_farmer_greenthumb, "Increase Farmer crops growth" },
           { text_tooltip_stat_farmer_agronomy, "Unlock a new seed for the Farmer to plant every 5 levels" },
           { text_tooltip_stat_farmer_kindness, "Increase Farmer chance to encounter a companion" },
           { text_tooltip_stat_farmer_luck, "Increase Farmer chance to befriend a companion" }
        };
    }
}
