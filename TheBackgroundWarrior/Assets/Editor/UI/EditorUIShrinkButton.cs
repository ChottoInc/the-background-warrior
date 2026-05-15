using UnityEditor;
using UnityEditor.UI;

[CustomEditor(typeof(UIShrinkButton))]
[CanEditMultipleObjects]
public class EditorUIShrinkButton : ButtonEditor
{
    private SerializedProperty s_multiplier;
    private SerializedProperty s_timerShrink;

    protected override void OnEnable()
    {
        base.OnEnable();

        s_multiplier = serializedObject.FindProperty("multiplier");
        s_timerShrink = serializedObject.FindProperty("timerShrink");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        base.OnInspectorGUI();

        EditorGUILayout.PropertyField(s_multiplier);
        EditorGUILayout.PropertyField(s_timerShrink);

        serializedObject.ApplyModifiedProperties();
    }
}
