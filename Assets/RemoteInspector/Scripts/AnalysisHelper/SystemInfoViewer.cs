using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UsingTheirs.RemoteInspector
{
    public class SystemInfoViewer : MonoBehaviour
    {
        public Vector2Int displayResolution { get; private set; }

        public bool updateSystenInfo = true;

        void Update()
        {
            if (updateSystenInfo)
            {
                updateSystenInfo = false;

                var display = Display.displays[0];
                displayResolution = new Vector2Int(display.systemWidth, display.systemHeight);
            }
        }
    }
}
