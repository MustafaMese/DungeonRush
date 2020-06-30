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
        [SerializeField] float timeForFinishTourET = 0.2f;
        public Dictionary<Tile, Swipe> avaibleTiles = new Dictionary<Tile, Swipe>();

        public Swipe swipe;
        private Card card;
        public bool isRunning = false;
        public bool isAttacker = false;

        public ProcessHandleChecker preparingProcess;
        public ProcessHandleChecker animationProcess;
        public ProcessHandleChecker moveProcess;
        private NonPlayerController nonPlayerController;

        private void Start()
        {
            nonPlayerController = FindObjectOfType<NonPlayerController>();
            card = GetComponent<Card>();
            InitProcessHandlers();
        }

        private void Update()
        {
            if (!isRunning) return;

            //if (card.GetMove().GetCard() == null) return;

            if (preparingProcess.IsRunning())
            {
                PrepareMoveProcess();
            }
            else if (animationProcess.IsRunning()) 
            {
                AnimationProcess(card);
            }
            else if (moveProcess.IsRunning())
            {
                ExecuteMoves();
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
                animationProcess.StartProcess();
            }
            else
            {
                swipe = Swipe.NONE;
                Notify();
                card.GetMove().Reset();
                preparingProcess.Finish();
                isRunning = false;
                avaibleTiles.Clear();
            }
        }

        private bool DoMove(Swipe swipe)
        {
            return card.GetShift().Define(card, swipe);
        }

        #endregion

        #region ANIMATION

        public void AnimationProcess(Card c)
        {
            if (animationProcess.start)
            {
                DoAnimation(c);
                animationProcess.EndProcess();
            }
            else if (animationProcess.end)
            {
                if (c.GetMove().GetMoveType() != MoveType.ATTACK)
                    moveProcess.StartProcess();
                else
                    StartCoroutine(FinishAnimationTurn());
                animationProcess.Finish();
            }
        }

        private void DoAnimation(Card c)
        {
            Move m = c.GetMove();
            //c.HandleCardEffect(m.GetMoveType(), m.GetTargetTile(), m.GetCardTile().GetListNumber());
        }

        private IEnumerator FinishAnimationTurn()
        {
            yield return new WaitForSeconds(0.27f);
            moveProcess.StartProcess();
        }

        #endregion

        #region MOVE PROCESS

        public void ExecuteMoves()
        {
            if (moveProcess.start)
            {
                var move = card.GetMove().GetCanMove();

                if (move)
                {
                    card.ExecuteMove();
                    moveProcess.ContinuingProcess(false);
                }
                else
                {
                    card.Attack(card.GetMove().GetTargetTile().GetCard());
                    moveProcess.EndProcess();
                }
            }
            else if (moveProcess.continuing)
            {
                if(card.GetComponent<Mover>().moveFinished && !Board.touched)
                {
                    card.GetComponent<Mover>().moveFinished = false;
                    moveProcess.EndProcess();
                }
            }
            else if (moveProcess.end)
            {
                Notify();
                StartCoroutine(EndTurn());
                moveProcess.Finish();
                preparingProcess.StartProcess();
                swipe = Swipe.NONE;
                isRunning = false;
            }
        }

        private void Notify() 
        {
            isAttacker = false;
            nonPlayerController.OnNotify();
        }

        private IEnumerator EndTurn()
        {
            Board.touched = false;
            yield return new WaitForSeconds(0);
        }

        #endregion

        public void InitProcessHandlers()
        {
            preparingProcess.Init(true);
            animationProcess.Init(false);
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
            isAttacker = true;
            preparingProcess.StartProcess();
        }

        public bool IsRunning()
        {
            return isRunning;
        }


    }
}
