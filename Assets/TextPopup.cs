﻿using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextPopup : MonoBehaviour
{
    [SerializeField] float xSpace = 0;
    [SerializeField] float ySpace = 0;

    [SerializeField] TextMeshPro textMeshPro = null;
    [SerializeField] Color normalHit;
    [SerializeField] Color criticalHit;
    [SerializeField] float disapperTime = 0;

    private void Start()
    {
        DOTween.Init();
    }

    public void Setup(int damage, Vector3 target, bool isCriticalHit = false)
    {
        if (!isCriticalHit)
            textMeshPro.color = normalHit;
        else
            textMeshPro.color = criticalHit;
        print("Ben burdayım");
        textMeshPro.text = damage.ToString();
        Move(target);
    }

    private void Move(Vector3 target)
    {
        Vector2 targetPos = new Vector2(target.x + xSpace, target.y + ySpace);
        print(targetPos);
        transform.DOMove(targetPos, disapperTime);
    }

    public float GetDisapperTime()
    {
        return disapperTime;
    }
}
