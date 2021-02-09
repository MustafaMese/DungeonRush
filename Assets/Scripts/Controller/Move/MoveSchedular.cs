using DungeonRush.Field;
using DungeonRush.Managers;
using DungeonRush.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DungeonRush.Controller
{
    public class MoveSchedular : MonoBehaviour
    {
        private static MoveSchedular instance = null;
        // Game Instance Singleton
        public static MoveSchedular Instance
        {
            get { return instance; }
            set { instance = value; }
        }
        /// <summary>
        /// -1 for Nothing, 0 for Player, 1 for NonPlayers, 2 for Traps
        /// </summary>
        [SerializeField] int turnNumber;
        [SerializeField] int oldTurnNumber;
        [SerializeField] int tourCount;

        public PlayerController playerController;

        [SerializeField] EnemyManager enemyControllerPrefab;
        [SerializeField] TrapManager trapControllerPrefab;

        public EnemyManager enemyController;
        public TrapManager trapController;

        private void Awake()
        {
            Instance = this;
        }

        protected void Initialize()
        {
            tourCount = 0;
            turnNumber = 0;
            oldTurnNumber = -1;

            enemyController = Instantiate(enemyControllerPrefab);
            trapController = Instantiate(trapControllerPrefab);
        }

        public void StartGame()
        {
            Initialize();
            playerController.Begin();
            GameManager.Instance.SetGameState(GameState.PLAY);
        }

        public void IncreaseTour() 
        {
            tourCount++;
        }

        public void OnNotify() 
        {
            if (GameManager.Instance.gameState == GameState.DEFEAT || GameManager.Instance.gameState == GameState.PAUSE) return;

            if(turnNumber != 2)
            {
                oldTurnNumber = turnNumber;
                turnNumber = 2;

                if (oldTurnNumber == 0)
                    IncreaseTour();
            }
            else
            {
                if (oldTurnNumber == 0)
                {
                    turnNumber = 1;
                    //board.SetTileDarkness();
                }
                else
                    turnNumber = 0;

                oldTurnNumber = -1;
            }

            CardManager.Instance.ReshuffleCards();
            if (turnNumber == -1) 
            {
                // Nothing
            }
            else if (turnNumber == 0)
            {
                UIManager.Instance.InitializePlayerTurn();
                playerController.Begin();
                playerController.ActivateStatuses();
            }
            else if (turnNumber == 1)
            {
                enemyController.Begin();
            }
            else if (turnNumber == 2)
            {
                trapController.Begin();
            }
        }
    }
}
