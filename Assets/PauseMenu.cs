using DungeonRush.Managers;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;

    public void Resume()
    {
        GameManager.gameState = GameState.PLAY;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Pause()
    {
        GameManager.gameState = GameState.PAUSE;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }
}
