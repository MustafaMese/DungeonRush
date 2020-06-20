using System;

namespace UsingTheirs.RemoteInspector
{

    [Serializable]
    public class ChangedArray
    {
        public ArrayChangeType type;
        public string name;
        public int length;
        public int index;
        public string value;
    }

}
