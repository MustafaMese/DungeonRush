using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using DungeonRush.Managers;
using DungeonRush.Saving;

namespace DungeonRush.UI
{
    public class ImageCloser : MonoBehaviour
    {
        [SerializeField] Image image = null;
        [SerializeField] float endValue = 0f;
        private float endTime = 0f;

        private bool touched = false;

        private void Update()
        {
            if (!touched && Input.anyKey)
            {
                touched = true;
                Scale();
            }
        }

        private void Scale()
        {
            image.transform.DOScale(endValue, endTime);
            SavingSystem.DeletePlayerInstantSaveFile();
            GameManager.Instance.SetGameState(GameState.LEVEL_TRANSITION);
        }
    }
}