using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UsingTheirs.RemoteInspector
{

    [System.Serializable]
    public class RemoteProperty
    {
        public string name;
        public string type;
        public string value;
        public bool isPublicRead;
        public bool isPublicWrite;
        public bool canRead;
        public bool canWrite;

        [System.NonSerialized]
        public bool isEdited;
        [System.NonSerialized]
        public string changedValue;
    }

}
