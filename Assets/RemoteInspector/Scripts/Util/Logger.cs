//#define RI_DEBUG
using UnityEngine;

namespace UsingTheirs.RemoteInspector
{

    public static class Logger
    {
        public static void Log(string format, params object[] args)
        {
#if RI_DEBUG
            var msg = string.Format(format, args);
            Debug.Log(msg);
#endif
        }

        public static void LogWarning(string format, params object[] args)
        {
#if RI_DEBUG
            var msg = string.Format(format, args);
            Debug.LogWarning(msg);
#endif
        }
        public static void LogError(string format, params object[] args)
        {
#if RI_DEBUG
            var msg = string.Format(format, args);
            Debug.LogError(msg);
#endif
        }
    }

}