using System.Collections;
using System.Collections.Generic;
using DungeonRush.Attacking;
using DungeonRush.Cards;
using DungeonRush.Data;
using UnityEngine;

namespace DungeonRush.Property
{
    public class StaticAttacker : MonoBehaviour, IAttacker
    {
        [Header("Attacker Properties")]
        [SerializeField] AttackStyle attackStyle = null;

        [SerializeField] TextPopup textPopup = null;
        private ObjectPool poolForTextPopup = new ObjectPool();

        private int power = 0; 
        private ObjectPool poolForAttackStyle = new ObjectPool();
        private GameObject effectObject = null;

        private Card card = null;
        private bool attackFinished = false;
        private void Start()
        {
            card = GetComponent<Card>();

            effectObject = attackStyle.GetEffect();

            FillThePool(poolForTextPopup, textPopup.gameObject, 2);

            poolForAttackStyle.SetObject(effectObject);
            poolForAttackStyle.FillPool(2);

            power = attackStyle.GetPower();
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
            Damage(move);
        }

        private void Damage(Move move)
        {
            Vector3 cardPos = move.GetCardTile().GetCoordinate();
            float time = attackStyle.GetAnimationTime();

            attackStyle.Attack(move, power);
            StartCoroutine(StartAttackAnimation(poolForAttackStyle, cardPos, time));
            StartCoroutine(StartTextPopup(poolForTextPopup, cardPos, power));
        }

        private IEnumerator StartTextPopup(ObjectPool pool, Vector3 tPos, int damage)
        {
            GameObject obj = pool.PullObjectFromPool();
            obj.transform.position = tPos;
            TextPopup objTxt = obj.GetComponent<TextPopup>();
            objTxt.Setup(damage, tPos);
            float t = objTxt.GetDisapperTime();
            yield return new WaitForSeconds(t);
            pool.AddObjectToPool(obj);
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
            this.attackStyle = attackStyle;
            power = attackStyle.GetPower();
        }

        public AttackStyle GetAttackStyle()
        {
            return attackStyle;
        }
    }
}
