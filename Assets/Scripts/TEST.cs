using DungeonRush.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST : MonoBehaviour
{


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            GameManager.Instance.SetGameState(GameState.LEVEL_TRANSITION);
    }
}
