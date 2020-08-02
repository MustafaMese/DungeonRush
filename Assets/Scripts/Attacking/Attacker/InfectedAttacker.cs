using DungeonRush.Cards;
using UnityEngine;
using DG.Tweening;
using DungeonRush.Data;
using System.Collections;
using DungeonRush.Attacking;

namespace DungeonRush
{
    namespace Property
    {
        public class InfectedAttacker : MonoBehaviour, IAttacker
        {
            [Header("Attacker Properties")]
            [SerializeField] float range = 0.8f;
            [SerializeField] int power = 5;
            [SerializeField] AttackStyle attackStyle = null;

            [Header("Animation Variables")]
            [SerializeField] float closingToEnemyTime = 0.1f;
            [SerializeField] float damageTime = 0.1f;
            [SerializeField] float getBackTime = 0.1f;
            [SerializeField] Animator animator = null;
            [SerializeField] GameObject particul = null;
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

                poolForParticul.SetObject(particul);
                poolForParticul.FillPool(3);

                effectObject = attackStyle.GetEffect();
                poolForAttackStyle.SetObject(effectObject);
                poolForAttackStyle.FillPool(2);
            }

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

            private void MoveToAttackRange()
            {
                Move move = card.GetMove();
                var dir = GetDirection(move);

                Vector2 targetPos = new Vector2(move.GetCardTile().GetCoordinate().x + dir.x * range, move.GetCardTile().GetCoordinate().y + dir.y * range);
                UpdateAnimation(true, false);
                move.GetCard().transform.DOMove(targetPos, closingToEnemyTime).OnComplete(() => StartCoroutine(FinishAttack(move)));
            }

            public void Attack()
            {
                attackFinished = false;
                MoveToAttackRange();
            }

            private void Damage(Move move)
            {
                Transform card = move.GetCard().transform;
                Transform target = move.GetTargetTile().transform;
                Vector3 tPos = move.GetTargetTile().GetCoordinate();
                float time = attackStyle.GetAnimationTime();

                attackStyle.Attack(move, power);
                StartCoroutine(StartAttackAnimation(poolForAttackStyle, tPos, card, target, time));
                StartCoroutine(StartAttackAnimation(poolForParticul, tPos, card, null, particulTime));

            }

            private IEnumerator StartAttackAnimation(ObjectPool pool, Vector3 tPos, Transform card, Transform target, float time)
            {
                GameObject obj = pool.PullObjectFromPool();
                attackStyle.SetEffectPosition(obj, tPos, target);
                yield return new WaitForSeconds(time);
                attackStyle.SetEffectPosition(obj, tPos, card);
                pool.AddObjectToPool(obj);
            }

            private IEnumerator FinishAttack(Move move)
            {
                UpdateAnimation(false, false);
                UpdateAnimation(true, true);
                Damage(move);
                yield return new WaitForSeconds(damageTime);
                move.GetCard().transform.DOMove(move.GetCardTile().GetCoordinate(), getBackTime).OnComplete(() => FinaliseAttack());
            }

            private void FinaliseAttack()
            {
                attackFinished = true;
            }

            private Vector3 GetDirection(Move move)
            {
                var heading = move.GetTargetTile().GetCoordinate() - move.GetCardTile().GetCoordinate();
                var distance = heading.magnitude;
                var direction = heading / distance;
                return direction;
            }

            private void UpdateAnimation(bool play, bool isAttack)
            {
                if(card.GetCardType() != CardType.TRAP)
                    if (isAttack)
                        animator.SetTrigger("attack");
                    else
                        animator.SetBool("walk", play);
            }

            public bool GetAttackFinished()
            {
                return attackFinished;
            }

            public void SetAttackFinished(bool b)
            {
                attackFinished = b;
            }

            public void SetAttackAnimation()
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
