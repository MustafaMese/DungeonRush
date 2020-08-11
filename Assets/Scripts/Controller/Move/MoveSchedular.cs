using DungeonRush.Field;
using DungeonRush.Managers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DungeonRush.Controller
{
    public class MoveSchedular : MonoBehaviour
    {
        public TextMeshProUGUI tourText;

        [SerializeField] bool notify = false;
        /// <summary>
        /// -1 for Nothing, 0 for Player, 1 for NonPlayers, 2 for Traps
        /// </summary>
        [SerializeField] int turnNumber;
        [SerializeField] int oldTurnNumber;
        [SerializeField] int tourCount;

        public PlayerController playerController;
        public EnemyController enemyController;
        public TrapController trapController;
        public Board board;

        public bool isGameStarted = false;

        private void Start()
        {
            playerController = FindObjectOfType<PlayerController>();
            enemyController = FindObjectOfType<EnemyController>();
            trapController = FindObjectOfType<TrapController>();
            board = FindObjectOfType<Board>();

            tourCount = 0;
            tourText.text = tourCount.ToString();
            turnNumber = 0;
            oldTurnNumber = -1;
        }

        private void Update()
        {
            if(GameManager.gameState == GameState.BEGIN_LEVEL) 
            {
                playerController.Begin();
                GameManager.gameState = GameState.PLAY;
            }
        }

        public void IncreaseTour() 
        {
            tourCount++;
            tourText.text = tourCount.ToString();
        }

        public void OnNotify() 
        {
            if (GameManager.gameState == GameState.STOP_GAME || GameManager.gameState == GameState.PAUSE) return;

            if(turnNumber != 2)
            {
                oldTurnNumber = turnNumber;
                turnNumber = 2;

                if (oldTurnNumber == 0)
                    IncreaseTour();

               // OnNotify();
            }
            else
            {
                if (oldTurnNumber == 0)
                {
                    turnNumber = 1;
                    board.SetTileDarkness();
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
                playerController.Begin();
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
