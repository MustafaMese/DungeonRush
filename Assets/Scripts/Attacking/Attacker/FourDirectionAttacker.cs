using DG.Tweening;
using DungeonRush.Cards;
using DungeonRush.Data;
using DungeonRush.Managers;
using DungeonRush.Property;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush.Attacking
{
    public class FourDirectionAttacker : Attacker
    {
        [Header("Attacker Properties")]
        [SerializeField] float damageTime = 0.3f;

        public override void Attack()
        {
            attackFinished = false;
            Move move = card.GetMove();
            StartCoroutine(FinishAttack(move));
        }

        private IEnumerator FinishAttack(Move move)
        {
            UpdateAnimation(true, true);
            Damage(move);
            yield return new WaitForSeconds(damageTime);
            attackFinished = true;
        }

        private void Damage(Move move)
        {
            List<Card> cards = attackStyle.GetAttackedCards(move);
            AttackAction(cards, move);
        }

        protected override void AttackEffect(ObjectPool<GameObject> pool, Move move, float time)
        {
            if (move.GetTargetTile().GetCard() != null)
            {
                Transform cardTransform = move.GetCard().transform;
                EffectOperator.Instance.Operate(pool, cardTransform, cardTransform, time, attackStyle);
            }
        }
    }
}