﻿using UnityEngine;
using DungeonRush.Cards;
using DungeonRush.Element;
using DungeonRush.Moves;
using System;
using DungeonRush.DataPackages;
using System.Collections;
using System.Collections.Generic;

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

            MoveType moveType;

            public List<Card> highLevelCards;
            public List<int> attackersListnumbers;
            public int attackerIndex = 0;

            public static MoveMaker moveMaker;
            public static CardManager cardManager;
            [HideInInspector] public TourManager tourManager;
            [HideInInspector] public CoinCounter coinCounter;
            [HideInInspector] public CardController cardController;
            [HideInInspector] public AnimationHandler animHandler;

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
            [SerializeField, Range(0, 0.5f)] float outOfAnimationStateTimer;

            private void Awake()
            {
                moveMaker = FindObjectOfType<MoveMaker>();
                cardManager = FindObjectOfType<CardManager>();
                tourManager = FindObjectOfType<TourManager>();
                coinCounter = FindObjectOfType<CoinCounter>();
                cardController = FindObjectOfType<CardController>();
                animHandler = FindObjectOfType<AnimationHandler>();
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
                print("aI: " + attackerIndex);
                // -----> Player's move
                if (playerMoveProcess.IsRunning())
                {
                    //Move(cardManager.GetPlayerCard().GetTile().GetListNumber());
                    Move(cardManager.GetInstantPlayerTile().GetListNumber(), true);
                }
                // -----> 

                // -----> Dungeon's move
                if (dungeonBrainProcess.IsRunning())
                {
                    if (dungeonBrainProcess.start)
                    {
                        attackersListnumbers = DecideAttackerEnemies();
                        dungeonBrainProcess.ContinuingProcess(false);
                    }
                    else if (dungeonBrainProcess.continuing)
                    {
                        Move(attackersListnumbers[attackerIndex], false);
                    }
                    else if (dungeonBrainProcess.end)
                    {
                        attackersListnumbers.Remove(attackerIndex);
                        attackerIndex++;
                        if (attackersListnumbers.Count > 0)
                            dungeonBrainProcess.StartProcess();
                        else
                        {
                            dungeonBrainProcess.Finish();
                            playerMoveProcess.Init(true);
                        }
                    }
                }
                // ----->
            }

            #region MOVE METHODS

            #region PLAYER
            private void Move(int listNumber, bool isPlayer)
            {
                // -----> Determining tiles process
                if (moveProcess.IsRunning())
                {
                    if (moveProcess.start)
                    {
                        MakeMove(listNumber, isPlayer);
                    }
                    else if (moveProcess.end)
                    {
                        moveProcess.Finish();
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
                        animationProcess.EndProcess();
                    }
                    else if (animationProcess.end)
                    {
                        if(moveType != MoveType.Attack)
                            forwardCardProcess.StartProcess();
                        else
                            StartCoroutine(FinishAnimationTurn());
                        animationProcess.Finish();
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
                    else if (forwardCardProcess.continuing)
                    {
                        if (MoveMaker.movesFinished && !Board.touched)
                        {
                            moveMaker.ResetMoves();
                            MoveMaker.movesFinished = false;
                            forwardCardProcess.EndProcess();
                        }
                    }
                    else if (forwardCardProcess.end)
                    {
                        moveMaker.ResetMoves();
                        StartCoroutine(EndTurn());
                        forwardCardProcess.Finish();
                        if (playerMoveProcess.IsRunning())
                        {
                            playerMoveProcess.Finish();
                            SelectHighLevelCards();
                            dungeonBrainProcess.StartProcess();
                        }
                        else if (dungeonBrainProcess.IsRunning())
                        {
                            dungeonBrainProcess.EndProcess();
                        }
                    }
                }
                // ----->
            }

            private void MakeMove(int listNumber, bool isPlayer)
            {
                if (isPlayer)
                {
                    if (!Board.touched && SwipeManager.swipeDirection != Swipe.None)
                    {
                        targetTile = null;
                        targetTile2 = null;
                        targetTile3 = null;
                        targetTile4 = null;
                        // Can we start move?
                        canStartMoves = DoMove(listNumber, Swipe.None);
                        moveProcess.EndProcess();
                    }
                }
                else
                {
                    targetTile = null;
                    targetTile2 = null;
                    targetTile3 = null;
                    targetTile4 = null;
                    canStartMoves = DoMove(listNumber, SelectTileToAttack(listNumber));
                    moveProcess.EndProcess();
                }
            }

            #endregion

            #region DUNGEONBRAIN

            public void SelectHighLevelCards()
            {
                highLevelCards = cardManager.GetHighLevelCards();
            }

            private List<int> DecideAttackerEnemies()
            {
                List<int> listnumbers = new List<int>();
                var attackerCount = highLevelCards.Count % 4;
                for (int i = 0; i < attackerCount; i++)
                {
                    var number = UnityEngine.Random.Range(0, highLevelCards.Count);
                    if (!listnumbers.Contains(highLevelCards[number].GetTile().GetListNumber()))
                        listnumbers.Add(number);
                }
                return listnumbers;
            }

            public List<Swipe> GetAvailableTiles(int listnumber)
            {
                Tile ownTile = Board.tiles[listnumber];
                List<Swipe> avaibleTiles = new List<Swipe>();
                Tile lowerTile, leftTile, rightTile, upperTile;
                if (listnumber > 3)
                {
                    upperTile = Board.tiles[listnumber - 4];
                    if(ownTile.GetCard().GetCharacterType().IsEnemy(upperTile.GetCard().GetCharacterType()))
                        avaibleTiles.Add(Swipe.Up);
                }
                if (listnumber % 4 != 0)
                {
                    leftTile = Board.tiles[listnumber - 1];
                    if (ownTile.GetCard().GetCharacterType().IsEnemy(leftTile.GetCard().GetCharacterType()))
                        avaibleTiles.Add(Swipe.Left);
                }
                if (listnumber % 4 != 3)
                {
                    rightTile = Board.tiles[listnumber + 1];
                    if (ownTile.GetCard().GetCharacterType().IsEnemy(rightTile.GetCard().GetCharacterType()))
                        avaibleTiles.Add(Swipe.Right);
                }
                if (listnumber < 12)
                {
                    lowerTile = Board.tiles[listnumber + 4];
                    if (ownTile.GetCard().GetCharacterType().IsEnemy(lowerTile.GetCard().GetCharacterType()))
                        avaibleTiles.Add(Swipe.Down);
                }
                return avaibleTiles;
            }

            public Swipe SelectTileToAttack(int listnumber)
            {
                var tiles = GetAvailableTiles(listnumber);
                var number = UnityEngine.Random.Range(0, tiles.Count);
                return tiles[number];
            }

            private void DungeonMove(int listNumber)
            {
                
            }
            #endregion

            #region MOVE
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
                return cardController.AssignMoves(targetTile, targetTile2, targetTile3, targetTile4, out moveType);
            }

            private void MoveForward()
            {
                cardController.StartMoves();
                tourManager.IncreaseTourNumber();
            }

            private void StartMoves()
            {
                if (canStartMoves)
                {
                    MoveForward();
                    moveMaker.Move();
                    forwardCardProcess.ContinuingProcess(false);
                }
                else
                {
                    JustAttackMove();
                    forwardCardProcess.Finish();
                    moveProcess.StartProcess();
                }
            }

            private void JustAttackMove()
            {
                cardController.JustAttack();
                tourManager.FinishTour(false);
            }

            #endregion

            #region ANIMATION

            private IEnumerator FinishAnimationTurn()
            {
                yield return new WaitForSeconds(outOfAnimationStateTimer);
                forwardCardProcess.StartProcess();
            }

            private void DoAnimation()
            {
                animHandler.DoAnim(moveType, targetTile);
            }

            #endregion

            // TODO Tur olayı bir tek burda var. Burayı unutma..
            private IEnumerator EndTurn()
            {
                yield return new WaitForSeconds(0.1f);
                AddCard();
                yield return new WaitForSeconds(0.1f);
                tourManager.FinishTour(true);
                moveProcess.StartProcess();
            }

            #endregion

            #region CORE METHODS

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

            /// <summary>
            /// Adding card function for end of tour. Not for casual usage.
            /// </summary>
            public void AddCard()
            {
                int number = UnityEngine.Random.Range(0, 101);
                if (number < 70)
                    AddCard(GiveRandomCard(enemyCards), moveMaker.targetTileForAddingCard, false, board, true);
                else if (number < 95)
                    AddCard(GiveRandomCard(itemCards), moveMaker.targetTileForAddingCard, false, board, true);
                else
                    AddCard(GiveRandomCard(coinCards), moveMaker.targetTileForAddingCard, false, board, true);

                moveMaker.targetTileForAddingCard = null;
                NullControlOnTiles();
            }

            #endregion
        }
    }
}
