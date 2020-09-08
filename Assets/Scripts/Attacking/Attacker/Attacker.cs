using DG.Tweening;
using DungeonRush.Attacking;
using DungeonRush.Cards;
using DungeonRush.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush.Property
{
    public abstract class Attacker : MonoBehaviour
    {
        [SerializeField] protected AttackStyle attackStyle = null;
        [SerializeField] protected TextPopup textPopup = null;
        [SerializeField] Animator animator = null;

        public int power = 0;
        protected bool attackFinished = false; 
        protected ObjectPool poolForTextPopup = new ObjectPool();
        protected ObjectPool poolForAttackStyle = new ObjectPool();
        protected GameObject effectObject = null;
        protected Card card = null;

        private void Start()
        {
            DOTween.Init();
            card = GetComponent<Card>();
            FillThePool(poolForTextPopup, textPopup.gameObject, 1);
            effectObject = attackStyle.GetEffect();
            FillThePool(poolForAttackStyle, effectObject, 1);
            power = attackStyle.GetPower();
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
            int criticChance = card.CriticChance * 2;
            if (criticChance > 0)
            {
                int number = Random.Range(0, 100);
                if (number <= criticChance)
                    return true;
            }
            return false;
        }

        protected IEnumerator StartTextPopup(ObjectPool pool, Vector3 tPos, int damage, bool isCritical = false)
        {
            GameObject obj = pool.PullObjectFromPool();
            obj.transform.position = tPos;
            TextPopup objTxt = obj.GetComponent<TextPopup>();
            objTxt.Setup(damage, tPos, isCritical);

            float t = objTxt.GetDisapperTime();
            yield return new WaitForSeconds(t);

            obj.transform.SetParent(this.transform);
            pool.AddObjectToPool(obj);
        }

        protected virtual IEnumerator StartAttackAnimation(ObjectPool pool, Vector3 tPos, Transform card, Transform target, float time)
        {
            GameObject obj = pool.PullObjectFromPool();
            attackStyle.SetEffectPosition(obj, tPos, target);
            yield return new WaitForSeconds(time);
            attackStyle.SetEffectPosition(obj, tPos, card);
            pool.AddObjectToPool(obj);
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

        protected void FillThePool(ObjectPool pool, GameObject effect, int objectCount)
        {
            pool.SetObject(effect);
            pool.FillPool(objectCount);
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
    }
}