using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UsingTheirs.RemoteInspector
{

    [System.Serializable]
    public class RemoteField
    {
        public string name;
        public string type;
        public string value;
        public bool isPublic;

        // For Client
        [System.NonSerialized]
        public bool isEdited;
    }

}
