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
        [Header("Attacker Properties")]
        [SerializeField] AttackStyle attackStyle = null;
        [SerializeField] float damageTime = 0.3f;
        private int power = 0;

        [SerializeField] TextPopup textPopup = null;
        private ObjectPool poolForTextPopup = new ObjectPool();

        [Header("Animation Variables")]
        [SerializeField] Animator animator = null;

        private ObjectPool poolForAttackStyle = new ObjectPool();
        private GameObject effectObject = null;

        private bool attackFinished = false;
        private Card card = null;

        private void Start()
        {
            DOTween.Init();
            card = GetComponent<Card>();

            FillThePool(poolForTextPopup, textPopup.gameObject, 2);

            effectObject = attackStyle.GetEffect();
            FillThePool(poolForAttackStyle, effectObject, 2);

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

        private IEnumerator StartTextPopup(ObjectPool pool, Vector3 tPos, int damage)
        {
            GameObject obj = pool.PullObjectFromPool();
            obj.transform.position = tPos;
            TextPopup objTxt = obj.GetComponent<TextPopup>();
            objTxt.Setup(damage, tPos);
            float t = objTxt.GetDisapperTime();
            yield return new WaitForSeconds(t);
            obj.transform.SetParent(this.transform);
            pool.AddObjectToPool(obj);
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

            power = attackStyle.GetPower();
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