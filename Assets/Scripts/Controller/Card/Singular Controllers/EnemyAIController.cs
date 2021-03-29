using DungeonRush.Cards;
using DungeonRush.Customization;
using DungeonRush.Data;
using DungeonRush.Field;
using DungeonRush.Managers;
using DungeonRush.Property;
using DungeonRush.States;
using DungeonRush.Traits;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush.Controller
{
    public class EnemyAIController : MonoBehaviour, IMoveController
    {
        private ControllerAct statusAct;

        private State state = State.NONE;

        private bool isMoving = false;
        private Swipe swipe;
        private Swipe swipeForAim;
        private EnemyCard card;
        private bool isRunning = false;

        private ProcessHandleChecker preparingProcess;
        private ProcessHandleChecker attackProcess;
        private ProcessHandleChecker moveProcess;

        private Mover mover;
        private Attacker attacker;
        private ICustomization customization;
        private StatusController statusController;

        [SerializeField] GameObject exclamation;
        [SerializeField] ActionState actionState;

        private void Start()
        {
            Initialize();
            InitProcessHandlers();
            
            EnemyManager.subscribedEnemies.Add(this);
        }

        private void Update()
        {
            if (!isRunning) return;

            MakeMove();
        }

        private void Initialize()
        {
            card = GetComponent<EnemyCard>();
            mover = card.GetComponent<Mover>();
            attacker = card.GetComponent<Attacker>();

            customization = card.GetComponent<ICustomization>();

            if (transform.position.y < 0)
                customization.ChangeLayer(false, (int)transform.position.y);
            else if (transform.position.y > 0)
                customization.ChangeLayer(true, (int)transform.position.y);

            statusController = card.GetComponent<StatusController>();
            statusAct = new ControllerAct();
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
        private void PrepareMoveProcess()
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
        private  bool DoMove(Swipe swipe)
        {
            if (isMoving)
                return card.GetShift().Define(card, swipe);
            else
                return card.GetAttackStyle().Define(card, swipe);
        }
        // TODO end kısımlarını kaldır. 1 frame'e mahal oluyo.
        private void AttackProcess()
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
        private void MoveProcess()
        {
            if (moveProcess.start)
            {
                card.ExecuteMove();

                float y = card.GetMove().GetTargetTile().GetCoordinate().y;
                if (y < card.transform.position.y)
                    customization.ChangeLayer(true);
                else if (y > card.transform.position.y)
                    customization.ChangeLayer(false);

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
        private Swipe SelectTileToAttack(EnemyCard card)
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
        private Swipe SelectTileToMove(EnemyCard card)
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

        private Swipe SelectTileForSwipe(EnemyCard card)
        {
            Swipe s = Swipe.NONE;
            if (state == State.NONE)
            {
                isMoving = true;
                return Swipe.NONE;
            }

            if (state == State.WAIT) return Swipe.NONE;

            if (state == State.RANGE)
            {
                SelectSwipeForAim(card);
                return Swipe.NONE;
            }

            if (state == State.RANGE_ATTACK)
                return swipeForAim;

            if ((state == State.ATTACK || state == State.ATTACK2) && statusAct.CanAttack)
            {
                s = SelectTileToAttack(card);
                if (s != Swipe.NONE)
                    return s;
            }

            if (statusAct.CanMove)
                s = SelectTileToMove(card);

            return s;
        }

        private void SelectSwipeForAim(EnemyCard card)
        {
            List<Tile> list;
            Dictionary<Tile, Swipe> tiles;

            tiles = card.GetAttackStyle().GetAvaibleTiles(card);
            list = new List<Tile>(tiles.Keys);

            isMoving = false;
            int count = tiles.Count;
            int number = GiveRandomEncounter(list, count);

            if (list.Count > 0)
            {
                Tile t = list[number];
                swipeForAim = tiles[t];
            }
            else
                swipeForAim = Swipe.NONE;
        }
        #endregion

        #region STATE METHODS
        public void ChangeSkinState(bool state)
        {
            customization.ChangeSkinState(state);
        }
        #endregion

        #region CARD CONTROL METHODS

        public void ActivateStatuses()
        {
            statusController.ActivateStatuses();
        }

        private void ChangeState()
        {
            if (statusAct.Anger)
            {
                state = State.ATTACK;
                exclamation.SetActive(false);
            }
            else
                state = actionState.ChangeState(state, exclamation);

        }

        public void Stop()
        {
            isRunning = false;
            swipe = Swipe.NONE;
            card.GetMove().Reset();
            // if(cardType == CardType.ENEMY)
            //     customization.ChangeLayer(transform.position.y);
        }

        public bool IsRunning()
        {
            return isRunning;
        }

        private void InitProcessHandlers()
        {
            preparingProcess.Init(true);
            attackProcess.Init(false);
            moveProcess.Init(false);
        }
        public Sprite GetCardIcon()
        {
            return card.GetCharacterIcon();
        }

        public void Run()
        {
            statusAct.Reset();
            statusAct.ActControl(statusController.activeStatuses);
            
            swipe = SelectTileForSwipe(card);
            ChangeState();
            isRunning = true;
            preparingProcess.StartProcess();
        }

        private int GiveRandomEncounter(List<Tile> list, int count)
        {
            int missCount = 0;
            int number = -1;

            if (count == 1)
                return 0;

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

        public void Notify()
        {
            if (card.GetStats().InstantMoveCount > 0)
            {
                Run();
                ActivateStatuses();
                card.GetStats().InstantMoveCount--;
            }
            else
            {
                card.GetStats().InstantMoveCount = card.GetStats().TotalMoveCount;
                MoveSchedular.Instance.enemyController.OnNotify();
            }
        }
        #endregion
    }
}
