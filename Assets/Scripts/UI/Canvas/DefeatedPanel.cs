﻿using DungeonRush.Cards;
using DungeonRush.Data;
using DungeonRush.Managers;
using DungeonRush.Saving;
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
        private const string GOLD = "Earned Gold: ";
        private const string XP = "Earned Experience: ";

        [SerializeField] GameObject defeatedPanel;
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
            SaveUtilities();
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

            s = GOLD + player.Gold.ToString();
            goldText.text = s;

            s = XP + player.Experience.ToString();
            xpText.text = s;
        }

        public void PanelControl(bool activate)
        {
            defeatedPanel.SetActive(activate);
            button.gameObject.SetActive(activate);
        }

        private void SaveUtilities()
        {
            PlayerUtility utilities = SavingSystem.LoadUtilities();
            int xp = player.Experience + utilities.totalXp;
            int gold = player.Gold + utilities.gold;
            SavingSystem.SaveUtilities(xp, gold);
        }
    }
}