﻿using DG.Tweening;
using DungeonRush.Cards;
using DungeonRush.Data;
using DungeonRush.Shifting;
using System.Collections;
using UnityEngine;

namespace DungeonRush.Property
{
    public abstract class Mover : MonoBehaviour
    {
        [Header("Shifting Properties")]
        [SerializeField] protected Shift shifting = null;
        private Shift tempShift = null;

        [SerializeField] protected float movingTime = 0.2f;
        
        [SerializeField] protected float particulTime = 0;

        protected GameObject walkParticul = null;
        protected ObjectPool walkParticulPool = new ObjectPool();
        protected bool isMoveFinished = false;
        protected Move move;
        protected Card card;
        protected Animator animator = null;

        private void Start()
        {
            DOTween.Init();
            move = new Move();
            card = GetComponent<Card>();
            animator = card.Animator;

            walkParticul = shifting.GetEffect();
            FillThePool(walkParticulPool, walkParticul, 2);

            Initialize();
        }

        public abstract void Move();
        protected virtual void Initialize() { }

        private void FillThePool(ObjectPool pool, GameObject effect, int count)
        {
            pool.SetObject(effect);
            pool.FillPool(count, transform);
        }

        public void ChangeShiftOneTurn(bool isTempGonnaBeNull, Shift s = null)
        {
            if (isTempGonnaBeNull)
            {
                if (tempShift == null) return;

                SetShifting(tempShift);
                tempShift = null;
            }
            else
            {
                tempShift = shifting;
                SetShifting(s);
            }
        }

        public void SetShifting(Shift s)
        {
            walkParticulPool.DeleteObjectsInPool();
            shifting = s;
            walkParticul = shifting.GetEffect();
            FillThePool(walkParticulPool, walkParticul, 2);
        }

        public Shift GetShift()
        {
            return shifting;
        }
        public bool IsMoveFinished()
        {
            if (isMoveFinished)
                ChangeShiftOneTurn(true);

            return isMoveFinished;
        }

        public void SetIsMoveFinished(bool b)
        {
            isMoveFinished = b;
        }
        protected void UpdateAnimation(bool b)
        {
            animator.SetBool("walk", b);
        }

        protected IEnumerator StartMoveAnimation(Vector3 pos, float time)
        {
            // TODO Ses noktası

            GameObject obj = walkParticulPool.PullObjectFromPool(transform);
            obj.transform.position = pos;
            obj.transform.SetParent(null);
            yield return new WaitForSeconds(time);
            obj.transform.SetParent(transform);
            walkParticulPool.AddObjectToPool(obj);
        }
    }
}
