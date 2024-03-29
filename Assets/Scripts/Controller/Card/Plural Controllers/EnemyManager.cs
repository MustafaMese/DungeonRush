﻿using DungeonRush.Data;
using DungeonRush.Field;
using DungeonRush.UI;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush.Controller
{
    public class EnemyManager : MonoBehaviour, ICardController
    {
        [SerializeField] int attackerDistance = 5;

        private PlayerController playerController;
        private ProcessHandleChecker determineProcess;
        private ProcessHandleChecker assigningProcess;
        private bool moveFinished = false;
        private bool isRunning = false;
        private int attackerIndex;

        public static List<EnemyAIController> subscribedEnemies = new List<EnemyAIController>();
        public List<EnemyAIController> attackerCards;

        private void Start()
        {
            playerController = MoveSchedular.Instance.playerController;
            InitProcessHandlers();
        }

        private void Update()
        {
            if (IsRunning())
            {
                if (determineProcess.IsRunning())
                {
                    Board.touched = true;
                    DetermineAttackers();
                    if (attackerCards.Count > 0)
                        UIManager.Instance.InitalizeEnemyTurn(attackerCards);
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
        private List<EnemyAIController> GetAttackers()
        {
            List<EnemyAIController> l = new List<EnemyAIController>();
            for (int i = 0; i < subscribedEnemies.Count; i++)
            {
                EnemyAIController ai = subscribedEnemies[i];

                if (ai == null) continue;
                
                ai.ActivateStatuses();

                if (ai == null) continue;

                var distance = GetDistance(ai.transform.position);
                if (distance <= attackerDistance)
                {
                    SetAttackerSkinState(ai, false);
                    l.Add(ai);
                }
                else
                    SetAttackerSkinState(ai, true);
            }

            return l;
        }

        public void SetAttackerSkinState(EnemyAIController ai, bool shadowed)
        {
            if (shadowed)
                ai.ChangeSkinState(false);
            else
                ai.ChangeSkinState(true);
        }

        private float GetDistance(Vector3 i)
        {
            if (playerController == null) return 0;

            return (i - playerController.transform.position).sqrMagnitude;
        }

        #endregion

        #region ASSINGING PROCESS

        private void MoveControllers()
        {
            if (moveFinished && attackerCards[attackerIndex] != null)
            {
                UIManager.Instance.ChangeIcons(attackerIndex);
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
            ConfigureSurroundingCardsSkinStates();
            MoveSchedular.Instance.OnNotify();
        }

        public void ConfigureSurroundingCardsSkinStates()
        {
            for (int i = 0; i < subscribedEnemies.Count; i++)
            {

                if (subscribedEnemies[i] == null) continue;

                var distance = GetDistance(subscribedEnemies[i].transform.position);
                if (distance <= attackerDistance)
                    SetAttackerSkinState(subscribedEnemies[i], false);
                else
                    SetAttackerSkinState(subscribedEnemies[i], true);
            }
        }

        #endregion

        public static void UnsubscribeCard(EnemyAIController controller)
        {
            subscribedEnemies.Remove(controller);
        }

        private void OnDestroy()
        {
            subscribedEnemies.Clear();
        }
    }
}
