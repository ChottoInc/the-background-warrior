using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIShopPanelRedeem : MonoBehaviour
{
    private const int CODE_LENGTH = 10;

    // ------- ONE TWO ------ //
    private const string FIGHT_STAT_CODE = "00";
    private const string MINER_STAT_CODE = "01";
    private const string BLACKSMITH_STAT_CODE = "02";
    private const string FISHER_STAT_CODE = "03";
    private const string FARMER_STAT_CODE = "04";
    private const string MAGE_STAT_CODE = "05";
    private const string NECROMANCER_STAT_CODE = "07";

    private const string ITEM_ADD_CODE = "80";

    // ------- THREE FOUR ------ //
    private const string LEVEL_CODE = "00";
    private const string AVAILABLE_POINTS_CODE = "01";

    private const string COPPERORE_CODE = "00";
    private const string IRONORE_CODE = "01";
    private const string BRONZEORE_CODE = "02";
    private const string SILVERORE_CODE = "03";
    private const string GOLDORE_CODE = "04";

    private const string COPPER_CODE = "30";
    private const string IRON_CODE = "31";
    private const string BRONZE_CODE = "32";
    private const string SILVER_CODE = "33";
    private const string GOLD_CODE = "34";

    private const string CARD_MONSTERSTAMPEDE_CODE = "80";
    private const string CARD_ERISPAGE1_CODE = "83";
    private const string CARD_ERISPAGE2_CODE = "84";
    private const string CARD_ERISPAGE3_CODE = "85";
    private const string CARD_ERISPAGE4_CODE = "86";
    private const string CARD_ERISPAGE5_CODE = "87";

    private const string FISH_GROUP_POINTS_CODE = "FH";

    // ------- FIVE SIX ------ //
    private const string FISH_GROUP_LIFE_CODE = "AA";
    private const string FISH_GROUP_PREDATOR_CODE = "AB";
    private const string FISH_GROUP_GUARDIAN_CODE = "AC";
    private const string FISH_GROUP_DART_CODE = "AD";



    [SerializeField] TMP_InputField inputCode;

    public void OnButtonRedeem()
    {
        AudioManager.Instance.PlayClickUI();

        bool redeemSuccess = AnalyzeRedeem(inputCode.text);

        if (!redeemSuccess)
        {
            //Debug.Log("Redeem denied");
            // little ui animation of button shaking if not success?
        }
        else
        {
            //Debug.Log("Redeem successful");
            PlayerManager.Instance.SaveAll();
        }
    }

    private bool AnalyzeRedeem(string code)
    {
        if (code.Length != CODE_LENGTH) return false;

        // first two digits is class or item
        // from 80 to 99 are items or anything needed
        // 3rd and 4th are stat or id item to add
        // 5th and 6th quantity

        string onetwo = "" + code[0] + code[1];
        switch (onetwo)
        {
            case FIGHT_STAT_CODE:
            case MINER_STAT_CODE:
            case BLACKSMITH_STAT_CODE:
            case FISHER_STAT_CODE:
            case FARMER_STAT_CODE:
            case MAGE_STAT_CODE:
            case NECROMANCER_STAT_CODE:
                return HandleStatRedeem(code);

            case ITEM_ADD_CODE:
                return HandleItemRedeem(code);
        }


        return false;
    }

    private bool HandleStatRedeem(string code)
    {
        string onetwo = "" + code[0] + code[1];
        string threefour = "" + code[2] + code[3];

        // get quantity
        string remains = string.Empty;
        for (int i = 4; i < code.Length; i++)
        {
            remains += code[i];
        }

        int quantity = int.Parse(remains);
        int maxLevel;
        //Debug.Log("Quantity added: " + quantity);


        IBasePlayerData playerData = null;

        switch (onetwo)
        {
            default: return false;
            case FIGHT_STAT_CODE: playerData = PlayerManager.Instance.PlayerFightData; maxLevel = UtilsWarrior.MAX_LEVEL_WARRIOR; break;
            case MINER_STAT_CODE: playerData = PlayerManager.Instance.PlayerMinerData; maxLevel = UtilsMiner.MAX_LEVEL_MINER; break;
            case BLACKSMITH_STAT_CODE: playerData = PlayerManager.Instance.PlayerBlacksmithData; maxLevel = UtilsBlacksmith.MAX_LEVEL_BLACKSMITH; break;
            case FISHER_STAT_CODE: playerData = PlayerManager.Instance.PlayerFisherData; maxLevel = UtilsFisher.MAX_LEVEL_FISHER; break;
            case FARMER_STAT_CODE: playerData = PlayerManager.Instance.PlayerFarmerData; maxLevel = UtilsFarmer.MAX_LEVEL_FARMER; break;
            case MAGE_STAT_CODE: playerData = PlayerManager.Instance.PlayerMageData; maxLevel = UtilsMage.MAX_LEVEL_MAGE; break;
            case NECROMANCER_STAT_CODE: playerData = PlayerManager.Instance.PlayerNecromancerData; maxLevel = UtilsNecromancer.MAX_LEVEL_NECROMANCER; break;
        }

        if(playerData != null)
        {
            switch (threefour)
            {
                default: return false;
                case LEVEL_CODE: playerData.AddLevel(quantity, maxLevel); break;
                case AVAILABLE_POINTS_CODE: playerData.AddStatPoints(quantity); break;
            }
            return true;
        }
        
        return false;
    }

    private bool HandleItemRedeem(string code)
    {
        string threefour = "" + code[2] + code[3];

        if (!threefour.Equals(FISH_GROUP_POINTS_CODE))  // TODO: ADD HERE OTHER CHECKS
        {
            // get quantity
            string remains = string.Empty;
            for (int i = 4; i < code.Length; i++)
            {
                remains += code[i];
            }

            int quantity = int.Parse(remains);
            //Debug.Log("Quantity added: " + quantity);

            switch (threefour)
            {
                default: return false;
                case COPPERORE_CODE: PlayerManager.Instance.Inventory.AddItem(0, quantity); break;
                case IRONORE_CODE: PlayerManager.Instance.Inventory.AddItem(1, quantity); break;
                case BRONZEORE_CODE: PlayerManager.Instance.Inventory.AddItem(2, quantity); break;
                case SILVERORE_CODE: PlayerManager.Instance.Inventory.AddItem(3, quantity); break;
                case GOLDORE_CODE: PlayerManager.Instance.Inventory.AddItem(4, quantity); break;

                case COPPER_CODE: PlayerManager.Instance.Inventory.AddItem(150, quantity); break;
                case IRON_CODE: PlayerManager.Instance.Inventory.AddItem(151, quantity); break;
                case BRONZE_CODE: PlayerManager.Instance.Inventory.AddItem(152, quantity); break;
                case SILVER_CODE: PlayerManager.Instance.Inventory.AddItem(153, quantity); break;
                case GOLD_CODE: PlayerManager.Instance.Inventory.AddItem(154, quantity); break;

                case CARD_MONSTERSTAMPEDE_CODE: PlayerManager.Instance.Inventory.AddItem(80, quantity);
                    if (!PlayerManager.Instance.PlayerJobsData.IsMageUnlocked)
                    {
                        PlayerManager.Instance.PlayerJobsData.AddAvailableJob(UtilsPlayer.PlayerJob.Mage);
                    }
                    break;
                case CARD_ERISPAGE1_CODE: PlayerManager.Instance.Inventory.AddItem(83, quantity); break;
                case CARD_ERISPAGE2_CODE: PlayerManager.Instance.Inventory.AddItem(84, quantity); break;
                case CARD_ERISPAGE3_CODE: PlayerManager.Instance.Inventory.AddItem(85, quantity); break;
                case CARD_ERISPAGE4_CODE: PlayerManager.Instance.Inventory.AddItem(86, quantity); break;
                case CARD_ERISPAGE5_CODE: PlayerManager.Instance.Inventory.AddItem(87, quantity); break;
            }
        }
        else
        {
            switch (threefour)
            {
                default: return false;
                case FISH_GROUP_POINTS_CODE: return HandleFishGroupRedeem(code);
            }
        }

        return true;
    }

    private bool HandleFishGroupRedeem(string code)
    {
        string fivesix = "" + code[4] + code[5];
        List<int> fishes = new List<int>();
        switch (fivesix)
        {
            default: return false;
            case FISH_GROUP_LIFE_CODE: fishes.AddRange(new int[] { 200, 201, 217, 219, 226, 227, 232, 203 }); break;
            case FISH_GROUP_PREDATOR_CODE: fishes.AddRange(new int[] { 204, 206, 225, 228, 230, 241, 243, 207 }); break;
            case FISH_GROUP_GUARDIAN_CODE: fishes.AddRange(new int[] { 212, 213, 214, 215, 216, 229, 276 }); break;
            case FISH_GROUP_DART_CODE: fishes.AddRange(new int[] { 208, 221, 236, 251, 253, 266, 277 }); break;
        }

        Debug.Log("getting: " + fishes.ToString());
        PlayerManager.Instance.Inventory.AddItems(fishes);
        PlayerManager.Instance.PlayerFisherData.FillFishGroupsSeriesCompletion();
        return true;
    }
}
