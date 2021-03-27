using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(ArmorImpact))]
public class ArmorImpactDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        label = EditorGUI.BeginProperty(position, label, property);
        Rect contentPosition = EditorGUI.PrefixLabel(position, label);
        contentPosition.width *= 0.5f;
        EditorGUI.indentLevel = 0;
        EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("impactType"), GUIContent.none);

        contentPosition.x += contentPosition.width;
        EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("power"), GUIContent.none);
        EditorGUI.EndProperty();
    }
}
