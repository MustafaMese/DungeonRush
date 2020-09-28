using DG.Tweening;
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
    [SerializeField] Color missHit;
    [SerializeField] float disapperTime = 0;

    private void Start()
    {
        DOTween.Init();
    }

    public void Setup(int damage, Vector3 target, bool isCriticalHit = false)
    {
        if (!isCriticalHit)
        {
            textMeshPro.color = normalHit;
            textMeshPro.text = damage.ToString();
        }
        else
        {
            textMeshPro.color = criticalHit;
            damage = damage * 2;
            textMeshPro.text = damage.ToString();
        }
        Move(target);
    }

    public void Setup(string text, Vector3 target)
    {
        textMeshPro.color = missHit;
        textMeshPro.text = text;
        Move(target);
    }

    private void Move(Vector3 target)
    {
        float number = Random.Range(-xSpace * 2, xSpace * 2);
        Vector2 targetPos = new Vector2(target.x + number, target.y + ySpace);
        transform.DOMove(targetPos, disapperTime).OnComplete(() => FinishMove());
    }

    private void FinishMove()
    {
        if(gameObject.activeSelf)
            gameObject.SetActive(false);
    }

    public float GetDisapperTime()
    {
        return disapperTime;
    }
}
