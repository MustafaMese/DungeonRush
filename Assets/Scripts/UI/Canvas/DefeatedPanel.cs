using DungeonRush.Cards;
using DungeonRush.Managers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DungeonRush.UI
{
    public class DefeatedPanel : MonoBehaviour, ICanvasController
    {
        private const string DUNGEON = ". dungeon";
        private const string ENCOUNTER = ". encounter";
        private const string GOLD = "Gold: ";
        private const string XP = "Experience: ";

        [SerializeField] GameObject defeatedPanel;

        [SerializeField] TextMeshProUGUI dungeonText;
        [SerializeField] TextMeshProUGUI encounterText;
        [SerializeField] TextMeshProUGUI goldText;
        [SerializeField] TextMeshProUGUI xpText;
        [SerializeField] Button button;

        private PlayerCard player;

        private void Start()
        {
            player = FindObjectOfType<PlayerCard>();
        }

        private void SetDefeat()
        {
            defeatedPanel.SetActive(true);
            button.gameObject.SetActive(true);
            SetValues();
        }

        public void Defeat()
        {
            SetDefeat();
        }

        public void MainMenu()
        {
            GameManager.Instance.SetGameState(GameState.END);
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

        public void PanelControl(bool activate)
        {
            defeatedPanel.SetActive(activate);
            button.gameObject.SetActive(activate);
        }
    }
}