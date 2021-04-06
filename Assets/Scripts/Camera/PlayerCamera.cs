using DungeonRush.Controller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace DungeonRush.Camera
{
    public class PlayerCamera : MonoBehaviour
    {
        private static PlayerCamera instance = null;
        // Game Instance Singleton
        public static PlayerCamera Instance
        {
            get { return instance; }
            set { instance = value; }
        }

        private void Awake()
        {
            Instance = this;
        }

        [SerializeField] float moveDuration = 0.5f;

        private void Start()
        {
            DOTween.Init();
            GetComponent<AudioListener>().enabled = true;
        }


        public void MoveCamera(Vector3 targetPosition)
        {
            transform.DOKill();

            // TODO Burada bi randomizasyon uygulanabilir.
            targetPosition = new Vector3(targetPosition.x, targetPosition.y + 1, transform.position.z);
            transform.DOMove(targetPosition, moveDuration);
            //SoundManager.Instance.transform.position = transform.position;
        }

        public void CameraShake(float duration, float magnitude)
        {
            StartCoroutine(Shake(duration, magnitude));
        }

        private IEnumerator Shake(float duration, float magnitude)
        {
            Vector3 originalPos = transform.localPosition;
            float elapsed = 0.0f;
            while(elapsed < duration)
            {
                float x = Random.Range(-1f, 1f) * magnitude;
                float y = Random.Range(-1f, 1f) * magnitude;

                transform.localPosition = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);
                elapsed += Time.deltaTime;
                yield return null;
            }
            transform.localPosition = originalPos;
        }

    }
}
