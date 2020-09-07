using DG.Tweening;
using DungeonRush.Cards;
using DungeonRush.Data;
using DungeonRush.Property;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush.Attacking
{
    public class GoblinAttacker : Attacker
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
            Transform card = move.GetCard().transform;
            Vector3 cardPos = card.position;
            float time = attackStyle.GetAnimationTime();

            attackStyle.Attack(move, power);
            StartCoroutine(StartAttackAnimation(poolForAttackStyle, cardPos, card, time));
            List<Card> cards = attackStyle.GetAttackedCards();
            for (int i = 0; i < cards.Count; i++)
            {
                Vector3 pos = cards[i].transform.position;
                StartCoroutine(StartTextPopup(poolForTextPopup, pos, power));
            }
        }

        private IEnumerator StartAttackAnimation(ObjectPool pool, Vector3 tPos, Transform card, float time)
        {
            GameObject obj = pool.PullObjectFromPool();
            attackStyle.SetEffectPosition(obj, tPos, card);
            yield return new WaitForSeconds(time);
            pool.AddObjectToPool(obj);
        }
    }
}