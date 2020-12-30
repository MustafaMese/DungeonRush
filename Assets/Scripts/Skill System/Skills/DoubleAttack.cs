using DungeonRush.Cards;
using DungeonRush.Data;
using DungeonRush.Property;
using DungeonRush.Skills;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush.Skills
{
    public class DoubleAttack : PassiveSkill
    {
        private Attacker attacker;

        public override void Initialize(Card card)
        {
            base.Initialize(card);
            attacker = card.GetComponent<Attacker>();
        }

        public override void Execute(Move move)
        {
            if(!canExecute) return;

            Card card = move.GetCard();

            if (card != null)
                attacker.AttackAction(move);
        }
    }
}