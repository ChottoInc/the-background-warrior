using System.Collections.Generic;
using System.Globalization;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerJobWarriorSO))]
public class EditorJobWarriorData : Editor
{
    private const int ID_MAX_HP = 0;
    private const int ID_ATK = 1;
    private const int ID_DEF = 2;
    private const int ID_ATKSPD = 3;
    private const int ID_CRITRATE = 4;
    private const int ID_CRITDMG = 5;
    private const int ID_LUCK = 6;

    private const int ID_BASE_EXP = 8;
    private const int ID_EXPO_EXP = 9;
    private const int ID_FLAT_EXP = 10;



    private const int GAIN_PER_LEVEL = 1;
    private const int MAX_LEVEL = 2;

    private const int COL_LEVEL_EXP = 1;

    PlayerJobWarriorSO m_Script;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        m_Script = (PlayerJobWarriorSO)target;

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
        for (int i = 0; i < 7; i++)
        {
            string[] parts = datas[i].Split(",", System.StringSplitOptions.None);

            float gain = float.Parse(parts[GAIN_PER_LEVEL], en_us);
            int maxLevel = int.Parse(parts[MAX_LEVEL], en_us);

            switch (i)
            {
                case ID_MAX_HP: 
                    m_Script.SetPerLevelGainMaxHp(gain);
                    m_Script.SetMaxLevelMaxHp(maxLevel);
                    break;

                case ID_ATK:
                    m_Script.SetPerLevelGainAtk(gain);
                    m_Script.SetMaxLevelAtk(maxLevel);
                    break;

                case ID_DEF:
                    m_Script.SetPerLevelGainDef(gain);
                    m_Script.SetMaxLevelDef(maxLevel);
                    break;

                case ID_ATKSPD:
                    m_Script.SetPerLevelGainAtkSpd(gain);
                    m_Script.SetMaxLevelAtkSpd(maxLevel);
                    break;

                case ID_CRITRATE:
                    m_Script.SetPerLevelGainCritRate(gain);
                    m_Script.SetMaxLevelCritRate(maxLevel);
                    break;

                case ID_CRITDMG:
                    m_Script.SetPerLevelGainCritDmg(gain);
                    m_Script.SetMaxLevelCritDmg(maxLevel);
                    break;

                case ID_LUCK:
                    m_Script.SetPerLevelGainLuck(gain);
                    m_Script.SetMaxLevelLuck(maxLevel);
                    break;
            }
        }

        for (int i = 8; i < 11; i++)
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

        EditorUtility.SetDirty(m_Script);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

#endif
    }
}
