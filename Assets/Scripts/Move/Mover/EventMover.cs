using System.Collections;
using DG.Tweening;
using DungeonRush.Data;
using DungeonRush.Events;
using UnityEngine;

namespace DungeonRush.Property
{
    public class EventMover : Mover
    {
        [Header("Shifting Properties")]
        [SerializeField] float range = 0f;
        [SerializeField] float achievingTime = 0;

        private IGameEvent gameEvent;

        public override void Move()
        {
            if (move.GetCard() == null)
                move = card.GetMove();

            // YÜRÜME ANİM.
            UpdateAnimation(true, false);
            MoveToTheRange();
        }

        private void MoveToTheRange()
        {
            var dir = GetDirection(move);
            Vector2 targetPos = new Vector2(move.GetCardTile().GetCoordinate().x + dir.x * range, move.GetCardTile().GetCoordinate().y + dir.y * range);
            gameEvent = move.GetTargetTile().GetCard().GetComponent<IGameEvent>();

            if (gameEvent == null)
                Finalise();

            if (gameEvent.GetEventType() == MoveEventType.TREASURE)
            {
                StartCoroutine(StartEventAnimation(targetPos, particulTime));
                move.GetCard().transform.DOMove(targetPos, movingTime).OnComplete(() => StartCoroutine(TreasureMove()));
            }
            else if (gameEvent.GetEventType() == MoveEventType.PORTAL)
                move.GetCard().transform.DOMove(targetPos, movingTime).OnComplete(() => StartCoroutine(PortalMove()));
        }

        #region EVENT METHODS
        private IEnumerator StartEventAnimation(Vector3 pos, float time)
        {
            GameObject obj = walkParticulPool.PullObjectFromPool();
            obj.transform.position = pos;
            yield return new WaitForSeconds(time);
            walkParticulPool.AddObjectToPool(obj);
        }

        private IEnumerator TreasureMove()
        {
            UpdateAnimation(false, false);
            UpdateAnimation(false, true);
            yield return new WaitForSeconds(achievingTime);
            gameEvent.GetEvent(card);
            move.GetCard().transform.DOMove(move.GetCardTile().GetCoordinate(), movingTime).OnComplete(() => Finalise());
        }

        private IEnumerator PortalMove()
        {
            UpdateAnimation(true, false);
            yield return new WaitForSeconds(achievingTime);
            UpdateAnimation(false, false);
            gameEvent.GetEvent(card);
            Finalise();
        }
        #endregion

        private void Finalise()
        {
            isMoveFinished = true;
            move.Reset();
            gameEvent = null;
        }

        private Vector3 GetDirection(Move move)
        {
            var heading = move.GetTargetTile().GetCoordinate() - move.GetCardTile().GetCoordinate();
            var distance = heading.magnitude;
            var direction = heading / distance;
            return direction;
        }

        private void UpdateAnimation(bool play, bool isAchieved)
        {
            if (isAchieved)
                animator.SetTrigger("treasure");
            else
                animator.SetBool("walk", play);
        }
    }
}
