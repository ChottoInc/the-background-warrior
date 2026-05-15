using UnityEditor;
using static UtilsQuest;

[CustomEditor(typeof(QuestDailySO))]
public class EditorQuestDailySO : Editor
{
    private QuestDailySO _quest;

    private SerializedProperty s_uniqueId;
    private SerializedProperty s_availableFor;
    private SerializedProperty s_questData;



    private bool showQuestData = true;

    private SerializedProperty s_questObjectiveType;

    // --------- Quest Kill ---------
    private SerializedProperty s_questKillSpecific;

    // --- Specific
    private SerializedProperty s_monsterId;

    private SerializedProperty s_amountKill;

    // --------- Quest Obtain ---------
    private SerializedProperty s_itemType;
    private SerializedProperty s_questObtainSpecific;

    // --- Specific
    private SerializedProperty s_itemId;

    private SerializedProperty s_amountObtain;

    // --------- Quest Level Up ---------
    private SerializedProperty s_questLevelUpSpecific;

    // --- Specific
    private SerializedProperty s_statId;

    private SerializedProperty s_amountStat;

    // --------- Quest Unlock Map ---------

    private SerializedProperty s_mapId;

    // --------- Quest Obtain ---------
    private SerializedProperty s_questBefriendSpecific;

    // --- Specific
    private SerializedProperty s_companionSO;

    private SerializedProperty s_amountBefriend;



    private void OnEnable()
    {
        s_uniqueId = serializedObject.FindProperty("uniqueId");
        s_availableFor = serializedObject.FindProperty("availableFor");

        s_questData = serializedObject.FindProperty("questData");


        s_questObjectiveType = s_questData.FindPropertyRelative("questObjectiveType");

        s_questKillSpecific = s_questData.FindPropertyRelative("questKillSpecific");
        s_monsterId = s_questData.FindPropertyRelative("monsterId");
        s_amountKill = s_questData.FindPropertyRelative("amountKill");

        s_itemType = s_questData.FindPropertyRelative("itemType");
        s_questObtainSpecific = s_questData.FindPropertyRelative("questObtainSpecific");
        s_itemId = s_questData.FindPropertyRelative("itemId");
        s_amountObtain = s_questData.FindPropertyRelative("amountObtain");

        s_questLevelUpSpecific = s_questData.FindPropertyRelative("questLevelUpSpecific");
        s_statId = s_questData.FindPropertyRelative("statId");
        s_amountStat = s_questData.FindPropertyRelative("amountStat");

        s_mapId = s_questData.FindPropertyRelative("mapId");

        s_questBefriendSpecific = s_questData.FindPropertyRelative("questBefriendSpecific");
        s_companionSO = s_questData.FindPropertyRelative("companionSO");
        s_amountBefriend = s_questData.FindPropertyRelative("amountBefriend");
    }

    public override void OnInspectorGUI()
    {
        _quest = (QuestDailySO)target;

        serializedObject.Update();


        EditorGUILayout.PropertyField(s_uniqueId);
        EditorGUILayout.PropertyField(s_availableFor);

        showQuestData = EditorGUILayout.Foldout(
            showQuestData,
            "Quest Data",
            true
        );

        if (showQuestData)
        {
            EditorGUI.indentLevel++;

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(s_questObjectiveType);

            switch (_quest.QuestData.questObjectiveType)
            {
                case QuestObjectiveType.Kill:
                    EditorGUILayout.Space();
                    EditorGUILayout.PropertyField(s_questKillSpecific);

                    if (_quest.QuestData.questKillSpecific)
                    {
                        EditorGUILayout.Space();
                        EditorGUILayout.PropertyField(s_monsterId);
                    }

                    EditorGUILayout.Space();
                    EditorGUILayout.PropertyField(s_amountKill);

                    break;

                case QuestObjectiveType.Obtain:
                    EditorGUILayout.Space();
                    EditorGUILayout.PropertyField(s_itemType);

                    EditorGUILayout.Space();
                    EditorGUILayout.PropertyField(s_questObtainSpecific);

                    if (_quest.QuestData.questObtainSpecific)
                    {
                        EditorGUILayout.Space();
                        EditorGUILayout.PropertyField(s_itemId);
                    }

                    EditorGUILayout.Space();
                    EditorGUILayout.PropertyField(s_amountObtain);

                    break;

                case QuestObjectiveType.LevelUp:
                    EditorGUILayout.Space();
                    EditorGUILayout.PropertyField(s_questLevelUpSpecific);

                    if (_quest.QuestData.questLevelUpSpecific)
                    {
                        EditorGUILayout.Space();
                        EditorGUILayout.PropertyField(s_statId);
                    }

                    EditorGUILayout.Space();
                    EditorGUILayout.PropertyField(s_amountStat);

                    break;

                case QuestObjectiveType.UnlockMap:
                    EditorGUILayout.Space();
                    EditorGUILayout.PropertyField(s_mapId);

                    break;

                case QuestObjectiveType.Befriend:
                    EditorGUILayout.Space();
                    EditorGUILayout.PropertyField(s_questBefriendSpecific);

                    if (_quest.QuestData.questBefriendSpecific)
                    {
                        EditorGUILayout.Space();
                        EditorGUILayout.PropertyField(s_companionSO);
                    }

                    EditorGUILayout.Space();
                    EditorGUILayout.PropertyField(s_amountBefriend);

                    break;
            }
        }





        serializedObject.ApplyModifiedProperties();
    }
}
