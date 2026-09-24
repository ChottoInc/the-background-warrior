using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UITabJobMiner : UITabWindow
{
    private int transparencyAmount = Shader.PropertyToID("_Transparency");



    [SerializeField] UITabPlayerJob panelJob;

    [Space(10)]
    [SerializeField] GameObject requirementPrefab;
    [SerializeField] Transform container;

    private List<GameObject> requirementObjs;

    [Header("Weapon")]
    [SerializeField] TMP_Text _textMaxLevel;
    [SerializeField] Image imageSword;
    [SerializeField] TMP_Text textLevel;
    [SerializeField] TMP_Text textStats;
    [SerializeField] TMP_Text textNextLevel;
    [SerializeField] TMP_Text textNextStats;

    [Space(10)]
    [SerializeField] float timerChangeTransparency = 0.75f;
    [SerializeField] float timerChangeTransparencyIdle = 0.5f;

    private int lastWeaponLevel;

    [Header("Buttons")]
    [SerializeField] Button buttonLevelUp;

    private bool isAnimatingLevelUp;

    private List<ItemGroup> requirements;


    private Material matImageWeapon;

    private bool isInitialized;


    private PlayerMiner player;


    public override void Open()
    {
        base.Open();

        InitializedIfNeeded();

        if(player == null)
        {
            player = FindFirstObjectByType<PlayerMiner>();
        }

        PlayerMinerData data;

        if (player != null)
        {
            data = player.PlayerData;
        }
        else
        {
            data = PlayerManager.Instance.PlayerMinerData;
        }

        lastWeaponLevel = data.WeaponLevel;

        _textMaxLevel.text = string.Format(UtilsText.AllText[UtilsText.text_description_miner_weapon], UtilsMiner.MINER_WEAPON_MAX_LEVEL);

        UpdateMinerSwordUI();

        panelJob.ChangeCurrentTab(this, UtilsPlayer.PlayerJob.Miner);

        RefreshRequirements();
    }

    private void InitializedIfNeeded()
    {
        if (isInitialized) return;

        // copy material image ui
        matImageWeapon = new Material(imageSword.material);
        imageSword.material = matImageWeapon;

        isInitialized = true;
    }

    public void OnButtonBack()
    {
        AudioManager.Instance.PlayClickUI();

        Close();
        panelJob.ChangeCurrentTab(null, UtilsPlayer.PlayerJob.None);
    }

    private void RefreshRequirements()
    {
        // clear list and refill requirements updated to inventory numbers
        requirementObjs = ClearList(requirementObjs);
        FillRequirements();

        // check max level
        buttonLevelUp.interactable = CheckEnableLevelUp();

        // if can level up show requirements
        if(!IsWeaponMaxLevel())
            FillRequirementsUI();
    }

    private List<GameObject> ClearList(List<GameObject> list)
    {
        if(list == null)
            list = new List<GameObject>();

        foreach (var item in list)
        {
            Destroy(item);
        }

        list.Clear();
        return list;
    }

    private void FillRequirements()
    {
        requirementObjs = new List<GameObject>();

        requirements = UtilsMiner.GetRequirementsForMinerWeaponLevel(PlayerManager.Instance.PlayerMinerData.WeaponLevel + 1);
    }

    private void FillRequirementsUI()
    {
        for (int i = 0; i < requirements.Count; i++)
        {
            GameObject prefab = Instantiate(requirementPrefab, transform.position, Quaternion.identity);
            prefab.transform.SetParent(container);

            prefab.transform.localScale = new Vector3(1, 1, 1);
            prefab.SetActive(true);

            if (prefab.TryGetComponent(out UIMinerWeaponRequirement obj))
            {
                obj.Setup(requirements[i]);
            }
            requirementObjs.Add(prefab);
        }
    }


    /// <summary>
    /// If not all requirements are met returns false
    /// </summary>
    private bool CheckEnableLevelUp()
    {
        foreach (var requirement in requirements)
        {
            if (!PlayerManager.Instance.Inventory.HasEnough(requirement.IdItem, requirement.Quantity))
                return false;
        }

        if (IsWeaponMaxLevel()) return false;

        return true;
    }

    private bool IsWeaponMaxLevel()
    {
        return lastWeaponLevel >= UtilsMiner.MINER_WEAPON_MAX_LEVEL;
    }

    /// <summary>
    /// Change sprite of the sword every N levels
    /// </summary>
    private void UpdateMinerSwordUI()
    {
        PlayerMinerData data;

        if(player != null)
        {
            data = player.PlayerData;
        }
        else
        {
            data = PlayerManager.Instance.PlayerMinerData;
        }

        int weaponLevel = data.WeaponLevel;

        // check for different sprite
        bool isDifferentLevel = lastWeaponLevel != weaponLevel;

        Sprite sprite = UtilsMiner.GetWeaponSpriteByLevel(weaponLevel);
        if(sprite == null)
        {
            sprite = UtilsMiner.GetWeaponSpriteByLevel(1);
        }

        
        if(sprite == null)
        {
            Debug.Log("sprite is null");
        }

        if (data == null)
        {
            Debug.Log("data is null");
        }

        textLevel.text = string.Format(UtilsText.AllText[UtilsText.text_job_miner_weapon_currentlevel], weaponLevel);

        // Multiply by 100 to get percentage, and minus 100 to remove base multiplier
        float multiplier = UtilsMiner.GetMinerWeaponMultiplier(data.WeaponLevel);
        textStats.text = string.Format(UtilsText.AllText[UtilsText.text_job_miner_weapon_currentstats], UtilsGeneral.FormatDecimal((multiplier * 100f) - 100f));
        //Debug.Log("dmg: " + multiplier);

        if (!IsWeaponMaxLevel())
        {
            textNextLevel.text = string.Format(UtilsText.AllText[UtilsText.text_job_miner_weapon_nextlevel]);

            // Multiply by 100 to get percentage, and minus 100 to remove base multiplier
            float multiplierNextLevel = UtilsMiner.GetMinerWeaponMultiplier(data.WeaponLevel + 1);
            textNextStats.text = string.Format(UtilsText.AllText[UtilsText.text_job_miner_weapon_currentstats], UtilsGeneral.FormatDecimal((multiplierNextLevel * 100f) - 100f));

            textNextLevel.gameObject.SetActive(true);
            textNextStats.gameObject.SetActive(true);
        }
        else
        {
            textNextLevel.gameObject.SetActive(false);
            textNextStats.gameObject.SetActive(false);
        }

        // Check if need change
        if (!isDifferentLevel)
        {
            imageSword.sprite = sprite;
        }
        else
        {
            // check if Animation is enabled
            if (SettingsManager.Instance.AreLevelUpEquipmentOn)
            {
                StartCoroutine(CoChangeWeaponSprite(sprite));
            }
            else
            {
                imageSword.sprite = sprite;
            }
        }

        lastWeaponLevel = weaponLevel;
    }


    private IEnumerator CoChangeWeaponSprite(Sprite newSprite)
    {
        isAnimatingLevelUp = true;

        float elapsedTime = 0;

        float lerpedTransparency = 0;

        // lerp from 0 to 1
        while (elapsedTime < timerChangeTransparency)
        {
            elapsedTime += Time.unscaledDeltaTime;

            lerpedTransparency = Mathf.Clamp01(elapsedTime / timerChangeTransparency);

            matImageWeapon.SetFloat(transparencyAmount, lerpedTransparency);

            yield return null;
        }

        // change sprite
        imageSword.sprite = newSprite;

        // idle
        yield return new WaitForSecondsRealtime(timerChangeTransparencyIdle);

        elapsedTime = 0;

        lerpedTransparency = 1;

        // lerp back from 1 to 0
        while (elapsedTime < timerChangeTransparency)
        {
            elapsedTime += Time.unscaledDeltaTime;

            lerpedTransparency = Mathf.Lerp(1f, 0f, elapsedTime / timerChangeTransparency);

            matImageWeapon.SetFloat(transparencyAmount, lerpedTransparency);

            yield return null;
        }

        isAnimatingLevelUp = false;
    }



    public void OnButtonGather()
    {
        if (player != null)
        {
            panelJob.OnButtonClose(false);
        }

        LastSceneSettings settings = new LastSceneSettings();
        settings.lastSceneName = "MinerScene";
        settings.lastSceneType = SceneLoaderManager.SceneType.Miner;

        SceneLoaderManager.Instance.LoadScene(settings);
    }

    public void OnButtonLevelUp()
    {
        // check if animations are on and animating right now, so you can't interrupt the animation and bug it
        if (SettingsManager.Instance.AreLevelUpEquipmentOn && isAnimatingLevelUp) return;

        // remove requirements from inventory
        foreach (var requirement in requirements)
        {
            PlayerManager.Instance.Inventory.RemoveItem(requirement.IdItem, requirement.Quantity);
        }

        // save inventory
        PlayerManager.Instance.SaveInventoryData();

        // add level
        if(player == null)
        {
            // update directly from save if not in miner scene
            PlayerManager.Instance.PlayerMinerData.AddMinerWeaponLevel(1);
        }
        else
        {
            // or update from temp data if in miner scene, and update from there
            player.AddMinerWeaponLevel(1);
            PlayerManager.Instance.UpdateMinerData(player.PlayerData);
        }

        // save miner data
        PlayerManager.Instance.SaveMinerData();

        // refresh
        RefreshRequirements();

        // update ui
        UpdateMinerSwordUI();
    }
}
