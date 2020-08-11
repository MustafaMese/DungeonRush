using DG.Tweening;
using DungeonRush.Cards;
using DungeonRush.Data;
using DungeonRush.Property;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush.Attacking
{
    public class GoblinAttacker : MonoBehaviour, IAttacker
    {
        [SerializeField] AttackStyle attackStyle = null;

        [SerializeField] int power = 5;
        [SerializeField] float damageTime = 0.3f;
        [SerializeField] Animator animator = null;
        [SerializeField] GameObject fadingParticul = null;
        [SerializeField] float particulTime = 1f;

        private ObjectPool poolForParticul = new ObjectPool();
        private ObjectPool poolForAttackStyle = new ObjectPool();
        private GameObject effectObject = null;

        private bool attackFinished = false;
        private Card card = null;

        private void Start()
        {
            DOTween.Init();
            card = GetComponent<Card>();
            FillThePool(poolForParticul, fadingParticul, 3);
            effectObject = attackStyle.GetEffect();
            FillThePool(poolForAttackStyle, effectObject, 2);
        }

        private void FillThePool(ObjectPool pool, GameObject effect, int objectCount)
        {
            pool.SetObject(effect);
            pool.FillPool(objectCount);
        }


        public void Attack()
        {
            attackFinished = false;
            Move move = card.GetMove();
            StartCoroutine(FinishAttack(move));
        }

        private IEnumerator FinishAttack(Move move)
        {
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
            StartCoroutine(StartAttackAnimation(poolForParticul, cardPos, card, particulTime));
        }

        public bool CanMove(Card enemy)
        {
            switch (enemy.GetCardType())
            {
                case CardType.PLAYER:
                    return false;
                case CardType.ENEMY:
                    return false;
            }

            return true;
        }

        private IEnumerator StartAttackAnimation(ObjectPool pool, Vector3 tPos, Transform card, float time)
        {
            GameObject obj = pool.PullObjectFromPool();
            attackStyle.SetEffectPosition(obj, tPos, card);
            yield return new WaitForSeconds(time);
            pool.AddObjectToPool(obj);
        }

        public bool GetAttackFinished()
        {
            return attackFinished;
        }
        public AttackStyle GetAttackStyle()
        {
            return attackStyle;
        }

        public void SetAttackFinished(bool b)
        {
            attackFinished = b;
        }

        public void SetAttackStyle(AttackStyle attackStyle)
        {
            poolForAttackStyle.DeleteObjectsInPool();
            this.attackStyle = attackStyle;
            effectObject = attackStyle.GetEffect();
            FillThePool(poolForAttackStyle, effectObject, 2);
        }

        private void UpdateAnimation(bool play, bool isAttack)
        {
            if (card.GetCardType() != CardType.TRAP)
                if (isAttack)
                    animator.SetTrigger("attack");
                else
                    animator.SetBool("walk", play);
        }
    }
}