using DungeonRush.Cards;
using DungeonRush.Managers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DefeatedPanel : MonoBehaviour
{
    private const string DUNGEON = ". dungeon";
    private const string ENCOUNTER = ". encounter";
    private const string GOLD = "Gold: ";
    private const string XP = "Experience: ";

    [SerializeField] GameObject defeatedPanel;
    [SerializeField] GameManager gameManager;

    [SerializeField] TextMeshProUGUI dungeonText;
    [SerializeField] TextMeshProUGUI encounterText;
    [SerializeField] TextMeshProUGUI goldText;
    [SerializeField] TextMeshProUGUI xpText;

    private PlayerCard player;

    [SerializeField] List<GameObject> panels = new List<GameObject>();

    private void Start()
    {
        player = FindObjectOfType<PlayerCard>();
    }

    public void SetDefeat()
    {
        GameManager.gameState = GameState.STOP_GAME;
        defeatedPanel.SetActive(true);
        DeactivatePanels();
        SetValues();
    }

    public void MainMenu()
    {
        StartCoroutine(Fading());
    }

    private IEnumerator Fading()
    {
        yield return gameManager.FadeOut();
        GameManager.gameState = GameState.END;
    }

    private void SetValues()
    {
        string s = "";
        int index = SceneManager.GetActiveScene().buildIndex + 1;

        s = index.ToString() + ENCOUNTER;
        encounterText.text = s;

        s = 1.ToString() + DUNGEON;
        dungeonText.text = s;

        s = GOLD + player.Coins.ToString();
        goldText.text = s;

        s = XP + player.Experience.ToString();
        xpText.text = s;
    }

    private void DeactivatePanels()
    {
        for (int i = 0; i < panels.Count; i++)
        {
            panels[i].SetActive(false);
        }
    }
}
