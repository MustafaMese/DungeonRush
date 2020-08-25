using DungeonRush.Controller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;


    private void Start()
    {
    }

    void LateUpdate()
    {
        if (target == null)
            target = FindObjectOfType<PlayerController>().transform;

        transform.position = new Vector3(target.position.x, target.position.y + 1, transform.position.z);
    }

}
