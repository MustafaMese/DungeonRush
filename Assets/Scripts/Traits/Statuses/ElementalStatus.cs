using System.Collections;
using DungeonRush.Cards;
using DungeonRush.Data;
using DungeonRush.Field;
using UnityEngine;

namespace DungeonRush.Traits
{
    public class ElementalStatus : Status
    {
        public override void Execute(Card card, Tile tile)
        {
            if(card != null)
            {
                if (Power > 0)
                    card.GetDamagable().IncreaseHealth(Power);
                else
                    card.GetDamagable().DecreaseHealth(Power);
            }
            
        }
     }
}