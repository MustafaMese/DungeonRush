using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DungeonRush.UI
{
    public class FadingCanvas : MonoBehaviour, ICanvasController
    {
        [SerializeField] float fadeInTime = 1f;
        [SerializeField] float fadeOutTime = 2f;
        [SerializeField] GameObject panel;
        [SerializeField] Image image;
        

        public IEnumerator FadeOut()
        {
            image.material.SetFloat("Vector1_AB1BDB34", 0);

            var value = image.material.GetFloat("Vector1_AB1BDB34");
            gameObject.SetActive(true);
            while (value < 1)
            {
                value += Time.deltaTime / fadeOutTime;
                image.material.SetFloat("Vector1_AB1BDB34", value);
                yield return null;
            }
        }

        public IEnumerator FadeIn()
        {
            image.material.SetFloat("Vector1_AB1BDB34", 1);

            var value = image.material.GetFloat("Vector1_AB1BDB34");
            while (value > 0)
            {
                value -= Time.deltaTime / fadeInTime;
                image.material.SetFloat("Vector1_AB1BDB34", value);
                yield return null;
            }
            gameObject.SetActive(true);
        }

        public void PanelControl(bool activate)
        {
            panel.SetActive(activate);
        }
    }
}