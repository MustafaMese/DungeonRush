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
            public MoveSchedular moveSchedular = null;
            [SerializeField] CanvasGroup canvasGroup = null;
            public static GameState gameState = GameState.STOP;
            public bool start = false;

            [SerializeField] float fadeInTime = 0f;

            // silinecekler
            [SerializeField] Health card = null;
            [SerializeField] GameObject text = null;
            [SerializeField] LoadManager l = null;
            private void Start()
            {
                Application.targetFrameRate = 60;
                moveSchedular = FindObjectOfType<MoveSchedular>();

                StartCoroutine(StartGame());
            }

            private void Update()
            {
                if(card != null && card.GetCurrentHealth() <= 0) 
                {
                    StartCoroutine(FinishGame());
                }

            }

            private IEnumerator FinishGame()
            {
                gameState = GameState.STOP;
                text.gameObject.SetActive(true);
                yield return new WaitForSeconds(1f);
                yield return FadeOut(1f);
                l.LoadNextScene();
            }

            private IEnumerator StartGame()
            {
                yield return FadeIn(fadeInTime);
                gameState = GameState.BEGIN;
            }

            public IEnumerator FadeOut(float time)
            {
                while(canvasGroup.alpha < 1)
                {
                    canvasGroup.alpha += Time.deltaTime / time;
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
                gameState = GameState.STOP;
            }
        }
    }
}
