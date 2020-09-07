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
        public class InfectedAttacker : Attacker
        {
            [Header("Attacker Properties")]
            [SerializeField] float range = 0.8f;
            [SerializeField] float closingToEnemyTime = 0.1f;
            [SerializeField] float damageTime = 0.1f;
            [SerializeField] float getBackTime = 0.1f;

            private void MoveToAttackRange()
            {
                Move move = card.GetMove();
                var dir = GetDirection(move);

                Vector2 targetPos = new Vector2(move.GetCardTile().GetCoordinate().x + dir.x * range, move.GetCardTile().GetCoordinate().y + dir.y * range);
                UpdateAnimation(true, false);
                move.GetCard().transform.DOMove(targetPos, closingToEnemyTime).OnComplete(() => StartCoroutine(FinishAttack(move)));
            }

            public override void Attack()
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
                StartCoroutine(StartTextPopup(poolForTextPopup, tPos, power));
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
        }
    }
}
