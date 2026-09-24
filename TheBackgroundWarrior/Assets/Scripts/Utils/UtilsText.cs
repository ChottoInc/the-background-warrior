using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public static class UtilsText
{
    public static Dictionary<string, string> GeneralDictionary;

    public static Dictionary<string, string> ItemNamesTextDictionary;
    public static Dictionary<string, string> ItemDescsTextDictionary;

    public static Dictionary<string, string> CreditsTextDictionary;

    public static Dictionary<string, string> HelpTextDictionary;

    public static Dictionary<string, string> AllText;

    #region ALL

    // -------------------- TUTORIAL --------------------- //

    public const string text_tutorial_continue = "text_tutorial_continue";
    public const string text_tutorial_skip = "text_tutorial_skip";

    public const string text_tutorial_intro_1 = "text_tutorial_intro_1";
    public const string text_tutorial_intro_2 = "text_tutorial_intro_2";
    public const string text_tutorial_intro_3 = "text_tutorial_intro_3";
    public const string text_tutorial_intro_4 = "text_tutorial_intro_4";
    public const string text_tutorial_intro_5 = "text_tutorial_intro_5";
    public const string text_tutorial_intro_6 = "text_tutorial_intro_6";
    public const string text_tutorial_intro_7 = "text_tutorial_intro_7";
    public const string text_tutorial_intro_8 = "text_tutorial_intro_8";
    public const string text_tutorial_intro_9 = "text_tutorial_intro_9";
    public const string text_tutorial_intro_10 = "text_tutorial_intro_10";
    public const string text_tutorial_intro_11 = "text_tutorial_intro_11";

    // -------------------- NAMES --------------------- //

    public const string text_name_buff_greed = "text_name_buff_greed";
    public const string text_name_buff_veteran = "text_name_buff_veteran";
    public const string text_name_buff_storyteller = "text_name_buff_storyteller";
    public const string text_name_buff_stoned = "text_name_buff_stoned";
    public const string text_name_buff_sailor = "text_name_buff_sailor";
    public const string text_name_buff_dwarf = "text_name_buff_dwarf";
    public const string text_name_buff_tamer = "text_name_buff_tamer";
    public const string text_name_buff_arcanist = "text_name_buff_arcanist";
    public const string text_name_buff_morningangler = "text_name_buff_morningangler";
    public const string text_name_buff_afternoonangler = "text_name_buff_afternoonangler";
    public const string text_name_buff_nightangler = "text_name_buff_nightangler";
    public const string text_name_buff_ironskin = "text_name_buff_ironskin";
    public const string text_name_buff_inspiration = "text_name_buff_inspiration";

    public const string text_name_buff_greed_effect = "text_name_buff_greed_effect";
    public const string text_name_buff_veteran_effect = "text_name_buff_veteran_effect";
    public const string text_name_buff_storyteller_effect = "text_name_buff_storyteller_effect";
    public const string text_name_buff_stoned_effect = "text_name_buff_stoned_effect";
    public const string text_name_buff_sailor_effect = "text_name_buff_sailor_effect";
    public const string text_name_buff_dwarf_effect = "text_name_buff_dwarf_effect";
    public const string text_name_buff_tamer_effect = "text_name_buff_tamer_effect";
    public const string text_name_buff_arcanist_effect = "text_name_buff_arcanist_effect";
    public const string text_name_buff_morningangler_effect = "text_name_buff_morningangler_effect";
    public const string text_name_buff_afternoonangler_effect = "text_name_buff_afternoonangler_effect";
    public const string text_name_buff_nightangler_effect = "text_name_buff_nightangler_effect";
    public const string text_name_buff_ironskin_effect = "text_name_buff_ironskin_effect";
    public const string text_name_buff_inspiration_effect = "text_name_buff_inspiration_effect";

    public const string text_name_class_warrior = "text_name_class_warrior";
    public const string text_name_class_miner = "text_name_class_miner";
    public const string text_name_class_fisher = "text_name_class_fisher";
    public const string text_name_class_farmer = "text_name_class_farmer";
    public const string text_name_class_blacksmith = "text_name_class_blacksmith";
    public const string text_name_class_mage = "text_name_class_mage";
    public const string text_name_class_alchemist = "text_name_class_alchemist";
    public const string text_name_class_necromancer = "text_name_class_necromancer";

    public const string text_name_warrior_stat_maxhp = "text_name_warrior_stat_maxhp";
    public const string text_name_warrior_stat_atk = "text_name_warrior_stat_atk";
    public const string text_name_warrior_stat_def = "text_name_warrior_stat_def";
    public const string text_name_warrior_stat_atkspd = "text_name_warrior_stat_atkspd";
    public const string text_name_warrior_stat_critrate = "text_name_warrior_stat_critrate";
    public const string text_name_warrior_stat_critdmg = "text_name_warrior_stat_critdmg";
    public const string text_name_warrior_stat_luck = "text_name_warrior_stat_luck";

    public const string text_name_miner_stat_power = "text_name_miner_stat_power";
    public const string text_name_miner_stat_smashspeed = "text_name_miner_stat_smashspeed";
    public const string text_name_miner_stat_shockwave = "text_name_miner_stat_shockwave";
    public const string text_name_miner_stat_luck = "text_name_miner_stat_luck";

    public const string text_name_blacksmith_stat_craftspeed = "text_name_blacksmith_stat_craftspeed";
    public const string text_name_blacksmith_stat_efficiency = "text_name_blacksmith_stat_efficiency";
    public const string text_name_blacksmith_stat_luck = "text_name_blacksmith_stat_luck";
    public const string text_name_blacksmith_stat_metallurgy = "text_name_blacksmith_stat_metallurgy";

    public const string text_name_fisher_stat_calmness = "text_name_fisher_stat_calmness";
    public const string text_name_fisher_stat_reflex = "text_name_fisher_stat_reflex";
    public const string text_name_fisher_stat_knowledge = "text_name_fisher_stat_knowledge";
    public const string text_name_fisher_stat_luck = "text_name_fisher_stat_luck";

    public const string text_name_farmer_stat_greenthumb = "text_name_farmer_stat_greenthumb";
    public const string text_name_farmer_stat_agronomy = "text_name_farmer_stat_agronomy";
    public const string text_name_farmer_stat_kindness = "text_name_farmer_stat_kindness";
    public const string text_name_farmer_stat_luck = "text_name_farmer_stat_luck";

    public const string text_name_mage_stat_insight = "text_name_mage_stat_insight";
    public const string text_name_mage_stat_castspeed = "text_name_mage_stat_castspeed";
    public const string text_name_mage_stat_scholar = "text_name_mage_stat_scholar";
    public const string text_name_mage_stat_proficiency = "text_name_mage_stat_proficiency";

    public const string text_name_alchemist_stat_routine = "text_name_alchemist_stat_routine";
    public const string text_name_alchemist_stat_yield = "text_name_alchemist_stat_yield";
    public const string text_name_alchemist_stat_research = "text_name_alchemist_stat_research";
    public const string text_name_alchemist_stat_stability = "text_name_alchemist_stat_stability";

    public const string text_name_necromancer_stat_aptitude = "text_name_necromancer_stat_aptitude";
    public const string text_name_necromancer_stat_summon = "text_name_necromancer_stat_summon";
    public const string text_name_necromancer_stat_might = "text_name_necromancer_stat_might";
    public const string text_name_necromancer_stat_lifespan = "text_name_necromancer_stat_lifespan";
    public const string text_name_necromancer_stat_horde = "text_name_necromancer_stat_horde";
    public const string text_name_necromancer_stat_luck = "text_name_necromancer_stat_luck";


    public const string text_name_daymoment_morning = "text_name_daymoment_morning";
    public const string text_name_daymoment_afternoon = "text_name_daymoment_afternoon";
    public const string text_name_daymoment_evening = "text_name_daymoment_evening";
    public const string text_name_daymoment_night = "text_name_daymoment_night";

    public const string text_name_map0 = "text_name_map0";
    public const string text_name_map1 = "text_name_map1";
    public const string text_name_map2 = "text_name_map2";
    public const string text_name_map3 = "text_name_map3";
    public const string text_name_map4 = "text_name_map4";
    public const string text_name_map5 = "text_name_map5";
    public const string text_name_map6 = "text_name_map6";
    public const string text_name_map7 = "text_name_map7";
    public const string text_name_map8 = "text_name_map8";
    public const string text_name_map9 = "text_name_map9";

    public const string text_name_card_rarity_common = "text_name_card_rarity_common";
    public const string text_name_card_rarity_uncommon = "text_name_card_rarity_uncommon";
    public const string text_name_card_rarity_rare = "text_name_card_rarity_rare";

    public const string text_name_fish_rarity_riverfolk = "text_name_fish_rarity_riverfolk";
    public const string text_name_fish_rarity_deepwater = "text_name_fish_rarity_deepwater";
    public const string text_name_fish_rarity_tideborn = "text_name_fish_rarity_tideborn";
    public const string text_name_fish_rarity_ancient = "text_name_fish_rarity_ancient";
    public const string text_name_fish_rarity_mythic = "text_name_fish_rarity_mythic";

    // -------------------- TOOLTIPS --------------------- //

    public const string text_tooltip_panel_autobattle = "text_tooltip_panel_autobattle";

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

    public const string text_tooltip_stat_mage_insight = "text_tooltip_stat_mage_insight";
    public const string text_tooltip_stat_mage_castspeed = "text_tooltip_stat_mage_castspeed";
    public const string text_tooltip_stat_mage_scholar = "text_tooltip_stat_mage_scholar";
    public const string text_tooltip_stat_mage_proficiency = "text_tooltip_stat_mage_proficiency";

    public const string text_tooltip_stat_alchemist_routine = "text_tooltip_stat_alchemist_routine";
    public const string text_tooltip_stat_alchemist_yield = "text_tooltip_stat_alchemist_yield";
    public const string text_tooltip_stat_alchemist_research = "text_tooltip_stat_alchemist_research";
    public const string text_tooltip_stat_alchemist_stability = "text_tooltip_stat_alchemist_stability";

    public const string text_tooltip_stat_necromancer_aptitude = "text_tooltip_stat_necromancer_aptitude";
    public const string text_tooltip_stat_necromancer_summon = "text_tooltip_stat_necromancer_summon";
    public const string text_tooltip_stat_necromancer_might = "text_tooltip_stat_necromancer_might";
    public const string text_tooltip_stat_necromancer_lifespan = "text_tooltip_stat_necromancer_lifespan";
    public const string text_tooltip_stat_necromancer_horde = "text_tooltip_stat_necromancer_horde";
    public const string text_tooltip_stat_necromancer_luck = "text_tooltip_stat_necromancer_luck";

    public const string text_tooltip_focusfishing = "text_tooltip_focusfishing";
    public const string text_rules_focusfishing = "text_rules_focusfishing";

    // -------------------- TITLE --------------------- //

    public const string text_title_buffs = "text_title_buffs";
    public const string text_title_shop = "text_title_shop";
    public const string text_title_quests = "text_title_quests";
    public const string text_title_quests_bountieslist = "text_title_quests_bountieslist";
    public const string text_title_settings = "text_title_settings";
    public const string text_title_inventory = "text_title_inventory";
    public const string text_title_inventory_convertlist = "text_title_inventory_convertlist";
    public const string text_title_jobs = "text_title_jobs";
    public const string text_title_jobs_back = "text_title_jobs_back";
    public const string text_title_jobs_warrior_maps = "text_title_jobs_warrior_maps";

    // -------------------- SHOP --------------------- //

    public const string text_shop_item_purchased = "text_shop_item_purchased";
    public const string text_shop_insertredeeem = "text_shop_insertredeeem";
    public const string text_shop_insertdebug = "text_shop_insertdebug";

    // -------------------- QUESTS --------------------- //

    public const string text_quest_desc_kill_specific = "text_quest_desc_kill_specific";
    public const string text_quest_desc_kill_nonspecific = "text_quest_desc_kill_nonspecific";
    public const string text_quest_desc_kill_nonspecific_plural = "text_quest_desc_kill_nonspecific_plural";

    public const string text_quest_desc_obtain_item_category_ores = "text_quest_desc_obtain_item_category_ores";
    public const string text_quest_desc_obtain_item_category_ores_plural = "text_quest_desc_obtain_item_category_ores_plural";
    public const string text_quest_desc_obtain_item_category_cards = "text_quest_desc_obtain_item_category_cards";
    public const string text_quest_desc_obtain_item_category_cards_plural = "text_quest_desc_obtain_item_category_cards_plural";
    public const string text_quest_desc_obtain_item_category_metals = "text_quest_desc_obtain_item_category_metals";
    public const string text_quest_desc_obtain_item_category_metals_plural = "text_quest_desc_obtain_item_category_metals_plural";
    public const string text_quest_desc_obtain_item_category_fishes = "text_quest_desc_obtain_item_category_fishes";
    public const string text_quest_desc_obtain_item_category_fishes_plural = "text_quest_desc_obtain_item_category_fishes_plural";

    public const string text_quest_desc_obtain_specific = "text_quest_desc_obtain_specific";
    public const string text_quest_desc_obtain_nonspecific = "text_quest_desc_obtain_nonspecific";

    public const string text_quest_desc_craft_specific = "text_quest_desc_craft_specific";
    public const string text_quest_desc_craft_nonspecific = "text_quest_desc_craft_nonspecific";

    public const string text_quest_desc_levelup_specific_once = "text_quest_desc_levelup_specific_once";
    public const string text_quest_desc_levelup_specific_multiple = "text_quest_desc_levelup_specific_multiple";
    public const string text_quest_desc_levelup_nonspecific_once = "text_quest_desc_levelup_nonspecific_once";
    public const string text_quest_desc_levelup_nonspecific_multiple = "text_quest_desc_levelup_nonspecific_multiple";

    public const string text_quest_desc_unlockmap = "text_quest_desc_unlockmap";

    public const string text_quest_desc_befriend_specific = "text_quest_desc_befriend_specific";
    public const string text_quest_desc_befriend_nonspecific = "text_quest_desc_befriend_nonspecific";

    public const string text_quest_desc_spellrankup_specific_once = "text_quest_desc_spellrankup_specific_once";
    public const string text_quest_desc_spellrankup_specific_multiple = "text_quest_desc_spellrankup_specific_multiple";
    public const string text_quest_desc_spellrankup_nonspecific_once = "text_quest_desc_spellrankup_nonspecific_once";
    public const string text_quest_desc_spellrankup_nonspecific_multiple = "text_quest_desc_spellrankup_nonspecific_multiple";

    public const string text_quest_desc_summon_plural = "text_quest_desc_summon_plural";

    public const string text_quest_reward_bounty = "text_quest_reward_bounty";

    // -------------------- SETTINGS --------------------- //

    public const string text_settings_general_titlevolume = "text_settings_general_titlevolume";
    public const string text_settings_general_titlelanguage = "text_settings_general_titlelanguage";
    public const string text_settings_general_titleexit = "text_settings_general_titleexit";
    public const string text_settings_general_button_titlescreen = "text_settings_general_button_titlescreen";
    public const string text_settings_general_button_quit = "text_settings_general_button_quit";

    public const string text_settings_general_lang_english = "text_settings_general_lang_english";
    public const string text_settings_general_lang_italian = "text_settings_general_lang_italian";

    public const string text_settings_gameplay_titlebattle = "text_settings_gameplay_titlebattle";
    public const string text_settings_gameplay_autobattle = "text_settings_gameplay_autobattle";
    public const string text_settings_gameplay_titlehud = "text_settings_gameplay_titlehud";
    public const string text_settings_gameplay_option_invertedhud = "text_settings_gameplay_option_invertedhud";
    public const string text_settings_gameplay_option_autoclosehud = "text_settings_gameplay_option_autoclosehud";
    public const string text_settings_gameplay_option_timerautoclose = "text_settings_gameplay_option_timerautoclose";
    public const string text_settings_gameplay_option_minutes = "text_settings_gameplay_option_minutes";
    public const string text_settings_gameplay_option_showadvancedstats = "text_settings_gameplay_option_showadvancedstats";
    public const string text_settings_gameplay_titlefloatinghud = "text_settings_gameplay_titlefloatinghud";
    public const string text_settings_gameplay_option_damage = "text_settings_gameplay_option_damage";
    public const string text_settings_gameplay_option_itemcollected = "text_settings_gameplay_option_itemcollected";
    public const string text_settings_gameplay_option_tooltips = "text_settings_gameplay_option_tooltips";
    public const string text_settings_gameplay_titleanimations = "text_settings_gameplay_titleanimations";
    public const string text_settings_gameplay_option_equipmentlevelup = "text_settings_gameplay_option_equipmentlevelup";
    public const string text_settings_gameplay_titlefisher = "text_settings_gameplay_titlefisher";
    public const string text_settings_gameplay_option_invertfishingspot = "text_settings_gameplay_option_invertfishingspot";
    public const string text_settings_gameplay_option_hidefishingbar = "text_settings_gameplay_option_hidefishingbar";

    public const string text_settings_video_option_alwaysontop = "text_settings_video_option_alwaysontop";
    public const string text_settings_video_option_clickthrough = "text_settings_video_option_clickthrough";
    public const string text_settings_video_titletargetfps = "text_settings_video_titletargetfps";
    public const string text_settings_video_option_toggle30 = "text_settings_video_option_toggle30";
    public const string text_settings_video_option_toggle60 = "text_settings_video_option_toggle60";
    public const string text_settings_video_button_changemonitor = "text_settings_video_button_changemonitor";

    // -------------------- JOBS --------------------- //

    public const string text_job_current_level = "text_job_current_level";
    public const string text_job_available_points = "text_job_available_points";
    public const string text_job_current_stat_level = "text_job_current_stat_level";
    public const string text_job_change_stat_level = "text_job_change_stat_level";

    public const string text_job_warrior_unlockconditions = "text_job_warrior_unlockconditions";
    public const string text_job_miner_unlockconditions = "text_job_miner_unlockconditions";
    public const string text_job_fisher_unlockconditions = "text_job_fisher_unlockconditions";
    public const string text_job_farmer_unlockconditions = "text_job_farmer_unlockconditions";
    public const string text_job_blacksmith_unlockconditions = "text_job_blacksmith_unlockconditions";
    public const string text_job_mage_unlockconditions = "text_job_mage_unlockconditions";
    public const string text_job_alchemist_unlockconditions = "text_job_alchemist_unlockconditions";
    public const string text_job_necromancer_unlockconditions = "text_job_necromancer_unlockconditions";
    public const string text_job_bard_unlockconditions = "text_job_bard_unlockconditions";

    public const string text_job_necromancer_arise_unlockconditions = "text_job_necromancer_arise_unlockconditions";
    public const string text_job_necromancer_afterlife_unlockconditions = "text_job_necromancer_afterlife_unlockconditions";
    public const string text_job_necromancer_ade_unlockconditions = "text_job_necromancer_ade_unlockconditions";

    public const string text_job_warrior_mapstage = "text_job_warrior_mapstage";
    public const string text_job_warrior_possiblemonsters = "text_job_warrior_possiblemonsters";
    public const string text_job_warrior_possiblecards = "text_job_warrior_possiblecards";


    public const string text_job_miner_weapon_currentlevel = "text_job_miner_weapon_currentlevel";
    public const string text_job_miner_weapon_nextlevel = "text_job_miner_weapon_nextlevel";
    public const string text_job_miner_weapon_currentstats = "text_job_miner_weapon_currentstats";
    public const string text_job_miner_requirements_levelup = "text_job_miner_requirements_levelup";
    public const string text_job_blacksmith_gear_currentstats = "text_job_blacksmith_gear_currentstats";
    public const string text_job_blacksmith_requirements_levelup = "text_job_blacksmith_requirements_levelup";

    public const string text_job_fisher_availablefishes = "text_job_fisher_availablefishes";
    public const string text_job_fisher_caughtsession = "text_job_fisher_caughtsession";
    public const string text_job_fisher_raritytooltip = "text_job_fisher_raritytooltip";
    public const string text_job_fisher_spawntime = "text_job_fisher_spawntime";
    public const string text_job_fisher_waittime = "text_job_fisher_waittime";


    public const string text_job_farmer_crop_basegrowthtime = "text_job_farmer_crop_basegrowthtime";
    public const string text_job_farmer_crop_attracts = "text_job_farmer_crop_attracts";
    public const string text_job_farmer_crop_companiondesc = "text_job_farmer_crop_companiondesc";
    public const string text_job_farmer_companion_cropdesc = "text_job_farmer_companion_cropdesc";
    public const string text_job_farmer_companion_equipped = "text_job_farmer_companion_equipped";


    public const string text_job_mage_spell_locked = "text_job_mage_spell_locked";
    public const string text_job_alchemist_recipe_locked = "text_job_alchemist_recipe_locked";
    public const string text_job_necromancer_ritual_locked = "text_job_necromancer_ritual_locked";


    public const string text_job_alchemist_ingredients = "text_job_alchemist_ingredients";


    // -------------------- BUTTONS --------------------- //

    public const string text_button_new = "text_button_new";
    public const string text_button_continue = "text_button_continue";
    public const string text_button_quit = "text_button_quit";

    public const string text_button_savechanges = "text_button_savechanges";

    public const string text_button_buy = "text_button_buy";
    public const string text_button_redeem = "text_button_redeem";
    public const string text_button_debug = "text_button_debug";

    public const string text_button_claim = "text_button_claim";
    public const string text_button_selectbounty = "text_button_selectbounty";
    public const string text_button_accept = "text_button_accept";

    public const string text_button_use = "text_button_use";
    public const string text_button_convert = "text_button_convert";
    public const string text_button_dismantle = "text_button_dismantle";
    public const string text_button_dismantle_cancel = "text_button_dismantle_cancel";
    public const string text_button_quickselect = "text_button_quickselect";

    public const string text_button_fight = "text_button_fight";

    public const string text_button_levelup = "text_button_levelup";
    public const string text_button_gather = "text_button_gather";
    public const string text_button_forge = "text_button_forge";
    public const string text_button_chant = "text_button_chant";
    public const string text_button_play = "text_button_play";

    public const string text_button_farm = "text_button_farm";
    public const string text_button_companions = "text_button_companions";
    public const string text_button_crops = "text_button_crops";
    public const string text_button_equip = "text_button_equip";
    public const string text_button_unequip = "text_button_unequip";

    public const string text_button_fish = "text_button_fish";

    public const string text_button_learn = "text_button_learn";

    public const string text_button_craft = "text_button_craft";

    public const string text_button_revealall = "text_button_revealall";

    public const string text_button_shop_filter_cardpacks = "text_button_shop_filter_cardpacks";
    public const string text_button_shop_filter_jobs = "text_button_shop_filter_jobs";
    public const string text_button_shop_filter_baits = "text_button_shop_filter_baits";
    public const string text_button_shop_filter_redeem = "text_button_shop_filter_redeem";
    public const string text_button_shop_filter_debug = "text_button_shop_filter_debug";

    public const string text_button_quests_filter_story = "text_button_quests_filter_story";
    public const string text_button_quests_filter_daily = "text_button_quests_filter_daily";
    public const string text_button_quests_filter_bounty = "text_button_quests_filter_bounty";

    public const string text_button_settings_filter_general = "text_button_settings_filter_general";
    public const string text_button_settings_filter_gameplay = "text_button_settings_filter_gameplay";
    public const string text_button_settings_filter_video = "text_button_settings_filter_video";
    public const string text_button_settings_filter_credits = "text_button_settings_filter_credits";
    public const string text_button_settings_filter_help = "text_button_settings_filter_help";

    public const string text_button_help_filter_warrior = "text_button_help_filter_warrior";
    public const string text_button_help_filter_miner = "text_button_help_filter_miner";
    public const string text_button_help_filter_fisher = "text_button_help_filter_fisher";
    public const string text_button_help_filter_blacksmith = "text_button_help_filter_blacksmith";
    public const string text_button_help_filter_farmer = "text_button_help_filter_farmer";
    public const string text_button_help_filter_mage = "text_button_help_filter_mage";
    public const string text_button_help_filter_alchemist = "text_button_help_filter_alchemist";
    public const string text_button_help_filter_necromancer = "text_button_help_filter_necromancer";
    public const string text_button_help_filter_bard = "text_button_help_filter_bard";

    public const string text_button_inventory_filter_all = "text_button_inventory_filter_all";
    public const string text_button_inventory_filter_ores = "text_button_inventory_filter_ores";
    public const string text_button_inventory_filter_metals = "text_button_inventory_filter_metals";
    public const string text_button_inventory_filter_fishes = "text_button_inventory_filter_fishes";
    public const string text_button_inventory_filter_crops = "text_button_inventory_filter_crops";
    public const string text_button_inventory_filter_baits = "text_button_inventory_filter_baits";
    public const string text_button_inventory_filter_concoctions = "text_button_inventory_filter_concoctions";
    public const string text_button_inventory_filter_cards = "text_button_inventory_filter_cards";


    // -------------------- YESNO --------------------- //

    public const string text_yesno_yes = "text_yesno_yes";
    public const string text_yesno_no = "text_yesno_no";
    public const string text_yesno_newgame = "text_yesno_newgame";
    public const string text_yesno_newgame_fatalerror = "text_yesno_newgame_fatalerror";

    public const string text_yesno_question_buy = "text_yesno_question_buy";

    public const string text_yesno_question_titlescreen = "text_yesno_question_titlescreen";
    public const string text_yesno_question_quitgame = "text_yesno_question_quitgame";

    #endregion

    #region ITEM NAMES

    public const string text_enemy_slime_name = "text_enemy_slime_name";
    public const string text_enemy_slime_name_plural = "text_enemy_slime_name_plural";
    public const string text_enemy_orc_name = "text_enemy_orc_name";
    public const string text_enemy_orc_name_plural = "text_enemy_orc_name_plural";
    public const string text_enemy_skeleton_name = "text_enemy_skeleton_name";
    public const string text_enemy_skeleton_name_plural = "text_enemy_skeleton_name_plural";
    public const string text_enemy_werewolf_name = "text_enemy_werewolf_name";
    public const string text_enemy_werewolf_name_plural = "text_enemy_werewolf_name_plural";
    public const string text_enemy_werebear_name = "text_enemy_werebear_name";
    public const string text_enemy_werebear_name_plural = "text_enemy_werebear_name_plural";
    public const string text_enemy_armoredskeleton_name = "text_enemy_armoredskeleton_name";
    public const string text_enemy_armoredskeleton_name_plural = "text_enemy_armoredskeleton_name_plural";
    public const string text_enemy_greatswordskeleton_name = "text_enemy_greatswordskeleton_name";
    public const string text_enemy_greatswordskeleton_name_plural = "text_enemy_greatswordskeleton_name_plural";
    public const string text_enemy_skeletonarcher_name = "text_enemy_skeletonarcher_name";
    public const string text_enemy_skeletonarcher_name_plural = "text_enemy_skeletonarcher_name_plural";
    public const string text_enemy_armoredorc_name = "text_enemy_armoredorc_name";
    public const string text_enemy_armoredorc_name_plural = "text_enemy_armoredorc_name_plural";
    public const string text_enemy_eliteorc_name = "text_enemy_eliteorc_name";
    public const string text_enemy_eliteorc_name_plural = "text_enemy_eliteorc_name_plural";
    public const string text_enemy_orcrider_name = "text_enemy_orcrider_name";
    public const string text_enemy_orcrider_name_plural = "text_enemy_orcrider_name_plural";

    public const string text_item_copperore_name = "text_item_copperore_name";
    public const string text_item_copperore_name_plural = "text_item_copperore_name_plural";
    public const string text_item_ironore_name = "text_item_ironore_name";
    public const string text_item_ironore_name_plural = "text_item_ironore_name_plural";
    public const string text_item_bronzeore_name = "text_item_bronzeore_name";
    public const string text_item_bronzeore_name_plural = "text_item_bronzeore_name_plural";
    public const string text_item_silverore_name = "text_item_silverore_name";
    public const string text_item_silverore_name_plural = "text_item_silverore_name_plural";
    public const string text_item_goldore_name = "text_item_goldore_name";
    public const string text_item_goldore_name_plural = "text_item_goldore_name_plural";

    public const string text_item_copper_name = "text_item_copper_name";
    //public const string text_item_copper_name_plural = "text_item_copper_name_plural";
    public const string text_item_iron_name = "text_item_iron_name";
    //public const string text_item_iron_name_plural = "text_item_iron_name_plural";
    public const string text_item_bronze_name = "text_item_bronze_name";
    //public const string text_item_bronze_name_plural = "text_item_bronze_name_plural";
    public const string text_item_silver_name = "text_item_silver_name";
    //public const string text_item_silver_name_plural = "text_item_silver_name_plural";
    public const string text_item_gold_name = "text_item_gold_name";
    //public const string text_item_gold_name_plural = "text_item_gold_name_plural";

    public const string text_item_card_01_name = "text_item_card_01_name";
    public const string text_item_card_02_name = "text_item_card_02_name";
    public const string text_item_card_03_name = "text_item_card_03_name";
    public const string text_item_card_04_name = "text_item_card_04_name";
    public const string text_item_card_05_name = "text_item_card_05_name";
    public const string text_item_card_06_name = "text_item_card_06_name";
    public const string text_item_card_07_name = "text_item_card_07_name";
    public const string text_item_card_08_name = "text_item_card_08_name";
    public const string text_item_card_09_name = "text_item_card_09_name";
    public const string text_item_card_10_name = "text_item_card_10_name";
    public const string text_item_card_11_name = "text_item_card_11_name";
    public const string text_item_card_12_name = "text_item_card_12_name";
    public const string text_item_card_13_name = "text_item_card_13_name";
    public const string text_item_card_14_name = "text_item_card_14_name";
    public const string text_item_card_15_name = "text_item_card_15_name";
    public const string text_item_card_16_name = "text_item_card_16_name";
    public const string text_item_card_17_name = "text_item_card_17_name";
    public const string text_item_card_18_name = "text_item_card_18_name";
    public const string text_item_card_19_name = "text_item_card_19_name";
    public const string text_item_card_20_name = "text_item_card_20_name";
    public const string text_item_card_21_name = "text_item_card_21_name";
    public const string text_item_card_22_name = "text_item_card_22_name";
    public const string text_item_card_23_name = "text_item_card_23_name";
    public const string text_item_card_24_name = "text_item_card_24_name";
    public const string text_item_card_25_name = "text_item_card_25_name";
    public const string text_item_card_26_name = "text_item_card_26_name";
    public const string text_item_card_27_name = "text_item_card_27_name";
    public const string text_item_card_28_name = "text_item_card_28_name";
    public const string text_item_card_29_name = "text_item_card_29_name";
    public const string text_item_card_30_name = "text_item_card_30_name";
    public const string text_item_card_31_name = "text_item_card_31_name";
    public const string text_item_card_32_name = "text_item_card_32_name";
    public const string text_item_card_33_name = "text_item_card_33_name";
    public const string text_item_card_34_name = "text_item_card_34_name";
    public const string text_item_card_35_name = "text_item_card_35_name";
    public const string text_item_card_36_name = "text_item_card_36_name";
    public const string text_item_card_37_name = "text_item_card_37_name";
    public const string text_item_card_38_name = "text_item_card_38_name";
    public const string text_item_card_39_name = "text_item_card_39_name";
    public const string text_item_card_40_name = "text_item_card_40_name";

    public const string text_item_fish_01_name = "text_item_fish_01_name";
    public const string text_item_fish_02_name = "text_item_fish_02_name";
    public const string text_item_fish_03_name = "text_item_fish_03_name";
    public const string text_item_fish_04_name = "text_item_fish_04_name";
    public const string text_item_fish_05_name = "text_item_fish_05_name";
    public const string text_item_fish_06_name = "text_item_fish_06_name";
    public const string text_item_fish_07_name = "text_item_fish_07_name";
    public const string text_item_fish_08_name = "text_item_fish_08_name";
    public const string text_item_fish_09_name = "text_item_fish_09_name";
    public const string text_item_fish_10_name = "text_item_fish_10_name";
    public const string text_item_fish_11_name = "text_item_fish_11_name";
    public const string text_item_fish_12_name = "text_item_fish_12_name";
    public const string text_item_fish_13_name = "text_item_fish_13_name";
    public const string text_item_fish_14_name = "text_item_fish_14_name";
    public const string text_item_fish_15_name = "text_item_fish_15_name";
    public const string text_item_fish_16_name = "text_item_fish_16_name";
    public const string text_item_fish_17_name = "text_item_fish_17_name";
    public const string text_item_fish_18_name = "text_item_fish_18_name";
    public const string text_item_fish_19_name = "text_item_fish_19_name";
    public const string text_item_fish_20_name = "text_item_fish_20_name";
    public const string text_item_fish_21_name = "text_item_fish_21_name";
    public const string text_item_fish_22_name = "text_item_fish_22_name";
    public const string text_item_fish_23_name = "text_item_fish_23_name";
    public const string text_item_fish_24_name = "text_item_fish_24_name";
    public const string text_item_fish_25_name = "text_item_fish_25_name";
    public const string text_item_fish_26_name = "text_item_fish_26_name";
    public const string text_item_fish_27_name = "text_item_fish_27_name";
    public const string text_item_fish_28_name = "text_item_fish_28_name";
    public const string text_item_fish_29_name = "text_item_fish_29_name";
    public const string text_item_fish_30_name = "text_item_fish_30_name";
    public const string text_item_fish_31_name = "text_item_fish_31_name";
    public const string text_item_fish_32_name = "text_item_fish_32_name";
    public const string text_item_fish_33_name = "text_item_fish_33_name";
    public const string text_item_fish_34_name = "text_item_fish_34_name";
    public const string text_item_fish_35_name = "text_item_fish_35_name";
    public const string text_item_fish_36_name = "text_item_fish_36_name";
    public const string text_item_fish_37_name = "text_item_fish_37_name";
    public const string text_item_fish_38_name = "text_item_fish_38_name";
    public const string text_item_fish_39_name = "text_item_fish_39_name";
    public const string text_item_fish_40_name = "text_item_fish_40_name";
    public const string text_item_fish_41_name = "text_item_fish_41_name";
    public const string text_item_fish_42_name = "text_item_fish_42_name";
    public const string text_item_fish_43_name = "text_item_fish_43_name";
    public const string text_item_fish_44_name = "text_item_fish_44_name";
    public const string text_item_fish_45_name = "text_item_fish_45_name";
    public const string text_item_fish_46_name = "text_item_fish_46_name";
    public const string text_item_fish_47_name = "text_item_fish_47_name";
    public const string text_item_fish_48_name = "text_item_fish_48_name";
    public const string text_item_fish_49_name = "text_item_fish_49_name";
    public const string text_item_fish_50_name = "text_item_fish_50_name";
    public const string text_item_fish_51_name = "text_item_fish_51_name";
    public const string text_item_fish_52_name = "text_item_fish_52_name";
    public const string text_item_fish_53_name = "text_item_fish_53_name";
    public const string text_item_fish_54_name = "text_item_fish_54_name";
    public const string text_item_fish_55_name = "text_item_fish_55_name";
    public const string text_item_fish_56_name = "text_item_fish_56_name";
    public const string text_item_fish_57_name = "text_item_fish_57_name";
    public const string text_item_fish_58_name = "text_item_fish_58_name";
    public const string text_item_fish_59_name = "text_item_fish_59_name";
    public const string text_item_fish_60_name = "text_item_fish_60_name";
    public const string text_item_fish_61_name = "text_item_fish_61_name";
    public const string text_item_fish_62_name = "text_item_fish_62_name";
    public const string text_item_fish_63_name = "text_item_fish_63_name";
    public const string text_item_fish_64_name = "text_item_fish_64_name";
    public const string text_item_fish_65_name = "text_item_fish_65_name";
    public const string text_item_fish_66_name = "text_item_fish_66_name";
    public const string text_item_fish_67_name = "text_item_fish_67_name";
    public const string text_item_fish_68_name = "text_item_fish_68_name";
    public const string text_item_fish_69_name = "text_item_fish_69_name";
    public const string text_item_fish_70_name = "text_item_fish_70_name";
    public const string text_item_fish_71_name = "text_item_fish_71_name";
    public const string text_item_fish_72_name = "text_item_fish_72_name";
    public const string text_item_fish_73_name = "text_item_fish_73_name";
    public const string text_item_fish_74_name = "text_item_fish_74_name";
    public const string text_item_fish_75_name = "text_item_fish_75_name";
    public const string text_item_fish_76_name = "text_item_fish_76_name";
    public const string text_item_fish_77_name = "text_item_fish_77_name";
    public const string text_item_fish_78_name = "text_item_fish_78_name";
    public const string text_item_fish_79_name = "text_item_fish_79_name";
    public const string text_item_fish_80_name = "text_item_fish_80_name";

    public const string text_item_crop_1_name = "text_item_crop_1_name";
    public const string text_item_crop_2_name = "text_item_crop_2_name";
    public const string text_item_crop_3_name = "text_item_crop_3_name";
    public const string text_item_crop_4_name = "text_item_crop_4_name";
    public const string text_item_crop_5_name = "text_item_crop_5_name";
    public const string text_item_crop_6_name = "text_item_crop_6_name";

    public const string text_item_bait_1_name = "text_item_bait_1_name";
    public const string text_item_bait_2_name = "text_item_bait_2_name";
    public const string text_item_bait_3_name = "text_item_bait_3_name";
    public const string text_item_bait_4_name = "text_item_bait_4_name";
    public const string text_item_bait_5_name = "text_item_bait_5_name";
    public const string text_item_bait_6_name = "text_item_bait_6_name";
    public const string text_item_bait_7_name = "text_item_bait_7_name";
    public const string text_item_bait_8_name = "text_item_bait_8_name";
    public const string text_item_bait_9_name = "text_item_bait_9_name";

    public const string text_item_concoction_1_name = "text_item_concoction_1_name";
    public const string text_item_concoction_2_name = "text_item_concoction_2_name";
    public const string text_item_concoction_3_name = "text_item_concoction_3_name";
    public const string text_item_concoction_4_name = "text_item_concoction_4_name";
    public const string text_item_concoction_5_name = "text_item_concoction_5_name";
    public const string text_item_concoction_6_name = "text_item_concoction_6_name";
    public const string text_item_concoction_7_name = "text_item_concoction_7_name";
    public const string text_item_concoction_8_name = "text_item_concoction_8_name";
    public const string text_item_concoction_9_name = "text_item_concoction_9_name";
    public const string text_item_concoction_10_name = "text_item_concoction_10_name";
    public const string text_item_concoction_11_name = "text_item_concoction_11_name";
    public const string text_item_concoction_12_name = "text_item_concoction_12_name";


    public const string text_item_fish_group_life_name = "text_item_fish_group_life_name";
    public const string text_item_fish_group_predator_name = "text_item_fish_group_predator_name";
    public const string text_item_fish_group_guardian_name = "text_item_fish_group_guardian_name";
    public const string text_item_fish_group_dart_name = "text_item_fish_group_dart_name";
    public const string text_item_fish_group_sharp_name = "text_item_fish_group_sharp_name";
    public const string text_item_fish_group_piercing_name = "text_item_fish_group_piercing_name";
    public const string text_item_fish_group_golden_name = "text_item_fish_group_golden_name";
    public const string text_item_fish_group_elder_name = "text_item_fish_group_elder_name";
    public const string text_item_fish_group_quick_name = "text_item_fish_group_quick_name";


    public const string text_mage_spell_fireball_name = "text_mage_spell_fireball_name";
    public const string text_mage_spell_explosion_name = "text_mage_spell_explosion_name";
    public const string text_mage_spell_chillwind_name = "text_mage_spell_chillwind_name";
    public const string text_mage_spell_poisongas_name = "text_mage_spell_poisongas_name";
    public const string text_mage_spell_zap_name = "text_mage_spell_zap_name";


    public const string text_necromancer_ritual_summon_name = "text_necromancer_ritual_summon_name";
    public const string text_necromancer_ritual_arise_name = "text_necromancer_ritual_arise_name";
    public const string text_necromancer_ritual_afterlife_name = "text_necromancer_ritual_afterlife_name";
    public const string text_necromancer_ritual_ade_name = "text_necromancer_ritual_ade_name";



    public const string text_shopitem_cardpack01_name = "text_shopitem_cardpack01_name";
    public const string text_shopitem_cardpack02_name = "text_shopitem_cardpack02_name";
    public const string text_shopitem_cardpack03_name = "text_shopitem_cardpack03_name";
    public const string text_shopitem_cardpack04_name = "text_shopitem_cardpack04_name";
    public const string text_shopitem_cardpack05_name = "text_shopitem_cardpack05_name";


    public const string text_shopitem_job_farmer_name = "text_shopitem_job_farmer_name";
    public const string text_shopitem_job_bard_name = "text_shopitem_job_bard_name";


    public const string text_shopitem_bait_normalmorning_name = "text_shopitem_bait_normalmorning_name";
    public const string text_shopitem_bait_greatmorning_name = "text_shopitem_bait_greatmorning_name";
    public const string text_shopitem_bait_maxmorning_name = "text_shopitem_bait_maxmorning_name";
    public const string text_shopitem_bait_normalafternoon_name = "text_shopitem_bait_normalafternoon_name";
    public const string text_shopitem_bait_greatafternoon_name = "text_shopitem_bait_greatafternoon_name";
    public const string text_shopitem_bait_maxafternoon_name = "text_shopitem_bait_maxafternoon_name";
    public const string text_shopitem_bait_normalnight_name = "text_shopitem_bait_normalnight_name";
    public const string text_shopitem_bait_greatnight_name = "text_shopitem_bait_greatnight_name";
    public const string text_shopitem_bait_maxnight_name = "text_shopitem_bait_maxnight_name";



    public const string text_companion_0_name = "text_companion_0_name";
    public const string text_companion_1_name = "text_companion_1_name";
    public const string text_companion_2_name = "text_companion_2_name";
    public const string text_companion_3_name = "text_companion_3_name";
    public const string text_companion_4_name = "text_companion_4_name";

    #endregion

    #region ITEM DESCS      

    public const string text_item_copperore_desc = "text_item_copperore_desc";
    public const string text_item_ironore_desc = "text_item_ironore_desc";
    public const string text_item_bronzeore_desc = "text_item_bronzeore_desc";
    public const string text_item_silverore_desc = "text_item_silverore_desc";
    public const string text_item_goldore_desc = "text_item_goldore_desc";

    public const string text_item_copper_desc = "text_item_copper_desc";
    public const string text_item_iron_desc = "text_item_iron_desc";
    public const string text_item_bronze_desc = "text_item_bronze_desc";
    public const string text_item_silver_desc = "text_item_silver_desc";
    public const string text_item_gold_desc = "text_item_gold_desc";

    public const string text_item_card_01_desc = "text_item_card_01_desc";
    public const string text_item_card_02_desc = "text_item_card_02_desc";
    public const string text_item_card_03_desc = "text_item_card_03_desc";
    public const string text_item_card_04_desc = "text_item_card_04_desc";
    public const string text_item_card_05_desc = "text_item_card_05_desc";
    public const string text_item_card_06_desc = "text_item_card_06_desc";
    public const string text_item_card_07_desc = "text_item_card_07_desc";
    public const string text_item_card_08_desc = "text_item_card_08_desc";
    public const string text_item_card_09_desc = "text_item_card_09_desc";
    public const string text_item_card_10_desc = "text_item_card_10_desc";
    public const string text_item_card_11_desc = "text_item_card_11_desc";
    public const string text_item_card_12_desc = "text_item_card_12_desc";
    public const string text_item_card_13_desc = "text_item_card_13_desc";
    public const string text_item_card_14_desc = "text_item_card_14_desc";
    public const string text_item_card_15_desc = "text_item_card_15_desc";
    public const string text_item_card_16_desc = "text_item_card_16_desc";
    public const string text_item_card_17_desc = "text_item_card_17_desc";
    public const string text_item_card_18_desc = "text_item_card_18_desc";
    public const string text_item_card_19_desc = "text_item_card_19_desc";
    public const string text_item_card_20_desc = "text_item_card_20_desc";
    public const string text_item_card_21_desc = "text_item_card_21_desc";
    public const string text_item_card_22_desc = "text_item_card_22_desc";
    public const string text_item_card_23_desc = "text_item_card_23_desc";
    public const string text_item_card_24_desc = "text_item_card_24_desc";
    public const string text_item_card_25_desc = "text_item_card_25_desc";
    public const string text_item_card_26_desc = "text_item_card_26_desc";
    public const string text_item_card_27_desc = "text_item_card_27_desc";
    public const string text_item_card_28_desc = "text_item_card_28_desc";
    public const string text_item_card_29_desc = "text_item_card_29_desc";
    public const string text_item_card_30_desc = "text_item_card_30_desc";
    public const string text_item_card_31_desc = "text_item_card_31_desc";
    public const string text_item_card_32_desc = "text_item_card_32_desc";
    public const string text_item_card_33_desc = "text_item_card_33_desc";
    public const string text_item_card_34_desc = "text_item_card_34_desc";
    public const string text_item_card_35_desc = "text_item_card_35_desc";
    public const string text_item_card_36_desc = "text_item_card_36_desc";
    public const string text_item_card_37_desc = "text_item_card_37_desc";
    public const string text_item_card_38_desc = "text_item_card_38_desc";
    public const string text_item_card_39_desc = "text_item_card_39_desc";
    public const string text_item_card_40_desc = "text_item_card_40_desc";

    public const string text_item_fish_01_desc = "text_item_fish_01_desc";
    public const string text_item_fish_02_desc = "text_item_fish_02_desc";
    public const string text_item_fish_03_desc = "text_item_fish_03_desc";
    public const string text_item_fish_04_desc = "text_item_fish_04_desc";
    public const string text_item_fish_05_desc = "text_item_fish_05_desc";
    public const string text_item_fish_06_desc = "text_item_fish_06_desc";
    public const string text_item_fish_07_desc = "text_item_fish_07_desc";
    public const string text_item_fish_08_desc = "text_item_fish_08_desc";
    public const string text_item_fish_09_desc = "text_item_fish_09_desc";
    public const string text_item_fish_10_desc = "text_item_fish_10_desc";
    public const string text_item_fish_11_desc = "text_item_fish_11_desc";
    public const string text_item_fish_12_desc = "text_item_fish_12_desc";
    public const string text_item_fish_13_desc = "text_item_fish_13_desc";
    public const string text_item_fish_14_desc = "text_item_fish_14_desc";
    public const string text_item_fish_15_desc = "text_item_fish_15_desc";
    public const string text_item_fish_16_desc = "text_item_fish_16_desc";
    public const string text_item_fish_17_desc = "text_item_fish_17_desc";
    public const string text_item_fish_18_desc = "text_item_fish_18_desc";
    public const string text_item_fish_19_desc = "text_item_fish_19_desc";
    public const string text_item_fish_20_desc = "text_item_fish_20_desc";
    public const string text_item_fish_21_desc = "text_item_fish_21_desc";
    public const string text_item_fish_22_desc = "text_item_fish_22_desc";
    public const string text_item_fish_23_desc = "text_item_fish_23_desc";
    public const string text_item_fish_24_desc = "text_item_fish_24_desc";
    public const string text_item_fish_25_desc = "text_item_fish_25_desc";
    public const string text_item_fish_26_desc = "text_item_fish_26_desc";
    public const string text_item_fish_27_desc = "text_item_fish_27_desc";
    public const string text_item_fish_28_desc = "text_item_fish_28_desc";
    public const string text_item_fish_29_desc = "text_item_fish_29_desc";
    public const string text_item_fish_30_desc = "text_item_fish_30_desc";
    public const string text_item_fish_31_desc = "text_item_fish_31_desc";
    public const string text_item_fish_32_desc = "text_item_fish_32_desc";
    public const string text_item_fish_33_desc = "text_item_fish_33_desc";
    public const string text_item_fish_34_desc = "text_item_fish_34_desc";
    public const string text_item_fish_35_desc = "text_item_fish_35_desc";
    public const string text_item_fish_36_desc = "text_item_fish_36_desc";
    public const string text_item_fish_37_desc = "text_item_fish_37_desc";
    public const string text_item_fish_38_desc = "text_item_fish_38_desc";
    public const string text_item_fish_39_desc = "text_item_fish_39_desc";
    public const string text_item_fish_40_desc = "text_item_fish_40_desc";
    public const string text_item_fish_41_desc = "text_item_fish_41_desc";
    public const string text_item_fish_42_desc = "text_item_fish_42_desc";
    public const string text_item_fish_43_desc = "text_item_fish_43_desc";
    public const string text_item_fish_44_desc = "text_item_fish_44_desc";
    public const string text_item_fish_45_desc = "text_item_fish_45_desc";
    public const string text_item_fish_46_desc = "text_item_fish_46_desc";
    public const string text_item_fish_47_desc = "text_item_fish_47_desc";
    public const string text_item_fish_48_desc = "text_item_fish_48_desc";
    public const string text_item_fish_49_desc = "text_item_fish_49_desc";
    public const string text_item_fish_50_desc = "text_item_fish_50_desc";
    public const string text_item_fish_51_desc = "text_item_fish_51_desc";
    public const string text_item_fish_52_desc = "text_item_fish_52_desc";
    public const string text_item_fish_53_desc = "text_item_fish_53_desc";
    public const string text_item_fish_54_desc = "text_item_fish_54_desc";
    public const string text_item_fish_55_desc = "text_item_fish_55_desc";
    public const string text_item_fish_56_desc = "text_item_fish_56_desc";
    public const string text_item_fish_57_desc = "text_item_fish_57_desc";
    public const string text_item_fish_58_desc = "text_item_fish_58_desc";
    public const string text_item_fish_59_desc = "text_item_fish_59_desc";
    public const string text_item_fish_60_desc = "text_item_fish_60_desc";
    public const string text_item_fish_61_desc = "text_item_fish_61_desc";
    public const string text_item_fish_62_desc = "text_item_fish_62_desc";
    public const string text_item_fish_63_desc = "text_item_fish_63_desc";
    public const string text_item_fish_64_desc = "text_item_fish_64_desc";
    public const string text_item_fish_65_desc = "text_item_fish_65_desc";
    public const string text_item_fish_66_desc = "text_item_fish_66_desc";
    public const string text_item_fish_67_desc = "text_item_fish_67_desc";
    public const string text_item_fish_68_desc = "text_item_fish_68_desc";
    public const string text_item_fish_69_desc = "text_item_fish_69_desc";
    public const string text_item_fish_70_desc = "text_item_fish_70_desc";
    public const string text_item_fish_71_desc = "text_item_fish_71_desc";
    public const string text_item_fish_72_desc = "text_item_fish_72_desc";
    public const string text_item_fish_73_desc = "text_item_fish_73_desc";
    public const string text_item_fish_74_desc = "text_item_fish_74_desc";
    public const string text_item_fish_75_desc = "text_item_fish_75_desc";
    public const string text_item_fish_76_desc = "text_item_fish_76_desc";
    public const string text_item_fish_77_desc = "text_item_fish_77_desc";
    public const string text_item_fish_78_desc = "text_item_fish_78_desc";
    public const string text_item_fish_79_desc = "text_item_fish_79_desc";
    public const string text_item_fish_80_desc = "text_item_fish_80_desc";

    public const string text_item_crop_1_desc = "text_item_crop_1_desc";
    public const string text_item_crop_2_desc = "text_item_crop_2_desc";
    public const string text_item_crop_3_desc = "text_item_crop_3_desc";
    public const string text_item_crop_4_desc = "text_item_crop_4_desc";
    public const string text_item_crop_5_desc = "text_item_crop_5_desc";
    public const string text_item_crop_6_desc = "text_item_crop_6_desc";

    public const string text_item_bait_1_desc = "text_item_bait_1_desc";
    public const string text_item_bait_2_desc = "text_item_bait_2_desc";
    public const string text_item_bait_3_desc = "text_item_bait_3_desc";
    public const string text_item_bait_4_desc = "text_item_bait_4_desc";
    public const string text_item_bait_5_desc = "text_item_bait_5_desc";
    public const string text_item_bait_6_desc = "text_item_bait_6_desc";
    public const string text_item_bait_7_desc = "text_item_bait_7_desc";
    public const string text_item_bait_8_desc = "text_item_bait_8_desc";
    public const string text_item_bait_9_desc = "text_item_bait_9_desc";

    public const string text_item_concoction_1_desc = "text_item_concoction_1_desc";
    public const string text_item_concoction_2_desc = "text_item_concoction_2_desc";
    public const string text_item_concoction_3_desc = "text_item_concoction_3_desc";
    public const string text_item_concoction_4_desc = "text_item_concoction_4_desc";
    public const string text_item_concoction_5_desc = "text_item_concoction_5_desc";
    public const string text_item_concoction_6_desc = "text_item_concoction_6_desc";
    public const string text_item_concoction_7_desc = "text_item_concoction_7_desc";
    public const string text_item_concoction_8_desc = "text_item_concoction_8_desc";
    public const string text_item_concoction_9_desc = "text_item_concoction_9_desc";
    public const string text_item_concoction_10_desc = "text_item_concoction_10_desc";
    public const string text_item_concoction_11_desc = "text_item_concoction_11_desc";
    public const string text_item_concoction_12_desc = "text_item_concoction_12_desc";

    public const string text_item_fish_group_life_desc = "text_item_fish_group_life_desc";
    public const string text_item_fish_group_predator_desc = "text_item_fish_group_predator_desc";
    public const string text_item_fish_group_guardian_desc = "text_item_fish_group_guardian_desc";
    public const string text_item_fish_group_dart_desc = "text_item_fish_group_dart_desc";
    public const string text_item_fish_group_sharp_desc = "text_item_fish_group_sharp_desc";
    public const string text_item_fish_group_piercing_desc = "text_item_fish_group_piercing_desc";
    public const string text_item_fish_group_golden_desc = "text_item_fish_group_golden_desc";
    public const string text_item_fish_group_elder_desc = "text_item_fish_group_elder_desc";
    public const string text_item_fish_group_quick_desc = "text_item_fish_group_quick_desc";

    public const string text_mage_spell_fireball_desc = "text_mage_spell_fireball_desc";
    public const string text_mage_spell_explosion_desc = "text_mage_spell_explosion_desc";
    public const string text_mage_spell_chillwind_desc = "text_mage_spell_chillwind_desc";
    public const string text_mage_spell_poisongas_desc = "text_mage_spell_poisongas_desc";
    public const string text_mage_spell_zap_desc = "text_mage_spell_zap_desc";

    public const string text_necromancer_ritual_summon_desc = "text_necromancer_ritual_summon_desc";
    public const string text_necromancer_ritual_arise_desc = "text_necromancer_ritual_arise_desc";
    public const string text_necromancer_ritual_afterlife_desc = "text_necromancer_ritual_afterlife_desc";
    public const string text_necromancer_ritual_ade_desc = "text_necromancer_ritual_ade_desc";



    public const string text_shopitem_cardpack01_desc = "text_shopitem_cardpack01_desc";
    public const string text_shopitem_cardpack02_desc = "text_shopitem_cardpack02_desc";
    public const string text_shopitem_cardpack03_desc = "text_shopitem_cardpack03_desc";
    public const string text_shopitem_cardpack04_desc = "text_shopitem_cardpack04_desc";
    public const string text_shopitem_cardpack05_desc = "text_shopitem_cardpack05_desc";


    public const string text_shopitem_job_farmer_desc = "text_shopitem_job_farmer_desc";
    public const string text_shopitem_job_bard_desc = "text_shopitem_job_bard_desc";


    public const string text_shopitem_bait_normalmorning_desc = "text_shopitem_bait_normalmorning_desc";
    public const string text_shopitem_bait_greatmorning_desc = "text_shopitem_bait_greatmorning_desc";
    public const string text_shopitem_bait_maxmorning_desc = "text_shopitem_bait_maxmorning_desc";
    public const string text_shopitem_bait_normalafternoon_desc = "text_shopitem_bait_normalafternoon_desc";
    public const string text_shopitem_bait_greatafternoon_desc = "text_shopitem_bait_greatafternoon_desc";
    public const string text_shopitem_bait_maxafternoon_desc = "text_shopitem_bait_maxafternoon_desc";
    public const string text_shopitem_bait_normalnight_desc = "text_shopitem_bait_normalnight_desc";
    public const string text_shopitem_bait_greatnight_desc = "text_shopitem_bait_greatnight_desc";
    public const string text_shopitem_bait_maxnight_desc = "text_shopitem_bait_maxnight_desc";


    public const string text_companion_0_desc = "text_companion_0_desc";
    public const string text_companion_1_desc = "text_companion_1_desc";
    public const string text_companion_2_desc = "text_companion_2_desc";
    public const string text_companion_3_desc = "text_companion_3_desc";
    public const string text_companion_4_desc = "text_companion_4_desc";

    #endregion

    #region CREDITS

    public const string text_credits_me = "text_credits_me";
    public const string text_credits_localization = "text_credits_localization";
    public const string text_credits_art = "text_credits_art";
    public const string text_credits_sound = "text_credits_sound";
    public const string text_credits_font = "text_credits_font";

    #endregion

    #region HELP

    public const string text_help_warrior = "text_help_warrior";
    public const string text_help_miner = "text_help_miner";
    public const string text_help_fisher = "text_help_fisher";
    public const string text_help_blacksmith = "text_help_blacksmith";
    public const string text_help_farmer = "text_help_farmer";
    public const string text_help_mage = "text_help_mage";
    public const string text_help_alchemist = "text_help_alchemist";
    public const string text_help_necromancer = "text_help_necromancer";
    public const string text_help_bard = "text_help_bard";

    public const string text_description_warrior = "text_description_warrior";
    public const string text_description_miner = "text_description_miner";
    public const string text_description_miner_weapon = "text_description_miner_weapon";
    public const string text_description_fisher = "text_description_fisher";
    public const string text_description_farmer_crops = "text_description_farmer_crops";
    public const string text_description_farmer_companions = "text_description_farmer_companions";
    public const string text_description_blacksmith = "text_description_blacksmith";
    public const string text_description_blacksmith_equipment = "text_description_blacksmith_equipment";
    public const string text_description_mage = "text_description_mage";
    public const string text_description_mage_slots = "text_description_mage_slots";
    public const string text_description_alchemist = "text_description_alchemist";
    public const string text_description_necromancer = "text_description_necromancer";
    public const string text_description_bard = "text_description_bard";

    #endregion

    public static void Initialize()
    {
        FillDefaultValues();

        MergeDictionaries();
    }

    private static void MergeDictionaries()
    {
        if(AllText == null)
        {
            AllText = new Dictionary<string, string>();
        }
        else 
        { 
            AllText.Clear(); 
        }

        AllText.AddRange(GeneralDictionary);
        AllText.AddRange(ItemNamesTextDictionary);
        AllText.AddRange(ItemDescsTextDictionary);
        AllText.AddRange(CreditsTextDictionary);
        AllText.AddRange(HelpTextDictionary);
    }

    public static void FillValuesWithLang(UtilsGeneral.Language lang)
    {
        string folderpath = string.Empty;
        string finalPart = string.Empty;
        switch (lang)
        {
            default:
            case UtilsGeneral.Language.Eng: folderpath = "files/localize/eng"; finalPart = "_eng"; break;
            case UtilsGeneral.Language.Ita: folderpath = "files/localize/ita"; finalPart = "_ita"; break;
        }

        FillDictionaries(folderpath, finalPart);

        MergeDictionaries();
    }

    private static void FillDictionaries(string folderpath, string finalPart)
    {
        GeneralDictionary = GetLocalizedDictionary(Path.Combine(folderpath, "General" + finalPart));
        ItemNamesTextDictionary = GetLocalizedDictionary(Path.Combine(folderpath, "ItemNames" + finalPart));
        ItemDescsTextDictionary = GetLocalizedDictionary(Path.Combine(folderpath, "ItemDescs" + finalPart));
        CreditsTextDictionary = GetLocalizedDictionary(Path.Combine(folderpath, "Credits" + finalPart));
        HelpTextDictionary = GetLocalizedDictionary(Path.Combine(folderpath, "Help" + finalPart));
    }

    /*
    private static Dictionary<string, string> GetLocalizedDictionary(string filepath)
    {
        var list = UtilsGeneral.GetFileStrings(filepath);
        var dict = UtilsGeneral.GetDictionaryFromList(list);
        /*
        if(dict.TryGetValue("text_tutorial_intro_2", out string val))
        {
            Debug.Log("dict val: " + val);
        }

        return dict;
    }*/

    private static Dictionary<string, string> GetLocalizedDictionary(string filepath)
    {
        TextAsset jsonFile = Resources.Load<TextAsset>(filepath);
        if (jsonFile == null)
        {
            Debug.LogWarning($"Localization file not found: {filepath}");
            return new Dictionary<string, string>();
        }
        return Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonFile.text);
    }

    private static void FillDefaultValues()
    {
        GeneralDictionary = new Dictionary<string, string>()
        {

            // -------------------- TUTORIAL --------------------- //

            { text_tutorial_continue, "Click to continue" },
            { text_tutorial_skip, "Click here to skip tutorial" },

            { text_tutorial_intro_1, "This is the background Warrior." },
            { text_tutorial_intro_2, "He will keep fighting even when you are not looking." },
            { text_tutorial_intro_3, "Defeat monsters to advance the stages." },
            { text_tutorial_intro_4, "Once all the stages are cleared, a new map will be unlocked." },
            { text_tutorial_intro_5, "On the right side of the screen you will find various menus." },
            { text_tutorial_intro_6, "Click on the STATS icon to increase stats level." },
            { text_tutorial_intro_7, "If you want to select a different job, click on the JOB icon." },
            { text_tutorial_intro_8, "You can find more informations about jobs in the HELP section of the SETTINGS menu." },
            { text_tutorial_intro_9, "Check your items using the INVENTORY icon." },
            { text_tutorial_intro_10, "Click on the QUESTS icon to check your progress and claim your rewards." },
            { text_tutorial_intro_11, "Spend Bits in the shop to purchase cards and jobs." },

            // -------------------- NAMES --------------------- //

            { text_name_buff_greed, "Greed" },
            { text_name_buff_veteran, "Veteran" },
            { text_name_buff_storyteller, "Storyteller" },
            { text_name_buff_stoned, "Stoned" },
            { text_name_buff_sailor, "Sailor" },
            { text_name_buff_dwarf, "Dwarf" },
            { text_name_buff_tamer, "Tamer" },
            { text_name_buff_arcanist, "Arcanist" },
            { text_name_buff_morningangler, "Morning Angler" },
            { text_name_buff_afternoonangler, "Afternoon Angler" },
            { text_name_buff_nightangler, "Night Angler" },
            { text_name_buff_ironskin, "Iron Skin" },
            { text_name_buff_inspiration, "Inspiration" },

            { text_name_buff_greed_effect, "Increase Bits gain from all sources by 20% for {0} minutes." },
            { text_name_buff_veteran_effect, "Increase Experience gain from all sources by 20% for {0} minutes." },
            { text_name_buff_storyteller_effect, "Increase Cards drop rate by 20% for {0} minutes." },
            { text_name_buff_stoned_effect, "Increase Ores drop rate by 20% for {0} minutes." },
            { text_name_buff_sailor_effect, "Increase successful hooks by 20% for {0} minutes." },
            { text_name_buff_dwarf_effect, "Increase Blacksmith forged materials by 20% for {0} minutes." },
            { text_name_buff_tamer_effect, "Increase Farmer chance to befriend a companion by 20% for {0} minutes." },
            { text_name_buff_arcanist_effect, "Increase Mage learning ability by 20% for {0} minutes." },
            { text_name_buff_morningangler_effect, "Attracts fishes normally found in the morning." },
            { text_name_buff_afternoonangler_effect, "Attracts fishes normally found in the afternoon." },
            { text_name_buff_nightangler_effect, "Attracts fishes normally found in the night." },
            { text_name_buff_inspiration_effect, "Thanks to the songs played earlier, some abilities have a 10% boost." },



            { text_name_class_warrior, "Warrior" },
            { text_name_class_miner, "Miner" },
            { text_name_class_fisher, "Fisher" },
            { text_name_class_farmer, "Farmer" },
            { text_name_class_blacksmith, "Blacksmith" },
            { text_name_class_mage, "Mage" },
            { text_name_class_alchemist, "Alchemist" },
            { text_name_class_necromancer, "Necromancer" },

            { text_name_warrior_stat_maxhp, "Max Hp" },
            { text_name_warrior_stat_atk, "Atk" },
            { text_name_warrior_stat_def, "Def" },
            { text_name_warrior_stat_atkspd, "Atk Spd" },
            { text_name_warrior_stat_critrate, "Crit Rate" },
            { text_name_warrior_stat_critdmg, "Crit Dmg" },
            { text_name_warrior_stat_luck, "Luck" },

            { text_name_miner_stat_power, "Power" },
            { text_name_miner_stat_smashspeed, "Smash Speed" },
            { text_name_miner_stat_shockwave, "Shockwave" },
            { text_name_miner_stat_luck, "Luck" },

            { text_name_blacksmith_stat_craftspeed, "Craft Speed" },
            { text_name_blacksmith_stat_efficiency, "Efficiency" },
            { text_name_blacksmith_stat_luck, "Luck" },
            { text_name_blacksmith_stat_metallurgy, "Metallurgy" },

            { text_name_fisher_stat_calmness, "Calmness" },
            { text_name_fisher_stat_reflex, "Reflex" },
            { text_name_fisher_stat_knowledge, "Knowledge" },
            { text_name_fisher_stat_luck, "Luck" },

            { text_name_farmer_stat_greenthumb, "Greenthumb" },
            { text_name_farmer_stat_agronomy, "Agronomy" },
            { text_name_farmer_stat_kindness, "Kindness" },
            { text_name_farmer_stat_luck, "Luck" },

            { text_name_mage_stat_insight, "Insight" },
            { text_name_mage_stat_castspeed, "Cast Speed" },
            { text_name_mage_stat_scholar, "Scholar" },
            { text_name_mage_stat_proficiency, "Proficiency" },

            { text_name_alchemist_stat_routine, "Routine" },
            { text_name_alchemist_stat_yield, "Yield" },
            { text_name_alchemist_stat_research, "Research" },
            { text_name_alchemist_stat_stability, "Stability" },

            { text_name_necromancer_stat_aptitude, "Aptitude" },
            { text_name_necromancer_stat_summon, "Summon" },
            { text_name_necromancer_stat_might, "Might" },
            { text_name_necromancer_stat_lifespan, "Lifespan" },
            { text_name_necromancer_stat_horde, "Horde" },
            { text_name_necromancer_stat_luck, "Luck" },



            { text_name_daymoment_morning, "Morning" },
            { text_name_daymoment_afternoon, "Afternoon" },
            { text_name_daymoment_evening, "Evening" },
            { text_name_daymoment_night, "Night" },

            { text_name_map0, "Woods" },
            { text_name_map1, "Cave" },
            { text_name_map2, "Lakes" },
            { text_name_map3, "Ruined Village" },
            { text_name_map4, "Swamps" },
            { text_name_map5, "Mountains" },
            { text_name_map6, "Desert" },
            { text_name_map7, "Sequoia Forest" },
            { text_name_map8, "Shore" },
            { text_name_map9, "Ruined City" },

            { text_name_card_rarity_common, "Common" },
            { text_name_card_rarity_uncommon, "Uncommon" },
            { text_name_card_rarity_rare, "Rare" },

            { text_name_fish_rarity_riverfolk, "Riverfolk" },
            { text_name_fish_rarity_deepwater, "Deepwater" },
            { text_name_fish_rarity_tideborn, "Tideborn" },
            { text_name_fish_rarity_ancient, "Ancient" },
            { text_name_fish_rarity_mythic, "Mythic" },

            // -------------------- TOOLTIPS --------------------- //

            { text_tooltip_panel_autobattle, "If this option is enabled, automatically advance to the next stage when all enemies are slayed, or retry the current stage in case of death" },

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
            { text_tooltip_stat_farmer_luck, "Increase Farmer chance to befriend a companion" },

            { text_tooltip_stat_mage_insight, "Increase Mage learning ability" },
            { text_tooltip_stat_mage_castspeed, "Increase Mage and Warrior casting speed" },
            { text_tooltip_stat_mage_scholar, "Unlock a new spell for the Mage to learn every 5 levels" },
            { text_tooltip_stat_mage_proficiency, "Every 5 levels unlock a slot to equip a spell to cast when advancing through the stages" },

            { text_tooltip_stat_alchemist_routine, "Increase Alchemist crafting speed" },
            { text_tooltip_stat_alchemist_yield, "Increase Alchemist chance to craft extra materials" },
            { text_tooltip_stat_alchemist_research, "Every 5 levels unlock a new recipe for the Alchemist to craft" },
            { text_tooltip_stat_alchemist_stability, "Increase Alchemist successful crafts" },

            { text_tooltip_stat_necromancer_aptitude, "Every 5 levels the Necromancer will summon a new couple of undead for training" },
            { text_tooltip_stat_necromancer_summon, "Increase summon speed for the Warrior" },
            { text_tooltip_stat_necromancer_might, "Increase undeads strength summoned by the Warrior" },
            { text_tooltip_stat_necromancer_lifespan, "Increase undeads life summoned by the Warrior" },
            { text_tooltip_stat_necromancer_horde, "Every 5 levels the Warrior maximum horde counter is increased by 1" },
            { text_tooltip_stat_necromancer_luck, "Increase experience gained by the Necromancer and increase Warrior chance to summon a powerful undead" },

            { text_tooltip_focusfishing, "Play a minigame to catch a fish manually" },
            { text_rules_focusfishing, "Hold left mouse button to move up and down the fish, dodge the rocks and collect 5 stars to catch it." },

            // -------------------- TITLE --------------------- //

            { text_title_buffs, "Buffs" },
            { text_title_shop, "Shop" },
            { text_title_quests, "Quests" },
            { text_title_quests_bountieslist, "Choose a bounty" },
            { text_title_settings, "Settings" },
            { text_title_inventory, "Inventory" },
            { text_title_inventory_convertlist, "Select cards to convert" },
            { text_title_jobs, "Jobs" },
            { text_title_jobs_back, "<- Click here to go back to the job tree" },
            { text_title_jobs_warrior_maps, "Maps" },

            // -------------------- SHOP --------------------- //

            { text_shop_item_purchased, "Purchased" },
            { text_shop_insertredeeem, "Insert the redeem code" },
            { text_shop_insertdebug, "Insert the debug code" },
            
            // -------------------- QUESTS --------------------- //

            { text_quest_desc_kill_specific, "Kill {0} {1}" },
            { text_quest_desc_kill_nonspecific, "Kill {0} monster" },
            { text_quest_desc_kill_nonspecific_plural, "Kill {0} monsters" },

            { text_quest_desc_obtain_item_category_ores, "ore" },
            { text_quest_desc_obtain_item_category_ores_plural, "ores" },
            { text_quest_desc_obtain_item_category_cards, "card" },
            { text_quest_desc_obtain_item_category_cards_plural, "cards" },
            { text_quest_desc_obtain_item_category_metals, "metal" },
            { text_quest_desc_obtain_item_category_metals_plural, "metals" },
            { text_quest_desc_obtain_item_category_fishes, "fish" },
            { text_quest_desc_obtain_item_category_fishes_plural, "fishes" },

            { text_quest_desc_obtain_specific, "Obtain {0} {1}" },
            { text_quest_desc_obtain_nonspecific, "Obtain {0} {1}" },

            { text_quest_desc_craft_specific, "Craft {0} {1}" },
            { text_quest_desc_craft_nonspecific, "Craft {0} Concoctions" },

            { text_quest_desc_levelup_specific_once, "Level up {0} {1} time" },
            { text_quest_desc_levelup_specific_multiple, "Level up {0} {1} times" },
            { text_quest_desc_levelup_nonspecific_once, "Level up any stat {0} time" },
            { text_quest_desc_levelup_nonspecific_multiple, "Level up any stat {0} times" },

            { text_quest_desc_unlockmap, "Unlock {0} map" },

            { text_quest_desc_befriend_specific, "Obtain {0} {1}" },
            { text_quest_desc_befriend_nonspecific, "Obtain {0} companions" },

            { text_quest_desc_spellrankup_specific_once, "Rank up {0} {1} time" },
            { text_quest_desc_spellrankup_specific_multiple, "Rank up {0} {1} times" },
            { text_quest_desc_spellrankup_nonspecific_once, "Rank up any spell {0} time" },
            { text_quest_desc_spellrankup_nonspecific_multiple, "Rank up any spell {0} times" },

            { text_quest_desc_summon_plural, "Summon {0} Undeads with the Warrior" },

            { text_quest_reward_bounty, "Reward: {0} bits" },

             // -------------------- SETTINGS --------------------- //

            { text_settings_general_titlevolume, "Volume" },
            { text_settings_general_titlelanguage, "Language" },
            { text_settings_general_titleexit, "Exit" },
            { text_settings_general_button_titlescreen, "Title Screen" },
            { text_settings_general_button_quit, "Quit" },

            { text_settings_general_lang_english, "English" },
            { text_settings_general_lang_italian, "Italiano" },



            { text_settings_gameplay_titlebattle, "Battle" },
            { text_settings_gameplay_autobattle, "Auto-Battle" },
            { text_settings_gameplay_titlehud, "HUD" },
            { text_settings_gameplay_option_invertedhud, "Inverted HUD" },
            { text_settings_gameplay_option_autoclosehud, "Autoclose" },
            { text_settings_gameplay_option_timerautoclose, "Autoclose timer" },
            { text_settings_gameplay_option_minutes, "minutes" },
            { text_settings_gameplay_option_showadvancedstats, "Show advanced stats" },
            { text_settings_gameplay_titlefloatinghud, "Floating HUD" },
            { text_settings_gameplay_option_damage, "Damage" },
            { text_settings_gameplay_option_itemcollected, "Item Collected" },
            { text_settings_gameplay_option_tooltips, "Tooltips" },
            { text_settings_gameplay_titleanimations, "Animations" },
            { text_settings_gameplay_option_equipmentlevelup, "Equipment Level Up" },
            { text_settings_gameplay_titlefisher, "Fisher" },
            { text_settings_gameplay_option_invertfishingspot, "Invert fishing spot" },
            { text_settings_gameplay_option_hidefishingbar, "Hide fishing bar" },

            { text_settings_video_option_alwaysontop, "Always on top" },
            { text_settings_video_option_clickthrough, "Click through background" },
            { text_settings_video_titletargetfps, "Target FPS" },
            { text_settings_video_option_toggle30, "30 FPS" },
            { text_settings_video_option_toggle60, "60 FPS" },
            { text_settings_video_button_changemonitor, "Change Monitor" },

            // -------------------- JOBS --------------------- //

            { text_job_current_level, "Lv. : {0}" },
            { text_job_available_points, "Available points: {0}" },
            { text_job_current_stat_level, "Lv. {0}" },
            { text_job_change_stat_level, "Lv. {0} >> Lv. {1}" },

            { text_job_warrior_unlockconditions, "None" },
            { text_job_miner_unlockconditions, "None" },
            { text_job_fisher_unlockconditions, "None" },
            { text_job_farmer_unlockconditions, "Purchasable from the shop" },
            { text_job_blacksmith_unlockconditions, "Collect at least 1 gold ore." },
            { text_job_mage_unlockconditions, "Find the Ancient Tome card" },
            { text_job_alchemist_unlockconditions, "Find the Debug mode code" },
            { text_job_necromancer_unlockconditions, "Max out Mage's Scholar ability" },
            { text_job_bard_unlockconditions, "Purchasable from the shop" },

            { text_job_necromancer_arise_unlockconditions, "Max out Summon and Horde abilities to unlock" },
            { text_job_necromancer_afterlife_unlockconditions, "Max out Aptitude and Might abilities to unlock" },
            { text_job_necromancer_ade_unlockconditions, "Max out Lifespan and Luck abilities to unlock" },

            { text_job_warrior_mapstage, "Stage: {0}/{1}" },
            { text_job_warrior_possiblemonsters, "Possible monsters:<br>" },
            { text_job_warrior_possiblecards, "Possible cards:<br>" },


            { text_job_miner_weapon_currentlevel, "Lv. {0}" },
            { text_job_miner_weapon_nextlevel, "Next level" },
            { text_job_miner_weapon_currentstats, "Dmg: +{0:.0}%" },
            { text_job_miner_requirements_levelup, "Requirements for level up" },
            { text_job_blacksmith_gear_currentstats, "{0}: +{1:.0}%<br>" },
            { text_job_blacksmith_requirements_levelup, "Requirements for level up" },

            { text_job_fisher_availablefishes, "Current available fishes" },
            { text_job_fisher_caughtsession, "Fishes caught in this session until now:" },


            { text_job_farmer_crop_basegrowthtime, "Base growth time: {1}m{2}s<br>" },
            { text_job_farmer_crop_attracts, "Attracts:<br>" },
            { text_job_farmer_crop_companiondesc, "Equip companions that will help you defeating monsters" },
            { text_job_farmer_companion_cropdesc, "Growing Crops" },
            { text_job_farmer_companion_equipped, "Equipped" },


            { text_job_mage_spell_locked, "Spell locked" },
            { text_job_alchemist_recipe_locked, "Recipe not available" },
            { text_job_necromancer_ritual_locked, "Ritual locked" },


            { text_job_fisher_raritytooltip, "<color=#{1}>{2}</color><br>" },
            { text_job_fisher_spawntime, "Spawn time: {3}" },
            { text_job_fisher_waittime, "Average time: ~{0}m{1}s" },

            { text_job_alchemist_ingredients, "Ingredients" },

            
            // -------------------- BUTTONS --------------------- //
            
            { text_button_new, "New" },
            { text_button_continue, "Continue" },
            { text_button_quit, "Quit" },

            { text_button_savechanges, "Save changes" },

            { text_button_buy, "Buy" },
            { text_button_redeem, "Redeem" },
            { text_button_debug, "Debug" },

            { text_button_claim, "Claim" },
            { text_button_selectbounty, "--- Click to choose a bounty from the list ---" },
            { text_button_accept, "Accept" },

            { text_button_use, "Use" },
            { text_button_convert, "Convert" },
            { text_button_dismantle, "Dismantle" },
            { text_button_dismantle_cancel, "<- Cancel" },
            { text_button_quickselect, "Quick Select" },

            { text_button_fight, "Fight" },

            { text_button_levelup, "Level up" },
            { text_button_gather, "Gather" },
            { text_button_forge, "Forge" },
            { text_button_chant, "Chant" },
            { text_button_play, "Play" },

            { text_button_farm, "Farm" },
            { text_button_companions, "Companions" },
            { text_button_crops, "Crops" },
            { text_button_equip, "Equip" },
            { text_button_unequip, "Unequip" },

            { text_button_fish, "Fish" },

            { text_button_learn, "Learn" },

            { text_button_craft, "Craft" },

            { text_button_revealall, "Reveal All" },

            { text_button_shop_filter_cardpacks, "Card Packs" },
            { text_button_shop_filter_jobs, "Jobs" },
            { text_button_shop_filter_baits, "Baits" },
            { text_button_shop_filter_redeem, "Redeem" },
            { text_button_shop_filter_debug, "Debug" },

            { text_button_quests_filter_story, "Story" },
            { text_button_quests_filter_daily, "Daily" },
            { text_button_quests_filter_bounty, "Bounty" },

            { text_button_settings_filter_general, "General" },
            { text_button_settings_filter_gameplay, "Gameplay" },
            { text_button_settings_filter_video, "Video" },
            { text_button_settings_filter_credits, "Credits" },
            { text_button_settings_filter_help, "Help" },

            { text_button_help_filter_warrior, "Warrior" },
            { text_button_help_filter_miner, "Miner" },
            { text_button_help_filter_fisher, "Fisher" },
            { text_button_help_filter_blacksmith, "Blacksmith" },
            { text_button_help_filter_farmer, "Farmer" },
            { text_button_help_filter_mage, "Mage" },
            { text_button_help_filter_alchemist, "Alchemist" },
            { text_button_help_filter_necromancer, "Necromancer" },
            { text_button_help_filter_bard, "Bard" },

            { text_button_inventory_filter_all, "All" },
            { text_button_inventory_filter_ores, "Ores" },
            { text_button_inventory_filter_metals, "Metals" },
            { text_button_inventory_filter_fishes, "Fishes" },
            { text_button_inventory_filter_crops, "Crops" },
            { text_button_inventory_filter_baits, "Baits" },
            { text_button_inventory_filter_concoctions, "Concoctions" },
            { text_button_inventory_filter_cards, "Cards" },

             // -------------------- YESNO --------------------- //
            
            { text_yesno_yes, "Confirm" },
            { text_yesno_no, "Cancel" },
            { text_yesno_newgame, "You already have an adventure in progress, starting a new game will erase your current save files.<br>Are you sure you want to continue?" },
            { text_yesno_newgame_fatalerror, 
                "A fatal error has occured while loading your save files, try to restart the game.<br>" +
                "Starting a new game will erase your current save files.<br>" +
                "Are you sure you want to continue?" 
            },

            { text_yesno_question_buy, "Do you want to buy {0} for {1} bits?" },
            { text_yesno_question_titlescreen, "Return to title screen?" },
            { text_yesno_question_quitgame, "Close the game?" },

        };


        ItemNamesTextDictionary = new Dictionary<string, string>()
        {
            { text_enemy_slime_name, "Slime" },
            { text_enemy_slime_name_plural, "Slimes" },
            { text_enemy_orc_name, "Orc" },
            { text_enemy_orc_name_plural, "Orcs" },
            { text_enemy_skeleton_name, "Skeleton" },
            { text_enemy_skeleton_name_plural, "Skeletons" },
            { text_enemy_werewolf_name, "Werewolf" },
            { text_enemy_werewolf_name_plural, "Werewolves" },
            { text_enemy_werebear_name, "Werebear" },
            { text_enemy_werebear_name_plural, "Werebears" },
            { text_enemy_armoredskeleton_name, "Armored Skeleton" },
            { text_enemy_armoredskeleton_name_plural, "Armored Skeletons" },
            { text_enemy_greatswordskeleton_name, "Greatsword Skeleton" },
            { text_enemy_greatswordskeleton_name_plural, "Greatsword Skeletons" },
            { text_enemy_skeletonarcher_name, "Skeleton Archer" },
            { text_enemy_skeletonarcher_name_plural, "Skeleton Archers" },
            { text_enemy_armoredorc_name, "Armored Orc" },
            { text_enemy_armoredorc_name_plural, "Armored Orcs" },
            { text_enemy_eliteorc_name, "Elite Orc" },
            { text_enemy_eliteorc_name_plural, "Elite Orcs" },
            { text_enemy_orcrider_name, "Orc Rider" },
            { text_enemy_orcrider_name_plural, "Orc Riders" },

            { text_item_copperore_name, "Copper ore" },
            { text_item_copperore_name_plural, "Copper ores" },
            { text_item_ironore_name, "Iron ore" },
            { text_item_ironore_name_plural, "Iron ores" },
            { text_item_bronzeore_name, "Bronze ore" },
            { text_item_bronzeore_name_plural, "Bronze ores" },
            { text_item_silverore_name, "Silver ore" },
            { text_item_silverore_name_plural, "Silver ores" },
            { text_item_goldore_name, "Gold ore" },
            { text_item_goldore_name_plural, "Gold ores" },

            { text_item_copper_name, "Copper" },
            { text_item_iron_name, "Iron" },
            { text_item_bronze_name, "Bronze" },
            { text_item_silver_name, "Silver" },
            { text_item_gold_name, "Gold" },

            { text_item_card_01_name, "Slime" },
            { text_item_card_02_name, "Orc" },
            { text_item_card_03_name, "Skeleton" },
            { text_item_card_04_name, "Werewolf" },
            { text_item_card_05_name, "Werebear" },
            { text_item_card_06_name, "Armored Skeleton" },
            { text_item_card_07_name, "Armored Orc" },
            { text_item_card_08_name, "Skeleton Archer" },
            { text_item_card_09_name, "Elite Orc" },
            { text_item_card_10_name, "Greatsword Skeleton" },
            { text_item_card_11_name, "Armored Axeman" },
            { text_item_card_12_name, "Corrupted Priest" },
            { text_item_card_13_name, "Undead Soldier" },
            { text_item_card_14_name, "Dark Wizard" },
            { text_item_card_15_name, "Scorching Slime" },
            { text_item_card_16_name, "Rampaging Skeleton" },
            { text_item_card_17_name, "Orc King" },
            { text_item_card_18_name, "Alpha Werewolf" },
            { text_item_card_19_name, "Mark, City Guard" },
            { text_item_card_20_name, "Terry, Captain of the Kingdom Knights" },
            { text_item_card_21_name, "Bartolomeo, King's Advisor" },
            { text_item_card_22_name, "Connor, Chief of Strategists" },
            { text_item_card_23_name, "Colt, Knights' Trainer" },
            { text_item_card_24_name, "Titan, S Rank Adventurer" },
            { text_item_card_25_name, "Otto, The Strongest Magician" },
            { text_item_card_26_name, "Eris, Alchemist" },
            { text_item_card_27_name, "The Hero" },
            { text_item_card_28_name, "Magic Circle" },
            { text_item_card_29_name, "Mormegil, Titan's Sword" },
            { text_item_card_30_name, "Lapis, Otto's Scepter" },
            { text_item_card_31_name, "Monster Stampede, ancient tome" },
            { text_item_card_32_name, "Hero Curse" },
            { text_item_card_33_name, "Eris's Diary" },
            { text_item_card_34_name, "Eris's Diary Page 1" },
            { text_item_card_35_name, "Eris's Diary Page 2" },
            { text_item_card_36_name, "Eris's Diary Page 3" },
            { text_item_card_37_name, "Eris's Diary Page 4" },
            { text_item_card_38_name, "Eris's Diary Page 5" },
            { text_item_card_39_name, "Eris's Cards" },
            { text_item_card_40_name, "Screaming Voice" },

            { text_item_fish_01_name, "Progenetica" },
            { text_item_fish_02_name, "Clownfish" },
            { text_item_fish_03_name, "Blue Surgeonfish" },
            { text_item_fish_04_name, "Somber Surgeonfish" },
            { text_item_fish_05_name, "Gem Tang" },
            { text_item_fish_06_name, "Sailfin Surgeonfish" },
            { text_item_fish_07_name, "Angelfish" },
            { text_item_fish_08_name, "Quenn Angelfish" },
            { text_item_fish_09_name, "French Angelfish" },
            { text_item_fish_10_name, "Goldfish" },
            { text_item_fish_11_name, "Fighting Fish" },
            { text_item_fish_12_name, "Catfish" },
            { text_item_fish_13_name, "Blowfish" },
            { text_item_fish_14_name, "Spotted Pufferfish" },
            { text_item_fish_15_name, "Ocean Sunfish" },
            { text_item_fish_16_name, "Dorado Dolphinfish" },
            { text_item_fish_17_name, "Roule's Goby" },
            { text_item_fish_18_name, "Red Piranha" },
            { text_item_fish_19_name, "Lionfish" },
            { text_item_fish_20_name, "Red Scorpionfish" },
            { text_item_fish_21_name, "Stonefish" },
            { text_item_fish_22_name, "Flying Fish" },
            { text_item_fish_23_name, "Guppy" },
            { text_item_fish_24_name, "Sailfin Molly" },
            { text_item_fish_25_name, "Greater Weever" },
            { text_item_fish_26_name, "Red Salmon" },
            { text_item_fish_27_name, "Taimen" },
            { text_item_fish_28_name, "Atlantic Salmon" },
            { text_item_fish_29_name, "Japanese Trout" },
            { text_item_fish_30_name, "Monkfish" },
            { text_item_fish_31_name, "Humpback Anglerfish" },
            { text_item_fish_32_name, "Hairy Frogfish" },
            { text_item_fish_33_name, "European Carp" },
            { text_item_fish_34_name, "Golden Tench" },
            { text_item_fish_35_name, "Koi Carp" },
            { text_item_fish_36_name, "Barracuda" },
            { text_item_fish_37_name, "Cardinal Tetra" },
            { text_item_fish_38_name, "Emperor Tetra" },
            { text_item_fish_39_name, "Zembrafish" },
            { text_item_fish_40_name, "Petticoat Tetra" },
            { text_item_fish_41_name, "European Perch" },
            { text_item_fish_42_name, "Starry Sturgeon" },
            { text_item_fish_43_name, "Lake Sturgeon" },
            { text_item_fish_44_name, "Striped Marlin" },
            { text_item_fish_45_name, "Swordfish" },
            { text_item_fish_46_name, "Garfish" },
            { text_item_fish_47_name, "Sardine" },
            { text_item_fish_48_name, "Atlantic Herring" },
            { text_item_fish_49_name, "Blackspot Seabream" },
            { text_item_fish_50_name, "Silver Seabream" },
            { text_item_fish_51_name, "Atlantic Cod" },
            { text_item_fish_52_name, "Hake" },
            { text_item_fish_53_name, "Bluefin Tuna" },
            { text_item_fish_54_name, "Pike" },
            { text_item_fish_55_name, "Common Barbel" },
            { text_item_fish_56_name, "Tiger Barb" },
            { text_item_fish_57_name, "Cherry Barb" },
            { text_item_fish_58_name, "Moonfish" },
            { text_item_fish_59_name, "Blue Discus" },
            { text_item_fish_60_name, "Rainbow Trout" },
            { text_item_fish_61_name, "Ribbon Eel" },
            { text_item_fish_62_name, "Giant Mooray Eel" },
            { text_item_fish_63_name, "European Conger" },
            { text_item_fish_64_name, "Harlequin Snake Eel" },
            { text_item_fish_65_name, "Oarfish" },
            { text_item_fish_66_name, "African Coelacanth" },
            { text_item_fish_67_name, "Longnose Gar" },
            { text_item_fish_68_name, "Saddled Bichir" },
            { text_item_fish_69_name, "Gray Bichir" },
            { text_item_fish_70_name, "Turbot" },
            { text_item_fish_71_name, "European Seabass" },
            { text_item_fish_72_name, "European Anchovy" },
            { text_item_fish_73_name, "Humphead Wrasse" },
            { text_item_fish_74_name, "Common Stingray" },
            { text_item_fish_75_name, "Shortfin Mako Shark" },
            { text_item_fish_76_name, "River Lamprey" },
            { text_item_fish_77_name, "Racoon Butterfish" },
            { text_item_fish_78_name, "Atlantic Trumpetfish" },
            { text_item_fish_79_name, "Lyretail Anthias" },
            { text_item_fish_80_name, "Fire Goby" },

             { text_item_crop_1_name, "Bamboo" },
            { text_item_crop_2_name, "Lumberry" },
            { text_item_crop_3_name, "Oramelon" },
            { text_item_crop_4_name, "Pinnocchia" },
            { text_item_crop_5_name, "Pommodoro" },
            { text_item_crop_6_name, "Pumpky" },

            { text_item_bait_1_name, "Earthworm Bait" },
            { text_item_bait_2_name, "Earthworm Bait Plus" },
            { text_item_bait_3_name, "Earthworm Bait Max" },
            { text_item_bait_4_name, "Cricket Bait" },
            { text_item_bait_5_name, "Cricket Bait Plus" },
            { text_item_bait_6_name, "Cricket Bait Max" },
            { text_item_bait_7_name, "Shrimp Bait" },
            { text_item_bait_8_name, "Shrimp Bait Plus" },
            { text_item_bait_9_name, "Earthworm Bait Max" },

            { text_item_concoction_1_name, "Greed Elixir" },
            { text_item_concoction_2_name, "Veteran Drink" },
            { text_item_concoction_3_name, "Storyteller Sip" },
            { text_item_concoction_4_name, "Stoned Brew" },
            { text_item_concoction_5_name, "Sailor Elixir" },
            { text_item_concoction_6_name, "Dwarf Drink" },
            { text_item_concoction_7_name, "Tamer Drink" },
            { text_item_concoction_8_name, "Arcanist Elixir" },
            { text_item_concoction_9_name, "Iron Skin Potion" },
            { text_item_concoction_10_name, "Health Potion" },
            { text_item_concoction_11_name, "Attack Potion" },
            { text_item_concoction_12_name, "Defense Potion" },

            { text_item_fish_group_life_name, "Life Series" },
            { text_item_fish_group_predator_name, "Predator Series" },
            { text_item_fish_group_guardian_name, "Guardian Series" },
            { text_item_fish_group_dart_name, "Dart Series" },
            { text_item_fish_group_sharp_name, "Sharp Series" },
            { text_item_fish_group_piercing_name, "Piercing Series" },
            { text_item_fish_group_golden_name, "Golden Series" },
            { text_item_fish_group_elder_name, "Elder Series" },
            { text_item_fish_group_quick_name, "Quick Series" },

            { text_mage_spell_fireball_name, "Fireball" },
            { text_mage_spell_explosion_name, "Explosion" },
            { text_mage_spell_chillwind_name, "Chill Wind" },
            { text_mage_spell_poisongas_name, "Poison Gas" },
            { text_mage_spell_zap_name, "Zap" },


            { text_necromancer_ritual_summon_name, "Summon" },
            { text_necromancer_ritual_arise_name, "Arise" },
            { text_necromancer_ritual_afterlife_name, "Afterlife" },
            { text_necromancer_ritual_ade_name, "Ade" },



            { text_shopitem_cardpack01_name, "Copper Pack" },
            { text_shopitem_cardpack02_name, "Iron Pack" },
            { text_shopitem_cardpack03_name, "Bronze Pack" },
            { text_shopitem_cardpack04_name, "Silver Pack" },
            { text_shopitem_cardpack05_name, "Gold Pack" },

                                      
            { text_shopitem_job_farmer_name, "Class: Farmer" },
            { text_shopitem_job_bard_name, "Class: Bard" },


            { text_shopitem_bait_normalmorning_name, "Box of Earthworm Baits" },
            { text_shopitem_bait_greatmorning_name, "Box of Earthworm Baits Plus" },
            { text_shopitem_bait_maxmorning_name, "Box of Earthworm Baits Max" },
            { text_shopitem_bait_normalafternoon_name, "Box of Cricket Baits" },
            { text_shopitem_bait_greatafternoon_name, "Box of Cricket Baits Plus" },
            { text_shopitem_bait_maxafternoon_name, "Box of Cricket Baits Max" },
            { text_shopitem_bait_normalnight_name, "Box of Shrimp Baits" },
            { text_shopitem_bait_greatnight_name, "Box of Shrimp Baits Plus" },
            { text_shopitem_bait_maxnight_name, "Box of Shrimp Baits Max" },




            { text_companion_0_name, "Ragghost" },
            { text_companion_1_name, "Shemoon" },
            { text_companion_2_name, "Wingoat" },
            { text_companion_3_name, "Zompig" },
            { text_companion_4_name, "Zuccow" },
        };


        ItemDescsTextDictionary = new Dictionary<string, string>()
        {
            { text_item_copperore_desc, "Copper Ore" },
            { text_item_ironore_desc, "Iron Ore" },
            { text_item_bronzeore_desc, "Bronze Ore" },
            { text_item_silverore_desc, "Silver Ore" },
            { text_item_goldore_desc, "Gold Ore" },

            { text_item_copper_desc, "Copper" },
            { text_item_iron_desc, "Iron" },
            { text_item_bronze_desc, "Bronze" },
            { text_item_silver_desc, "Silver" },
            { text_item_gold_desc, "Gold" },

            { text_item_card_01_desc, "Weakest type of monster. They were almost annihilated." },
            { text_item_card_02_desc, "A creature with complete lack of intelligence. 100 years ago, they were used as training creatures." },
            { text_item_card_03_desc, "Some say they were once humans." },
            { text_item_card_04_desc, "Ferocious beasts, their body parts were used to make weapons." },
            { text_item_card_05_desc, "Apex predators among monsters. Their skin was used to make clothes." },
            { text_item_card_06_desc, "Every time they die, they improve." },
            { text_item_card_07_desc, "Slightly more intelligent than regular orcs. When they kill an enemy, they also steal their armor." },
            { text_item_card_08_desc, "It was hard to take down. One of the least monsters summoned by the mysterious spell." },
            { text_item_card_09_desc, "The most intelligent among orcs. They can lead hundreds of their kin." },
            { text_item_card_10_desc, "One of the most feared on the battlefield, hard to kill and stronger than simple soldiers." },
            { text_item_card_11_desc, "A mysterious spell scattered around the continent. Some men fell under its effects." },
            { text_item_card_12_desc, "They used to heal monsters on the battlefield. 50 years later, humans became the minority." },
            { text_item_card_13_desc, "Creatures that came back. They were once human." },
            { text_item_card_14_desc, "Once the most powerful soldiers. When wizards became corrupted, humans had to come up with a plan." },
            { text_item_card_15_desc, "The monsters started mutating, making the continent their new home." },
            { text_item_card_16_desc, "Sometimes monsters acted strangely. No fighting, no screaming, nothing… as if they were trying to recollect their thoughts." },
            { text_item_card_17_desc, "After several wins on the battlefield, Orc are crowned as kings." },
            { text_item_card_18_desc, "Mutated monsters. They absorbed so much magic during their lifespan to reach a higher evolution." },

            { text_item_card_19_desc, 
                "Just like any other day, he was on duty guarding the city walls, and relaxing. Not a single monster in sight. " +
                "Suddenly, he felt an ominous energy spreading around. Shortly after, he was witnessing the biggest swarm of enemies he'd ever seen." },

            { text_item_card_20_desc, "He knew something was about to happen, a gut feeling. He was ready. And as soon as the guards sounded the alarm, he rushed outside the city walls, a strategy formed in his mind." },
            { text_item_card_21_desc, "Because of him, some very wrong decisions were made during the war. Blinded by the power he held, he fully supported an all-out war, certain that humans would prevail once again." },
            { text_item_card_22_desc, 
                "Thanks to him, humans lived longer than expected. But even though he put all his effort into it, the monsters kept advancing. " +
                "Their numbers wouldn't decrease, it felt like fighting an infinite army." },

            { text_item_card_23_desc, 
                "He was there when the war started, but he would not live enough to see its end. In 60 years, he trained so many, and many of them died. " +
                "Before retirement, there was a rumor that some kind of ancient spell was going to save them all." },

            { text_item_card_24_desc, 
                "Always on the frontline, he stood against all kinds of monsters, turning into a symbol for all the humans still fighting. " +
                "Before the war, he was a prime adventurer who loved exploring different ruins every day. During one of those explorations, he recovered an ancient tome." },

            { text_item_card_25_desc, 
                "Magic had no secrets for him. He learned every language, even the most ancient ones, to expand his knowledge. " +
                "During the war, he was recruited to decipher an old tome that was recovered from some ruins." },

            { text_item_card_26_desc, 
                "First-class researcher and alchemist, she was recruited to use an ancient spell that would have put an end to the war. " +
                "Not trusting the tome, she refused to take part in it. Her memories were blurry. " +
                "She used to wake up at night, remembering different outcomes of that war, and wrote all her notes in a diary." },

            { text_item_card_27_desc, 
                "He woke up with no memories, and now he doesn’t know who he is. " +
                "All he can do is fight, because of a voice echoing in his head to do so. He doesn't want to fight, but he must, or else the voice won't stop." },

            { text_item_card_28_desc, "No one knows who created them and why, but monsters keep crawling out of it. 100 years ago, some new circles spawned in different places." },
            { text_item_card_29_desc, "The greatest sword ever created and companion of Titan. This sword proves the skills that humanity reached at its peak, even the strongest monster couldn't defend against it." },
            { text_item_card_30_desc, "The greatest scepter ever created and companion of Otto. This scepter shows the skills that humanity reached at its peak. A single spell could annihilate hundreds of monsters." },
            { text_item_card_31_desc, "No author, the title is barely visible. History about peace, endless monsters, and a powerful spell. Only the strongest can handle it, and the more people use it, the faster are the effects." },
            { text_item_card_32_desc, "\"A human will be born, talented for all masteries. A curse upon them, a screaming voice telling them to fight. They will not die until the war is over\"." },
            { text_item_card_33_desc, "The alchemist Eris was having weird dreams; she started to take note of them. They won the war, they lost the war, there was never a war. Most of its pages were lost." },
            { text_item_card_34_desc, "Humans won for the 8th time." },
            { text_item_card_35_desc, "5th wave didn't respawned." },
            { text_item_card_36_desc, "Debug.Log(\"6\")." },
            { text_item_card_37_desc, "4 statuses." },
            { text_item_card_38_desc, "Skeleton id: 1." },
            { text_item_card_39_desc, "I wrote down my memories into a new magical item of my creation. To whomever finds them, unveil the mysteries of my world." },
            { text_item_card_40_desc, "The hero has been cursed, this screaming voice telling him to fight won't be silent. The only thing he can do is to keep going forward." },

            { text_item_fish_01_desc, "Progenetica" },
            { text_item_fish_02_desc, "Clownfish" },
            { text_item_fish_03_desc, "Blue Surgeonfish" },
            { text_item_fish_04_desc, "Somber Surgeonfish" },
            { text_item_fish_05_desc, "Gem Tang" },
            { text_item_fish_06_desc, "Sailfin Surgeonfish" },
            { text_item_fish_07_desc, "Angelfish" },
            { text_item_fish_08_desc, "Quenn Angelfish" },
            { text_item_fish_09_desc, "French Angelfish" },
            { text_item_fish_10_desc, "Goldfish" },
            { text_item_fish_11_desc, "Fighting Fish" },
            { text_item_fish_12_desc, "Catfish" },
            { text_item_fish_13_desc, "Blowfish" },
            { text_item_fish_14_desc, "Spotted Pufferfish" },
            { text_item_fish_15_desc, "Ocean Sunfish" },
            { text_item_fish_16_desc, "Dorado Dolphinfish" },
            { text_item_fish_17_desc, "Roule's Goby" },
            { text_item_fish_18_desc, "Red Piranha" },
            { text_item_fish_19_desc, "Lionfish" },
            { text_item_fish_20_desc, "Red Scorpionfish" },
            { text_item_fish_21_desc, "Stonefish" },
            { text_item_fish_22_desc, "Flying Fish" },
            { text_item_fish_23_desc, "Guppy" },
            { text_item_fish_24_desc, "Sailfin Molly" },
            { text_item_fish_25_desc, "Greater Weever" },
            { text_item_fish_26_desc, "Red Salmon" },
            { text_item_fish_27_desc, "Taimen" },
            { text_item_fish_28_desc, "Atlantic Salmon" },
            { text_item_fish_29_desc, "Japanese Trout" },
            { text_item_fish_30_desc, "Monkfish" },
            { text_item_fish_31_desc, "Humpback Anglerfish" },
            { text_item_fish_32_desc, "Hairy Frogfish" },
            { text_item_fish_33_desc, "European Carp" },
            { text_item_fish_34_desc, "Golden Tench" },
            { text_item_fish_35_desc, "Koi Carp" },
            { text_item_fish_36_desc, "Barracuda" },
            { text_item_fish_37_desc, "Cardinal Tetra" },
            { text_item_fish_38_desc, "Emperor Tetra" },
            { text_item_fish_39_desc, "Zembrafish" },
            { text_item_fish_40_desc, "Petticoat Tetra" },
            { text_item_fish_41_desc, "European Perch" },
            { text_item_fish_42_desc, "Starry Sturgeon" },
            { text_item_fish_43_desc, "Lake Sturgeon" },
            { text_item_fish_44_desc, "Striped Marlin" },
            { text_item_fish_45_desc, "Swordfish" },
            { text_item_fish_46_desc, "Garfish" },
            { text_item_fish_47_desc, "Sardine" },
            { text_item_fish_48_desc, "Atlantic Herring" },
            { text_item_fish_49_desc, "Blackspot Seabream" },
            { text_item_fish_50_desc, "Silver Seabream" },
            { text_item_fish_51_desc, "Atlantic Cod" },
            { text_item_fish_52_desc, "Hake" },
            { text_item_fish_53_desc, "Bluefin Tuna" },
            { text_item_fish_54_desc, "Pike" },
            { text_item_fish_55_desc, "Common Barbel" },
            { text_item_fish_56_desc, "Tiger Barb" },
            { text_item_fish_57_desc, "Cherry Barb" },
            { text_item_fish_58_desc, "Moonfish" },
            { text_item_fish_59_desc, "Blue Discus" },
            { text_item_fish_60_desc, "Rainbow Trout" },
            { text_item_fish_61_desc, "Ribbon Eel" },
            { text_item_fish_62_desc, "Giant Mooray Eel" },
            { text_item_fish_63_desc, "European Conger" },
            { text_item_fish_64_desc, "Harlequin Snake Eel" },
            { text_item_fish_65_desc, "Oarfish" },
            { text_item_fish_66_desc, "African Coelacanth" },
            { text_item_fish_67_desc, "Longnose Gar" },
            { text_item_fish_68_desc, "Saddled Bichir" },
            { text_item_fish_69_desc, "Gray Bichir" },
            { text_item_fish_70_desc, "Turbot" },
            { text_item_fish_71_desc, "European Seabass" },
            { text_item_fish_72_desc, "European Anchovy" },
            { text_item_fish_73_desc, "Humphead Wrasse" },
            { text_item_fish_74_desc, "Common Stingray" },
            { text_item_fish_75_desc, "Shortfin Mako Shark" },
            { text_item_fish_76_desc, "River Lamprey" },
            { text_item_fish_77_desc, "Racoon Butterfish" },
            { text_item_fish_78_desc, "Atlantic Trumpetfish" },
            { text_item_fish_79_desc, "Lyretail Anthias" },
            { text_item_fish_80_desc, "Fire Goby" },

            { text_item_crop_1_desc, "Bamboo" },
            { text_item_crop_2_desc, "Lumberry" },
            { text_item_crop_3_desc, "Oramelon" },
            { text_item_crop_4_desc, "Pinnocchia" },
            { text_item_crop_5_desc, "Pommodoro" },
            { text_item_crop_6_desc, "Pumpky" },

            { text_item_bait_1_desc, "For 10 minutes, also attracts fishes normally found in the morning" },
            { text_item_bait_2_desc, "For 10 minutes, also attracts a lot of fishes normally found in the morning" },
            { text_item_bait_3_desc, "For 10 minutes, attracts only fishes normally found in the morning" },
            { text_item_bait_4_desc, "For 10 minutes, also attracts fishes normally found in the afternoon" },
            { text_item_bait_5_desc, "For 10 minutes, also attracts a lot of fishes normally found in the afternoon" },
            { text_item_bait_6_desc, "For 10 minutes, attracts only fishes normally found in the afternoon" },
            { text_item_bait_7_desc, "For 10 minutes, also attracts fishes normally found in the night" },
            { text_item_bait_8_desc, "For 10 minutes, also attracts a lot of fishes normally found in the night" },
            { text_item_bait_9_desc, "For 10 minutes, attracts only fishes normally found in the night" },

            { text_item_concoction_1_desc, "Grants Greed buff." },
            { text_item_concoction_2_desc, "Grants Veteran buff." },
            { text_item_concoction_3_desc, "Grants Storyteller buff." },
            { text_item_concoction_4_desc, "Grants Stoned buff." },
            { text_item_concoction_5_desc, "Grants Sailor buff." },
            { text_item_concoction_6_desc, "Grants Dwarf buff." },
            { text_item_concoction_7_desc, "Grants Tamer buff." },
            { text_item_concoction_8_desc, "Grants Arcanist buff." },
            { text_item_concoction_9_desc, "Grants Iron Skin buff." },
            { text_item_concoction_10_desc, "Increase by 100 Warrior base Max Hp" },
            { text_item_concoction_11_desc, "Increase by 2 Warrior base Attack" },
            { text_item_concoction_12_desc, "Increase by 2 Warrior base Defense" },

            { text_item_fish_group_life_desc, "Increase Max Hp for Warrior Job by 100%." },
            { text_item_fish_group_predator_desc, "Increase Atk for Warrior Job by 50%." },
            { text_item_fish_group_guardian_desc, "Increase Def for Warrior Job by 30%." },
            { text_item_fish_group_dart_desc, "Increase Atk Speed for Warrior Job by 20%." },
            { text_item_fish_group_sharp_desc, "Increase Crit rate for Warrior Job by 20%" },
            { text_item_fish_group_piercing_desc, "Increase Crit damage for Warrior Job by 20%" },
            { text_item_fish_group_golden_desc, "Increase Luck for Warrior Job by 10%" },
            { text_item_fish_group_elder_desc, "Increase Exp gain for Warrior Job by 20%" },
            { text_item_fish_group_quick_desc, "Increase Movement speed for Warrior Job by 20%" },

            { text_mage_spell_fireball_desc, "Deals {0} of max hp to a single enemy" },
            { text_mage_spell_explosion_desc, "Deals {0} of max hp to all enemies hit in a {1} unit radius" },
            { text_mage_spell_chillwind_desc, "Deals {0} of max hp to all enemies hit in a {1} unit radius, enemies hit by the spell will take {2} more damage from spell for a short time" },
            { text_mage_spell_poisongas_desc, "Deals {0} of max hp to all enemies hit in a {1} unit radius, enemies hit by the spell will give the player {2} lifesteal for a short time" },
            { text_mage_spell_zap_desc, "Deals {0} of max hp to a single enemy, the spell will bounce {1} times if another enemy is close enough" },

            { text_necromancer_ritual_summon_desc, "While advancing through the stages, the Warrior will summon Undeads to help him" },
            { text_necromancer_ritual_arise_desc, "When an Undead is summoned, there is a chance it will be stronger than average undeads" },
            { text_necromancer_ritual_afterlife_desc, "When an undead dies, it will create an explosion around it, damaging the enemies" },
            { text_necromancer_ritual_ade_desc, "Permanently increase your maximum horde counter by 1" },



            { text_shopitem_cardpack01_desc, "A humble card pack containing 3 cards." },
            { text_shopitem_cardpack02_desc, "A humble card pack containing 5 cards. An Uncommon card is guaranteed." },
            { text_shopitem_cardpack03_desc, "A fine card pack containing 8 cards. An Uncommon card is guaranteed." },
            { text_shopitem_cardpack04_desc, "A fine card pack containing 12 cards. An Uncommon card is guaranteed." },
            { text_shopitem_cardpack05_desc, "An elite card pack containing 15 cards. A Rare card is guaranteed." },

                                       
            { text_shopitem_job_farmer_desc, "Unlock the Farmer Class" },
            { text_shopitem_job_bard_desc, "Unlock the Bard Class" },


            { text_shopitem_bait_normalmorning_desc, "Contains 10 Earthworm Baits<br>Effect:{0}" },
            { text_shopitem_bait_greatmorning_desc, "Contains 10 Earthworm Baits Plus<br>Effect:{0}" },
            { text_shopitem_bait_maxmorning_desc, "Contains 10 Earthworm Baits Max<br>Effect:{0}" },
            { text_shopitem_bait_normalafternoon_desc, "Contains 10 Cricket Baits<br>Effect:{0}" },
            { text_shopitem_bait_greatafternoon_desc, "Contains 10 Cricket Baits Plus<br>Effect:{0}" },
            { text_shopitem_bait_maxafternoon_desc, "Contains 10 Cricket Baits Max<br>Effect:{0}" },
            { text_shopitem_bait_normalnight_desc, "Contains 10 Shrimp Baits<br>Effect:{0}" },
            { text_shopitem_bait_greatnight_desc, "Contains 10 Shrimp Baits Plus<br>Effect:{0}" },
            { text_shopitem_bait_maxnight_desc, "Contains 10 Shrimp Baits Max<br>Effect:{0}" },



            { text_companion_0_desc, "Increase Warrior Max Hp by {0}%" },
            { text_companion_1_desc, "Increase Warrior Atk by {0}%" },
            { text_companion_2_desc, "Increase Warrior Def by {0}%" },
            { text_companion_3_desc, "Increase Warrior Atk speed by {0}%" },
            { text_companion_4_desc, "Increase Warrior Crit damage by {0}%" },
        };


        CreditsTextDictionary = new Dictionary<string, string>()
        {
            { text_credits_me, "<align=\"center\">A game by Matteo Troilo aka Chotto Inc<br></align>" },

            { text_credits_localization, "<align=\"center\">Localization<br>----------------------------</align><br><align=\"center\">Italian<br>-<br>Translation & Review<br>Veronica Faroldi<br></align>" },

            { text_credits_art, 
                "<align=\"center\">Art<br>----------------------------</align><br>Character assets by Zerie (https://zerie.itch.io/tiny-rpg-character-asset-pack). " +
                "Some of this work has been modified from its original state<br><br>Fish sprites by happypotato100 (https://happypotato100.itch.io/fishing-icon-pack) licensed under an Attribution License" 
            },

            { text_credits_sound,
                "<align=\"center\">Sound<br>----------------------------</align><br>\"confirm 1.wav\" (used for UI click sounds) by JDWasabi (https://jdwasabi.itch.io/8-bit-16-bit-sound-effects-pack) " +
                "licensed under an Attribution License<br><br>" +
                "\"The Journey.wav\" (used in trailers) original music by Marllon Silva/xDeviruchi (https://www.youtube.com/xdeviruchi) " +
                "licensed under an Attribution License<br><br>" +
                "\"Magical Fantasy Music Pack\" (Bard songs) original album by AlkaCrab (https://alkakrab.itch.io/free-12-magical-fantasy-tracks-music-pack-no-ai) licensed with CC0"
            },

            { text_credits_font,
                "<align=\"center\">Fonts<br>----------------------------</align><br>Font \"m6x11plus.ttf\" by Daniel Linssen (https://managore.itch.io/m6x11) licensed under an Attribution License"
            },
        };


        HelpTextDictionary = new Dictionary<string, string>()
        {
            { text_help_warrior,
                "<align=\"center\">Warrior<br>----------------------------</align><br>Defeat the enemies to advance through the stages. Warrior stats can be increased by doing other jobs too.<br><br>" +
                "Enemies may drop collectable cards.<br>Exceeding cards can be dismantled into Bits, or convert 6 of them into a random one."
            },

            { text_help_miner,
                "<align=\"center\">Miner<br>----------------------------</align><br>Break rocks to collect ores and upgrade your weapon's level to increase the damage dealt to enemies.<br" +
                ">Rocks have a chance to drop at most one rarity higher than theirs, and not all rocks will drop ores."
            },

            { text_help_fisher,
                "<align=\"center\">Fisher<br>----------------------------</align><br>Catch all fishes of a group to unlock a bonus.<br>Extra fishes will be automatically converted into Bits.<br>" +
                "Different fishes can be found at different times of the day, like morning, afternoon, and evening.<br>Changing job will interrupt the current fishing session and reset the timer of the hook."
            },

            { text_help_blacksmith,
                "<align=\"center\">Blacksmith<br>----------------------------</align><br>Refine ores into metals to upgrade your equipment level. They will increase the Warrior stats.<br>" +
                "If there is not enough amount of the selected ore to refine, the Blacksmith will idle."
            },

            { text_help_farmer,
                "<align=\"center\">Farmer<br>----------------------------</align><br>Grow crops to attract wild creatures, and after they eat, there is a chance they become your companions.<br>" +
                "A new seed will be unlocked every 5 level of the Agronomy stat.<br>Companions can be equipped to help the Warrior through the stages.<br><br>" +
                "Extra befriended companions of the same species will be automatically converted into Bits.<br><br>Your companions may appear while farming and chill with you."
            },

            { text_help_mage,
                "<align=\"center\">Mage<br>----------------------------</align><br>The Mage learns spells to autocast them when advancing through the stages.<br><br>" +
                "Spells can be equipped through slots.<br>Spell can be ranked up to increase their effectivness.<br>" +
                "Every 5 levels of Scholar ability a new spell is unlocked, and every 5 levels of Proficiency ability a new slot to equip spells will be unlocked."
            },

            { text_help_alchemist,
                "<align=\"center\">Alchemist<br>----------------------------</align><br>The Alchemist crafts powerful items following repices.<br><br>" +
                "Every 5 levels of Research ability a new recipe will be unlocked,some can also  be purchased from the shop<br>" +
                "You can find them in the Inventory once they're crafted."
            },

            { text_help_necromancer,
                "<align=\"center\">Necromancer<br>----------------------------</align><br>Max out your Necromancer abilities to unlock new permanent rituals.<br>" +
                "The Necromancer trains by making is summons fight themselves, increasing the Aptitude ability will also increase the amount of summons that will fight.<br>" +
                "All Necromancer abilities will increase the summon statistics, except for aptitude and luck, that will also increase the amount of experience gained by the Necromancer."
            },

            { text_help_bard,
                "<align=\"center\">Bard<br>----------------------------</align><br>The songs played by the Bard have no direct impact on the gameplay, " +
                "you can chose whether play in random order or looping a single song.<br>" +
                "When switching job, some abilities will have a 10% boost for half of the time the Bard played."
            },



            { text_description_warrior,
                "Description:<br>Defeat the enemies to advance through the stages. Enemies may drop collectable cards"
            },

            { text_description_miner,
                "Description:<br>Collect ores to upgrade your weapon level. It will increase the damage dealt to enemies"
            },

            { text_description_miner_weapon,
                "Weapon max level: {0}"
            },


            { text_description_blacksmith,
                "Description:<br>Refine ores into metals to upgrade your equipment level. They will increase the Warrior stats"
            },

            { text_description_blacksmith_equipment,
                "Equipment max level: {0}"
            },

            { text_description_fisher,
                "Description:<br>Fish into the lake and catch all fishes of a group to unlock a bonus"
            },

            { text_description_farmer_crops,
                "Description:<br>Select crops to grow. Special creatures will be attracted by them, after they eat, there is a chance they become your companions"
            },

            { text_description_farmer_companions,
                "Companions will help you defeating monsters, equip up to 3 companions at the same time."
            },

            { text_description_mage,
                "Description: Learns spell to autocast while advancing through the stages"
            },

            { text_description_mage_slots,
                "Spells can be equipped in empty slots. A new slot is unlocked every 5 levels of Proficiency ability"
            },

            { text_description_alchemist,
                "Description:<br>Craft powerful items reported int the recipes"
            },

            { text_description_necromancer,
                "Description:<br>Max out your Necromancer abilities to unlock new permanent rituals"
            },

            { text_description_bard,
                "Description:<br>Play songs to chill a bit. Get a 10% boost on some stats for half the time of played songs"
            },
        };
    }
}
