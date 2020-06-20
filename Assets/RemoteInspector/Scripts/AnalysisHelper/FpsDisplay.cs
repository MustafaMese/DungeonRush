using UnityEngine;
using System.Collections;

namespace UsingTheirs.RemoteInspector
{
    class FpsDisplay : MonoBehaviour
    {
        public Rect displayRect = new Rect(0, 0, 200, 100);
        public int fontSize = 20;
        public Color fontColor = Color.white;
        public string prefix = "FPS: ";

        float lastFps = 0;
        float lastFrameTime = 0;

        IEnumerator Start()
        {
            const float checkInterval = 0.5f;
            float accumulatedTime = 0f;
            int frameCount = 0;

            while (true)
            {
                yield return null;

                accumulatedTime += Time.unscaledDeltaTime;
                frameCount++;

                if (accumulatedTime < checkInterval)
                    continue;

                lastFrameTime = accumulatedTime / (float)frameCount;
                lastFps = 1 / lastFrameTime;
                accumulatedTime = 0f;
                frameCount = 0;
            }
        }

        private void OnGUI()
        {
            int oldFontSize = GUI.skin.label.fontSize;
            Color oldColor = GUI.color;

            GUI.skin.label.fontSize = Screen.height / 720 * fontSize;
            GUI.color = fontColor;

            GUI.Label(displayRect, string.Format("{0}{1:0.0} ({2:0.0}ms)", prefix, lastFps, lastFrameTime * 1000));

            GUI.skin.label.fontSize = oldFontSize;
            GUI.color = oldColor;
        }
    }
}
