using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UsingTheirs.RemoteInspector
{
    public class ScreenSettings : MonoBehaviour
    {
        public Vector2Int screenResolution;
        public bool fullScreen;
        public bool applyScreenResolution;
        public bool updateScreenResolution = true;

        void Update()
        {
            if (updateScreenResolution)
            {
                updateScreenResolution = false;
                screenResolution = new Vector2Int(Screen.width, Screen.height);
                fullScreen = Screen.fullScreen;
            }

            if (applyScreenResolution)
            {
                applyScreenResolution = false;
                Screen.SetResolution(screenResolution.x, screenResolution.y, fullScreen);
            }
        }
    }
}
