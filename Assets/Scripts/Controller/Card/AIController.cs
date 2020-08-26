using DungeonRush.Cards;
using DungeonRush.Customization;
using DungeonRush.Data;
using DungeonRush.Field;
using DungeonRush.Managers;
using DungeonRush.Property;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush.Controller
{
    public abstract class AIController : MonoBehaviour, IMoveController
    {
        protected bool isMoving = false;
        protected Swipe swipe;
        protected Card card;
        protected bool isRunning = false;
        protected CardType cardType;

        protected ProcessHandleChecker preparingProcess;
        protected ProcessHandleChecker attackProcess;
        protected ProcessHandleChecker moveProcess;

        protected IMover mover;
        protected IAttacker attacker;
        protected ICustomization customization;

        [SerializeField] GameObject model;
        [SerializeField] protected GameObject exclamation;

        protected abstract void Notify();
        protected abstract void ChooseController();
        protected abstract Swipe SelectTileToAttack(Card card);
        protected abstract void ChangeState();

        private void Start()
        {
            Initialize();

            InitProcessHandlers();
            ChooseController();
        }

        private void Update()
        {
            if (!isRunning) return;

            MakeMove();
        }

        protected virtual void Initialize()
        {
            card = GetComponent<Card>();
            mover = card.GetComponent<IMover>();
            attacker = card.GetComponent<IAttacker>();
            customization = card.GetComponent<ICustomization>();
            cardType = card.GetCardType();
        }

        #region MOVE CONTROLLER METHODS

        public virtual void MakeMove()
        {
            if (preparingProcess.IsRunning())
            {
                PrepareMoveProcess();
            }
            else if (attackProcess.IsRunning())
            {
                AttackProcess();
            }
            else if (moveProcess.IsRunning())
            {
                MoveProcess();
            }
        }

        public virtual void PrepareMoveProcess()
        {
            var canMove = DoMove(swipe);
            if (canMove)
            {
                preparingProcess.Finish();
                var move = card.GetMove().GetCanMove();

                if (move)
                    moveProcess.StartProcess();
                else
                    attackProcess.StartProcess();
            }
            else
            {
                preparingProcess.Finish();
                Stop();
                Notify();
            }
        }

        private bool DoMove(Swipe swipe)
        {
            if (isMoving)
                return card.GetShift().Define(card, swipe);
            else
                return card.GetAttackStyle().Define(card, swipe);
        }

        public virtual void AttackProcess()
        {
            if (attackProcess.start)
            {
                card.ExecuteAttack();
                attackProcess.ContinuingProcess(false);
            }
            else if (attackProcess.continuing)
            {
                if (attacker.GetAttackFinished())
                {
                    attacker.SetAttackFinished(false);
                    attackProcess.EndProcess();
                }
            }
            else if (attackProcess.end)
            {
                Stop();
                attackProcess.Finish();
                Notify();
            }
        }

        public virtual void MoveProcess()
        {
            if (moveProcess.start)
            {
                card.ExecuteMove();
                moveProcess.ContinuingProcess(false);
            }
            else if (moveProcess.continuing)
            {
                
                if (mover.IsMoveFinished())
                {
                    mover.SetIsMoveFinished(false);
                    moveProcess.EndProcess();
                }
            }
            else if (moveProcess.end)
            {
                Stop();
                moveProcess.Finish();
                Notify();
            }
        }

        #endregion

        #region STATE METHODS
        public void ChangeAnimatorState(bool state)
        {
            model.GetComponent<Animator>().enabled = state;
            customization.ChangeSkinState(state);
        }

        public void ChangeShadowState(bool shadow)
        {
            if (shadow)
                customization.OverShadow();
            else
                customization.RemoveShadow();
        }
        #endregion

        protected virtual void Stop()
        {
            isRunning = false;
            swipe = Swipe.NONE;
            customization.Change(transform.position.y);
            card.GetMove().Reset();
        }
        public void InitProcessHandlers()
        {
            preparingProcess.Init(true);
            attackProcess.Init(false);
            moveProcess.Init(false);
        }

        public Card GetCard()
        {
            return card;
        }

        public void Run()
        {
            swipe = SelectTileToAttack(GetCard());
            ChangeState();
            isRunning = true;
            preparingProcess.StartProcess();
        }

        protected int GiveRandomEncounter(List<Tile> list, int count)
        {
            int missCount = 0;
            int number = -1;
            while (missCount < 3)
            {
                number = UnityEngine.Random.Range(0, count);
                try
                {
                    if (!list[number].IsTileOccupied())
                        return number;
                }
                catch (Exception e)
                {
                    print("Hata buldum. -1 döndürüyorum");
                    return -1;
                }

                missCount++;
            }

            for (int i = 0; i < list.Count; i++)
            {
                if (i == number) continue;

                Tile t = list[i];
                if (!t.IsTileOccupied())
                    return i;
            }

            return -1;
        }
    }
}
