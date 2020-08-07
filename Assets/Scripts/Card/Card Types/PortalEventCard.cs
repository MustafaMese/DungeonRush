﻿using System.Collections;
using System.Collections.Generic;
using DungeonRush.Managers;
using DungeonRush.Cards;
using UnityEngine;
using DungeonRush.Controller;

public class PortalEventCard : EventCard
{
    [SerializeField] LoadManager loadManager = null;
    [SerializeField] GameManager gameManager = null;

    public override void GetEvent(Card card)
    {
        GameManager.gameState = GameState.STOP;
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        yield return gameManager.FadeOut(2f);
        FindObjectOfType<PlayerController>().SavePlayer();
        print("FadeOut");
        loadManager.LoadNextScene();
    }
}
