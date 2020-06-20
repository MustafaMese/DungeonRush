using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UsingTheirs.RemoteInspector
{
    [System.Serializable]
    public class RemoteScene : ISerializationCallbackReceiver
    {
        public string path;
        public bool isActive;
        public RemoteGameObject[] rootGameObjects;

        [SerializeField]
        private List<RemoteGameObject> childGameObjects;

        public List<RemoteGameObject> GetChildGameObjects()
        {
            return childGameObjects;
        }
        
        public void OnBeforeSerialize()
        {
            childGameObjects = new List<RemoteGameObject>();
            foreach (var go in rootGameObjects)
            {
                CollectChildren(go);
            }
        }

        void CollectChildren(RemoteGameObject parentGO)
        {
            if (parentGO.childGameObjects == null)
                return;

            foreach (var childGO in parentGO.childGameObjects)
            {
                childGO.parentInstanceId = parentGO.instanceId;
                childGameObjects.Add(childGO);

                CollectChildren(childGO);
            }
        }

        public void OnAfterDeserialize()
        {
            foreach (var childGO in childGameObjects)
            {
                var parentGO = FindParent(childGO.parentInstanceId);
                if (parentGO == null)
                {
                    Debug.LogError(string.Format("[RemoteScene.OnAfterDeserialize] Parent({0}) of Child({1},{2}) Not Found",
                        childGO.parentInstanceId, childGO.name, childGO.instanceId));
                    continue;
                }

                if (parentGO.childGameObjects == null)
                    parentGO.childGameObjects = new List<RemoteGameObject>();

                parentGO.childGameObjects.Add(childGO);
            }
        }

        RemoteGameObject FindParent(int parentInstanceId)
        {
            var go = System.Array.Find(rootGameObjects, x => x.instanceId == parentInstanceId);
            if (go != null)
                return go;

            go = childGameObjects.Find(x => x.instanceId == parentInstanceId);
            return go;
        }

    }

}
