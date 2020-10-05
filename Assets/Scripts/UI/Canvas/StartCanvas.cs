using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using DungeonRush.Managers;
using DungeonRush.Saving;

namespace DungeonRush.UI
{
    public class StartCanvas : MonoBehaviour
    {
        [SerializeField] Image image = null;

        [SerializeField] float endValue = 0f;
        [SerializeField] float endTime = 0f;
        private bool touched = false;

        public void ExecuteStartButton()
        {
            if (!touched)
            {
                touched = true;
                StartCoroutine(Scale());
            }
        }

        private IEnumerator Scale()
        {
            image.transform.DOScale(endValue, endTime);
            SavingSystem.DeletePlayerInstantSaveFile();
            yield return new WaitForSeconds(endTime);
            GameManager.Instance.SetGameState(GameState.LEVEL_TRANSITION);
        }
    }
}