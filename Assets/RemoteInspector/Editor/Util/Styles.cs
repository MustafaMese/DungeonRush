using UnityEngine;
using UnityEditor;
using System.Diagnostics;
using System.IO;

namespace UsingTheirs.RemoteInspector
{

    public static class Styles
    {
        private static GUIStyle _linkButton;
        public static GUIStyle linkButton
        {
            get
            {
                if (_linkButton != null) return _linkButton;
                _linkButton = new GUIStyle(EditorStyles.label);
                _linkButton.richText = true;
                return _linkButton;
            }
        }
        
        public static GUIStyle _toolbarSearchTextField;
        public static GUIStyle toolbarSearchTextField
        {
            get
            {
                if (_toolbarSearchTextField != null) return _toolbarSearchTextField;
                _toolbarSearchTextField = new GUIStyle(GetStyle("ToolbarSeachTextField"));
                return _toolbarSearchTextField;
            }
        }
        
        public static GUIStyle _toolbarSearchTextFieldCancelButton;
        public static GUIStyle toolbarSearchTextFieldCancelButton
        {
            get
            {
                if (_toolbarSearchTextFieldCancelButton != null) return _toolbarSearchTextFieldCancelButton;
                _toolbarSearchTextFieldCancelButton = new GUIStyle(GetStyle("ToolbarSeachCancelButton"));
                return _toolbarSearchTextFieldCancelButton;
            }
        }
        
        public static GUIStyle _toolbarSearchTextFieldCancelButtonEmpty;
        public static GUIStyle toolbarSearchTextFieldCancelButtonEmpty
        {
            get
            {
                if (_toolbarSearchTextFieldCancelButtonEmpty != null) return _toolbarSearchTextFieldCancelButtonEmpty;
                _toolbarSearchTextFieldCancelButtonEmpty = new GUIStyle(GetStyle("ToolbarSeachCancelButton"));
                return _toolbarSearchTextFieldCancelButtonEmpty;
            }
        }

        public static GUIStyle _inspectorHeader;
        public static GUIStyle inspectorHeader
        {
            get
            {
                if (_inspectorHeader != null) return _inspectorHeader;
                _inspectorHeader = new GUIStyle(GetStyle("In BigTitle"));
                _inspectorHeader.margin = new RectOffset(0, 0, 0, 0);
                return _inspectorHeader;
            }
        }

        public static GUIStyle inspectorTitlebar { get { return GetStyle("In Title"); } }

        private static GUIStyle _inspectorTitlebarText;
        public static GUIStyle inspectorTitlebarText
        {
            get
            {
                if (_inspectorTitlebarText != null) return _inspectorTitlebarText;
                _inspectorTitlebarText = new GUIStyle(EditorStyles.foldout);
                _inspectorTitlebarText.fontStyle = FontStyle.Bold;
                return _inspectorTitlebarText;
            }
        }

        private static GUIStyle _path;
        public static GUIStyle path
        {
            get
            {
                if (_path != null) return _path;
                _path = new GUIStyle(GUI.skin.label);
                _path.wordWrap = true;
                return _path;
            }
        }
        
        private static GUIStyle _noMargin;
        public static GUIStyle noMargin
        {
            get
            {
                if (_noMargin != null) return _noMargin;
                _noMargin = new GUIStyle();
                _noMargin.margin = new RectOffset(0, 0, 0, 0);
                return _noMargin;
            }
        }

        private static GUIStyle _foldout;
        public static GUIStyle foldout
        {
            get
            {
                if (_foldout != null) return _foldout;
                _foldout = new GUIStyle(EditorStyles.foldout);
                _foldout.focused = _foldout.normal;
                _foldout.onFocused = _foldout.onNormal;
                return _foldout;
            }
        }

        public static float hierarchyItemHeight = 17;
        public static float hierarchySceneItemHeight = 18;
        public static float hierarchySceneIconSize = 16;


        private static Texture2D  _hierarchySelectedBackgroundTextureForLightSkin;
        private static Texture2D  hierarchySelectedBackgroundTextureForLightSkin
        {
            get 
            { 
                if ( _hierarchySelectedBackgroundTextureForLightSkin != null ) return _hierarchySelectedBackgroundTextureForLightSkin;
                _hierarchySelectedBackgroundTextureForLightSkin = MakeTex(1, 1, new Color(0.24f, 0.49f, 0.90f));
                return _hierarchySelectedBackgroundTextureForLightSkin;
            }
        } 
        private static Texture2D  _hierarchySelectedBackgroundTextureForDarkSkin;
        private static Texture2D  hierarchySelectedBackgroundTextureForDarkSkin
        {
            get 
            { 
                if ( _hierarchySelectedBackgroundTextureForDarkSkin != null ) return _hierarchySelectedBackgroundTextureForDarkSkin;
                _hierarchySelectedBackgroundTextureForDarkSkin = MakeTex(1, 1, new Color(0.24f, 0.37f, 0.58f));
                return _hierarchySelectedBackgroundTextureForDarkSkin;
            }
        }

        public static Texture2D hierarchySelectedBackgroundTexture
        {
            get
            {
                return EditorGUIUtility.isProSkin ? hierarchySelectedBackgroundTextureForDarkSkin : hierarchySelectedBackgroundTextureForLightSkin;
            }
        }

        public static Texture2D hierarchySceneIcon = EditorGUIUtility.FindTexture("SceneAsset Icon");
        public static GUIStyle hierarchySceneBackgroundStyle = "ProjectBrowserTopBarBg";

        public static float inspectorComponentHeight = 18;

        private static Texture2D _hierarchyWindowIcon;
        public static Texture2D hierarchyWindowIcon
        {
            get
            {
                if (_hierarchyWindowIcon != null) return _hierarchyWindowIcon;
                _hierarchyWindowIcon = AssetDatabase.LoadAssetAtPath<Texture2D>( windowIconPath);
                return _hierarchyWindowIcon;
            }
        }

        public static Texture2D inspectorWindowIcon
        {
            get
            {
                return hierarchyWindowIcon;
            }
        }

        private static Texture2D _characterIcon;
        public static Texture2D characterIcon
        {
            get
            {
                if (_characterIcon != null) return _characterIcon;
                _characterIcon = AssetDatabase.LoadAssetAtPath<Texture2D>( characterIconPath);
                return _characterIcon;
            }
        }

        private static string windowIconPath
        {
            get
            {
                return windowIconDirPath + "Window.png";
            }
        }

        private static string characterIconPath
        {
            get
            {
                return windowIconDirPath + "Icon128.png";
            }
        }

        private static string windowIconDirPath
        {
            get
            {
                var thisFilePath = new StackFrame(0, true).GetFileName();
                var dir = Path.GetDirectoryName(thisFilePath);
                var iconDirPath = Path.Combine(dir, ".." + Path.DirectorySeparatorChar + "Icon" + Path.DirectorySeparatorChar);
                iconDirPath = Path.GetFullPath(iconDirPath);  // Normalize
                var assetRootPath = Path.GetFullPath(Application.dataPath); // Normalize
                return "Assets" + iconDirPath.Substring(assetRootPath.Length); // Relative
            }

        }

        public static Texture2D TextureIcon
        {
            get
            {
                return (Texture2D)EditorGUIUtility.IconContent("Texture Icon").image;
            }
        }
        
        public static Texture2D CubemapIcon
        {
            get
            {
                return (Texture2D)EditorGUIUtility.IconContent("Cubemap Icon").image;
            }
        }
 

        public static Vector2Int inspectorGameObjectIconSize = new Vector2Int(30, 30);
        public static Texture2D inspectorGameObjectIcon
        {
            get
            {
                return (Texture2D)EditorGUIUtility.ObjectContent(null, typeof(GameObject)).image;
            }
        }

        public static Texture2D MaterialIcon
        {
            get
            {
                return (Texture2D)EditorGUIUtility.IconContent("Material Icon").image;
            }
        }

        public static GUIStyle _appendButton;

        public static GUIStyle appendButton
        {
            get
            {
                if (_appendButton != null) return _appendButton;
                _appendButton = new GUIStyle(GUI.skin.button);
                _appendButton.richText = true;
                _appendButton.padding = new RectOffset( 10, 10, 0, 0);
                _appendButton.alignment = TextAnchor.MiddleLeft;
                return _appendButton;
            }
        }


        private static GUIStyle GetStyle( string styleName)
        {
            GUIStyle guiStyle = GUI.skin.FindStyle(styleName) ?? EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector).FindStyle(styleName);
            if (guiStyle == null)
            {
                Logger.LogError("Style Not Found: " + styleName);
            }
            return guiStyle;
        }

        public static Texture2D MakeTex(int width, int height, Color col)
        {
            Color[] pix = new Color[width * height];

            for (int i = 0; i < pix.Length; i++)
                pix[i] = col;

            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();

            return result;
        }

    }

}
