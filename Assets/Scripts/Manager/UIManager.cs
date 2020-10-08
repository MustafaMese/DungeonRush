using DungeonRush.Controller;
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
        [SerializeField] PickItemCanvas pickItemCanvasPrefab;
        [SerializeField] FadingCanvas fadingCanvasPrefab;
        [SerializeField] TurnCanvas turnCanvasPrefab;
        [SerializeField] ActiveSkillCanvas activeSkillCanvasPrefab;
        
        private ActiveSkillCanvas _activeSkillCanvas;
        private PauseMenu _pauseMenu;
        private DefeatedPanel _defeatedPanel;
        private PickItemCanvas _pickItemCanvas;
        private FadingCanvas _fadingCanvas;
        private TurnCanvas _turnCanvas;

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
            if (number == 0)
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

                    if (uiState == UIState.PICKUP_ITEM)
                        _pauseMenu.PanelControl(false);

                    break;
                case GameState.PLAY:
                    Resume();
                    _turnCanvas.PanelControl(true);
                    _pauseMenu.PanelControl(true);
                    _activeSkillCanvas.PanelControl(true);
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

                if (_pickItemCanvas == null)
                    _pickItemCanvas = Instantiate(pickItemCanvasPrefab);

                if (_turnCanvas == null)
                    _turnCanvas = Instantiate(turnCanvasPrefab);

                if (_activeSkillCanvas == null)
                    _activeSkillCanvas = Instantiate(activeSkillCanvasPrefab);
            }
        }

        #region LEVEL CONTROL METHODS

        private IEnumerator BeginLevel()
        {
            if (_fadingCanvas == null)
            {
                print("fading");
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

        #region ACTIVE SKİLL CANVAS METHODS

        public void ButtonControl(SkillData skill, bool enable)
        {
            _activeSkillCanvas.EnableDisableButton(skill, enable);
        }

        public void AddSkillToButton(SkillData skill)
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

        public void AddSkillToSkillSet(Sprite sprite)
        {
            _pauseMenu.AddImageToPanel(sprite);
        }

        private void Pause()
        {
            Time.timeScale = 0f;
        }

        private void Resume()
        {
            Time.timeScale = 1f;
        }

        #endregion
    }
}
