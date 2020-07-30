using System.Collections;
using System.Collections.Generic;
using DungeonRush.Property;
using DungeonRush.Managers;
using DungeonRush.Cards;
using UnityEngine;

public class TreasureEventCard : EventCard
{
    [SerializeField] ItemCard Item;

    public override void GetEvent(Card card)
    {
        return item.GetComponent<Item>();
    }
}
