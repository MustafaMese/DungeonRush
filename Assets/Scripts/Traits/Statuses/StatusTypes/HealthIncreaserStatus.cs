using System.Collections;
using System.Collections.Generic;
using DungeonRush.Cards;
using UnityEngine;

namespace DungeonRush.Traits
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Status/HealthIncreaser")]
    public class HealthIncreaserStatus : Status
    {
        public override void Execute(Card card)
        {
            card.IncreaseHealth(Power);
        }
    }
}