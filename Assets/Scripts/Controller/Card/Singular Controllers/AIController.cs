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
    public class AIController : MonoBehaviour, IMoveController
    {
        public struct StatusActControl
        {
            public bool canMove;
            public bool canAttack;
            public bool anger;

            public void Reset()
            {
                canMove = true;
                canAttack = true;
                anger = false;
            }

            public void ActControl(List<StatusType> list)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i] == StatusType.DISARMED)
                        canAttack = false;
                    else if (list[i] == StatusType.ENTANGLED)
                        canMove = false;
                    else if (list[i] == StatusType.STUNNED)
                    {
                        canMove = false;
                        canAttack = false;
                    }
                    else if (list[i] == StatusType.ANGER)
                        anger = true;
                }
            }
        }
        protected StatusActControl statusAct;
        public State state = State.NONE;

        protected bool isMoving = false;
        protected Swipe swipe;
        protected Card card;
        protected bool isRunning = false;
        protected CardType cardType;
        
        protected ProcessHandleChecker preparingProcess;
        protected ProcessHandleChecker attackProcess;
        protected ProcessHandleChecker moveProcess;

        protected Mover mover;
        protected Attacker attacker;
        protected ICustomization customization;
        protected StatusController statusController;

        [SerializeField] GameObject model;
        [SerializeField] protected GameObject exclamation;
        [SerializeField] ActionState actionState;

        protected void Notify()
        {
            if (card.GetCardType() == CardType.ENEMY)
            {
                if (card.InstantMoveCount > 0)
                {
                    Run();
                    card.InstantMoveCount--;
                }
                else
                {
                    card.InstantMoveCount = card.TotalMoveCount;
                    MoveSchedular.Instance.enemyController.OnNotify();
                }
            }
            else
                MoveSchedular.Instance.trapController.OnNotify();
        }
        protected void ChooseController()
        {
            print(card.GetCardType());

            if (card.GetCardType() == CardType.ENEMY)
                EnemyController.subscribedEnemies.Add(this);
            else
                TrapController.subscribedTraps.Add(this);
        }
        protected Swipe SelectTileForSwipe(Card card)
        {
            if(state == State.NONE)
            {
                isMoving = true;
                return Swipe.NONE;
            }

            if (state == State.WAIT) return Swipe.NONE;

            if ((state == State.ATTACK || state == State.ATTACK2) && statusAct.canAttack)
                return SelectTileToAttack(card);

            if (statusAct.canMove)
                return SelectTileToMove(card);

            return Swipe.NONE;
        }
        private void ChangeState()
        {
            if (statusAct.anger)
            {
                state = State.ATTACK;
                exclamation.SetActive(false);
            }
            else
                state = actionState.ChangeState(state, exclamation);
        }

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
            mover = card.GetComponent<Mover>();
            attacker = card.GetComponent<Attacker>();
            customization = card.GetComponent<ICustomization>();
            statusController = card.GetComponent<StatusController>();
            statusAct = new StatusActControl();
            cardType = card.GetCardType();
        }

        #region MOVE CONTROLLER METHODS

        public void MakeMove()
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

        protected virtual bool DoMove(Swipe swipe)
        {
            if (isMoving)
                return card.GetShift().Define(card, swipe);
            else
                return card.GetAttackStyle().Define(card, swipe);
        }

        public void AttackProcess()
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

        public void MoveProcess()
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

        protected Swipe SelectTileToAttack(Card card)
        {
            List<Tile> list;
            Dictionary<Tile, Swipe> tiles;

            tiles = card.GetAttackStyle().GetAvaibleTiles(card);
            list = new List<Tile>(tiles.Keys);
            for (int i = 0; i < list.Count; i++)
            {
                Tile t = list[i];
                if (t.GetCard() != null && card.GetCharacterType().IsEnemy(t.GetCard().GetCharacterType()))
                {
                    isMoving = false;
                    return tiles[t];
                }
            }

            return Swipe.NONE;
        }

        protected Swipe SelectTileToMove(Card card)
        {
            List<Tile> list;
            Dictionary<Tile, Swipe> tiles;

            tiles = card.GetShift().GetAvaibleTiles(card);
            list = new List<Tile>(tiles.Keys);
            isMoving = true;
            int count = tiles.Count;
            int number = GiveRandomEncounter(list, count);
            if (number != -1)
            {
                Tile t = list[number];
                return tiles[t];
            }
            else
                return Swipe.NONE;
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

        #region CARD CONTROL METHODS
        public void ActivateStatuses()
        {
            statusController.ActivateStatuses();
        }

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
            statusAct.Reset();
            statusAct.ActControl(statusController.statusTypes);
            swipe = SelectTileForSwipe(GetCard());
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

        #endregion
    }
}
