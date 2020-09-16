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
                Initialize();
            }

            protected void Initialize()
            {
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
                gameState = GameState.START;
            }
        }
    }
}
