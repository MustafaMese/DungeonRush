using System.Collections;
using System.Collections.Generic;
using DungeonRush.Cards;
using DungeonRush.Data;
using DungeonRush.Field;
using UnityEngine;

namespace DungeonRush.Controller
{
    // TODO bu sınıfı optimize et.
    public class EnvironmentManager : MonoBehaviour, ICardController
    {
        [SerializeField] int activeTrapDistance = 4;

        private PlayerController playerController;
        private ProcessHandleChecker determineProcess;
        private ProcessHandleChecker assigningProcess;
        private bool moveFinished = false;
        private bool isRunning = false;
        private int trapIndex;
        private List<EnvironmentAIController> trapCards;

        public List<EnvironmentElement> elementPrefabs;

        public static List<EnvironmentAIController> subscribedEnvironmentCards = new List<EnvironmentAIController>();
        
        private void Start()
        {
            playerController = FindObjectOfType<PlayerController>();
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
                        MoveControllers();
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
        private List<EnvironmentAIController> GetActiveTraps()
        {
            List<EnvironmentAIController> l = new List<EnvironmentAIController>();

            for (int i = 0; i < subscribedEnvironmentCards.Count; i++)
            {
                if ((subscribedEnvironmentCards[i].transform.position - playerController.transform.position).sqrMagnitude <= activeTrapDistance)
                    l.Add(subscribedEnvironmentCards[i]);
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
            MoveSchedular.Instance.OnNotify();
        }
        public void OnNotify()
        {
            moveFinished = true;
            trapIndex++;
        }

        #endregion

        public static void UnsubscribeCard(EnvironmentAIController controller)
        {
            subscribedEnvironmentCards.Remove(controller);
        }

        private void OnDestroy()
        {
            subscribedEnvironmentCards.Clear();
        }
    }
}
