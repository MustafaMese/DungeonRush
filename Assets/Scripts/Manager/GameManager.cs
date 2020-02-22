using UnityEngine;
using DungeonRush.Cards;
using DungeonRush.Element;
using DungeonRush.Moves;
using System;
using DungeonRush.DataPackages;

namespace DungeonRush
{
    namespace Managers
    {
        public class GameManager : MonoBehaviour
        {
            Tile targetTile = null;
            Tile targetTile2 = null;
            Tile targetTile3 = null;
            Tile targetTile4 = null;

            public static MoveMaker moveMaker;
            public static CardManager cardManager;
            public TourManager tourManager;
            public CoinCounter coinCounter;
            public CardController cardController;

            [Header("Board")]
            public Board board;

            [Header("Characters")]
            public PlayerCard playerCard;
            public EnemyCard[] enemyCards;
            public ItemCard[] itemCards;
            public CoinCard[] coinCards;

            bool canStartMoves;
            private ProcessHandleChecker playerMoveProcess;
            private ProcessHandleChecker moveProcess;
            private ProcessHandleChecker animationProcess;
            private ProcessHandleChecker forwardCardProcess;
            private ProcessHandleChecker dungeonBrainProcess;

            private void Awake()
            {
                moveMaker = FindObjectOfType<MoveMaker>();
                cardManager = FindObjectOfType<CardManager>();
                tourManager = FindObjectOfType<TourManager>();
                coinCounter = FindObjectOfType<CoinCounter>();
                cardController = FindObjectOfType<CardController>();
            }

            private void Start()
            {
                playerMoveProcess.Init(true);
                moveProcess.Init(true);
                moveProcess.StartProcess();
                animationProcess.Init(false);
                forwardCardProcess.Init(false);
                dungeonBrainProcess.Init(false);
            }

            private void Update()
            {
                // -----> Player's move
                if (playerMoveProcess.IsRunning())
                {
                    //Move(cardManager.GetPlayerCard().GetTile().GetListNumber());
                    Move(cardManager.GetInstantPlayerTile().GetListNumber());
                }
                // -----> 

                // -----> Dungeon's move
                if (dungeonBrainProcess.start)
                {

                }
                // ----->
            }

            private void Move(int listNumber)
            {
                // -----> Determining tiles process
                if (moveProcess.IsRunning())
                {
                    if (moveProcess.start)
                    {
                        MakePlayerMove(listNumber);
                    }
                    else if (moveProcess.end)
                    {
                        moveProcess.EndProcess();
                        animationProcess.StartProcess();
                    }
                }
                // ----->

                // -----> Animation process
                else if (animationProcess.IsRunning())
                {
                    if (animationProcess.start)
                    {
                        DoAnimation();
                    }
                    else if (animationProcess.end)
                    {
                        Invoke("ForwardCardProcessSetTrue", 1f);
                        animationProcess.EndProcess();
                        forwardCardProcess.StartProcess();
                    }
                }
                // ----->

                // -----> Forward Card Process
                else if (forwardCardProcess.IsRunning())
                {
                    if (forwardCardProcess.start)
                    {
                        StartMoves();
                    }
                    else if (forwardCardProcess.end)
                    {
                        Invoke("MoveProcessSetTrue", 1f);
                    }
                }
                // ----->
            }

            private void StartMoves()
            {
                forwardCardProcess.start = false;
                playerMoveProcess.start = false;
                if (canStartMoves)
                    MoveForward();
                else
                    JustAttackMove();

                moveMaker.Move();
            }

            private void DoAnimation()
            {
                
            }

            private void MakePlayerMove(int listNumber)
            {
                if (!Board.touched && tourManager.IsTourNumbersEqual() && SwipeManager.swipeDirection != Swipe.None)
                {
                    targetTile = null;
                    targetTile2 = null;
                    targetTile3 = null;
                    targetTile4 = null;
                    // Can we start move?
                    canStartMoves = DoMove(listNumber, Swipe.None);
                    moveProcess.ContinuingProcess(true);
                }
            }

            private void JustAttackMove()
            {
                cardController.JustAttack();
                tourManager.FinishTour(false);
            }

            // Assign kısmını tamamen move'a taşı. Mesela MoveMaker'daki instant moves olablir.
            private bool DoMove(int listnumber, Swipe swipe)
            {
                try
                {
                    cardController.AssignTiles(listnumber, ref targetTile, ref targetTile2, ref targetTile3, ref targetTile4, swipe);
                }
                catch (Exception)
                {
                    throw new Exception("Dictionary'de sınırı aştın. No problema. Error 31");
                }
                return cardController.AssignMoves(targetTile, targetTile2, targetTile3, targetTile4);
            }

            private void MoveForward()
            {
                cardController.StartMoves();
                tourManager.IncreaseTourNumber();
            }

            public Card AddCard(Card piece, Tile tile, bool playerCard, Board board, bool inGame)
            {
                Card newPiece = Instantiate(piece, tile.transform.position, Quaternion.identity, board.transform);
                tile.SetCard(newPiece);
                newPiece.SetTile(tile);
                cardManager.AddToCards(newPiece, inGame);
                // Card informations bu if ile beraber oyundan fiilen çıkarılmış oldu.
                if(!inGame)
                    cardManager.cardInformations.Add(CardManager.CreateCardInformation(tile, newPiece, playerCard));
                return newPiece;
            }

            public static void RemoveCard(Tile tile)
            {
                foreach (var card in cardManager.cards)
                {
                    if(card.GetTile() == tile)
                    {
                        tile.SetCard(null);
                        Destroy(card.transform.gameObject);
                        return;
                    }
                }
            }

            public static void RemoveCard(Move move, bool isPlayerCard)
            {
                if (!isPlayerCard)
                {
                    if (!move.GetTargetTile().IsTileOccupied())
                        return;

                    foreach (var card in cardManager.cards)
                    {
                        if (card.GetTile() == move.GetTargetTile())
                        {
                            move.GetTargetTile().SetCard(null);
                            Destroy(card.transform.gameObject);
                            return;
                        }
                    }
                    throw new Exception("Error 31");
                }
                else
                {
                    foreach (var card in cardManager.cards)
                    {
                        if (card == move.GetCard())
                        {
                            Destroy(card.transform.gameObject);
                            return;
                        }
                    }
                    throw new Exception("Error 31");
                }
            }

            public static CardManager GetCardManager()
            {
                return cardManager;
            }

            public static MoveMaker GetMoveMaker()
            {
                return moveMaker;
            }

            public void NullControlOnTiles()
            {
                for (int i = 0; i < Board.tiles.Count; i++)
                {
                    if (Board.tiles[i].GetCard() == null)
                    {
                        foreach (var card in GameManager.GetCardManager().cards)
                        {
                            if (card.GetTile() == Board.tiles[i])
                            {
                                Board.tiles[i].SetCard(card);
                                return;
                            }
                        }
                        AddCardImmediately(Board.tiles[i]);
                        throw new Exception("Bişeyler ekledik.");
                    }
                }
            }

            public void AddCardImmediately(Tile tile)
            {
                int number = UnityEngine.Random.Range(0, 101);
                if (number < 70)
                    AddCard(GiveRandomCard(enemyCards), tile, false, board, true);
                else if (number < 95)
                    AddCard(GiveRandomCard(itemCards), tile, false, board, true);
                else
                    AddCard(GiveRandomCard(coinCards), tile, false, board, true);
            }

            private Card GiveRandomCard(Card[] card)
            {
                int length = card.Length;
                return card[UnityEngine.Random.Range(0, length)];
            }

            private void MoveProcessSetTrue()
            {
                forwardCardProcess.ContinuingProcess(true);
            }

            private void ForwardCardProcessSetTrue()
            {
                animationProcess.ContinuingProcess(true);
            }
        }
    }
}
