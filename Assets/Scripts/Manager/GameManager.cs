using DungeonRush.Controller;
using UnityEngine;
using System.Collections;
using DungeonRush.Property;
using DungeonRush.Saving;

namespace DungeonRush
{
    namespace Managers
    {
        public class GameManager : MonoBehaviour
        {
            [SerializeField] CanvasGroup canvasGroup = null;

            public static GameState gameState = GameState.STOP_GAME;
            public bool start = false;

            [SerializeField] float fadeInTime = 1f;
            [SerializeField] float fadeOutTime = 2f;

            public static float _fadeInTime = 0f;
            public static float _fadeOutTime = 0f;

            private void Awake()
            {
                Application.targetFrameRate = 60;
                _fadeInTime = fadeInTime;
                _fadeOutTime = fadeOutTime;
            }

            private void Start()
            {
                StartCoroutine(StartGame());
            }

            private IEnumerator StartGame()
            {
                yield return FadeIn(fadeInTime);
                gameState = GameState.BEGIN_LEVEL;
            }

            public IEnumerator FadeOut()
            {
                while(canvasGroup.alpha < 1)
                {
                    canvasGroup.alpha += Time.deltaTime / fadeOutTime;
                    yield return null;
                }
            }

            public IEnumerator FadeIn(float time)
            {
                while(canvasGroup.alpha > 0)
                {
                    canvasGroup.alpha -= Time.deltaTime / time;
                    yield return null;
                }
            }

            void OnDestroy()
            {
                gameState = GameState.STOP_GAME;
            }
        }
    }
}
