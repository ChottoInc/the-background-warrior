using System.Collections.Generic;
using System.Globalization;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerJobFisherSO))]
public class EditorJobFisherData : Editor
{
    private const int ID_CALMNESS = 0;
    private const int ID_REFLEX = 1;
    private const int ID_KNOWLEDGE = 2;
    private const int ID_LUCK = 3;

    private const int ID_BASE_EXP = 5;
    private const int ID_EXPO_EXP = 6;
    private const int ID_FLAT_EXP = 7;



    private const int GAIN_PER_LEVEL = 1;
    private const int MAX_LEVEL = 2;

    private const int COL_LEVEL_EXP = 1;

    PlayerJobFisherSO m_Script;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        m_Script = (PlayerJobFisherSO)target;

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
                case ID_CALMNESS:
                    m_Script.SetPerLevelGainCalmness(gain);
                    m_Script.SetMaxLevelCalmness(maxLevel);
                    break;

                case ID_REFLEX:
                    m_Script.SetPerLevelGainReflex(gain);
                    m_Script.SetMaxLevelReflex(maxLevel);
                    break;

                case ID_KNOWLEDGE:
                    m_Script.SetPerLevelGainKnowledge(gain);
                    m_Script.SetMaxLevelKnowledge(maxLevel);
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

        EditorUtility.SetDirty(m_Script);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

#endif
    }
}
