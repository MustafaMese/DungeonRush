using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UsingTheirs.RemoteInspector
{

    [System.Serializable]
    public class RemoteGameObject
    {
        public string name;
        public int instanceId;
        public bool active;
        public RemoteComponent[] components;

        // For Serialization
        public int parentInstanceId;

        [System.NonSerialized]
        public List<RemoteGameObject> childGameObjects;
    }

}
