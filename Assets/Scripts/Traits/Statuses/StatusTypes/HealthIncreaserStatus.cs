using System.Collections;
using System.Collections.Generic;
using DungeonRush.Cards;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Status/HealthIncreaser")]
public class HealthIncreaserStatus : Status
{
    public override void Execute(Card card)
    {
        card.IncreaseHealth(Power);
    }
}
