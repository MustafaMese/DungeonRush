using DG.Tweening;
using DungeonRush.Attacking;
using DungeonRush.Cards;
using DungeonRush.Data;
using DungeonRush.Field;
using DungeonRush.Traits;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush.Property
{
    public abstract class Attacker : MonoBehaviour
    {
        public struct StatusActControl
        {
            public int extraDodgeChance;
            public int extraCriticChance;

            public void Reset()
            {
                extraCriticChance = 0;
                extraCriticChance = 0;
            }

            public void ActControl(List<StatusType> list)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i] == StatusType.SLOWED)
                    {
                        extraDodgeChance--;
                        extraCriticChance--;
                    }
                    else if (list[i] == StatusType.HASTE)
                    {
                        extraCriticChance++;
                        extraDodgeChance++;
                    }
                }
            }
        }
        protected StatusActControl statusAct;

        [SerializeField] protected AttackStyle attackStyle = null;
        [SerializeField] Animator animator = null;

        public int power = 0;
        protected bool attackFinished = false; 
        
        protected ObjectPool poolForAttackStyle = new ObjectPool();
        protected GameObject effectObject = null;
        protected Card card = null;
        protected StatusController statusController = null;

        private void Start()
        {
            DOTween.Init();
            card = GetComponent<Card>();
            statusController = card.GetComponent<StatusController>();
            effectObject = attackStyle.GetEffect();
            FillThePool(poolForAttackStyle, effectObject, 1);
            power = attackStyle.GetPower();
            statusAct = new StatusActControl();
            Initialize();
        }

        protected virtual void Initialize() { }

        public abstract void Attack();

        // Saldırı eylemi için false, ilerleme eyleme için true.
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

        protected bool IsCriticAttack()
        {
            int criticChance = card.CriticChance + statusAct.extraCriticChance;
            criticChance *= 2;

            if (criticChance > 0)
            {
                int number = Random.Range(0, 100);
                if (number <= criticChance)
                    return true;
            }
            return false;
        }

        protected virtual IEnumerator StartAttackAnimation(ObjectPool pool, Move move, float time)
        {
            Transform cardTransform = move.GetCard().transform;
            Transform target = move.GetTargetTile().transform;

            if (move.GetTargetTile().GetCard() != null)
            {
                GameObject obj = pool.PullObjectFromPool(cardTransform);
                attackStyle.SetEffectPosition(obj, target.position, target);
                yield return new WaitForSeconds(time);
                attackStyle.SetEffectPosition(obj, target.position, cardTransform);
                pool.AddObjectToPool(obj);
            }
        }

        public bool GetAttackFinished()
        {
            return attackFinished;
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
            power = attackStyle.GetPower();
        }
        protected bool DoAttackAction(Move move)
        {
            bool isCritic = IsCriticAttack();

            if (!isCritic)
                attackStyle.Attack(move, power);
            else
                attackStyle.Attack(move, power * 2);
            return isCritic;
        }

        protected void FillThePool(ObjectPool pool, GameObject effect, int objectCount)
        {
            pool.SetObject(effect);
            pool.FillPool(objectCount, transform);
        }

        public AttackStyle GetAttackStyle()
        {
            return attackStyle;
        }

        protected void UpdateAnimation(bool play, bool isAttack)
        {
            if (card.GetCardType() != CardType.TRAP)
                if (isAttack)
                    animator.SetTrigger("attack");
                else
                    animator.SetBool("walk", play);
        }

        protected Vector3 GetDirection(Move move)
        {
            var heading = move.GetTargetTile().GetCoordinate() - move.GetCardTile().GetCoordinate();
            var distance = heading.magnitude;
            var direction = heading / distance;
            return direction;
        }

        protected bool IsMissed(Card card)
        {
            if (card == null) return false;

            int dodgeChance = card.DodgeChance + statusAct.extraDodgeChance;
            dodgeChance *= 2;

            if (dodgeChance > 0)
            {
                int number = Random.Range(0, 100);
                if (number <= dodgeChance)
                    return true;
            }
            return false;
        }

        public void AttackAction(Move move)
        {
            Card card = move.GetCard();
            Tile target = move.GetTargetTile();
            Vector3 tPos = target.transform.position;
            statusAct.Reset();
            statusAct.ActControl(statusController.statusTypes);

            if (move.GetTargetTile().GetCard() != null)
            {
                bool isMissed = IsMissed(target.GetCard());
                if (isMissed)
                    StartCoroutine(card.StartTextPopup(tPos, "MISS"));
                else
                {
                    bool isCritic = DoAttackAction(move);
                    StartCoroutine(card.StartTextPopup(tPos, power, isCritic));
                }
            }
            float time = attackStyle.GetAnimationTime();
            StartCoroutine(StartAttackAnimation(poolForAttackStyle, move, time));
        }

        protected void AttackAction(List<Card> targetCards, Move move)
        {
            for (int i = 0; i < targetCards.Count; i++)
            {
                Card tCard = targetCards[i];
                Vector3 tPos = tCard.transform.position;

                if (tCard != null)
                {
                    bool isMissed = IsMissed(tCard);
                    if (isMissed)
                        StartCoroutine(tCard.StartTextPopup(tPos, "MISS"));
                    else
                    {
                        bool isCritic = DoAttackAction(move);
                        StartCoroutine(tCard.StartTextPopup(tPos, power, isCritic));
                    }
                }
            }
            float time = attackStyle.GetAnimationTime();
            StartCoroutine(StartAttackAnimation(poolForAttackStyle, move, time));
        }
    }
}