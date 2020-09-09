﻿using DG.Tweening;
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
            float time = attackStyle.GetAnimationTime();
            StartCoroutine(StartAttackAnimation(poolForAttackStyle, move, time));

            List<Card> cards = attackStyle.GetAttackedCards(move);
            AttackAction(cards, move);
        }

        protected override IEnumerator StartAttackAnimation(ObjectPool pool, Move move, float time)
        {
            Transform cardTransform = move.GetCard().transform;

            GameObject obj = pool.PullObjectFromPool();
            attackStyle.SetEffectPosition(obj, cardTransform.position, cardTransform);
            yield return new WaitForSeconds(time);
            pool.AddObjectToPool(obj);
        }
    }
}