using DungeonRush.Cards;
using DungeonRush.Data;
using DungeonRush.Field;
using DungeonRush.Managers;
using DungeonRush.Property;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush.Controller
{
    public class AIController : MonoBehaviour, IMoveController
    {
        private Swipe swipe;
        private Card card;
        private bool isRunning = false;

        private ProcessHandleChecker preparingProcess;
        private ProcessHandleChecker attackProcess;
        private ProcessHandleChecker moveProcess;

        private NonPlayerController nonPlayerController;
        private TrapController trapController;

        private Mover mover;
        private Attacker attacker;
        private void Start()
        {
            card = GetComponent<Card>();
            mover = card.GetComponent<Mover>();
            attacker = card.GetComponent<Attacker>();
            if (card.GetCardType() == CardType.ENEMY)
            {
                nonPlayerController = FindObjectOfType<NonPlayerController>();
                NonPlayerController.subscribedEnemies.Add(this);
            }
            else if (card.GetCardType() == CardType.TRAP)
            {
                trapController = FindObjectOfType<TrapController>();
                TrapController.subscribedTraps.Add(this);
            }

            
            InitProcessHandlers();
        }

        private void Update()
        {
            if (!isRunning) return;

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

        // TODO Swipe değişkeninden kurtul..

        #region PREPARE MOVE PROCESS

        public void PrepareMoveProcess()
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
            return card.GetShift().Define(card, swipe);
        }

        #endregion

        #region ATTACK PROCESS

        public void AttackProcess()
        {
            if (attackProcess.start)
            {
                card.ExecuteAttack();
                attackProcess.ContinuingProcess(false);
            }
            else if (attackProcess.continuing)
            {
                if (attacker.attackFinished)
                {
                    attacker.attackFinished = false;
                    attackProcess.EndProcess();
                }
            }
            else if (attackProcess.end)
            {
                StartCoroutine(EndTurn());
                Stop();
                attackProcess.Finish();
                Notify();
            }
        }
        #endregion

        #region MOVE PROCESS

        public void MoveProcess()
        {
            if (moveProcess.start)
            {
                card.ExecuteMove();
                moveProcess.ContinuingProcess(false);
            }
            else if (moveProcess.continuing)
            {
                
                if (mover.moveFinished)
                {
                    mover.moveFinished = false;
                    moveProcess.EndProcess();
                }
            }
            else if (moveProcess.end)
            {
                StartCoroutine(EndTurn());
                Stop();
                moveProcess.Finish();
                Notify();
            }
        }

        #endregion

        private IEnumerator EndTurn()
        {
            yield return new WaitForSeconds(0);
        }
        private void Notify()
        {
            nonPlayerController.OnNotify();
        }
        private void Stop()
        {
            isRunning = false;
            swipe = Swipe.NONE;
            card.GetMove().Reset();
        }
        public void InitProcessHandlers()
        {
            preparingProcess.Init(true);
            attackProcess.Init(false);
            moveProcess.Init(false);
        }
        public void SetSwipe(Swipe s)
        {
            swipe = s;
        }
        public void SetMove(Move move)
        {
            card.SetMove(move);
        }
        public Card GetCard()
        {
            return card;
        }
        public void Run()
        {
            swipe = GetCard().GetShift().SelectTileToAttack(GetCard().GetShift().GetAvaibleTiles(GetCard()), GetCard());
            isRunning = true;
            preparingProcess.StartProcess();
        }
        public bool IsRunning()
        {
            return isRunning;
        }
    }
}
