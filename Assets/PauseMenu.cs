using DungeonRush.Cards;
using DungeonRush.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] GameObject pauseMenuUI;
    [SerializeField] Button pauseButton;

    [Header("Character Stats")]
    [SerializeField] TextMeshProUGUI healthTxt = null;
    [SerializeField] TextMeshProUGUI damageTxt = null;
    [SerializeField] TextMeshProUGUI itemsTxt = null;

    private PlayerCard player;

    private void Start()
    {
        player = FindObjectOfType<PlayerCard>();
    }

    public void Resume()
    {
        GameManager.gameState = GameState.PLAY;
        pauseMenuUI.SetActive(false);
        pauseButton.gameObject.SetActive(true);
        Time.timeScale = 1f;
    }

    public void Pause()
    {
        GameManager.gameState = GameState.PAUSE;
        SetTexts();
        pauseMenuUI.SetActive(true);
        pauseButton.gameObject.SetActive(false);
        Time.timeScale = 0f;
    }

    private void SetTexts()
    {
        print(player.GetHealth());
        SetText(healthTxt, player.GetHealth());

        int damage = player.GetDamage();
        SetText(damageTxt, damage);

        List<string> names = player.GetItemNames();
        SetText(itemsTxt, names);
    }

    private void SetText(TextMeshProUGUI text, int value)
    {
        text.text = value.ToString();
    }

    private void SetText(TextMeshProUGUI text, List<string> values)
    {
        string sth = "";
        if (values != null && values.Count > 0)
        {
            for (int i = 0; i < values.Count; i++)
            {
                sth = String.Concat(sth, values[i]);
                sth = String.Concat(sth, "\n");
            }
        }

        text.text = sth;
    }

    public void MainMenu()
    {
        GameManager.gameState = GameState.STOP_GAME;
        StartCoroutine(GoToMainMenu());
    }

    private IEnumerator GoToMainMenu()
    {
        yield return gameManager.FadeOut();
        LoadManager.LoadStartScene();
    }
}
