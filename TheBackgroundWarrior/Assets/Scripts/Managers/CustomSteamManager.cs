using Steamworks;
using System;
using UnityEngine;

public class CustomSteamManager : SteamManager
{
    public event Action OnCheckedSettings;

    public bool CheckedSettings { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        if (Initialized)
        {
            OnInitialize();
        }
    }

    protected override void OnDestroy()
    {
        if (!SettingsManager.Instance.IsSteamPlatform) return;

        base.OnDestroy();
    }

    protected override void OnEnable()
    {
        if (!SettingsManager.Instance.IsSteamPlatform) return;

        base.OnEnable();
    }

    protected override void Update()
    {
        if (!SettingsManager.Instance.IsSteamPlatform) return;

        // once the initializer manager is init check if game has client language
        if (!CheckedSettings && InitializerManager.Instance.HasCheckFiles)
        {
            // if game language is the default one means the first open or the player set it to english, so check if game has client language
            CheckDefaultLang();
        }

        base.Update();
    }

    public void CheckDefaultLang()
    {
        if (SettingsManager.Instance.CurrentLanguage == UtilsGeneral.Language.Eng)
        {
            // get if the first check has been already done
            bool firstLangCheck = PlayerPrefs.GetInt("KEY_FIRST_LANG_CHECK", 0) != 0;

            if(!firstLangCheck)
            {
                SettingsManager.Instance.CheckSteamLanguage(SteamApps.GetCurrentGameLanguage());

                PlayerPrefs.SetInt("KEY_FIRST_LANG_CHECK", 1);
            }
        }

        UtilsGeneral.RefreshAll();

        // only check once
        CheckedSettings = true;
        OnCheckedSettings?.Invoke();
    }

    private void OnInitialize()
    {
        
    }

    protected virtual string GetUserInfo()
    {
        string id = SteamUser.GetSteamID().ToString();
        string lev = SteamUser.GetPlayerSteamLevel().ToString();

        return string.Format("name: {0}, lv: {1}", id, lev);
    }
}
