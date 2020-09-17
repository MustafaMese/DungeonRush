using System.Collections;
using System.Collections.Generic;
using DungeonRush.Cards;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Status/HealthDecreaser")]
public class HealthDecreaserStatus : Status
{
    public override void Execute(Card card)
    {
        card.DecreaseHealth(Power);
    }
}
