using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance = null;
    private static bool m_ShuttingDown;

    public static T Instance
    {
        get
        {
            if (m_ShuttingDown)
            {
                Debug.LogWarning("[Singleton] Instance '" + typeof(T) +
                    "' already destroyed. Returning null.");
                return null;
            }

            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        _instance = (T)FindObjectOfType(typeof(T));
       
        enabled = true;
        DontDestroyOnLoad(this.gameObject);
    }

    private void OnApplicationQuit()
    {
        m_ShuttingDown = true;
        _instance = null;
    }

    private void OnEnable()
    {
        Initialize();
    }

    protected virtual void Initialize() 
    {
        enabled = true;
    }

}
