using DungeonRush.Cards;
using DungeonRush.Data;
using DungeonRush.Field;
using DungeonRush.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush.Controller
{
    public class NonPlayerController : MonoBehaviour, ICardController
    {
        public ProcessHandleChecker determineProcess;
        public ProcessHandleChecker assigningProcess;

        public List<AIController> attackerCards;

        public bool moveFinished = false;
        [SerializeField] int attackerDistance = 4;

        public bool isRunning = false;
        bool finishTurn = false;
        public bool FinishTurn { get => finishTurn; set => finishTurn = value; }

        public PlayerController playerController;
        public MoveSchedular ms;

        public int attackerIndex;

        private void Start()
        {
            playerController = FindObjectOfType<PlayerController>();
            ms = FindObjectOfType<MoveSchedular>();
            InitProcessHandlers();
        }

        private void Update()
        {
            if(playerController == null) playerController = FindObjectOfType<PlayerController>();

            if (IsRunning())
            {
                if (determineProcess.IsRunning())
                {
                    DetermineHighLevelCards();
                    moveFinished = true;
                }
                else if (assigningProcess.IsRunning())
                {
                    if (attackerIndex < attackerCards.Count)
                    {
                        MoveControllers();
                    }
                    else
                        FinishMovement();
                }
            }
        }

        #region DETERMINING METHODS
        private void DetermineHighLevelCards() 
        {
            attackerCards = GetHighLevelCards();
            determineProcess.Finish();
            assigningProcess.StartProcess();
            if (attackerCards == null || attackerCards.Count <= 0)
            {
                print("np1.1");
                Stop();
            }
        }
        private List<AIController> GetHighLevelCards()
        {
            Vector2 coordinate = playerController.transform.position;
            int rL = Board.RowLength;
            Tile t;
            Vector2 temp;
            List<AIController> l = new List<AIController>();

            for (int i = -2; i < attackerDistance / 2 + 1; i++)
            {
                if (i == 0)
                    continue;

                for (int j = -2; j < attackerDistance / 2 + 1; j++)
                {
                    if (j == 0)
                        continue;

                    if (coordinate.x + j < 0 || coordinate.y + i < 0 || coordinate.x + j > rL - 1 || coordinate.y + i > rL - 1) 
                        continue;

                    temp = new Vector2(coordinate.x + j, coordinate.y + i);
                    t = Board.tilesByCoordinates[temp];
                    if (t != null && t.GetCard() != null)
                        l.Add((AIController)t.GetCard().Controller);
                }
            }

            return l;
        }

        #endregion

        #region ASSINGING PROCESS

        private void MoveControllers()
        {
            if (moveFinished && attackerCards[attackerIndex] != null)
            {
                attackerCards[attackerIndex].Run();
                moveFinished = false;
            }
            else if (attackerCards[attackerIndex] == null)
            {
                attackerIndex++;
                moveFinished = true;
            }
        }

        private void FinishMovement()
        {
            Stop();
            assigningProcess.Finish();
        }


        #endregion

        public void Stop()
        {
            isRunning = false;
            attackerCards.Clear();
            attackerIndex = 0;
            determineProcess.Finish();
            assigningProcess.Finish();
            Notify();
        }

        public void Run()
        {
            isRunning = true;
        }

        public void InitProcessHandlers()
        {
            determineProcess.Init(false);
            assigningProcess.Init(false);
        }

        public bool IsRunning()
        {
            return isRunning;
        }
    
        public void Begin() 
        {
            Run();
            determineProcess.StartProcess();
            attackerCards.Clear();
        }

        public void OnNotify() 
        {
            moveFinished = true;
            attackerIndex++;
        }

        private void Notify() 
        {
            ms.OnNotify();
        }
    }
}
