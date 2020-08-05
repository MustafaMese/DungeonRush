using DG.Tweening;
using DungeonRush.Cards;
using DungeonRush.Data;
using DungeonRush.Field;
using DungeonRush.Shifting;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush.Property
{
    public class EnemyMover : MonoBehaviour, IMover
    {
        private Move move;
        private bool isMoveFinished = false;

        [Header("Shifting Properties")]
        [SerializeField] Shift shifting = null;
        [SerializeField] float movingTime = 0.2f;
        [SerializeField] Animator animator = null;
        [SerializeField] GameObject walkParticul = null;
        [SerializeField] float particulTime = 0;

        private ObjectPool pool = new ObjectPool();
        private Card card;

        private void Start()
        {
            DOTween.Init();
            move = new Move();
            card = GetComponent<Card>();
            pool.SetObject(walkParticul);
            pool.FillPool(4);
        }

        public Shift GetShift()
        {
            return shifting;
        }

        public bool IsMoveFinished()
        {
            return isMoveFinished;
        }

        public void Move()
        {
            if (move.GetCard() == null)
                move = card.GetMove();

            Vector3 pos = move.GetCardTile().GetCoordinate();
            UpdateAnimation(true);
            StartCoroutine(StartMoveAnimation(pos, particulTime));
            move.GetCard().transform.DOMove(move.GetTargetTile().GetCoordinate(), movingTime).OnComplete(() => TerminateMove());
        }

        private void TerminateMove()
        {
            UpdateAnimation(false);
            move.GetCard().transform.position = move.GetTargetTile().GetCoordinate();
            Tile.ChangeTile(move, true, false);
            isMoveFinished = true;
            move.Reset();
        }

        private IEnumerator StartMoveAnimation(Vector3 pos, float time)
        {
            GameObject obj = pool.PullObjectFromPool();
            obj.transform.position = pos;
            yield return new WaitForSeconds(time);
            pool.AddObjectToPool(obj);
        }

        public void SetIsMoveFinished(bool b)
        {
            isMoveFinished = b;
        }

        private void UpdateAnimation(bool b)
        {
            if (move.GetCard().GetCardType() != CardType.TRAP)
                animator.SetBool("walk", b);
        }
    }
}
