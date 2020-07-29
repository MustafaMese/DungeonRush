using System;
using System.Collections;
using DG.Tweening;
using DungeonRush.Data;
using DungeonRush.Shifting;
using UnityEngine;

namespace DungeonRush.Property
{
    public class EventMover : MonoBehaviour, IMover
    {
        private Move move;
        private bool isMoveFinished = false;

        [SerializeField] Animator animator;
        [SerializeField] Shift shifting = null;
        [SerializeField] float range = 0f;
        [SerializeField] float movingTime = 0f;
        [SerializeField] float achievingTime = 0;

        private void Start()
        {
            DOTween.Init();
            move = new Move();
        }

        public void Move()
        {
            // YÜRÜME ANİM.
            UpdateAnimation(true, false);
            MoveToTheRange();
        }

        private void MoveToTheRange()
        {
            var dir = GetDirection(move);
            Vector2 targetPos = new Vector2(move.GetCardTile().GetCoordinate().x + dir.x * range, move.GetCardTile().GetCoordinate().y + dir.y * range);
            // YÜRÜMEYİ BİTİR
            UpdateAnimation(false, false);
            move.GetCard().transform.DOMove(targetPos, movingTime).OnComplete(() => StartCoroutine(FinishMovement()));
        }

        private IEnumerator FinishMovement()
        {
            UpdateAnimation(false, true);
            yield return new WaitForSeconds(achievingTime);
            move.GetCard().transform.DOMove(move.GetCardTile().GetCoordinate(), movingTime).OnComplete(() => Finalise());
        }

        private void Finalise()
        {
            isMoveFinished = true;
            move.Reset();
        }

        private Vector3 GetDirection(Move move)
        {
            var heading = move.GetTargetTile().GetCoordinate() - move.GetCardTile().GetCoordinate();
            var distance = heading.magnitude;
            var direction = heading / distance;
            return direction;
        }

        public Shift GetShift()
        {
            return shifting;
        }

        public bool IsMoveFinished()
        {
            return isMoveFinished;
        }

        public void SetIsMoveFinished(bool b)
        {
            isMoveFinished = b;
        }

        public Move GetMove()
        {
            return move;
        }

        public void SetMove(Move move)
        {
            this.move = move;
        }

        private void UpdateAnimation(bool play, bool isAchieved)
        {
            if (isAchieved)
                animator.SetTrigger("hurt");
            else
                animator.SetBool("walk", play);
        }
    }
}
