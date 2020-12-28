using DungeonRush.Data;
using DungeonRush.Property;
using System.Collections;
using UnityEngine;
using DG.Tweening;

namespace DungeonRush.Attacking
{
    public class LongRangeAttacker : Attacker
    {
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
            AttackAction(move);
        }

        protected override void AttackEffect(ObjectPool<GameObject> pool, Move move, float time)
        {
            Vector2 pos = move.GetTargetTile().GetCoordinate();
            Transform cardTransform = move.GetCardTile().transform;

            if (move.GetTargetTile().GetCard() != null)
            {
                GameObject obj = pool.Pull(cardTransform);
                attackStyle.SetEffectPosition(obj, cardTransform.position, cardTransform);
                obj.transform.DOMove(pos, time).OnComplete(() => FinishAnimation(obj, pool, move));
            }
        }

        private void FinishAnimation(GameObject obj, ObjectPool<GameObject> pool, Move move)
        {
            Transform cardTransform = move.GetCardTile().transform;

            attackStyle.SetEffectPosition(obj, cardTransform.position, cardTransform);
            pool.AddObjectToPool(obj);
        }
    }
}