using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush.UI
{
    public class FadingCanvas : MonoBehaviour, ICanvasController
    {
        [SerializeField] CanvasGroup canvasGroup = null;
        [SerializeField] float fadeInTime = 1f;
        [SerializeField] float fadeOutTime = 2f;

        [SerializeField] GameObject panel;

        public IEnumerator FadeOut()
        {
            canvasGroup.gameObject.SetActive(true);
            while (canvasGroup.alpha < 1)
            {
                canvasGroup.alpha += Time.deltaTime / fadeOutTime;
                yield return null;
            }
        }

        public IEnumerator FadeIn()
        {
            while (canvasGroup.alpha > 0)
            {
                canvasGroup.alpha -= Time.deltaTime / fadeInTime;
                yield return null;
            }
            canvasGroup.gameObject.SetActive(false);
        }

        public void PanelControl(bool activate)
        {
            panel.SetActive(activate);
        }
    }
}