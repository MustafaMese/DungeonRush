using System.Collections;
using System.Collections.Generic;
using DungeonRush.Cards;
using DungeonRush.Data;
using DungeonRush.Field;
using UnityEngine;

namespace DungeonRush.Controller
{
    public class TrapController : MonoBehaviour, ICardController
    {
        [SerializeField] int activeTrapDistance = 4;

        private PlayerController playerController;
        private MoveSchedular ms;
        private ProcessHandleChecker determineProcess;
        private ProcessHandleChecker assigningProcess;
        private bool moveFinished = false;
        private bool isRunning = false;
        private int trapIndex;

        public static List<AIController> subscribedTraps = new List<AIController>();
        private List<AIController> trapCards;

        private void Start()
        {
            playerController = FindObjectOfType<PlayerController>();
            ms = FindObjectOfType<MoveSchedular>();
            InitProcessHandlers();
        }

        private void Update()
        {
            if (playerController == null) playerController = FindObjectOfType<PlayerController>();

            if (IsRunning())
            {
                if (determineProcess.IsRunning())
                {
                    Board.touched = true;
                    DetermineActiveTraps();
                    moveFinished = true;
                }
                else if (assigningProcess.IsRunning())
                {
                    if (trapIndex < trapCards.Count)
                    {
                        MoveControllers();
                    }
                    else
                        FinishMovement();
                }
            }
        }

        #region DETERMINIG METHODS
        private void DetermineActiveTraps()
        {
            trapCards = GetActiveTraps();
            determineProcess.Finish();
            assigningProcess.StartProcess();
            if(trapCards == null || trapCards.Count <= 0)
            {
                Stop();
            }
        }
        private List<AIController> GetActiveTraps()
        {
            Vector2 coordinate = playerController.transform.position;
            int rL = Board.RowLength;
            Tile t;
            Vector2 temp;
            List<AIController> l = new List<AIController>();

            for (int i = -(activeTrapDistance / 2); i < activeTrapDistance / 2 + 1; i++)
            {
                for (int j = -(activeTrapDistance / 2); j < activeTrapDistance / 2 + 1; j++)
                {
                    if (coordinate.x + j < 0 || coordinate.y + i < 0 || coordinate.x + j > rL - 1 || coordinate.y + i > rL - 1 || (i == 0 && j == 0))
                        continue;
                    temp = new Vector2(coordinate.x + j, coordinate.y + i);
                    t = Board.tilesByCoordinates[temp];

                    if (t != null && t.GetCard() != null)
                    {
                        AIController c = (AIController)t.GetCard().Controller;
                        if (subscribedTraps.Contains(c))
                            l.Add(c);
                    }
                }
            }

            return l;
        }

        #endregion

        #region ASSIGNING PROCESS
        private void MoveControllers()
        {
            if (moveFinished && trapCards[trapIndex] != null)
            {
                trapCards[trapIndex].Run();
                moveFinished = false;
            }
            else if (trapCards[trapIndex] == null)
            {
                trapIndex++;
                moveFinished = true;
            }
        }

        private void FinishMovement()
        {
            Stop();
            assigningProcess.Finish();
        }

        #endregion

        #region CARD CONTROLLER METHODS

        public void Begin()
        {
            Run();
            determineProcess.StartProcess();
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
        public void Run()
        {
            isRunning = true;
        }
        public void Stop()
        {
            isRunning = false;
            trapCards.Clear();
            trapIndex = 0;
            determineProcess.Finish();
            assigningProcess.Finish();
            Notify();
            Board.touched = false;
        }
        public void Notify()
        {
            ms.OnNotify();
        }
        public void OnNotify()
        {
            moveFinished = true;
            trapIndex++;
        }

        #endregion

        public static void UnsubscribeCard(AIController controller)
        {
            subscribedTraps.Remove(controller);
        }
    }
}
