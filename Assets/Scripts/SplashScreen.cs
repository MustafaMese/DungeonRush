using DungeonRush.Managers;
using DungeonRush.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SplashScreen : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Next());
    }

    void Update()
    {
        
    }

    private IEnumerator Next()
    {
        yield return new WaitForSeconds(1f);
        GameManager.Instance.SetGameState(GameState.LEVEL_TRANSITION);
    }
}
