using DungeonRush.Controller;
using UnityEngine;
using DungeonRush.UI;

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
                print("Başlatıyorum");
                Application.targetFrameRate = 60;

                Instantiate(uiManagerPrefab);
                Instantiate(loadManagerPrefab);
                Instantiate(cardManagerPrefab);
                Instantiate(moveSchedularPrefab);
                Instantiate(collectableManagerPrefab);
                Instantiate(swipeManagerPrefab);

                SetGameState(GameState.BEGIN_LEVEL);
            }

            public void SetGameState(GameState state, UIState uiState = UIState.NONE)
            {
                gameState = state;
                UIManager.Instance.UpdateCanvasState(gameState, uiState);
            }

            void OnDestroy()
            {
                enabled = false;
                started = false;
                gameState = GameState.START;
            }
        }
    }
}
