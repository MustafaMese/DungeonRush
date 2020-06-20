using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Reflection;

namespace UsingTheirs.RemoteInspector
{

    public static class HandlerRefreshGameObject
    {
        public static string HandlerMain(string jsonRequest)
        {
            try
            {
                Logger.Log("[HandlerRefreshGameObject] Refresh. req:{0}", jsonRequest);

                var req = JsonUtility.FromJson<RefreshGameObjectReq>(jsonRequest);
                var go = HelperGameObjectFinder.FindGameObject(req.instanceId);
                if (go == null)
                    return ErrorString("GameObject Not Found");

                var res = new RefreshGameObjectRes();
                bool includeComponents = true;
                bool includeChildren = false;
                res.gameObject = HelperRemoteGameObjectCreator.CreateRemoteGameObject(go, includeComponents, includeChildren);

                // For Debugging
                var server = Object.FindObjectOfType<ServerRemoteInspector>();
                server.lastRefreashGameObjectRes = res;
                
                return JsonUtility.ToJson(res);
            }
            catch( System.Exception e )
            {
                return ErrorString(e.ToString());
            }
        }

        public static string ErrorString(string reason)
        {
            var res = new RefreshGameObjectRes();
            res.error = reason;
            return JsonUtility.ToJson(res);
        }

    }

}