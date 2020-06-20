using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Reflection;

namespace UsingTheirs.RemoteInspector
{

    public static class HelperGameObjectFinder
    {
        static GameObject lastResult;

        public static GameObject FindGameObject(int instanceId)
        {
            if (lastResult != null && lastResult.GetInstanceID() == instanceId)
                return lastResult;

            var scenes = HelperSceneList.GetScenes();
            for (int i = 0; i < scenes.Count; ++i)
            {
                var go = FindGameObjectInScene(instanceId, scenes[i]);
                if (go != null)
                {
                    lastResult = go;
                    return go;
                }
            }

            return null;
        }

        private static GameObject FindGameObjectInScene(int instanceId, Scene scene)
        {
            var rootObjs = scene.GetRootGameObjects();
            for (int i = 0; i < rootObjs.Length; ++i)
            {
                var go = FindGameObjectInChildren(instanceId, rootObjs[i]);
                if (go != null)
                    return go;
            }

            return null;
        }

        private static GameObject FindGameObjectInChildren(int instanceId, GameObject gameObject)
        {
            if (gameObject.GetInstanceID() == instanceId)
                return gameObject;

            var tr = gameObject.transform;
            for (int i = 0; i < tr.childCount; ++i)
            {
                var go = FindGameObjectInChildren(instanceId, tr.GetChild(i).gameObject);
                if (go != null)
                    return go;
            }

            return null;
        }
    }

}