using DG.Tweening;
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
        [SerializeField] protected Animator animator = null;
        [SerializeField] protected GameObject walkParticul = null;
        [SerializeField] protected float particulTime = 0;
        
        protected ObjectPool walkParticulPool = new ObjectPool();
        protected bool isMoveFinished = false;
        protected Move move;
        protected Card card;

        private void Start()
        {
            DOTween.Init();
            move = new Move();
            card = GetComponent<Card>();

            walkParticulPool.SetObject(walkParticul);
            walkParticulPool.FillPool(2, transform);
            Initialize();
        }

        public abstract void Move();
        protected virtual void Initialize() { }

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
            shifting = s;
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
            GameObject obj = walkParticulPool.PullObjectFromPool(transform);
            obj.transform.position = pos;
            obj.transform.SetParent(null);
            yield return new WaitForSeconds(time);
            obj.transform.SetParent(transform);
            walkParticulPool.AddObjectToPool(obj);
        }
    }
}
