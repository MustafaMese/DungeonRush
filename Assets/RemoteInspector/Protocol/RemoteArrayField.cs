using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UsingTheirs.RemoteInspector
{

    [System.Serializable]
    public class RemoteArrayField
    {
        public string name;
        public string elementType;
        public List<string> value;
        public bool isPublic;

        // For Client
        [System.NonSerialized]
        public bool isEdited;
    }

}
