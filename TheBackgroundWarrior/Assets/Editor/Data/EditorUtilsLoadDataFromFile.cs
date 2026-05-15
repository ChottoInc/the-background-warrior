using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UtilsLoadDataFromFile))]
public class EditorUtilsLoadDataFromFile : Editor
{
    private const int MAP_ID = 0;
    private const int ENEMY_NAME = 1;
    private const int POOL_NAME = 2;
    private const int SO_ID = 3;
    private const int MAX_HP = 4;
    private const int ATK = 5;
    private const int DEF = 6;
    private const int CRIT_DMG = 7;


    UtilsLoadDataFromFile m_Script;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        m_Script = (UtilsLoadDataFromFile)target;

        if (GUILayout.Button("Load enemy SO data"))
        {
            ReadEnemyData();
        }
    }

    private void ReadEnemyData()
    {
#if UNITY_EDITOR

        UtilsCombatMap.Initialize();

        string soBaseFolder = "Assets/Resources/Data/Enemies";

        // Filepath is searched from Resources
        string dataFilepath = "Files/Enemies/EnemySODatas";

        List<string> datas = UtilsGeneral.GetFileStrings(dataFilepath);

        foreach (string data in datas)
        {
            string[] parts = data.Split(",", System.StringSplitOptions.None);

            if (parts.Length > 1)
            {
                CultureInfo en_us = CultureInfo.GetCultureInfo("en-US");

                // Read datas
                int idMap = int.Parse(parts[MAP_ID]);
                string mapName = UtilsCombatMap.GetMapById(idMap).MapName.ToLower();

                string enemyName = parts[ENEMY_NAME];

                string poolName = parts[POOL_NAME].ToLower();

                int idSO = int.Parse(parts[SO_ID]);

                float maxHp = float.Parse(parts[MAX_HP], en_us);
                float atk = float.Parse(parts[ATK], en_us);
                float def = float.Parse(parts[DEF], en_us);
                float critDmg = float.Parse(parts[CRIT_DMG], en_us);

                // Create string with folder fullpath and filename
                string soFolderFullpath = Path.Combine(soBaseFolder, mapName);

                string soFileName = string.Format("{0}_{1}_{2}.asset", idSO, poolName, mapName);


                // If folder does not exists yet, make it
                if (!AssetDatabase.IsValidFolder(soFolderFullpath))
                {
                    string guid = AssetDatabase.CreateFolder(soBaseFolder, mapName);
                    soFolderFullpath = AssetDatabase.GUIDToAssetPath(guid);
                }

                // Set asset fullpath
                string fileFullpath = Path.Combine(soFolderFullpath, soFileName);

                // Get so
                EnemySO newSO = AssetDatabase.LoadAssetAtPath<EnemySO>(fileFullpath);

                if(newSO == null)
                {
                    newSO = ScriptableObject.CreateInstance<EnemySO>();
                    AssetDatabase.CreateAsset(newSO, fileFullpath);
                }

                newSO.SetEnemyName(enemyName);
                newSO.SetPoolName(poolName);
                newSO.SetId(idSO);
                newSO.SetBaseMaxhHp(maxHp);
                newSO.SetBaseAtk(atk);
                newSO.SetBaseDef(def);
                newSO.SetBaseCritDmg(critDmg);

                EditorUtility.SetDirty(newSO);
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

#endif
    }
}
