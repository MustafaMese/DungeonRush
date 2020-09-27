﻿using DungeonRush.Controller;
using DungeonRush.Items;
using DungeonRush.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush.UI
{
    public class UIManager : MonoBehaviour
    {
        private static UIManager instance = null;
        // Game Instance Singleton
        public static UIManager Instance
        {
            get { return instance; }
            set { instance = value; }
        }

        [SerializeField] PauseMenu pauseMenuPrefab;
        [SerializeField] DefeatedPanel defeatedPanelPrefab;
        [SerializeField] PickItemCanvas pickItemCanvasPrefab;
        [SerializeField] FadingCanvas fadingCanvasPrefab;
        [SerializeField] TurnCanvas turnCanvasPrefab;

        private PauseMenu _pauseMenu;
        private DefeatedPanel _defeatedPanel;
        private PickItemCanvas _pickItemCanvas;
        private FadingCanvas _fadingCanvas;
        private TurnCanvas _turnCanvas;

        private void Awake()
        {
            Instance = this;
        }

        public void UpdateCanvasState(GameState gameState, UIState uiState)
        {
            switch (gameState)
            {
                case GameState.START:
                    print("Oyun sahnesi açılır ve beklemede kalır. Burada bazı işlemler yapılabilir.");
                    break;
                case GameState.BEGIN_LEVEL:
                    StartCoroutine(BeginLevel());
                    break;
                case GameState.PAUSE:
                    Pause();
                    _turnCanvas.PanelControl(false);

                    if (uiState == UIState.PICKUP_ITEM)
                        _pauseMenu.PanelControl(false);

                    break;
                case GameState.PLAY:
                    Resume();
                    _turnCanvas.PanelControl(true);
                    _pauseMenu.PanelControl(true);
                    break;
                case GameState.LEVEL_TRANSITION:
                    _fadingCanvas.PanelControl(true);
                    StartCoroutine(NextLevel());
                    break;
                case GameState.DEFEAT:
                    _defeatedPanel.Defeat();

                    _turnCanvas.PanelControl(false);
                    _pauseMenu.PanelControl(false);
                    break;
                case GameState.END:
                    _fadingCanvas.PanelControl(true);
                    StartCoroutine(EndGame());
                    break;
                default:
                    break;
            }
        }

        private IEnumerator BeginLevel()
        {
            if (_fadingCanvas == null)
                InitializeCanvases();

            yield return _fadingCanvas.FadeIn();
            _fadingCanvas.PanelControl(false);
            MoveSchedular.Instance.StartGame();
        }

        private IEnumerator NextLevel()
        {
            yield return _fadingCanvas.FadeOut();
            GameManager.Instance.SetGameState(GameState.START);
            _fadingCanvas = null;
            LoadManager.Instance.LoadNextScene();
        }

        private IEnumerator EndGame()
        {
            yield return _fadingCanvas.FadeOut();
            _fadingCanvas = null;
            LoadManager.Instance.LoadStartScene();
        }

        private void Pause()
        {
            Time.timeScale = 0f;
        }

        private void Resume()
        {
            Time.timeScale = 1f;
        }

        private void InitializeCanvases()
        {
            _fadingCanvas = Instantiate(fadingCanvasPrefab);
            _pauseMenu = Instantiate(pauseMenuPrefab);
            _defeatedPanel = Instantiate(defeatedPanelPrefab);
            _pickItemCanvas = Instantiate(pickItemCanvasPrefab);
            _turnCanvas = Instantiate(turnCanvasPrefab);
        }

        #region TURN CANVAS METHODS

        public void InitalizeEnemyTurn(List<AIController> enemies)
        {
            _turnCanvas.ChangeText(false);
            _turnCanvas.SetCardIcons(enemies);
        }

        public void ChangeIcons(int index)
        {
            if (index > 0)
                _turnCanvas.Next();
            else
                _turnCanvas.SetImages(0);
        }

        public void InitializePlayerTurn()
        {
            _turnCanvas.Initialize();
            _turnCanvas.ChangeText(true);
        }
        #endregion

        #region PICK ITEM CANVAS

        public void EnableItemCanvas(Item i)
        {
            _pickItemCanvas.EnablePanel(i);
            GameManager.Instance.SetGameState(GameState.PAUSE, UIState.PICKUP_ITEM);
        }

        #endregion

    }
}