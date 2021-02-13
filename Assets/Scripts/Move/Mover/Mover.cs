using DG.Tweening;
using DungeonRush.Cards;
using DungeonRush.Data;
using DungeonRush.Managers;
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
        protected ObjectPool<GameObject> walkParticulPool = new ObjectPool<GameObject>();
        protected bool isMoveFinished = false;
        protected Move move;
        protected Card card;
        protected Animator animator = null;

        private void Start()
        {
            DOTween.Init();
            move = new Move();
            card = GetComponent<Card>();
            animator = card.GetAnimator();

            walkParticul = shifting.GetEffect();
            FillThePool(walkParticulPool, walkParticul, 2);

            Initialize();
        }

        public abstract void Move();
        protected virtual void Initialize() { }

        private void FillThePool(ObjectPool<GameObject> pool, GameObject effect, int count)
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
            TextPopupManager.Instance.DeleteObjectsInPool(walkParticulPool);

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

        protected void StartMoveAnimation(Vector3 pos, float time)
        {
            EffectOperator.Instance.Operate(walkParticulPool, transform, pos, time);
        }
    }
}
