using DG.Tweening;
using DungeonRush.Attacking;
using DungeonRush.Cards;
using DungeonRush.Data;
using DungeonRush.Field;
using DungeonRush.Managers;
using DungeonRush.Traits;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush.Property
{
    public abstract class Attacker : MonoBehaviour
    {
        protected AttackerAct statusAct;

        [SerializeField] protected AttackStyle attackStyle = null;
        private AttackStyle tempAttackStyle = null;
        private Animator animator = null;

        public int power = 0;
        protected bool attackFinished = false; 
        
        protected ObjectPool pool = new ObjectPool();
        protected GameObject effectObject = null;
        protected Card card = null;
        protected StatusController statusController = null;

        private void Start()
        {
            DOTween.Init();
            card = GetComponent<Card>();
            effectObject = attackStyle.GetEffect();
            FillThePool(pool, effectObject, 1);
            power = attackStyle.GetPower();
            Initialize();
        }

        protected virtual void Initialize() 
        {
            animator = card.GetAnimator();
            statusController = card.GetComponent<StatusController>();
            statusAct = new AttackerAct();
        }

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
            int criticChance = card.GetStats().CriticChance + statusAct.ExtraCriticChance;
            criticChance *= 2;

            if (criticChance > 0)
            {
                int number = Random.Range(0, 100);
                if (number <= criticChance)
                    return true;
            }
            return false;
        }

        protected virtual void AttackEffect(ObjectPool pool, Move move, float time)
        {
            Transform cardTransform = move.GetCard().transform;
            Transform target = move.GetTargetTile().transform;

            if (move.GetTargetTile().GetCard() != null)
                EffectOperator.Instance.Operate(pool, cardTransform, target, time, attackStyle);
        }


        public bool GetAttackFinished()
        {
            return attackFinished;
        }

        public void SetAttackFinished(bool b)
        {
            attackFinished = b;
            if (!b)
                ChangeAttackStyleOneTurn(true);
        }

        public void ChangeAttackStyleOneTurn(bool isTempGonnaBeNull, AttackStyle s = null)
        {
            if (isTempGonnaBeNull)
            {
                if (tempAttackStyle == null) return;

                SetAttackStyle(tempAttackStyle);
                tempAttackStyle = null;
            }
            else
            {
                tempAttackStyle = attackStyle;
                SetAttackStyle(s);
            }
        }

        public void SetAttackStyle(AttackStyle attackStyle)
        {
            TextPopupManager.Instance.DeleteObjectsInPool(pool);

            this.attackStyle = attackStyle;
            effectObject = attackStyle.GetEffect();
            FillThePool(pool, effectObject, 2);
            power = attackStyle.GetPower();
        }
        protected bool DoAttackAction(Move move, bool isTrap)
        {
            bool isCritic;

            if(!isTrap)
                isCritic = IsCriticAttack();
            else
                isCritic = false;

            if (!isCritic)
                attackStyle.Attack(move, power);
            else
                attackStyle.Attack(move, power * 2);

            if (!isTrap && statusAct.CanLifeSteal)
            {
                if (!isCritic)
                    card.GetDamagable().IncreaseHealth(power);
                else
                    card.GetDamagable().IncreaseHealth(power * 2);
            }

            return isCritic;
        }

        protected void FillThePool(ObjectPool pool, GameObject effect, int objectCount)
        {
            pool.SetObject(effect);
            pool.Fill(objectCount, transform);
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

            Attacker tAttacker = card.GetComponent<Attacker>();
            int dodgeChance;
            if (tAttacker != null)
            {
                tAttacker.StatusActResetAndControl();
                dodgeChance = card.GetStats().DodgeChance + tAttacker.statusAct.ExtraDodgeChance;
            }
            else
                dodgeChance = card.GetStats().DodgeChance;
            dodgeChance *= 2;

            if (dodgeChance > 0)
            {
                int number = Random.Range(0, 100);
                if (number <= dodgeChance)
                    return true;
            }
            return false;
        }

        public void StatusActResetAndControl()
        {
            statusAct.Reset();
            statusAct.ActControl(statusController.activeStatuses);
        }

        public void AttackAction(Move move)
        {
            Card card = move.GetCard();
            Tile target = move.GetTargetTile();
            Card tCard = move.GetTargetTile().GetCard();
            Vector3 tPos = target.transform.position;

            if(card.GetCardType() != CardType.TRAP)
                StatusActResetAndControl();

            if (move.GetTargetTile().GetCard() != null)
            {
                bool isMissed = IsMissed(target.GetCard());
                if (isMissed)
                    TextPopupManager.Instance.TextPopup(tPos, "MISS");
                else
                {
                    bool isCritic;
                    if (card.GetCardType() != CardType.TRAP)
                        isCritic = DoAttackAction(move, false);
                    else
                        isCritic = DoAttackAction(move, true);

                    List<StatusObject> impacts = attackStyle.GetImpacts();

                    if(impacts != null && impacts.Count > 0)
                    {
                        for (int i = 0; i < impacts.Count; i++)
                            tCard.GetComponent<StatusController>().AddStatus(impacts[i]);
                    }

                    TextPopupManager.Instance.TextPopup(tPos, power, isCritic);
                }
            }
            float time = attackStyle.GetAnimationTime();
            AttackEffect(pool, move, time);
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
                        TextPopupManager.Instance.TextPopup(tPos, "MISS");
                    else
                    {
                        bool isCritic = DoAttackAction(move, false);
                        TextPopupManager.Instance.TextPopup(tPos, power, isCritic);
                    }
                }
            }
            float time = attackStyle.GetAnimationTime();
            AttackEffect(pool, move, time);
        }
    }
}