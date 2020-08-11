using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using DungeonRush.Managers;
using DungeonRush.Saving;

public class ImageCloser : MonoBehaviour
{
    [SerializeField] GameManager gameManager = null;

    [SerializeField] Image image = null;
    [SerializeField] float endValue = 0f;
    private float endTime = 0f;

    private bool touched = false;

    private void Start()
    {
        endTime = GameManager._fadeOutTime;
    }

    private void Update()
    {
        if(!touched && Input.anyKey)
        {
            touched = true;
            Scale();
        }    
    }

    private void Scale()
    {
        image.transform.DOScale(endValue, endTime);
        StartCoroutine(FadeOut());
        StartCoroutine(LoadNewScene());
    }

    private IEnumerator FadeOut()
    {
        yield return gameManager.FadeOut();
    }

    private IEnumerator LoadNewScene()
    {
        SavingSystem.DeletePlayerInstantSaveFile();
        yield return new WaitForSeconds(endTime);
        GameManager.gameState = GameState.START;
    }
}
