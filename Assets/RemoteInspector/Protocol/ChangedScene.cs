using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UsingTheirs.RemoteInspector
{

    [System.Serializable]
    public class ChangedScene
    {
        public string path;
        public ChangedGameObject[] rootGameObjects;
    }

}
