using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UsingTheirs.RemoteInspector
{

    public static class HelperSceneList
    {
        public static List<Scene> GetScenes()
        {
            var scenes = new List<Scene>(SceneManager.sceneCount + 1);
            for (int i = 0; i < SceneManager.sceneCount; ++i)
                scenes.Add(SceneManager.GetSceneAt(i));

            scenes.Add(GetDontDestroyOnLoadScene());
            return scenes;
        }

        static Scene GetDontDestroyOnLoadScene()
        {
            var go = new GameObject("RemoteInspectorTemp");
            Object.DontDestroyOnLoad(go);
            var scene = go.scene;
            Object.DestroyImmediate(go);
            return scene;
        }

    }

}
