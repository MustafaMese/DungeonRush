using DungeonRush.Cards;
using DungeonRush.Data;
using DungeonRush.Field;
using DungeonRush.Managers;
using DungeonRush.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush.Controller
{
    public class EnemyController : MonoBehaviour, ICardController
    {
        [SerializeField] int attackerDistance = 5;

        private PlayerController playerController;
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
        private List<AIController> GetAttackers()
        {
            List<AIController> l = new List<AIController>();
            for (int i = 0; i < subscribedEnemies.Count; i++)
            {
                subscribedEnemies[i].ActivateStatuses();

                var distance = GetDistance(subscribedEnemies[i].transform.position);
                if (distance <= attackerDistance)
                {
                    SetAttackerSkinState(subscribedEnemies[i], false);
                    l.Add(subscribedEnemies[i]);
                }
                else
                    SetAttackerSkinState(subscribedEnemies[i], true);
            }

            return l;
        }

        public void SetAttackerSkinState(AIController ai, bool shadowed)
        {
            if (shadowed)
            {
                ai.ChangeShadowState(true);
                ai.ChangeAnimatorState(false);
            }
            else
            {
                ai.ChangeShadowState(false);
                ai.ChangeAnimatorState(true);
            }
        }

        private float GetDistance(Vector3 i)
        {
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

        public static void UnsubscribeCard(AIController controller)
        {
            subscribedEnemies.Remove(controller);
        }

        private void OnDestroy()
        {
            subscribedEnemies.Clear();
        }
    }
}
