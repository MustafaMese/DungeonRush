using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UsingTheirs.RemoteInspector
{
    [System.Serializable]
    public class ChangedComponent
    {
        public int instanceId;
        public bool hasEnabled;
        public bool enabled;
        public ChangedField field;
        public ChangedProperty property;
        public ChangedArray arrayField;
        public ChangedArray arrayProperty;
    }
}
