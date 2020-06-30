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
        List<Tile> avaibleTiles = new List<Tile>();

        public ProcessHandleChecker determineProcess;
        public ProcessHandleChecker decideProcess;
        public ProcessHandleChecker assigningProcess;

        public List<Card> highLevelCards;
        public List<AIController> attackers;

        [SerializeField] int attackerDistance = 4;
        public bool movesFinished = false;

        public bool isRunning = false;
        bool finishTurn = false;
        public bool FinishTurn { get => finishTurn; set => finishTurn = value; }

        public PlayerController playerController;
        public MoveSchedular ms;

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
                    print("np1");
                }
                else if (decideProcess.IsRunning())
                {
                    print("np2");
                    DecidingProcess();
                }
                else if (assigningProcess.IsRunning())
                {
                    print("np4");
                    if (assigningProcess.start) 
                    {
                        AssignControllers();
                    }
                    else if (assigningProcess.end) 
                    {
                        if (movesFinished)
                        {
                            FinishMovement();
                        }
                    }
                }
            }
        }

        #region DETERMINING METHODS
        private void DetermineHighLevelCards() 
        {
            highLevelCards = GetHighLevelCards();
            determineProcess.Finish();
            decideProcess.StartProcess();
            if (highLevelCards == null || highLevelCards.Count <= 0)
            {
                print("np1.1");
                Stop();
            }
        }
        private List<Card> GetHighLevelCards()
        {
            Vector2 coordinate = playerController.transform.position;
            int rL = Board.RowLength;
            Tile t;
            Vector2 temp;
            List<Card> l = new List<Card>();

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
                        l.Add(t.GetCard());
                }
            }

            return l;
        }

        #endregion

        #region DECIDING METHODS
        private void DecidingProcess() 
        {
            attackers = DecideAttackerEnemies();
            highLevelCards.Clear();
            decideProcess.Finish();
            assigningProcess.StartProcess();
            if (attackers == null || attackers.Count <= 0)
            {
                print("np3");
                Stop();
            }
        }
        private List<AIController> DecideAttackerEnemies()
        {
            List<AIController> cards = new List<AIController>();
            var attackerCount = highLevelCards.Count;
            foreach (var hc in highLevelCards)
            {
                cards.Add((AIController)hc.Controller);
            }
            return cards;
        }

        #endregion

        #region ASSINGING PROCESS
        private void FinishMovement()
        {
            Stop();
            assigningProcess.Finish();
            movesFinished = false;
        }
        private void AssignControllers() 
        {
            SetAvaibleTiles();
            SelectTilesToAttack();
            foreach (var attacker in attackers)
            {
                print("np5");
                attacker.Run();
            }
            assigningProcess.EndProcess();
        }
        private void SelectTilesToAttack()
        {
            foreach (var attacker in attackers)
            {
                attacker.swipe = attacker.GetCard().GetShift().SelectTileToAttack(attacker.avaibleTiles);
            }
        }
        private void SetAvaibleTiles()
        {
            List<AIController> keys = new List<AIController>(attackers);
            foreach (var attacker in keys)
            {
                attacker.avaibleTiles = attacker.GetCard().GetShift().GetAvaibleTiles(attacker.GetCard());
                if (avaibleTiles != null && attacker.avaibleTiles != null)
                {
                    List<Tile> keyList = new List<Tile>(attacker.avaibleTiles.Keys);
                    foreach (var tile in keyList)
                    {
                        if (avaibleTiles.Contains(tile) || (tile.GetCard() != null && attackers.Contains((AIController)tile.GetCard().Controller)))
                        {
                            attacker.avaibleTiles.Remove(tile);
                        }
                        else
                        {
                            avaibleTiles.Add(tile);
                        }
                    }
                }
                else
                    attackers.Remove(attacker);
            }
        }
        #endregion

        public void Stop()
        {
            isRunning = false;
            highLevelCards.Clear();
            attackers.Clear();
            avaibleTiles.Clear();
            decideProcess.Finish();
            determineProcess.Finish();
            assigningProcess.Finish();
            movesFinished = false;
            Notify();
        }

        public void Run()
        {
            isRunning = true;
        }

        public void InitProcessHandlers()
        {
            determineProcess.Init(false);
            decideProcess.Init(false);
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
            movesFinished = false;
            attackers.Clear();
            highLevelCards.Clear();
            avaibleTiles.Clear();
        }

        public void OnNotify() 
        {
            foreach (var attacker in attackers)
            {
                if (attacker.GetCard().isMoving)
                    return;
            }
            movesFinished = true;
        }

        private void Notify() 
        {
            ms.OnNotify();
        }
    }
}
