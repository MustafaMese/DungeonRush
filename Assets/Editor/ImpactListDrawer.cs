using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DungeonRush.Data;

[CustomPropertyDrawer(typeof(ImpactList))]
public class ImpactListDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        label = EditorGUI.BeginProperty(position, label, property);
        Rect contentPosition = EditorGUI.PrefixLabel(position, label);
        contentPosition.width *= 0.5f;
        EditorGUI.indentLevel = 0;
        EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("impact"), GUIContent.none);

        contentPosition.x += contentPosition.width;
        EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("element"), GUIContent.none);
        EditorGUI.EndProperty();
    }
}
