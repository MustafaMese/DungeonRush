using System.Collections;
using System.Collections.Generic;
using DungeonRush.Attacking;
using DungeonRush.Cards;
using DungeonRush.Data;
using UnityEngine;

namespace DungeonRush.Property
{
    public class StaticAttacker :  Attacker
    {
        protected override void Initialize()
        {
            return;
        }

        public override void Attack()
        {
            attackFinished = false;
            Move move = card.GetMove();
            Damage(move);
        }

        private void Damage(Move move)
        {
            float time = attackStyle.GetAnimationTime();
            Card tCard = move.GetTargetTile().GetCard();

            if (!tCard.CanBlockTraps)
            {
                AttackAction(move);
                StartCoroutine(FinaliseAttack(time));
            }
        }

        private IEnumerator FinaliseAttack(float time)
        {
            yield return new WaitForSeconds(time);
            attackFinished = true;
        }
    }
}
