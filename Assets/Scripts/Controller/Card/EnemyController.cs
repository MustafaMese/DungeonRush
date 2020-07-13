using DungeonRush.Cards;
using DungeonRush.Data;
using DungeonRush.Field;
using DungeonRush.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush.Controller
{
    public class EnemyController : MonoBehaviour, ICardController
    {
        [SerializeField] int attackerDistance = 4;

        private PlayerController playerController;
        private MoveSchedular ms;
        private ProcessHandleChecker determineProcess;
        private ProcessHandleChecker assigningProcess;
        private bool moveFinished = false;
        private bool isRunning = false;
        private int attackerIndex;

        public static List<AIController> subscribedEnemies = new List<AIController>();
        public List<AIController> attackerCards;

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
                    Board.touched = true;
                    DetermineAttackers();
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
        private void DetermineAttackers() 
        {
            attackerCards = GetAttackers();
            determineProcess.Finish();
            assigningProcess.StartProcess();
            if (attackerCards == null || attackerCards.Count <= 0)
            {
                Stop();
            }
        }
        private List<AIController> GetAttackers()
        {
            Vector2 coordinate = playerController.transform.position;
            int rL = Board.RowLength;
            Tile t;
            Vector2 temp;
            List<AIController> l = new List<AIController>();

            for (int i = -(attackerDistance / 2); i < attackerDistance / 2 + 1; i++)
            {
                for (int j = -(attackerDistance / 2); j < attackerDistance / 2 + 1; j++)
                {
                    if (coordinate.x + j < 0 || coordinate.y + i < 0 || coordinate.x + j > rL - 1 || coordinate.y + i > rL - 1 || (i == 0 && j == 0)) 
                        continue;
                    temp = new Vector2(coordinate.x + j, coordinate.y + i);
                    t = Board.tilesByCoordinates[temp];
                    
                    if (t != null && t.GetCard() != null)
                    {
                        AIController c = (AIController)t.GetCard().Controller;
                        if(subscribedEnemies.Contains(c))
                            l.Add(c);
                    }
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

        #region CARD CONTROLLER METHODS
        public void Stop()
        {
            isRunning = false;
            attackerCards.Clear();
            attackerIndex = 0;
            determineProcess.Finish();
            assigningProcess.Finish();
            Notify();
            Board.touched = false;
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
        }

        public void OnNotify() 
        {
            moveFinished = true;
            attackerIndex++;
        }

        public void Notify() 
        {
            ms.OnNotify();
        }

        #endregion

        public static void UnsubscribeCard(AIController controller)
        {
            subscribedEnemies.Remove(controller);
        }
    }
}
