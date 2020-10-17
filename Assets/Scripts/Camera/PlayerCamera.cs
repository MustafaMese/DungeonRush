using DungeonRush.Controller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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

    [SerializeField] float duration = 0.5f;

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
        transform.DOMove(targetPosition, duration);
        //SoundManager.Instance.transform.position = transform.position;
    }

}
