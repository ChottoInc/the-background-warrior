using System.Collections.Generic;
using System.Globalization;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerJobMinerSO))]
public class EditorJobMinerData : Editor
{
    private const int ID_POWER = 0;
    private const int ID_SMASHSPEED = 1;
    private const int ID_SHOCKWAVE = 2;
    private const int ID_LUCK = 3;

    private const int ID_BASE_EXP = 5;
    private const int ID_EXPO_EXP = 6;
    private const int ID_FLAT_EXP = 7;

    private const int ID_WEAPON_LINEAR = 9;
    private const int ID_WEAPON_QUADRATIC = 10;



    private const int GAIN_PER_LEVEL = 1;
    private const int MAX_LEVEL = 2;

    private const int COL_LEVEL_EXP = 1;

    private const int COL_WEAPON_GROWTH = 1;

    PlayerJobMinerSO m_Script;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        m_Script = (PlayerJobMinerSO)target;

        EditorGUILayout.Space();
        if (GUILayout.Button("Load Data From SO"))
        {
            ReadData();
        }
    }

    private void ReadData()
    {
#if UNITY_EDITOR

        CultureInfo en_us = CultureInfo.GetCultureInfo("en-US");

        // get filepath from so
        List<string> datas = UtilsGeneral.GetFileStrings(m_Script.DataPath);

        // first read warrior statistics
        for (int i = 0; i < 4; i++)
        {
            string[] parts = datas[i].Split(",", System.StringSplitOptions.None);

            float gain = float.Parse(parts[GAIN_PER_LEVEL], en_us);
            int maxLevel = int.Parse(parts[MAX_LEVEL], en_us);

            switch (i)
            {
                case ID_POWER:
                    m_Script.SetPerLevelGainPower(gain);
                    m_Script.SetMaxLevelPower(maxLevel);
                    break;

                case ID_SMASHSPEED:
                    m_Script.SetPerLevelGainSmashSpeed(gain);
                    m_Script.SetMaxLevelSmashSpeed(maxLevel);
                    break;

                case ID_SHOCKWAVE:
                    m_Script.SetPerLevelGainShockwave(gain);
                    m_Script.SetMaxLevelShockwave(maxLevel);
                    break;

                case ID_LUCK:
                    m_Script.SetPerLevelGainLuck(gain);
                    m_Script.SetMaxLevelLuck(maxLevel);
                    break;
            }
        }

        for (int i = 5; i < 8; i++)
        {
            string[] parts = datas[i].Split(",", System.StringSplitOptions.None);

            float value = float.Parse(parts[COL_LEVEL_EXP], en_us);

            switch (i)
            {
                case ID_BASE_EXP: m_Script.SetBaseExpGrowth(value); break;
                case ID_EXPO_EXP: m_Script.SetExpoExpGrowth(value); break;
                case ID_FLAT_EXP: m_Script.SetFlatExpGrowth(value); break;
            }
        }

        for (int i = 9; i < 11; i++)
        {
            string[] parts = datas[i].Split(",", System.StringSplitOptions.None);

            float value = float.Parse(parts[COL_WEAPON_GROWTH], en_us);

            switch (i)
            {
                case ID_WEAPON_LINEAR: m_Script.SetWeaponLinearGrowth(value); break;
                case ID_WEAPON_QUADRATIC: m_Script.SetWeaponQuadraticGrowth(value); break;
            }
        }

        EditorUtility.SetDirty(m_Script);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

#endif
    }
}
