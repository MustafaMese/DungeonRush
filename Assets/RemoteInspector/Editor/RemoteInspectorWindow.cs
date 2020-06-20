using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEditor;


namespace UsingTheirs.RemoteInspector
{
    public class RemoteInspectorWindow : EditorWindowBase
    {
        [MenuItem("Window/Using Theirs/Remote Inspector/Inspector", false, 0)]
        static void Init()
        {
            var win = EditorWindow.GetWindow(typeof(RemoteInspectorWindow));
            win.Show();
        }

        public static RemoteInspectorWindow I { get; private set; }

        void Awake() { I = this; }

        void OnInspectorUpdate() { I = this; }
        void OnDestroy() { I = null; }

        Vector2 scrollPt;
        RemoteGameObject targetGameObject;

        bool needToSetTitle = true;
        void SetTitle()
        {
            if (needToSetTitle)
            {
                titleContent = new GUIContent("Inspector", Styles.hierarchyWindowIcon);
                needToSetTitle = false;
            }
        }

        void OnGUI()
        {
            SetTitle();

            EditorGUIUtility.wideMode = true;
            EditorGUIUtility.hierarchyMode = true;
            EditorGUI.indentLevel = 1;

            ShowRefreshUI();

            //ShowSearchUI();

            if (!base.HasErrorMessage())
                ShowInspector();

            GUILayout.FlexibleSpace();

            if (base.HasErrorMessage() || targetGameObject == null )
            {
                base.ShowCharacterIcon();
            }

            base.ShowLink();

            base.ShowHelpMessage();
        }

        private void ShowRefreshUI()
        {
            var selectedGO = RemoteHierarchyWindow.selectedGameObject;
            if (selectedGO == null && targetGameObject == null)
            {
                base.SetHelpMessage("No Selected Object", MessageType.Info);
            }

            try
            {
                GUILayout.BeginHorizontal(EditorStyles.toolbar);
                if (GUILayout.Button("Refresh", EditorStyles.toolbarButton))
                {
                    Refresh();
                }
                
                ShowSearchUI();
            }
            finally
            {
                GUILayout.EndHorizontal();
            }

        }

        private void ShowInspector()
        {
            try
            {
                scrollPt = EditorGUILayout.BeginScrollView(scrollPt);

                if (targetGameObject != null)
                {
                    var go = targetGameObject;

                    GUILayout.BeginHorizontal(Styles.inspectorHeader);
                    GUILayout.Label(
                        Styles.inspectorGameObjectIcon,
                        GUILayout.Width(Styles.inspectorGameObjectIconSize.x),
                        GUILayout.Height(Styles.inspectorGameObjectIconSize.y),
                        GUILayout.ExpandWidth(false)
                        );
                    bool newValue = EditorGUILayout.ToggleLeft((string)go.name, (bool)go.active);

                    GUILayout.EndHorizontal();

                    if (newValue != go.active)
                    {
                        go.active = newValue;
                        if (go != null)
                            go.active = newValue;
                        ApplyChangedValue(null, null, null, null, null);
                    }

                    foreach (var comp in go.components)
                    {
                        DrawComponent(comp);
                    }
                }
            }
            finally
            {
                EditorGUILayout.EndScrollView();
            }
        }

        public void Refresh()
        {

            var selectedGO = RemoteHierarchyWindow.selectedGameObject;
            if (selectedGO == null && targetGameObject == null)
            {
                base.SetHelpMessage("No Selected Object", MessageType.Error);
                return;
            }

            base.SetHelpMessage("Refreshing...", MessageType.Info);

            var oldTargetGameObject = targetGameObject;
            // This make the window not render it's content while refreshing.
            //targetGameObject = null;

            var newTargetGameObject = selectedGO ?? oldTargetGameObject;
            var instanceId = newTargetGameObject.instanceId;

            var req = new RefreshGameObjectReq();
            req.instanceId = instanceId;

            var reqJson = JsonUtility.ToJson(req);

            RemoteInspectorClient.RefreshGameObject(RemoteHierarchyWindow.urlPrefix, reqJson, (json, error) =>
            {
                if (!string.IsNullOrEmpty(error))
                {
                    base.SetHelpMessage(error, MessageType.Error);
                    return;
                }

                RefreshGameObjectRes res = null;
                try
                {
                    res = JsonUtility.FromJson<RefreshGameObjectRes>(json);
                }
                catch (Exception)
                {
                    // ignored
                }

                if (res == null)
                {
                    base.SetHelpMessage("Failed to parse response: res=\n" + json , MessageType.Error);
                    return;
                }
                
                if (!string.IsNullOrEmpty(res.error))
                {
                    base.SetHelpMessage(res.error, MessageType.Error);
                    return;
                }

                base.ClearHelpMessage();

                if ( targetGameObject != null && res.gameObject != null )
                {
                    if (targetGameObject.instanceId != res.gameObject.instanceId)
                        scrollPt = Vector2.zero;
                }

                targetGameObject = res.gameObject;
                Repaint();
            });
        }

        HashSet<int> collpasedComponents = new HashSet<int>();
        HashSet<object> collpasedArray = new HashSet<object>();
        
        void DrawComponent(RemoteComponent comp)
        {
            //Debug.Log("DrawComponent : " + comp.name);

            bool oldFoldOut = !collpasedComponents.Contains(comp.instanceId);
            bool oldEnabled = comp.enabled;

            bool newFoldOut = oldFoldOut;
            bool newEnabled = oldEnabled;

            DrawComponentHeader(comp.type, comp.hasEnabled, ref newFoldOut, ref newEnabled);

            if (newEnabled != oldEnabled)
            {
                comp.enabled = newEnabled;
                ApplyChangedValue(comp, null, null, null, null);
            }

            if (oldFoldOut && !newFoldOut)
                collpasedComponents.Add(comp.instanceId);
            else if (!oldFoldOut && newFoldOut)
                collpasedComponents.Remove(comp.instanceId);

            if (!newFoldOut)
                return;

            foreach (var f in comp.fields)
            {
                DrawField(comp, f);
            }
            
            foreach (var p in comp.properties)
            {
                DrawProperty(comp, p);
            }
            
            foreach (var af in comp.arrayFields)
            {
                DrawArrayField(comp, af);
            }
            
            foreach (var ap in comp.arrayProperties)
            {
                DrawArrayProperty(comp, ap);
            }

            
            // var methods = compT.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic );
            // foreach( var m in methods )
            // {
            // 	DrawMethod( comp, m);

            // }
        }

        void DrawComponentHeader(string name, bool hasEnabled, ref bool foldOut, ref bool enabled)
        {
            Rect rc = EditorGUILayout.GetControlRect(false, Styles.inspectorComponentHeight, Styles.noMargin);

            GUI.Label(rc, GUIContent.none, Styles.inspectorTitlebar);

            if (hasEnabled)
            {
                Rect rcToggle = rc;
                rcToggle.width = 30;
                enabled = EditorGUI.Toggle(rcToggle, enabled);
            }

            foldOut = EditorGUI.Foldout(rc, foldOut, GUIContent.none, true, Styles.foldout);

            Rect rcLabel = rc;
            if (hasEnabled)
                rcLabel.x += 15;
            rcLabel = EditorGUI.IndentedRect(rcLabel);
            GUI.Label(rcLabel, name, EditorStyles.boldLabel);
        }


        void DrawField(RemoteComponent comp, RemoteField field)
        {
            if (isSearching && !IsSearchKeywordMatched(field.name))
                return;
            
            Type type = TypeCache.GetFromCacheOrFind(field.type);

            if (type == null)
            {
                Logger.LogError("type is null. " + comp.type + "." + field.name);
                return;
            }
            
            InputFieldDrawer.DrawInputField(type, field.name, field.value, false, (newValue) =>
            {
               field.value = newValue;
               ApplyChangedValue(comp, field, null, null, null);
            });
            
        }
        
        void DrawArrayField(RemoteComponent comp, RemoteArrayField field)
        {
            if (isSearching && !IsSearchKeywordMatched(field.name))
                return;
            
            Type elementType = TypeCache.GetFromCacheOrFind(field.elementType);

            if (elementType == null)
            {
                Logger.LogError("type is null. " + comp.type + "." + field.name);
                return;
            }
            
            bool oldFoldOut = !collpasedArray.Contains(field.value);
            bool newFoldOut = oldFoldOut;

            InputFieldDrawer.DrawInputArrayField(elementType, field.name, field.value, 0,  ref newFoldOut, null,
                (changeType, length, index, newValue) =>
                {
                    EditorArrayUtil.ApplyChange(field.value, changeType, length, index, newValue, elementType);
                    var changedArrayField =
                        EditorArrayUtil.CreateChangedArray(field.name, field.value , changeType, length, index, newValue);
                    ApplyChangedValue(comp, null, null, changedArrayField, null);
                });
            
            if (oldFoldOut && !newFoldOut)
                collpasedArray.Add(field.value);
            else if (!oldFoldOut && newFoldOut)
                collpasedArray.Remove(field.value);
        }

        void DrawProperty(RemoteComponent comp, RemoteProperty property)
        {
            if (isSearching && !IsSearchKeywordMatched(property.name))
                return;
            
            Type type = TypeCache.GetFromCacheOrFind(property.type);

            if (type == null)
            {
                Logger.LogError("type is null. " + comp.type + "." + property.name);
                return;
            }

            // Do not display a write-only property
            if (!property.canRead)
                return;

            InputFieldDrawer.DrawInputField(type, property.name, property.value, !property.canWrite, (newValue) =>
          {
              property.value = newValue;
              ApplyChangedValue(comp, null, property, null, null);
          });
        }
        
        void DrawArrayProperty(RemoteComponent comp, RemoteArrayProperty property)
        {
            if (isSearching && !IsSearchKeywordMatched(property.name))
                return;
            
            Type elementType = TypeCache.GetFromCacheOrFind(property.elementType);

            if (elementType == null)
            {
                Logger.LogError("type is null. " + comp.type + "." + property.name);
                return;
            }
            
            // Do not display a write-only property
            if (!property.canRead)
                return;
            
            bool oldFoldOut = !collpasedArray.Contains(property.value);
            bool newFoldOut = oldFoldOut;

            InputFieldDrawer.DrawInputArrayField(elementType, property.name, property.value, 0, ref newFoldOut, null,
                (changeType, length, index, newValue) =>
                {
                    EditorArrayUtil.ApplyChange(property.value, changeType, length, index, newValue, elementType);
                    var changedArrayProperty =
                        EditorArrayUtil.CreateChangedArray(property.name, property.value , changeType, length, index, newValue);
                    ApplyChangedValue(comp, null, null, null, changedArrayProperty);
                });
            
            if (oldFoldOut && !newFoldOut)
                collpasedArray.Add(property.value);
            else if (!oldFoldOut && newFoldOut)
                collpasedArray.Remove(property.value);
        }

        void ApplyChangedValue(RemoteComponent comp, RemoteField field, RemoteProperty property, 
            ChangedArray changedArrayField, ChangedArray changedArrayProperty)
        {
            var go = targetGameObject;

            ChangedField changedField = null;
            if (field != null)
            {
                changedField = new ChangedField();
                changedField.name = field.name;
                changedField.value = field.value;
            }

            ChangedProperty changedProperty = null;
            if (property != null)
            {
                changedProperty = new ChangedProperty();
                changedProperty.name = property.name;
                changedProperty.value = property.value;
            }

            var changedComponent = new ChangedComponent();
            if (comp != null)
            {
                changedComponent.instanceId = comp.instanceId;
                changedComponent.hasEnabled = comp.hasEnabled;
                changedComponent.enabled = comp.enabled;
                changedComponent.field = changedField;
                changedComponent.property = changedProperty;
                changedComponent.arrayField = changedArrayField;
                changedComponent.arrayProperty = changedArrayProperty;
            }
            

            var changedGO = new ChangedGameObject();
            changedGO.instanceId = go.instanceId;
            changedGO.active = go.active;
            changedGO.component = changedComponent;

            var req = new ChangeGameObjectReq();
            req.gameObject = changedGO;

            base.SetHelpMessage("Applying Changes...", MessageType.Info);
            var reqContent = JsonUtility.ToJson(req);
            //Debug.Log("req = " + reqContent);
            RemoteInspectorClient.ApplyChangedGameObject(RemoteHierarchyWindow.urlPrefix, reqContent, (resText, error) =>
            {
                if (!string.IsNullOrEmpty(error))
                {
                    base.SetHelpMessage(error, MessageType.Error);
                    return;
                }

                ChangeGameObjectRes res = null;
                try
                {
                    res = JsonUtility.FromJson<ChangeGameObjectRes>(resText);
                }
                catch (Exception)
                {
                    // ignored
                }

                if (res == null)
                {
                    base.SetHelpMessage("Failed to parse response: res=\n" + resText , MessageType.Error);
                    return;
                }
                    
                if (!string.IsNullOrEmpty(res.error))
                {
                    base.SetHelpMessage(res.error, MessageType.Error);
                    return;
                }

                base.ClearHelpMessage();
                Refresh();
            });
        }

        void DrawMethod(Component comp, MethodInfo mi)
        {
            if (GUILayout.Button("Call " + mi.Name) && mi.GetParameters().Length == 0)
            {
                mi.Invoke(comp, new object[] { });
            }
        }
    }

}
