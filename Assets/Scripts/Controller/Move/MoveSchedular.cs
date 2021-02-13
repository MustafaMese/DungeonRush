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
        private enum Tour {PLAYER, ENEMY, TRAP}
        private Tour tour;
        private bool isPlayerTurn;

        private static MoveSchedular instance = null;
        // Game Instance Singleton
        public static MoveSchedular Instance
        {
            get { return instance; }
            set { instance = value; }
        }

        [SerializeField] EnemyManager enemyControllerPrefab;
        [SerializeField] EnvironmentManager environmentControllerPrefab;

        [HideInInspector] public EnemyManager enemyController;
        [HideInInspector] public EnvironmentManager environmentController;
        [HideInInspector] public PlayerController playerController;

        private void Awake()
        {
            Instance = this;
        }

        protected void Initialize()
        {
            tour = Tour.PLAYER;
            isPlayerTurn = false;

            enemyController = Instantiate(enemyControllerPrefab);
            environmentController = Instantiate(environmentControllerPrefab);
        }

        public void StartGame()
        {
            Initialize();
            playerController.Begin();
            GameManager.Instance.SetGameState(GameState.PLAY);
        }

        public void OnNotify() 
        {
            if (GameManager.Instance.gameState == GameState.DEFEAT || GameManager.Instance.gameState == GameState.PAUSE) return;
            CardManager.Instance.ReshuffleCards();
            switch (tour)
            {
                case Tour.PLAYER:
                    environmentController.Begin();
                    isPlayerTurn = false;
                    tour = Tour.TRAP;
                    break;
                case Tour.ENEMY:
                    environmentController.Begin();
                    isPlayerTurn = true;
                    tour = Tour.TRAP;
                    break;
                case Tour.TRAP:
                    if(isPlayerTurn)
                    {
                        UIManager.Instance.InitializePlayerTurn();
                        playerController.Begin();
                        playerController.ActivateStatuses();
                        tour = Tour.PLAYER;
                    }
                    else
                    {
                        enemyController.Begin();
                        tour = Tour.ENEMY;
                    }
                    break;
            }
        }
    }
}
