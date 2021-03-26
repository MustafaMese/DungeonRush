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
        [SerializeField] Transform skillSetPanel;
        [SerializeField] Transform itemSetPanel;

        [Header("Character Stats")]
        [SerializeField] TextMeshProUGUI healthTxt = null;
        [SerializeField] TextMeshProUGUI damageTxt = null;
        [SerializeField] TextMeshProUGUI criticChance = null;
        [SerializeField] TextMeshProUGUI dodgeChance = null;

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

            int critic = player.GetStats().CriticChance;
            SetText(criticChance, critic);

            int dodge = player.GetStats().DodgeChance;
            SetText(dodgeChance, dodge);

            List<string> names = player.GetItemNames();
            //SetText(itemsTxt, names);
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
            UIManager.Instance.ChangeFrameRate(false);

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

            UIManager.Instance.ChangeFrameRate(true);
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

        public Image AddImageToPanel(Sprite sprite, bool isSkill)
        {
            var obj = new GameObject("Image").AddComponent<Image>();
            obj.sprite = sprite;
            if(isSkill)
                return Instantiate(obj, skillSetPanel);
            else
                return Instantiate(obj, itemSetPanel);
        }
    }
}