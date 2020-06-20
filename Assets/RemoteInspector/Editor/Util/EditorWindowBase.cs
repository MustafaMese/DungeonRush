using System;
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

namespace UsingTheirs.RemoteInspector
{

    public class EditorWindowBase : EditorWindow
    {
        #region Help Message
        string helpMessage;
        MessageType helpMessageType;
        double lastMessageTime;
        int lastMessageIndex;
        const float minHelpMessageShowSec = 0.1f;

        protected void ShowHelpMessage()
        {
            if (!string.IsNullOrEmpty(helpMessage))
            {
                const float yBegin = 20;
                
                //EditorGUILayout.HelpBox(helpMessage, helpMessageType);
                var width = EditorGUIUtility.currentViewWidth - 20;
                var height  = EditorStyles.helpBox.CalcHeight(new GUIContent(helpMessage), width);
                height = Mathf.Max(40, height);
                var rc = new Rect(10, yBegin, width, height);
                EditorGUI.HelpBox(rc, helpMessage, helpMessageType);
                EditorGUI.HelpBox(rc, helpMessage, helpMessageType);
                EditorGUI.HelpBox(rc, helpMessage, helpMessageType);

                if (helpMessageType == MessageType.Error && helpMessage.Contains("Net Error:"))
                {
                    var btnRc = new Rect(10,  yBegin + height + 3, width, 20);
                    if (GUI.Button(btnRc, "Visit a Page for <b>Connection Troubleshooting</b>", Styles.linkButton))
                        Application.OpenURL("https://codingdad.me/2019/10/30/remote-inspector/");
                }
            }
        }

        protected bool HasErrorMessage()
        {
            return !string.IsNullOrEmpty(helpMessage) && helpMessageType == MessageType.Error;
        }

        protected void SetHelpMessage(string message, MessageType type)
        {
            helpMessage = message;
            helpMessageType = type;
            lastMessageTime = EditorApplication.timeSinceStartup;
            lastMessageIndex++;
            Repaint();
        }

        protected void ClearHelpMessage()
        {
            if (EditorApplication.timeSinceStartup - lastMessageTime >= minHelpMessageShowSec)
            {
                ClearHelpMessageImpl();
            }
            else
            {
                EditorCoroutine.Start(DelayedClearHelpMessage(lastMessageIndex));
            }

        }

        IEnumerator DelayedClearHelpMessage(int clearMessageIndex)
        {
            yield return new EditorCoroutine.CustomWaitForSeconds(minHelpMessageShowSec);

            if (clearMessageIndex == lastMessageIndex)
                ClearHelpMessageImpl();
        }

        void ClearHelpMessageImpl()
        {
            helpMessage = null;
            Repaint();
        }
        #endregion

        #region Icon
        bool hideCharacterIcon;
        bool hideCharacterIconLoaded;
        protected void ShowCharacterIcon()
        {
            if ( ! hideCharacterIconLoaded )
            {
                hideCharacterIconLoaded = true;
                hideCharacterIcon = EditorPrefs.GetBool( GetHideIconKey(), false );
            }

            var icon = Styles.characterIcon;
            if ( icon == null )
                return;


            if ( ! hideCharacterIcon )
            {
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();

                EditorGUILayout.LabelField( new GUIContent(Styles.characterIcon)
                    , GUILayout.Width(icon.width), GUILayout.Height(icon.height));

                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();

            }

            EditorGUI.BeginChangeCheck();

            hideCharacterIcon = EditorGUILayout.ToggleLeft( "Hide Character Icon", hideCharacterIcon);

            if ( EditorGUI.EndChangeCheck() )
                EditorPrefs.SetBool(  GetHideIconKey(), hideCharacterIcon );

        }

        string GetHideIconKey()
        {
            return GetType().Name + "_HideIcon";
        }
        #endregion

        #region Link
        protected void ShowLink()
        {
            EditorGUILayout.BeginHorizontal();
            
            if (GUILayout.Button("Kinds", EditorStyles.miniButton))
                Application.OpenURL( Consts.kKindsAssetStoreUrlLite );
            
            GUILayout.FlexibleSpace();
            
            GUILayout.Label( string.Format("Ver {0}",Consts.kVersion), EditorStyles.miniLabel);
            
            if (GUILayout.Button("Online Doc", EditorStyles.miniButton))
                Application.OpenURL( Consts.kOnlineDocUrl);
            
            if (GUILayout.Button("Review", EditorStyles.miniButton))
                Application.OpenURL( Consts.kAssetStoreReviewUrlLite);
            
            EditorGUILayout.EndHorizontal();
        }
        #endregion

        #region Search
        private string searchKeyword = string.Empty;

        protected bool isSearching
        {
            get { return !string.IsNullOrEmpty(searchKeyword); }
        }
        
        protected void ShowSearchUI()
        {
            //GUILayout.BeginHorizontal(EditorStyles.toolbar);
            searchKeyword = EditorGUILayout.TextField( searchKeyword, Styles.toolbarSearchTextField);
            if (GUILayout.Button(GUIContent.none,
                string.IsNullOrEmpty(searchKeyword)
                    ? Styles.toolbarSearchTextFieldCancelButtonEmpty
                    : Styles.toolbarSearchTextFieldCancelButton))
            {
                searchKeyword = string.Empty;
                GUIUtility.keyboardControl = 0;
            }
            
            //GUILayout.EndHorizontal();
        }

        protected bool IsSearchKeywordMatched(string name)
        {
            return name.IndexOf(searchKeyword, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        

        #endregion
    }
}