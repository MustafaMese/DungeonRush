using DungeonRush.Cards;
using DungeonRush.Customization;
using DungeonRush.Data;
using DungeonRush.Field;
using DungeonRush.Managers;
using DungeonRush.Property;
using System.Collections;
using UnityEngine;

namespace DungeonRush.Controller
{
    public class PlayerController : MonoBehaviour, ICardController, IMoveController
    {
        private bool isRunning = false;
        private Card player;
        private ProcessHandleChecker preparingProcess;
        private ProcessHandleChecker attackProcess;
        private ProcessHandleChecker moveProcess;
        private MoveSchedular ms;
        private IAttacker attacker;
        private ICustomization customization;

        private void Start()
        {
            InitProcessHandlers();
            player = GetComponent<PlayerCard>();
            ms = FindObjectOfType<MoveSchedular>();
            attacker = player.GetComponent<IAttacker>();
            customization = player.GetComponent<ICustomization>();
            FindObjectOfType<MoveSchedular>().playerController = this;
        }

        private void Update()
        {
            if (!isRunning) return;

            MakeMove();
        }

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

        #region PREPARE MOVE METHODS
        public void PrepareMoveProcess()
        {
            if (!Board.touched && SwipeManager.swipeDirection != Swipe.NONE)
            {
                var canMove = DoMove(SwipeManager.swipeDirection);
                if (canMove)
                {
                    preparingProcess.Finish();
                    Board.touched = true;

                    var move = player.GetMove().GetCanMove();

                    if (move)
                        moveProcess.StartProcess();
                    else
                        attackProcess.StartProcess();
                }
                else
                {
                    player.GetMove().Reset();
                }
            }
        }

        private bool DoMove(Swipe swipe)
        {
            bool b = player.GetShift().Define(player, swipe);

            if (b && (swipe == Swipe.UP || swipe == Swipe.DOWN))
                customization.Change();

            return b;
        }

        #endregion

        #region ATTACKING METHODS

        public void AttackProcess()
        {
            if (attackProcess.start)
            {
                player.ExecuteAttack();
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
                StartCoroutine(EndTurn());
                Stop();
                Board.touched = false;
            }
        }

        #endregion

        #region MOVE PROCESS

        public void MoveProcess()
        {
            if (moveProcess.start)
            {
                 player.ExecuteMove();
                 moveProcess.ContinuingProcess(false); 
            }
            else if (moveProcess.continuing)
            {
                if (player.IsMoveFinished())
                {
                    player.SetIsMoveFinished(false);
                    moveProcess.EndProcess();
                }
            }
            else if (moveProcess.end)
            {
                StartCoroutine(EndTurn());
                Stop();
                Board.touched = false;
            }
        }

        #endregion

        private IEnumerator EndTurn()
        {
            yield return new WaitForSeconds(0.2f);
        }

        public void InitProcessHandlers()
        {
            preparingProcess.Init(false);
            attackProcess.Init(false);
            moveProcess.Init(false);
        }
        public bool IsRunning()
        {
            return isRunning;
        }
        public void Run()
        {
            isRunning = true;
        }
        public Card GetCard()
        {
            return player;
        }
        public void Stop() 
        {
            isRunning = false;
            preparingProcess.Finish();
            attackProcess.Finish();
            moveProcess.Finish();
            Notify();
        }
        public void Begin() 
        {
            Run();
            preparingProcess.StartProcess();
        }
        private void Notify()
        {
            ms.OnNotify();
        }
    }
}
