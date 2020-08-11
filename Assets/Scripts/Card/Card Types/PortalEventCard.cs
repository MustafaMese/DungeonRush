using System.Collections;
using System.Collections.Generic;
using DungeonRush.Managers;
using DungeonRush.Cards;
using UnityEngine;
using DungeonRush.Controller;

public class PortalEventCard : EventCard
{
    [SerializeField] GameManager gameManager = null;

    public override void GetEvent(Card card)
    {
        GameManager.gameState = GameState.STOP_GAME;
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        yield return gameManager.FadeOut();
        FindObjectOfType<PlayerController>().SavePlayer();
        GameManager.gameState = GameState.LEVEL_TRANSITION;
    }
}
