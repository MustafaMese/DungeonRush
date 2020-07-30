using DungeonRush.Controller;
using UnityEngine;
using System.Collections;

namespace DungeonRush
{
    namespace Managers
    {
        public class GameManager : MonoBehaviour
        {
            public MoveSchedular moveSchedular;
            [SerializeField] CanvasGroup canvasGroup;

            public static GameState gameState = GameState.STOP;

            public bool start;

            private void Start()
            {
                Application.targetFrameRate = 45;
                moveSchedular = FindObjectOfType<MoveSchedular>();

                StartCoroutine(FadeOutIn());
            }

            private void Update()
            {
                if(start)
                {
                    start = false;
                    StartGame();
                }
            }

            private void StartGame()
            {
                gameState = GameState.BEGIN;
            }

            private IEnumerator FadeOutIn()
            {
                yield return FadeOut(3f);
                print("Faded out");
                yield return FadeIn(1f);
                print("Faded in");
            }

            private IEnumerator FadeOut(float time)
            {
                while(canvasGroup.alpha < 1)
                {
                    canvasGroup.alpha += Time.deltaTime / time;
                    yield return null;
                }
            }

            private IEnumerator FadeIn(float time)
            {
                while(canvasGroup.alpha > 0)
                {
                    canvasGroup.alpha -= Time.deltaTime / time;
                    yield return null;
                }
            }

            void OnDestroy()
            {
                gameState = GameState.STOP;                
            }
        }
    }
}
