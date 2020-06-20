using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UsingTheirs.RemoteInspector
{

    [System.Serializable]
    public class RemoteComponent
    {
        public string type;
        public int instanceId;
        public bool hasEnabled;
        public bool enabled;
        public RemoteField[] fields;
        public RemoteArrayField[] arrayFields;
        public RemoteProperty[] properties;
        public RemoteArrayProperty[] arrayProperties;
    }

}
