using System.Collections;
using System.Collections.Generic;
using DungeonRush.Cards;
using UnityEngine;

public class HealStatus : Status
{
    public override void Execute(Card card)
    {
        card.IncreaseHealth(Power);
    }
}
