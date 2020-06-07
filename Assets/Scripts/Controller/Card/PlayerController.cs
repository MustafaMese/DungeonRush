using DungeonRush.Cards;
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
        [SerializeField] float timeForFinishTourET = 0.1f;

        bool isRunning = false;
        Card player;

        public ProcessHandleChecker preparingProcess;
        public ProcessHandleChecker animationProcess;
        public ProcessHandleChecker moveProcess;
        public NonPlayerController nonPlayerCont;
        public MoveSchedular ms;

        private void Start()
        {
            InitProcessHandlers();
            player = GetComponent<PlayerCard>();
            ms = FindObjectOfType<MoveSchedular>();
        }

        private void Update()
        {
            if (preparingProcess.IsRunning())
            {
                PrepareMoveProcess();
            }
            else if (animationProcess.IsRunning())
            {
                AnimationProcess(player);
            }
            else if (moveProcess.IsRunning())
            {
                ExecuteMoves();
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
                    animationProcess.StartProcess();
                }
                else
                {
                    player.GetMove().Reset();
                }
            }
        }

        private bool DoMove(Swipe swipe)
        {
            return player.GetShift().Define(player, swipe);
        }

        #endregion

        #region ANIMATION METHODS

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
                var move = player.GetMove().GetCanMove();

                if (move)
                {
                    player.ExecuteMove();
                    moveProcess.ContinuingProcess(false);
                }
                else
                {
                    player.Attack(player.GetMove().GetTargetTile().GetCard());
                    moveProcess.EndProcess();
                }
            }
            else if (moveProcess.continuing)
            {
                if (player.GetComponent<Mover>().moveFinished && !Board.touched)
                {
                    player.GetComponent<Mover>().moveFinished = false;
                    moveProcess.EndProcess();
                }
            }
            else if (moveProcess.end)
            {
                StartCoroutine(EndTurn());
                Stop();
            }
        }

        private IEnumerator EndTurn()
        {
            Board.touched = false;
            yield return new WaitForSeconds(timeForFinishTourET);
        }

        #endregion

        public void InitProcessHandlers()
        {
            preparingProcess.Init(false);
            animationProcess.Init(false);
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
            animationProcess.Finish();
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
