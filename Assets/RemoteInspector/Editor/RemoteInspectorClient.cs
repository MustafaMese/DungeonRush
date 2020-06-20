using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace UsingTheirs.RemoteInspector
{

    public static class RemoteInspectorClient
    {
        // done: res, error
        public static void RefreshHierachy(string urlPrefix, System.Action<string, string> done)
        {
            EditorCoroutine.Start(PostImpl(urlPrefix + "/refreshHierarchy", "{}", done));
        }

        // done: res, error
        public static void ApplyChangedGameObject(string urlPrefix, string reqContent, System.Action<string, string> done)
        {
            EditorCoroutine.Start(PostImpl(urlPrefix + "/applyGameObject", reqContent, done));
        }

        // done: res, error
        public static void RefreshGameObject(string urlPrefix, string reqContent, System.Action<string, string> done)
        {
            EditorCoroutine.Start(PostImpl(urlPrefix + "/refreshGameObject", reqContent, done));
        }

        // done: res, error
        static IEnumerator PostImpl(string url, string reqContent, System.Action<string, string> done)
        {
            var www = UnityWebRequest.Post(url, reqContent);
            UnityWebRequestAsyncOperation op = www.SendWebRequest();
            yield return new EditorCoroutine.CustomYieldInstruction(() => op.isDone);

            if (www.isNetworkError)
            {
                done(null, "Net Error:" + www.error);
                yield break;
            }
            else if (www.isHttpError)
            {
                done(null, string.Format("[RemoteInspectorClient] Http Error:{0}, {1}", www.responseCode, www.error));
                yield break;
            }

            while (!www.downloadHandler.isDone)
                yield return null;

            //Logger.Log( "[RemoteInpectorClient] Code:{0}, Res:{1}", www.responseCode, www.downloadHandler.text );
            done(www.downloadHandler.text, null);
        }

    }

}
