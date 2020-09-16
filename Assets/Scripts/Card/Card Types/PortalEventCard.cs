using System.Collections;
using System.Collections.Generic;
using DungeonRush.Managers;
using DungeonRush.Cards;
using UnityEngine;
using DungeonRush.Controller;

public class PortalEventCard : EventCard
{
    public override void GetEvent(Card card)
    {
        Portal();
    }

    private void Portal()
    {
        FindObjectOfType<PlayerController>().SavePlayer();
        GameManager.Instance.SetGameState(GameState.LEVEL_TRANSITION);
    }
}
