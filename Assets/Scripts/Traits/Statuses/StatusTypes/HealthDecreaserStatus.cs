﻿using DungeonRush.Cards;
using UnityEngine;

namespace DungeonRush.Traits
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Status/HealthDecreaser")]
    public class HealthDecreaserStatus : Status
    {
        public override void Execute(Card card, bool lastTurn = false)
        {
            card.DecreaseHealth(Power);
        }
    }
}