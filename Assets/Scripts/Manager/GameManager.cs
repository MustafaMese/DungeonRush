using UnityEngine;
using DungeonRush.Cards;
using DungeonRush.Field;
using System;
using DungeonRush.Data;
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
            bool dungeonBrainZeroMove;
            public static bool addCard;
            public static int cardListNumber;

            public List<Card> highLevelCards;
            public List<int> attackersListnumbers;
            public int attackerIndex = 0;

            public static MoveMaker moveMaker;
            public static CardManager cardManager;

            TourManager tourManager;
            CoinCounter coinCounter;

            [HideInInspector] public AnimationHandler animHandler;

            [Header("Board")]
            public Board board;

            bool canStartMoves;
            private ProcessHandleChecker playerMoveProcess;
            private ProcessHandleChecker moveProcess;
            private ProcessHandleChecker animationProcess;
            private ProcessHandleChecker forwardCardProcess;
            private ProcessHandleChecker dungeonBrainProcess;

            [SerializeField, Range(0, 0.5f)] float outOfAnimationStateTimer;
            [SerializeField, Range(0, 0.5f)] float timeForFinishTourET = 0.1f;
            [SerializeField, Range(0, 0.5f)] float timeForAddingCardET = 0.1f;

            private void Awake()
            {
                Application.targetFrameRate = 60;
                moveMaker = GetComponent<MoveMaker>();
                cardManager = GetComponent<CardManager>();
                tourManager = GetComponent<TourManager>();
                coinCounter = GetComponent<CoinCounter>();
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
                        attackerIndex = 0;
                        attackersListnumbers = DecideAttackerEnemies();
                        if (attackersListnumbers.Count > 0)
                        {
                            dungeonBrainProcess.ContinuingProcess(false);
                        }
                        else
                        {
                            dungeonBrainProcess.Finish();
                            playerMoveProcess.Init(true);
                        }
                    }
                    else if (dungeonBrainProcess.continuing)
                    {
                        if (attackerIndex >= 0 && attackersListnumbers != null && attackerIndex < attackersListnumbers.Count)
                            Move(attackersListnumbers[attackerIndex], false);
                        else
                        {
                            dungeonBrainProcess.EndProcess();
                            attackerIndex = -1;
                            attackersListnumbers = null;
                        }
                    }
                    else if (dungeonBrainProcess.end)
                    {
                        if (addCard && cardListNumber != -1)
                        {
                            cardManager.AddCard(Board.tiles[cardListNumber]);
                            addCard = false;
                            cardListNumber = -1;
                            highLevelCards = cardManager.GetHighLevelCards();
                            attackersListnumbers = DecideAttackerEnemies();
                            attackerIndex = 0;
                            UnnecessaryCardControl();
                        }

                        if (attackerIndex != -1 && attackersListnumbers != null && attackerIndex + 1 != attackersListnumbers.Count)
                        {
                            attackerIndex++;
                            dungeonBrainProcess.ContinuingProcess(false);
                        }
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
            
            private void MakePlayerMove(int listNumber)
            {
                if (!Board.touched && SwipeManager.swipeDirection != Swipe.NONE)
                {
                    targetTile = null;
                    targetTile2 = null;
                    targetTile3 = null;
                    targetTile4 = null;
                    // Can we start move?
                    canStartMoves = DoMove(listNumber, Swipe.PLAYER);
                    moveProcess.EndProcess();
                }
            }

            #endregion

            #region DUNGEONBRAIN

            public bool MakeEnemyMove(int listNumber)
            {
                targetTile = null;
                targetTile2 = null;
                targetTile3 = null;
                targetTile4 = null;
                var swipe = SelectTileToAttack(listNumber);
                if (swipe != Swipe.NONE)
                {
                    canStartMoves = DoMove(listNumber, swipe);
                    moveProcess.EndProcess();
                    return true;
                }
                else
                    return false;
            }

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
                    var listnumber = highLevelCards[i].GetTile().GetListNumber();
                    if (!listnumbers.Contains(listnumber))
                        listnumbers.Add(listnumber);
                }
                return listnumbers;
            }

            public List<Swipe> GetAvailableTiles(int listnumber)
            {
                Tile ownTile = Board.tiles[listnumber];
                List<Swipe> avaibleTiles = new List<Swipe>();
                Tile lowerTile, leftTile, rightTile, upperTile;

                if (ownTile.GetCard() == null) return null;
                try
                {
                    if (listnumber > 3)
                    {
                        upperTile = Board.tiles[listnumber - 4];
                        //print("t: " + upperTile);
                        if (ownTile.GetCard().GetCharacterType().IsEnemy(upperTile.GetCard().GetCharacterType()))
                            avaibleTiles.Add(Swipe.UP);
                    }
                    if (listnumber % 4 != 0)
                    {
                        leftTile = Board.tiles[listnumber - 1];
                        //print("t: " + leftTile);
                        if (ownTile.GetCard().GetCharacterType().IsEnemy(leftTile.GetCard().GetCharacterType()))
                            avaibleTiles.Add(Swipe.LEFT);
                    }
                    if (listnumber % 4 != 3)
                    {
                        rightTile = Board.tiles[listnumber + 1];
                        //print("t: " + rightTile);
                        if (ownTile.GetCard().GetCharacterType().IsEnemy(rightTile.GetCard().GetCharacterType()))
                            avaibleTiles.Add(Swipe.RIGHT);
                    }
                    if (listnumber < 12)
                    {
                        lowerTile = Board.tiles[listnumber + 4];
                        //print("t: " + lowerTile);
                        if (ownTile.GetCard().GetCharacterType().IsEnemy(lowerTile.GetCard().GetCharacterType()))
                            avaibleTiles.Add(Swipe.DOWN);
                    }
                }
                catch (Exception e)
                {
                    print("oT: " + ownTile.GetCard());
                }
                return avaibleTiles;
            }

            public Swipe SelectTileToAttack(int listnumber)
            {
                var tiles = GetAvailableTiles(listnumber);
                if (tiles != null && tiles.Count != 0)
                {
                    var number = UnityEngine.Random.Range(0, tiles.Count);
                    if (number < 0)
                        number = 0;
                    return tiles[number];
                }
                return Swipe.NONE;
            }

            #endregion

            #region MOVE
            // Assign kısmını tamamen move'a taşı. Mesela MoveMaker'daki instant moves olablir.
            private bool DoMove(int listnumber, Swipe swipe)
            {
                CardProcessAssigner.Instance.AssignTiles(listnumber, ref targetTile, ref targetTile2, ref targetTile3, ref targetTile4, swipe);
                return CardProcessAssigner.Instance.AssignMoves(targetTile, targetTile2, targetTile3, targetTile4, out moveType);
            }

            private bool MoveForward()
            {
                var canStart = CardProcessAssigner.Instance.StartMoves();
                tourManager.IncreaseTourNumber();
                return canStart;
            }

            private bool StartMoves()
            {
                if (canStartMoves)
                {
                    var canMove = MoveForward();
                    if (canMove)
                    {
                        moveMaker.Move();
                        forwardCardProcess.ContinuingProcess(false);
                    }
                    else
                    {
                        forwardCardProcess.Finish();
                        moveProcess.StartProcess();
                        return false;
                    }
                }
                else
                {
                    JustAttackMove();
                    forwardCardProcess.Finish();
                    moveProcess.StartProcess();
                }
                return true;
            }

            private void JustAttackMove()
            {
                CardProcessAssigner.Instance.JustAttack();
                tourManager.FinishTour(false);
            }

            private void Move(int listNumber, bool isPlayer)
            {
                if (moveProcess.IsRunning())
                {
                    PrepareMoveProcess(listNumber, isPlayer);
                }
                else if (animationProcess.IsRunning())
                {
                    AnimationProcess(listNumber);
                }
                else if (forwardCardProcess.IsRunning())
                {
                    ExecuteMoves();
                }
            }

            private void ExecuteMoves()
            {
                bool canMove = false;
                if (forwardCardProcess.start)
                {
                    canMove = StartMoves();
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
                    forwardCardProcess.Finish();
                    if (playerMoveProcess.IsRunning())
                    {
                        playerMoveProcess.Finish();
                        //StartCoroutine(EndTurn(canMove));
                        SelectHighLevelCards();
                        dungeonBrainProcess.StartProcess();
                    }
                    else if (dungeonBrainProcess.IsRunning())
                    {
                        dungeonBrainProcess.EndProcess();
                    }

                    if (dungeonBrainZeroMove)
                        StartCoroutine(EndTurn(true));
                    else
                        StartCoroutine(EndTurn(canMove));
                }
            }

            private void AnimationProcess(int listNumber)
            {
                if (animationProcess.start)
                {
                    DoAnimation(listNumber);
                    animationProcess.EndProcess();
                }
                else if (animationProcess.end)
                {
                    if (moveType != MoveType.ATTACK)
                        forwardCardProcess.StartProcess();
                    else
                        StartCoroutine(FinishAnimationTurn());
                    animationProcess.Finish();
                }
            }

            private void PrepareMoveProcess(int listNumber, bool isPlayer)
            {
                if (moveProcess.start)
                {
                    dungeonBrainZeroMove = false;
                    if (isPlayer)
                        MakePlayerMove(listNumber);
                    else
                    {
                        var canMove = MakeEnemyMove(listNumber);
                        if (!canMove)
                        {
                            dungeonBrainZeroMove = true;
                            moveProcess.Finish();
                            forwardCardProcess.Init(true);
                            forwardCardProcess.EndProcess();
                        }
                    }
                }
                else if (moveProcess.end)
                {
                    moveProcess.Finish();
                    animationProcess.StartProcess();
                }
            }

            #endregion

            #region ANIMATION

            private IEnumerator FinishAnimationTurn()
            {
                yield return new WaitForSeconds(outOfAnimationStateTimer);
                forwardCardProcess.StartProcess();
            }

            private void DoAnimation(int listNumber)
            {
                Board.tiles[listNumber].GetCard().HandleCardEffect(moveType, targetTile, listNumber);
                animHandler.DoAnim(moveType, targetTile, listNumber);
            }

            #endregion

            private IEnumerator EndTurn(bool canMove)
            {
                if (!canMove)
                {
                    yield return new WaitForSeconds(timeForAddingCardET);
                    cardManager.AddCard(moveMaker.targetTileForAddingCard);
                    moveMaker.targetTileForAddingCard = null;
                    NullControlOnTiles();
                    yield return new WaitForSeconds(timeForFinishTourET);
                    tourManager.FinishTour(true);
                    moveProcess.StartProcess();
                }
                else
                {
                    yield return new WaitForSeconds(timeForFinishTourET);
                    moveProcess.StartProcess();
                }
            }

            #endregion

            #region CORE METHODS

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
                        foreach (var card in cardManager.cards)
                        {
                            if (card.GetTile() == Board.tiles[i])
                            {
                                Board.tiles[i].SetCard(card);
                                return;
                            }
                        }
                        cardManager.AddCard(Board.tiles[i]);
                        print("Bişeyler ekledik.");
                    }
                }
            }

            public void UnnecessaryCardControl()
            {
                Card[] cards = FindObjectsOfType<Card>();

                if (cards.Length <= 16) return; 

                foreach (var card in FindObjectsOfType<Card>())
                {
                    if (card != card.GetTile().GetCard())
                    {
                        print("Amına koyduk");
                        Destroy(card.gameObject);
                    }
                }
            }
            #endregion
        }
    }
}
