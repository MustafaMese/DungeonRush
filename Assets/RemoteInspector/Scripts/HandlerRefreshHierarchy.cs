using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Reflection;

namespace UsingTheirs.RemoteInspector
{

    public static class HandlerRefreshHierarchy
    {
        public static string HandlerMain(string jsonRequest)
        {
            try
            {
                Logger.Log("[RefreshHierarchyHandler] Refresh. req:{0}", jsonRequest);

                var res = new RefreshHierarchyRes();
                res.scenes = CreateRemoteSceneArray();

                // For Debugging
                var server = Object.FindObjectOfType<ServerRemoteInspector>();
                server.lastRefreashHierarchyRes = res;

                return JsonUtility.ToJson(res);
            }
            catch( System.Exception e )
            {
                return ErrorString(e.ToString());
            }
        }

        static RemoteScene[] CreateRemoteSceneArray()
        {
            var remoteScenes = new RemoteScene[SceneManager.sceneCount + 1];
            var scenes = HelperSceneList.GetScenes();
            for (int i = 0; i < scenes.Count; ++i)
            {
                Scene scene = scenes[i];

                remoteScenes[i] = new RemoteScene();
                remoteScenes[i].path = scene.path;
                remoteScenes[i].isActive = SceneManager.GetActiveScene() == scene;
                remoteScenes[i].rootGameObjects = CreateRemoteGameObjectArray(scene.GetRootGameObjects());
            }

            return remoteScenes;
        }

        static RemoteGameObject[] CreateRemoteGameObjectArray(GameObject[] gameObjects)
        {
            var items = new RemoteGameObject[gameObjects.Length];
            for (int i = 0; i < gameObjects.Length; ++i)
            {
                bool includeComponents = false;
                bool includeChildren = true;
                items[i] = HelperRemoteGameObjectCreator.CreateRemoteGameObject(gameObjects[i], includeComponents, includeChildren);
            }
            return items;
        }

        static string ErrorString(string reason)
        {
            var res = new RefreshHierarchyRes();
            res.error = reason;
            return JsonUtility.ToJson(res);
        }
    }


}