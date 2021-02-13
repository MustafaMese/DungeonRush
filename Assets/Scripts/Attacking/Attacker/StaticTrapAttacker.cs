using System.Collections;
using System.Collections.Generic;
using DungeonRush.Attacking;
using DungeonRush.Cards;
using DungeonRush.Data;
using UnityEngine;

namespace DungeonRush.Property
{
    public class StaticTrapAttacker :  Attacker
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

        private void Damage(Move move)
        {
            float time = attackStyle.GetAnimationTime();
            Card tCard = move.GetTargetTile().GetCard();

            if (!tCard.GetStats().CanBlockTraps)
            {
                AttackAction(move);
                StartCoroutine(FinaliseAttack(time));
            }
            else
                StartCoroutine(FinaliseAttack(0));
        }

        private IEnumerator FinaliseAttack(float time)
        {
            yield return new WaitForSeconds(time);
            attackFinished = true;
        }
    }
}
