using DungeonRush.Controller;
using UnityEngine;
using DungeonRush.UI;
using DungeonRush.Camera;

namespace DungeonRush
{
    namespace Managers
    {
        public class GameManager : MonoBehaviour
        {
            private static GameManager instance = null;
            // Game Instance Singleton
            public static GameManager Instance
            {
                get
                {
                    return instance;
                }
                set { instance = value; }
            }

            public GameState gameState = GameState.DEFEAT;

            [SerializeField] UIManager uiManagerPrefab;
            [SerializeField] LoadManager loadManagerPrefab;
            [SerializeField] CardManager cardManagerPrefab;
            [SerializeField] MoveSchedular moveSchedularPrefab;
            [SerializeField] CollectableManager collectableManagerPrefab;
            [SerializeField] SwipeManager swipeManagerPrefab;
            [SerializeField] PlayerCamera playerCameraPrefab;
            [SerializeField] SoundManager soundManagerPrefab;
            [SerializeField] TextPopupManager textPopupManager;
            [SerializeField] EffectOperator effectOperator;

            public int targetFrameRate = 60;

            private bool started = false;
            private void Awake()
            {
                if (Instance != null)
                    Destroy(Instance);
                else
                {
                    Instance = this;
                    DontDestroyOnLoad(this);
                }

                Initialize();
            }

            private void OnLevelWasLoaded(int level)
            {
                if (!started)
                {
                    started = true;
                    Initialize();
                }
            }

            protected void Initialize()
            {
                Application.targetFrameRate = targetFrameRate;

                Instantiate(uiManagerPrefab);
                Instantiate(loadManagerPrefab);
                Instantiate(swipeManagerPrefab);
                //Instantiate(soundManagerPrefab);

                var number = LoadManager.GetSceneIndex();
                if (number > 1)
                {
                    Instantiate(cardManagerPrefab);
                    Instantiate(moveSchedularPrefab);
                    Instantiate(collectableManagerPrefab);
                    Instantiate(playerCameraPrefab);
                    Instantiate(textPopupManager);
                    Instantiate(effectOperator);

                    SetGameState(GameState.BEGIN_LEVEL);
                }
            }

            public void SetGameState(GameState state, UIState uiState = UIState.NONE)
            {
                if (state == GameState.LEVEL_TRANSITION || state == GameState.END)
                    started = false;

                gameState = state;
                UIManager.Instance.UpdateCanvasState(gameState, uiState);
            }

            void OnDestroy()
            {
                enabled = false;
                gameState = GameState.START;
            }
        }
    }
}
