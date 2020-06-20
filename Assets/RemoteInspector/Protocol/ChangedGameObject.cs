using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UsingTheirs.RemoteInspector
{

    [System.Serializable]
    public class ChangedGameObject
    {
        public int instanceId;
        public bool active;
        public ChangedComponent component;
    }

}