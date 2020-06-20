using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UsingTheirs.RemoteInspector
{

    [System.Serializable]
    public class RemoteArrayProperty
    {
        public string name;
        public string elementType;
        public List<string> value;
        public bool isPublicRead;
        public bool isPublicWrite;
        public bool canRead;
        public bool canWrite;

        // For Client
        [System.NonSerialized]
        public bool isEdited;
        [System.NonSerialized]
        public List<string> changedValue;
    }

}
