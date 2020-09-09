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
        public override void Attack()
        {
            attackFinished = false;
            Move move = card.GetMove();
            Damage(move);
        }

        private void Damage(Move move)
        {
            Vector3 cardPos = move.GetCardTile().GetCoordinate();
            float time = attackStyle.GetAnimationTime();

            attackStyle.Attack(move, power);
            StartCoroutine(StartAttackAnimation(poolForAttackStyle, cardPos, time));
            StartCoroutine(card.StartTextPopup(cardPos, power));
        }

        private IEnumerator StartAttackAnimation(ObjectPool pool, Vector3 tPos, float time)
        {
            GameObject obj = pool.PullObjectFromPool();
            attackStyle.SetEffectPosition(obj, tPos);
            yield return new WaitForSeconds(time);
            pool.AddObjectToPool(obj);
            FinaliseAttack();
        }

        private void FinaliseAttack()
        {
            attackFinished = true;
        }
    }
}
