using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

namespace UsingTheirs.RemoteInspector
{

    public class RemoteHierarchyWindow : EditorWindowBase
    {
        [MenuItem("Window/Using Theirs/Remote Inspector/Hierarchy", false, 0)]
        static void Init()
        {
            var win = EditorWindow.GetWindow<RemoteHierarchyWindow>();
            win.Show();
        }

        public static RemoteGameObject selectedGameObject { get; private set; }
        Vector2 scrollPt;
        RefreshHierarchyRes hierarchyRes;
        HashSet<int> expandedInstanceIds = new HashSet<int>();
        HashSet<string> collapsedScenePaths = new HashSet<string>();
        public static string urlPrefix { get; private set; }
        
        bool needToSetTitle = true;
        void SetTitle()
        {
            if (needToSetTitle)
            {
                titleContent = new GUIContent("Hierarchy", Styles.hierarchyWindowIcon);
                needToSetTitle = false;
            }
        }

        void OnGUI()
        {
            SetTitle();

            ShowServerConnectionUI();
            
            //ShowSearchUI();

            if (!base.HasErrorMessage())
                ShowHierarchy();

            GUILayout.FlexibleSpace();

            if (base.HasErrorMessage() || hierarchyRes == null )
            {
                base.ShowCharacterIcon();
            }

            ShowPathUI();

            base.ShowLink();
            base.ShowHelpMessage();
        }

        private void ShowHierarchy()
        {
            try
            {
                scrollPt = EditorGUILayout.BeginScrollView(scrollPt);

                if (hierarchyRes != null)
                {
                    EditorGUIUtility.hierarchyMode = true;

                    foreach (var scene in hierarchyRes.scenes)
                    {
                        EditorGUI.indentLevel = 1;

                        Rect rc = EditorGUILayout.GetControlRect(false, Styles.hierarchySceneItemHeight, Styles.noMargin);
                        bool foldout = FoldoutScene(scene, rc);
                        if (foldout)
                        {
                            foreach (var go in scene.rootGameObjects)
                            {
                                DrawGameObject(go, 2);
                            }
                        }
                    }

                    EditorGUI.indentLevel = 0;
                }
            }
            finally
            {
                EditorGUILayout.EndScrollView();
            }

        }

        private void ShowServerConnectionUI()
        {

            if (urlPrefix == null)
                urlPrefix = EditorPrefs.GetString("RemoteInspector_UrlPrefix", "http://localhost:8080");

            try
            {
                GUILayout.BeginHorizontal(EditorStyles.toolbar);
                string newUrlPrefix = EditorGUILayout.TextField(urlPrefix, EditorStyles.toolbarTextField);
                newUrlPrefix = newUrlPrefix.TrimEnd(new char[] {'/'});
                if (newUrlPrefix != urlPrefix)
                {
                    urlPrefix = newUrlPrefix;
                    EditorPrefs.SetString("RemoteInspector_UrlPrefix", urlPrefix);
                }

                if (GUILayout.Button("Refresh", EditorStyles.toolbarButton))
                {
                    base.SetHelpMessage("Refreshing...", MessageType.Info);
                    hierarchyRes = null;
                    RemoteInspectorClient.RefreshHierachy(urlPrefix, (json, error) =>
                    {
                        if (!string.IsNullOrEmpty(error))
                        {
                            base.SetHelpMessage(error, MessageType.Error);
                            return;
                        }

                        hierarchyRes = null;

                        try
                        {
                            hierarchyRes = JsonUtility.FromJson<RefreshHierarchyRes>(json);
                        }
                        catch (Exception)
                        {
                            // ignored
                        }

                        if (hierarchyRes == null)
                        {
                            base.SetHelpMessage("Failed to parse response: res=\n" + json , MessageType.Error);
                            return;
                        }

                        if (!string.IsNullOrEmpty(hierarchyRes.error))
                        {
                            base.SetHelpMessage(hierarchyRes.error, MessageType.Error);
                            return;
                        }

                        base.ClearHelpMessage();
                        Repaint();
                    });
                } 
                
                ShowSearchUI();
            }
            finally
            {
                GUILayout.EndHorizontal();
            }

        }

        private bool FoldoutScene(RemoteScene scene, Rect rc)
        {
            bool oldFoldOut = !collapsedScenePaths.Contains(scene.path);
            GUI.Label(rc, GUIContent.none, Styles.hierarchySceneBackgroundStyle);

            bool newFoldOut = EditorGUI.Foldout(rc, oldFoldOut, GUIContent.none, true, Styles.foldout);

            Rect rcIcon = rc;
            rcIcon = EditorGUI.IndentedRect(rcIcon);
            rcIcon.x += 2;
            rcIcon.width = Styles.hierarchySceneIconSize;
            rcIcon.height = Styles.hierarchySceneIconSize;

            // Unity 2019.3 seemed to remove "SceneAsset Icon"
            if (Styles.hierarchySceneIcon != null)
                GUI.DrawTexture(rcIcon, Styles.hierarchySceneIcon);
            

            Rect rcLabel = rc;
            rcLabel.x += 20;
            rcLabel = EditorGUI.IndentedRect(rcLabel);

            GUIStyle labelStyle = scene.isActive ? EditorStyles.boldLabel : EditorStyles.label;
            GUI.Label(rcLabel, ScenePathToName(scene.path), labelStyle);

            if (oldFoldOut && !newFoldOut)
                collapsedScenePaths.Add(scene.path);
            else if (!oldFoldOut && newFoldOut)
                collapsedScenePaths.Remove(scene.path);
            return newFoldOut;
        }

        string ScenePathToName(string path)
        {
            int idx = path.LastIndexOf('/');
            if (idx == -1)
                return path;
            else
                return path.Substring(idx + 1, path.Length - 6 - (idx + 1));  // name only without .unity
        }

        void DrawGameObject(RemoteGameObject go, int indentLevel)
        {
            EditorGUI.indentLevel = indentLevel;

            Rect rcFull = Rect.zero;
            if (!isSearching || IsSearchKeywordMatched(go.name))
            {
                rcFull = EditorGUILayout.GetControlRect(false, Styles.hierarchyItemHeight, Styles.noMargin);

                FillSelectionBackground(go, rcFull);

                var rcLabel = EditorGUI.IndentedRect(rcFull);
                if (go.active)
                {
                    if (GUI.Button(rcLabel, go.name, EditorStyles.label))
                        SelectGameObject(go);
                }
                else
                {
                    if (GUI.Button(rcLabel, GUIContent.none, EditorStyles.label))
                        SelectGameObject(go);

                    GUI.enabled = false;
                    GUI.Label(rcLabel, go.name, EditorStyles.label);
                    GUI.enabled = true;
                }
            }

            if (go.childGameObjects != null && go.childGameObjects.Count > 0)
            {
                if (!isSearching)
                {
                    bool foldout = FoldoutGameObject(go, rcFull);
                    if (foldout)
                    {
                        foreach (var childItem in go.childGameObjects)
                        {
                            DrawGameObject(childItem, indentLevel + 1);
                        }
                    }
                }
                else
                {
                    foreach (var childItem in go.childGameObjects)
                    {
                        DrawGameObject(childItem, indentLevel);
                    }
                }
            }
        }

        bool FoldoutGameObject(RemoteGameObject go, Rect rc)
        {
            bool oldFoldOut = expandedInstanceIds.Contains(go.instanceId);
            bool newFoldOut = EditorGUI.Foldout(rc, oldFoldOut, "", true, Styles.foldout);

            if (oldFoldOut && !newFoldOut)
                expandedInstanceIds.Remove(go.instanceId);
            else if (!oldFoldOut && newFoldOut)
                expandedInstanceIds.Add(go.instanceId);

            return newFoldOut;
        }

        void FillSelectionBackground(RemoteGameObject go, Rect rc)
        {
            if (Event.current.type != EventType.Repaint)
                return;

            if (selectedGameObject != go)
                return;

            GUI.DrawTexture(rc, Styles.hierarchySelectedBackgroundTexture);
        }

        private RemoteGameObject gameObjectForPath = null;
        private string searchedObjectPath = string.Empty;

        private void ShowPathUI()
        {
            if (!isSearching)
                return;
            
            GUILayout.Label("Path:");

            if (selectedGameObject != null)
            {
                if (selectedGameObject != gameObjectForPath)
                {
                    // find path
                    searchedObjectPath = FindPath(hierarchyRes, selectedGameObject);
                    gameObjectForPath = selectedGameObject;
                }
                
                GUILayout.TextArea(searchedObjectPath, Styles.path);
            }
        }

        private static string FindPath(RefreshHierarchyRes res, RemoteGameObject selectedGameObject)
        {
            var pathList = new List<string>();

            var parentIdToFind = selectedGameObject.parentInstanceId;
            while (parentIdToFind != 0)
            {
                RemoteGameObject foundParent = null;
                foreach (var scene in res.scenes)
                {
                    foundParent = scene.rootGameObjects.Where(x => x.instanceId == parentIdToFind).FirstOrDefault();
                    if (foundParent != null)
                        break;
                    
                    foundParent = scene.GetChildGameObjects().Where(x => x.instanceId == parentIdToFind).FirstOrDefault();
                    if (foundParent != null)
                        break;
                }

                if (foundParent == null)
                    break;
                
                pathList.Insert(0, foundParent.name);
                parentIdToFind = foundParent.parentInstanceId;
            }

            return string.Join(" > ", pathList.ToArray());
        }

        private void SelectGameObject(RemoteGameObject go)
        {
            selectedGameObject = go;
            Logger.Log("Select " + go.name);

            if (RemoteInspectorWindow.I != null)
                RemoteInspectorWindow.I.Refresh();
        }
    }

}
