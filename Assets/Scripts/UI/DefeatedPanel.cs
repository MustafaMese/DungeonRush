using DungeonRush.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefeatedPanel : MonoBehaviour
{
    [SerializeField] GameObject defeatedPanel;
    [SerializeField] GameManager gameManager;


    public void SetDefeat()
    {
        GameManager.gameState = GameState.STOP_GAME;
        defeatedPanel.SetActive(true);

        StartCoroutine(GotoMainMenu());
    }

    private IEnumerator GotoMainMenu()
    {
        yield return new WaitForSeconds(1f);
        yield return gameManager.FadeOut();
        GameManager.gameState = GameState.END;
    }
}
