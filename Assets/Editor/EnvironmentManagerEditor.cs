using DungeonRush.Controller;
using UnityEditor;

[CustomEditor(typeof(EnvironmentManager)), CanEditMultipleObjects]
public class EnvironmentManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        // EditorList.Show(serializedObject.FindProperty("integers"), EditorListOption.ListSize);
        // EditorList.Show(serializedObject.FindProperty("vectors"));
        // EditorList.Show(serializedObject.FindProperty("colorPoints"), EditorListOption.Buttons);
        EditorList.Show(serializedObject.FindProperty("elementPrefabs"), EditorListOption.Buttons);
        // EditorList.Show(serializedObject.FindProperty("notAList"));
        serializedObject.ApplyModifiedProperties();
    }
}
