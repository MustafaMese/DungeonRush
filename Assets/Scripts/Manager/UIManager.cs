﻿using DungeonRush.Controller;
using DungeonRush.Items;
using DungeonRush.Managers;
using DungeonRush.Skills;
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
        [SerializeField] PickCanvas pickItemCanvasPrefab;
        [SerializeField] FadingCanvas fadingCanvasPrefab;
        [SerializeField] TurnCanvas turnCanvasPrefab;
        [SerializeField] ActiveSkillCanvas activeSkillCanvasPrefab;
        [SerializeField] ChoiceCanvas choiceCanvasPrefab;
        [SerializeField] RewardCanvas rewardCanvasPrefab;
        
        private ActiveSkillCanvas _activeSkillCanvas;
        private PauseMenu _pauseMenu;
        private DefeatedPanel _defeatedPanel;
        private PickCanvas _pickCanvas;
        private FadingCanvas _fadingCanvas;
        private TurnCanvas _turnCanvas;
        private ChoiceCanvas _choiceCanvas;
        private RewardCanvas _rewardCanvas;

        private void Awake()
        {
            if (Instance != null)
                Destroy(Instance);
            else
                Instance = this;

            InitializeByBuildIndex();
        }

        private void InitializeByBuildIndex()
        {
            var number = LoadManager.GetSceneIndex();
            if (number < 2)
            {
                InitializeCanvases(true);
                StartCoroutine(_fadingCanvas.FadeIn());
            }
            else
                InitializeCanvases(false);
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
                    _activeSkillCanvas.PanelControl(false);

                    if (uiState == UIState.PICKUP || uiState == UIState.CHOICE)
                        _pauseMenu.PanelControl(false);

                    break;
                case GameState.PLAY:
                    Resume();
                    _turnCanvas.PanelControl(true);
                    _pauseMenu.PanelControl(true);
                    _activeSkillCanvas.PanelControl(true);
                    break;
                case GameState.REWARD:
                    _turnCanvas.PanelControl(false);
                    _pauseMenu.PanelControl(false);
                    _activeSkillCanvas.PanelControl(false);

                    _rewardCanvas.PanelControl(true);                
                    break;
                case GameState.LEVEL_TRANSITION:
                    _fadingCanvas.PanelControl(true);
                    
                    StartCoroutine(NextLevel());
                    break;
                case GameState.DEFEAT:
                    _turnCanvas.PanelControl(false);
                    _pauseMenu.PanelControl(false);

                    _defeatedPanel.Defeat();
                    break;
                case GameState.END:
                    _fadingCanvas.PanelControl(true);
                    StartCoroutine(EndGame());
                    break;
                default:
                    break;
            }
        }
        
        private void InitializeCanvases(bool isStartMenu)
        {
            if (_fadingCanvas == null)
                _fadingCanvas = Instantiate(fadingCanvasPrefab);

            if (!isStartMenu)
            {
                if (_pauseMenu == null)
                    _pauseMenu = Instantiate(pauseMenuPrefab);

                if (_defeatedPanel == null)
                    _defeatedPanel = Instantiate(defeatedPanelPrefab);

                if (_pickCanvas == null)
                    _pickCanvas = Instantiate(pickItemCanvasPrefab);

                if (_turnCanvas == null)
                    _turnCanvas = Instantiate(turnCanvasPrefab);

                if (_activeSkillCanvas == null)
                    _activeSkillCanvas = Instantiate(activeSkillCanvasPrefab);

                if(_choiceCanvas == null)
                    _choiceCanvas = Instantiate(choiceCanvasPrefab);

                if(_rewardCanvas == null)
                    _rewardCanvas = Instantiate(rewardCanvasPrefab);
            }
        }

        public void EnableChoiceCanvas(Item item ,Item loot, ItemUser itemUser)
        {
            _choiceCanvas.EnablePanel(item, loot, itemUser);
            GameManager.Instance.SetGameState(GameState.PAUSE, UIState.CHOICE);
        }

        #region LEVEL CONTROL METHODS

        private IEnumerator BeginLevel()
        {
            if (_fadingCanvas == null)
            {
                InitializeByBuildIndex();
            }

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

        #endregion

        #region TURN CANVAS METHODS

        public void InitalizeEnemyTurn(List<EnemyAIController> enemies)
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

        #region PICK CANVASES

        public void EnableItemCanvas(Item i)
        {
            _pickCanvas.EnablePanel(i);
            GameManager.Instance.SetGameState(GameState.PAUSE, UIState.PICKUP);
        }

        public void EnableSkillCanvas(Skill s)
        {
            _pickCanvas.EnablePanel(s);
            GameManager.Instance.SetGameState(GameState.PAUSE, UIState.PICKUP);
        }

        #endregion

        #region ACTIVE SKİLL CANVAS METHODS

        public void ButtonControl(Skill skill, bool enable)
        {
            _activeSkillCanvas.EnableDisableButton(skill, enable);
        }

        public void AddSkillToButton(Skill skill)
        {
            if (_activeSkillCanvas == null)
            {
                print("active");
                InitializeByBuildIndex();
            }

            _activeSkillCanvas.AddSkill(skill);
        }

        #endregion

        #region PAUSE CANVAS METHODS

        public void AddToSkillSet(Sprite sprite)
        {
            _pauseMenu.AddImageToPanel(sprite, true);
        }

        public void AddToItemSet(Sprite sprite)
        {
            _pauseMenu.AddImageToPanel(sprite, false);
        }

        #endregion

        public void Pause()
        {
            Time.timeScale = 0f;
        }

        public void Resume()
        {
            Time.timeScale = 1f;
        }

        public void ChangeFrameRate(bool reduce)
        {
            if(reduce)
                Application.targetFrameRate = 7;
            else
                Application.targetFrameRate = GameManager.Instance.targetFrameRate;
        }
    }
}
