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

    private const string ITEM_STAT_CODE = "80";

    // ------- THREE FOUR ------ //
    private const string LEVEL_CODE = "00";
    private const string AVAILABLE_POINTS_CODE = "01";

    private const string COPPERORE_POINTS_CODE = "00";
    private const string IRONORE_POINTS_CODE = "01";
    private const string BRONZEORE_POINTS_CODE = "02";
    private const string SILVERORE_POINTS_CODE = "03";
    private const string GOLDORE_POINTS_CODE = "04";

    private const string COPPER_POINTS_CODE = "30";
    private const string IRON_POINTS_CODE = "31";
    private const string BRONZE_POINTS_CODE = "32";
    private const string SILVER_POINTS_CODE = "33";
    private const string GOLD_POINTS_CODE = "34";

    // ------- FIVE SIX SEVEN EIGHT ------ //
    //private const string QUANTITY_CODE = "00";



    [SerializeField] TMP_InputField inputCode;

    public void OnButtonRedeem()
    {
        AudioManager.Instance.PlayClickUI();

        bool redeemSuccess = AnalyzeRedeem(inputCode.text);

        /*
        switch (inputCode.text)
        {
            default: Debug.Log("Redeem denied"); break;

            case UtilsShop.REDEEM_ERIS_CODE:

                if (!ShopManager.Instance.HasRedeemedErisCode)
                {
                    redeemSuccess = true;
                    ShopManager.Instance.SetRedeemCode(UtilsShop.ID_REDEEM_ERIS_CODE);
                    ShopManager.Instance.SaveShopData();
                    Debug.Log("Redeem success: " + UtilsShop.REDEEM_ERIS_CODE);
                }
                break;
        }
        */

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
                return HandleStatRedeem(code);

            case ITEM_STAT_CODE:
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
        //Debug.Log("Quantity added: " + quantity);


        IBasePlayerData playerData = null;

        switch (onetwo)
        {
            default: return false;
            case FIGHT_STAT_CODE: playerData = PlayerManager.Instance.PlayerFightData; break;
            case MINER_STAT_CODE: playerData = PlayerManager.Instance.PlayerMinerData; break;
            case BLACKSMITH_STAT_CODE: playerData = PlayerManager.Instance.PlayerBlacksmithData; break;
            case FISHER_STAT_CODE: playerData = PlayerManager.Instance.PlayerFisherData; break;
            case FARMER_STAT_CODE: playerData = PlayerManager.Instance.PlayerFarmerData; break;
        }

        if(playerData != null)
        {
            switch (threefour)
            {
                default: return false;
                case LEVEL_CODE: playerData.AddLevel(quantity); break;
                case AVAILABLE_POINTS_CODE: playerData.AddStatPoints(quantity); break;
            }
            return true;
        }
        
        return false;
    }

    private bool HandleItemRedeem(string code)
    {
        string threefour = "" + code[2] + code[3];

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
            case COPPERORE_POINTS_CODE: PlayerManager.Instance.Inventory.AddItem(0, quantity); break;
            case IRONORE_POINTS_CODE: PlayerManager.Instance.Inventory.AddItem(1, quantity); break;
            case BRONZEORE_POINTS_CODE: PlayerManager.Instance.Inventory.AddItem(2, quantity); break;
            case SILVERORE_POINTS_CODE: PlayerManager.Instance.Inventory.AddItem(3, quantity); break;
            case GOLDORE_POINTS_CODE: PlayerManager.Instance.Inventory.AddItem(4, quantity); break;

            case COPPER_POINTS_CODE: PlayerManager.Instance.Inventory.AddItem(150, quantity); break;
            case IRON_POINTS_CODE: PlayerManager.Instance.Inventory.AddItem(151, quantity); break;
            case BRONZE_POINTS_CODE: PlayerManager.Instance.Inventory.AddItem(152, quantity); break;
            case SILVER_POINTS_CODE: PlayerManager.Instance.Inventory.AddItem(153, quantity); break;
            case GOLD_POINTS_CODE: PlayerManager.Instance.Inventory.AddItem(154, quantity); break;
        }

        return true;
    }
}
