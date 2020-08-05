using System;
using System.Collections;
using DG.Tweening;
using DungeonRush.Cards;
using DungeonRush.Data;
using DungeonRush.Shifting;
using UnityEngine;

namespace DungeonRush.Property
{
    public class EventMover : MonoBehaviour, IMover
    {
        private Move move;
        private bool isMoveFinished = false;
        
        [Header("Shifting Properties")]
        [SerializeField] Animator animator = null;
        [SerializeField] Shift shifting = null;
        [SerializeField] float range = 0f;
        [SerializeField] float movingTime = 0f;
        [SerializeField] float achievingTime = 0;
        [SerializeField] GameObject brokeTreasureParticle = null;
        [SerializeField] float particleTime = 0;
        private ObjectPool pool = new ObjectPool();

        private Card card;
        private IGameEvent gameEvent;
   
        private void Start()
        {
            DOTween.Init();
            move = new Move();
            card = GetComponent<Card>();

            pool.SetObject(brokeTreasureParticle);
            pool.FillPool(1);
        }

        public void Move()
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

            if (gameEvent.GetEventType() == EventType.TREASURE)
            {
                StartCoroutine(StartEventAnimation(targetPos, particleTime));
                move.GetCard().transform.DOMove(targetPos, movingTime).OnComplete(() => StartCoroutine(TreasureMove()));
            }
            else if (gameEvent.GetEventType() == EventType.PORTAL)
                move.GetCard().transform.DOMove(targetPos, movingTime).OnComplete(() => StartCoroutine(PortalMove()));
        }

        #region EVENT METHODS
        private IEnumerator StartEventAnimation(Vector3 pos, float time)
        {
            GameObject obj = pool.PullObjectFromPool();
            obj.transform.position = pos;
            yield return new WaitForSeconds(time);
            pool.AddObjectToPool(obj);
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

        #region IMOVER METHODS
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
