using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Reflection;

namespace UsingTheirs.RemoteInspector
{

    [RequireComponent(typeof(ServerHttpJsonPost))]
    public class ServerRemoteInspector : MonoBehaviour
    {
        ServerHttpJsonPost httpServer;
        public RefreshHierarchyRes lastRefreashHierarchyRes;
        public RefreshGameObjectRes lastRefreashGameObjectRes;

        private static ServerRemoteInspector _instance;

        void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
            }
        }

        void Start()
        {
            DontDestroyOnLoad(gameObject);

            httpServer = GetComponent<ServerHttpJsonPost>();
            httpServer.AddHandler("/refreshHierarchy", HandlerRefreshHierarchy.HandlerMain);
            httpServer.AddHandler("/applyGameObject", HandlerApplyGameObject.HandlerMain);
            httpServer.AddHandler("/refreshGameObject", HandlerRefreshGameObject.HandlerMain);
            httpServer.AddHandler("/", HandlerConnectionTest);
        }

        void OnDestroy()
        {
            if (_instance != this)
                return;
            
            httpServer.RemoveHandler("/refreshHierachy");
            httpServer.RemoveHandler("/applyGameObject");
            httpServer.RemoveHandler("/refreshGameObject");
            httpServer.RemoveHandler("/");
        }

        string HandlerConnectionTest( string jsonRequest )
        {
            return "Welcome to Remote Inspector!";
        }

    }

}
