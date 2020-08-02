﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using DungeonRush.Managers;

public class ImageCloser : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] LoadManager loadManager;

    [SerializeField] Image image = null;
    [SerializeField] float endValue = 0f;
    [SerializeField] float endTime = 0f;

    private bool touched = false;
    
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
        yield return gameManager.FadeOut(endTime);
    }

    private IEnumerator LoadNewScene()
    {
        yield return new WaitForSeconds(endTime);
        StartCoroutine(loadManager.LoadNewScene());
    }
}
