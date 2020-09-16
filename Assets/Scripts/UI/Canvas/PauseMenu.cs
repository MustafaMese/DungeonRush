using DungeonRush.Cards;
using DungeonRush.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DungeonRush.UI
{
    public class PauseMenu : MonoBehaviour, ICanvasController
    {
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

        #region TEXT OPERATIONS
        private void SetTexts()
        {
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
        #endregion

        #region BUTTON OPERATIONS
        public void Resume()
        {
            GameManager.Instance.SetGameState(GameState.PLAY);
            pauseMenuUI.SetActive(false);
            pauseButton.gameObject.SetActive(true);
        }

        public void Pause()
        {
            GameManager.Instance.SetGameState(GameState.PAUSE, UIState.PAUSE);
            SetTexts();
            pauseMenuUI.SetActive(true);
            pauseButton.gameObject.SetActive(false);
        }

        public void MainMenu()
        {
            Resume();
            GameManager.Instance.SetGameState(GameState.DEFEAT);
        }

        #endregion

        public void PanelControl(bool activate)
        {
            pauseButton.gameObject.SetActive(activate);
        }
    }
}