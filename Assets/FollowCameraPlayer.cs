using DG.Tweening;
using DungeonRush.Cards;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCameraPlayer : MonoBehaviour
{
    PlayerCard player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        player = FindObjectOfType<PlayerCard>();
        transform.DOMove(new Vector3(player.transform.position.x, player.transform.position.y, -10), 0.5f);
       // transform.DOLookAt(player.transform.position, 0);
    }
}
