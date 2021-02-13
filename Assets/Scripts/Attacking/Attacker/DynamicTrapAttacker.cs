using System.Collections;
using System.Collections.Generic;
using DungeonRush.Cards;
using DungeonRush.Data;
using DungeonRush.Field;
using DungeonRush.Property;
using UnityEngine;

namespace DungeonRush.Attacking
{
    public class DynamicTrapAttacker : Attacker
    {
        protected override void Initialize()
        {
            return;
        }

        public override void Attack()
        {
            attackFinished = false;
            Damage(card.GetMove());
        }

        private IEnumerator FinaliseAttack(float time)
        {
            yield return new WaitForSeconds(time);
            attackFinished = true;
        }

        private void Damage(Move move)
        {
            float time = attackStyle.GetAnimationTime();
            Card tCard = move.GetTargetTile().GetCard();
            
            if(!tCard.GetStats().CanBlockTraps)
            {
                AttackAction(move);
                StartCoroutine(FinaliseAttack(time));
            }
            else
                StartCoroutine(FinaliseAttack(0));
        }
    }
}


