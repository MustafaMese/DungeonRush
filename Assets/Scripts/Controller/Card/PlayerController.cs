using DungeonRush.Cards;
using DungeonRush.Data;
using DungeonRush.Field;
using DungeonRush.Managers;
using DungeonRush.Property;
using System.Collections;
using UnityEngine;

namespace DungeonRush.Controller
{
    public class PlayerController : MonoBehaviour, ICardController
    {
        MoveType moveType;

        [SerializeField] float animationFinishTime = 0.1f;
        [SerializeField] float timeForAddingCardET = 0.1f;
        [SerializeField] float timeForFinishTourET = 0.1f;

        bool canMoveToTarget = false;
        bool isRunning = false;
        public bool moveFinished = false;
        Card player;

        public ProcessHandleChecker preparingProcess;
        public ProcessHandleChecker animationProcess;
        public ProcessHandleChecker moveProcess;
        public NonPlayerController nonPlayerCont;

        private void Start()
        {
            Application.targetFrameRate = 60;
            InitProcessHandlers();
            preparingProcess.StartProcess();
            player = GetComponent<PlayerCard>();
            nonPlayerCont = FindObjectOfType<NonPlayerController>();
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
                print("1");
                print(SwipeManager.swipeDirection);
                var canMove = DoMove(SwipeManager.swipeDirection);

                if (canMove)
                {
                    print("2");
                    preparingProcess.Finish();
                    animationProcess.StartProcess();
                }
                else
                {
                    player.GetMove().Reset();
                    print("3");
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
                if (moveType != MoveType.ATTACK)
                    moveProcess.StartProcess();
                else
                    StartCoroutine(FinishAnimationTurn());
                animationProcess.Finish();
            }
        }

        private void DoAnimation(Card c)
        {
            Move m = c.GetMove();
            print("mGMT: " + m.GetMoveType());
            print("mGTT: " + m.GetTargetTile());
            print("mGLN: " + m.GetCard().GetTile().GetListNumber());
            c.HandleCardEffect(m.GetMoveType(), m.GetTargetTile(), m.GetCard().GetTile().GetListNumber());
        }

        private IEnumerator FinishAnimationTurn()
        {
            yield return new WaitForSeconds(0.27f);
            moveProcess.StartProcess();
        }

        #endregion

        #region MOVE PROCESS

        private void ExecuteMoves()
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
                if (moveFinished && !Board.touched)
                {
                    moveFinished = false;
                    moveProcess.EndProcess();
                }
            }
            else if (moveProcess.end)
            {
                StartCoroutine(EndTurn(canMoveToTarget));
                //nonPlayerCont.Run();
                moveProcess.Finish();
                preparingProcess.StartProcess();
            }
        }

        private IEnumerator EndTurn(bool c)
        {
            if (c)
            {
                yield return new WaitForSeconds(timeForAddingCardET);
                CardManager.Instance.AddCard(MoveMaker.Instance.targetTileForAddingCard);
                MoveMaker.Instance.targetTileForAddingCard = null;
                yield return new WaitForSeconds(timeForFinishTourET);
                
            }
            else
            {
                Board.touched = false;
                yield return new WaitForSeconds(timeForFinishTourET);
            }
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
            preparingProcess.StartProcess();
        }
    }
}
