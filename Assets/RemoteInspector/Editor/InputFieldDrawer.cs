using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

namespace UsingTheirs.RemoteInspector
{

    public static class InputFieldDrawer
    {
        public enum ArrayChangeType
        {
            Resize,
            Insert,
            Delete,
            Modify,
            Append
        }

        [Flags]
        public enum ArrayOptionFlag
        {
            ReadOnly = 1,
            ShowAppendMenu = 2,
            DisableResize = 4,
            DisableDuplicate = 8,
            DisableModify = 16
        }
        
        public delegate void OnArrayChanged(ArrayChangeType type, int length, int index, string value);

        private static object expandedAppendMenu = null;
        private static string appendCustomText = string.Empty;
        private static List<string> appendButtons = new List<string>();    // Optimization
        
        public static void DrawInputArrayField(Type elementType, string label, List<string> value, ArrayOptionFlag flag, 
            ref bool foldout, Func<List<string>> appendItemList, OnArrayChanged onArrayChanged)
        {
            // Do Not support array manipulation for Object types
            if (elementType.IsSubclassOf(typeof(UnityEngine.Object)))
                flag |= ArrayOptionFlag.ReadOnly;
                
            bool readOnly = (flag & ArrayOptionFlag.ReadOnly) != 0;
                
            
            bool oldEnabled = GUI.enabled;
            if (readOnly) GUI.enabled = false;
            
            foldout = EditorGUILayout.Foldout(foldout, label, true);
            
            if (foldout)
            {
                EditorGUI.indentLevel++;

                bool oldEnabledSize = GUI.enabled;
                if ((flag & ArrayOptionFlag.DisableResize) != 0)
                    GUI.enabled = false;
                
                var newCount = EditorGUILayout.IntField("Size", value.Count);
                if (newCount != value.Count)
                    onArrayChanged(ArrayChangeType.Resize, newCount, 0, null);

                GUI.enabled = oldEnabledSize;

                for (int i = 0; i < value.Count; ++i)
                {
                    var elementValue = value[i];
                    DrawInputArrayElementField(elementType, elementValue, onArrayChanged, i, flag);

                    ShowContextMenuForArrayElement(elementValue, onArrayChanged, i, flag);
                }
                
                ShowAppendUI(value, flag, appendItemList, onArrayChanged);

                EditorGUI.indentLevel--;
                
            }

            GUI.enabled = oldEnabled;
        }

        private static void ShowAppendUI(List<string> value, ArrayOptionFlag flag, Func<List<string>> appendItemList, OnArrayChanged onArrayChanged)
        {
            if ((flag & ArrayOptionFlag.ReadOnly) == 0 &&
                (flag & ArrayOptionFlag.ShowAppendMenu) != 0)
            {
                var oldFoldout = (expandedAppendMenu == value);
                var newFoldout = oldFoldout;

                newFoldout = EditorGUILayout.Foldout(newFoldout, "Append ...");

                if (newFoldout && !oldFoldout)
                {
                    appendCustomText = string.Empty;
                    expandedAppendMenu = value;
                }
                else if (!newFoldout && oldFoldout)
                {
                    expandedAppendMenu = null;
                }

                if (newFoldout)
                {
                    EditorGUI.indentLevel++;

                    appendCustomText = EditorGUILayout.TextField("Custom", appendCustomText);

                    appendButtons.Clear();
                    appendButtons.Add(appendCustomText);
                    var itemList = appendItemList != null ? appendItemList() : null;
                    if (itemList != null)
                        appendButtons.AddRange(itemList);

                    foreach (var item in appendButtons)
                    {
                        if (GUI.Button(GetAppendItemRect(),
                            string.Format("Append <b>{0}</b>", item), Styles.appendButton))
                        {
                            expandedAppendMenu = null; // Collapse
                            onArrayChanged(ArrayChangeType.Append, 0, 0, item);
                        }
                    }

                    EditorGUI.indentLevel--;
                }
            }
        }

        private static Rect GetAppendItemRect()
        {
            var rc = EditorGUILayout.GetControlRect(true, Styles.hierarchyItemHeight);
            rc = EditorGUI.IndentedRect(rc);
            return rc;
        }

        private static void ShowContextMenuForArrayElement(string elementValue, OnArrayChanged onArrayChanged,
            int index,
            ArrayOptionFlag flag)
        {
            var rc = GUILayoutUtility.GetLastRect();
            var current = Event.current;
            if (rc.Contains(current.mousePosition) && current.type == EventType.ContextClick)
            {
                var menu = new GenericMenu();

                if ((flag & ArrayOptionFlag.DisableDuplicate) == 0)
                {
                    menu.AddItem(new GUIContent("Duplicate Array Element"), false,
                        () => { onArrayChanged(ArrayChangeType.Insert, 0, index, elementValue); });
                }
                
                menu.AddItem(new GUIContent("Delete Array Element"), false,
                    () => { onArrayChanged(ArrayChangeType.Delete, 0, index, elementValue); });

                menu.ShowAsContext();

                current.Use();
            }
        }

        private static void DrawInputArrayElementField(Type elementType, string elementValue,
            OnArrayChanged onArrayChanged, int i, ArrayOptionFlag flag)
        {
            var oldEnabled = GUI.enabled;
            if ((flag & ArrayOptionFlag.DisableModify) != 0)
                GUI.enabled = false;
            
            object oldValue = SerializationCustom.Deserialize(elementType, elementValue);
            object newValue = DrawImpl(elementType, "Element" + i, oldValue);
            if (!oldValue.Equals(newValue))
            {
                //Debug.Log(string.Format("old:{0}, new:{1}", oldValue, newValue));
                onArrayChanged(ArrayChangeType.Modify, 0, i,
                    SerializationCustom.Serialize(elementType, newValue));
            }

            GUI.enabled = oldEnabled;
        }


        public static void DrawInputField(Type type, string label, string value, bool readOnly, Action<string> onNewValue)
        {
            var oldEnabled = GUI.enabled;
            if (readOnly) GUI.enabled = false;

            object oldValue = SerializationCustom.Deserialize(type, value);
            object newValue = DrawImpl(type, label, oldValue);
            if (!oldValue.Equals(newValue))
                onNewValue(SerializationCustom.Serialize(type, newValue));

            GUI.enabled = oldEnabled;
        }

        static object DrawImpl(Type type, string label, object value)
        {
            if (type == typeof(Int32)) return DrawImpl_Int32(type, label, value);
            else if (type == typeof(bool)) return DrawImpl_bool(type, label, value);
            else if (type == typeof(float)) return DrawImpl_float(type, label, value);
            else if (type == typeof(string)) return DrawImpl_string(type, label, value);
            else if (type == typeof(Vector3)) return DrawImpl_Vector3(type, label, value);
            else if (type.IsEnum) return DrawImpl_Enum(type, label, value);
            else if (type == typeof(Bounds)) return DrawImpl_Bounds(type, label, value);
            else if (type == typeof(Color)) return DrawImpl_Color(type, label, value);
            else if (type == typeof(double)) return DrawImpl_double(type, label, value);
            else if (type == typeof(long)) return DrawImpl_long(type, label, value);
            else if (type == typeof(Rect)) return DrawImpl_Rect(type, label, value);
            else if (type == typeof(Vector2)) return DrawImpl_Vector2(type, label, value);
            else if (type == typeof(Vector4)) return DrawImpl_Vector4(type, label, value);
            else if (type == typeof(Material)) return DrawImpl_Material(type, label, value);
            else if (type == typeof(Texture2D)) return DrawImpl_Texture2D(type, label, value);
            else if (type == typeof(Texture3D)) return DrawImpl_Texture3D(type, label, value);
            else if (type == typeof(Cubemap)) return DrawImpl_Cubemap(type, label, value);
            else if (type == typeof(Texture2DArray)) return DrawImpl_Texture2DArray(type, label, value);
            else if (type == typeof(CubemapArray)) return DrawImpl_CubemapArray(type, label, value);
#if UNITY_2017_2_OR_NEWER
            else if (type == typeof(BoundsInt)) return DrawImpl_BoundsInt(type, label, value);
            else if (type == typeof(RectInt)) return DrawImpl_RectInt(type, label, value);
            else if (type == typeof(Vector2Int)) return DrawImpl_Vector2Int(type, label, value);
            else if (type == typeof(Vector3Int)) return DrawImpl_Vector3Int(type, label, value);
#endif
            else throw new Exception(string.Format("[InputFieldDrawer] Unknown Type :{0}", type.Name));
        }
        static object DrawImpl_Int32(Type type, string label, object value)
        {
#if UNITY_2017_3_OR_NEWER
            return EditorGUILayout.DelayedIntField(label, (int)value);
#else
		return EditorGUILayout.IntField( label, (int)value );
#endif
        }

        static object DrawImpl_bool(Type type, string label, object value)
        {
            return EditorGUILayout.Toggle(label, (bool)value);
        }
        static object DrawImpl_float(Type type, string label, object value)
        {
#if UNITY_2017_3_OR_NEWER
            return EditorGUILayout.DelayedFloatField(label, (float)value);
#else
		return EditorGUILayout.FloatField( label, (float)value );
#endif
        }

        static object DrawImpl_string(Type type, string label, object value)
        {
#if UNITY_2017_3_OR_NEWER
            return EditorGUILayout.DelayedTextField(label, (string)value);
#else
		return EditorGUILayout.TextField( label, (string)value );
#endif
        }

        static object DrawImpl_Vector3(Type type, string label, object value)
        {
            return EditorGUILayout.Vector3Field(label, (Vector3)value);
        }

        static object DrawImpl_Enum(Type type, string label, object value)
        {
            return EditorGUILayout.EnumPopup(label, (Enum)value);
        }

        static object DrawImpl_Bounds(Type type, string label, object value)
        {
            return EditorGUILayout.BoundsField(label, (Bounds)value);
        }

        static object DrawImpl_Color(Type type, string label, object value)
        {
            return EditorGUILayout.ColorField(label, (Color)value);
        }

        static object DrawImpl_double(Type type, string label, object value)
        {
#if UNITY_2017_3_OR_NEWER
            return EditorGUILayout.DelayedDoubleField(label, (double)value);
#else
        return EditorGUILayout.DoubleField( label, (double)value );
#endif
        }

        static object DrawImpl_long(Type type, string label, object value)
        {
            return EditorGUILayout.LongField(label, (long)value);
        }

        static object DrawImpl_Rect(Type type, string label, object value)
        {
            return EditorGUILayout.RectField(label, (Rect)value);
        }

        static object DrawImpl_Vector2(Type type, string label, object value)
        {
            return EditorGUILayout.Vector2Field(label, (Vector2)value);
        }

        static object DrawImpl_Vector4(Type type, string label, object value)
        {
            return EditorGUILayout.Vector4Field(label, (Vector4)value);
        }
        
        static object DrawImpl_Material(Type type, string label, object value)
        {
            var m = (MaterialRef) value;

            string text;
            if (m.instanceID != 0)
                text = string.Format("{0} ({1})", m.name, m.shaderName);
            else
                text = "(None)";
            
            EditorGUILayout.LabelField(new GUIContent(label), new GUIContent(text, Styles.MaterialIcon));
            return value;
        }
        
        static object DrawImpl_Texture2D(Type type, string label, object value)
        {
            var t = (Texture2DRef) value;

            string text;
            if (t.instanceID != 0)
                text = string.Format("{0} ({1}x{2} {3})", t.name, t.width, t.height, t.format);
            else
                text = "(None)";
            
            EditorGUILayout.LabelField(new GUIContent(label), new GUIContent(text, Styles.TextureIcon));
            return value;
        }
        
        static object DrawImpl_Texture3D(Type type, string label, object value)
        {
            var t = (Texture3DRef) value;

            string text;
            if (t.instanceID != 0)
                text = string.Format("{0} ({1}x{2}x{3} {4})", t.name, t.width, t.height, t.depth, t.format);
            else
                text = "(None)";
            
            EditorGUILayout.LabelField(new GUIContent(label), new GUIContent(text, Styles.TextureIcon));
            return value;
        }
        
        static object DrawImpl_Cubemap(Type type, string label, object value)
        {
            var t = (TextureCubeRef) value;

            string text;
            if (t.instanceID != 0)
                text = string.Format("{0} ({1}x{2} {3})", t.name, t.width, t.height, t.format);
            else
                text = "(None)";
            
            EditorGUILayout.LabelField(new GUIContent(label), new GUIContent(text, Styles.CubemapIcon));
            return value;
        }
        
        static object DrawImpl_Texture2DArray(Type type, string label, object value)
        {
            var t = (Texture2DArrayRef) value;

            string text;
            if (t.instanceID != 0)
                text = string.Format("{0} ({1}x{2}x{3} {4})", t.name, t.width, t.height, t.depth, t.format);
            else
                text = "(None)";
            
            EditorGUILayout.LabelField(new GUIContent(label), new GUIContent(text, Styles.TextureIcon));
            return value;
        }
        
        static object DrawImpl_CubemapArray(Type type, string label, object value)
        {
            var t = (TextureCubeArrayRef) value;

            string text;
            if (t.instanceID != 0)
                text = string.Format("{0} ({1}x{2}x{3} {4})", t.name, t.width, t.height, t.count, t.format);
            else
                text = "(None)";
            
            EditorGUILayout.LabelField(new GUIContent(label), new GUIContent(text, Styles.TextureIcon));
            return value;
        }

#if UNITY_2017_2_OR_NEWER
        static object DrawImpl_BoundsInt(Type type, string label, object value)
        {
            return EditorGUILayout.BoundsIntField(label, (BoundsInt)value);
        }

        static object DrawImpl_RectInt(Type type, string label, object value)
        {
            return EditorGUILayout.RectIntField(label, (RectInt)value);
        }

        static object DrawImpl_Vector2Int(Type type, string label, object value)
        {
            return EditorGUILayout.Vector2IntField(label, (Vector2Int)value);
        }

        static object DrawImpl_Vector3Int(Type type, string label, object value)
        {
            return EditorGUILayout.Vector3IntField(label, (Vector3Int)value);
        }
#endif
    }

}
