using System.Collections;
using System.Collections.Generic;
using DungeonRush.Attacking;
using DungeonRush.Cards;
using DungeonRush.Field;
using DungeonRush.Property;
using UnityEngine;

namespace DungeonRush.Traits
{
    public class AttackStyleStatus : Status
    {
        [SerializeField] AttackStyle attackStyle;

        public override void Execute(Card card, Tile tile = null)
        {
            if (card != null)
                card.GetComponent<Attacker>().ChangeAttackStyleOneTurn(false, attackStyle);
        }
    }

}