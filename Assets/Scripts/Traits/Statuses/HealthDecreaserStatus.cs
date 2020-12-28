using DungeonRush.Cards;
using UnityEngine;

namespace DungeonRush.Traits
{
    public class HealthDecreaserStatus : Status
    {
        public override void Execute(Card card)
        {
            if(Power > 0)
                card.IncreaseHealth(Power);
            else
                card.DecreaseHealth(Power);
        }
    }
}