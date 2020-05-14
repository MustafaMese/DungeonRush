using DungeonRush.Cards;
using DungeonRush.Data;
using DungeonRush.Field;
using DungeonRush.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush.Controller
{
    public class AIController : MonoBehaviour
    {
        Tile targetTile = null;
        Tile targetTile2 = null;
        Tile targetTile3 = null;
        Tile targetTile4 = null;

        MoveType moveType;
        [SerializeField] float animationFinishTime = 0.1f;
        [SerializeField] float timeForAddingCardET = 0.1f;
        [SerializeField] float timeForFinishTourET = 0.1f;

        bool canMoveToTarget = false;
        bool isRunning = false;
        Card card;

        public ProcessHandleChecker preparingProcess;
        public ProcessHandleChecker animationProcess;
        public ProcessHandleChecker moveProcess;
        private NonPlayerController nonPlayerController;

        private void Start()
        {
            nonPlayerController = FindObjectOfType<NonPlayerController>();
            InitProcessHandlers();
            preparingProcess.StartProcess();
            card = GetComponent<Card>();
        }

        private void Update()
        {
            if (isRunning)
            {
                // TODO RUNNİNG TRUE KALIYO.
                print("1");
                if (preparingProcess.IsRunning())
                {
                    print("2");
                    var anyMove = PrepareMoveProcess(card.GetTile().GetListNumber());
                    preparingProcess.Finish();
                    preparingProcess.run = false;
                    if (anyMove)
                    {
                        print("2.1");
                        animationProcess.StartProcess();
                    }
                    else
                    {
                        print("2.2");
                        moveProcess.EndProcess();
                    }
                }
                else if (animationProcess.IsRunning())
                {
                    print("3");
                    AnimationProcess(card);
                }
                else if (moveProcess.IsRunning())
                {
                    print("4");
                    ExecuteMoves();
                }
            }
        }

        #region PREPARE MOVE METHODS
        public bool PrepareMoveProcess(int listNumber)
        {
            targetTile = null;
            targetTile2 = null;
            targetTile3 = null;
            targetTile4 = null;
            var swipe = SelectTileToAttack(listNumber);
            if (swipe != Swipe.NONE)
            {
                canMoveToTarget = DoMove(listNumber, swipe);
                return canMoveToTarget;
            }
            else
                return false;
        }

        private bool DoMove(int listnumber, Swipe swipe)
        {
            CardProcessAssigner.Instance.AssignTiles(listnumber, ref targetTile, ref targetTile2, ref targetTile3, ref targetTile4, swipe);
            return CardProcessAssigner.Instance.AssignMoves(targetTile, targetTile2, targetTile3, targetTile4, out moveType);
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
        #endregion

        #region ANIMATION METHODS
        public void AnimationProcess(Card c)
        {
            if (animationProcess.start)
            {
                DoAnimation(c);
                animationProcess.EndProcess();
            }
            else if (animationProcess.end)
            {
                if (moveType != MoveType.ATTACK)
                    moveProcess.StartProcess();
                else
                    StartCoroutine(FinishAnimationTurn());
                animationProcess.Finish();
            }
        }

        private void DoAnimation(Card c)
        {
            Move m = c.GetMove();
            c.HandleCardEffect(m.GetMoveType(), m.GetTargetTile(), m.GetCardTile().GetListNumber());
        }

        private IEnumerator FinishAnimationTurn()
        {
            yield return new WaitForSeconds(0.35f);
            moveProcess.StartProcess();
        }

        #endregion

        #region MOVE PROCESS

        private void ExecuteMoves()
        {
            if (moveProcess.start)
            {
                PassToAnotherTileMove();
                moveProcess.ContinuingProcess(false);
            }
            else if (moveProcess.continuing)
            {
                if (MoveMaker.movesFinished && !Board.touched)
                {
                    // Securing the process
                    MoveMaker.Instance.ResetMoves();
                    MoveMaker.movesFinished = false;
                    moveProcess.EndProcess();
                }
            }
            else if (moveProcess.end)
            {
                MoveMaker.Instance.ResetMoves();
                StartCoroutine(EndTurn(canMoveToTarget));
                //preparingProcess.StartProcess();
                moveProcess.Finish();
                isRunning = false;
                nonPlayerController.FinishTurn = true;
            }
        }

        private void PassToAnotherTileMove()
        {
            CardProcessAssigner.Instance.StartMoves();
            //MoveMaker.Instance.Move();
        }

        private IEnumerator EndTurn(bool c)
        {
            if (c)
            {
                yield return new WaitForSeconds(timeForAddingCardET);
                CardManager.Instance.AddCard(MoveMaker.Instance.targetTileForAddingCard);
                MoveMaker.Instance.targetTileForAddingCard = null;
                yield return new WaitForSeconds(timeForFinishTourET);

            }
            else
            {
                Board.touched = false;
                yield return new WaitForSeconds(timeForFinishTourET);
            }
        }

        #endregion

        public void InitProcessHandlers()
        {
            preparingProcess.Init(false);
            animationProcess.Init(false);
            moveProcess.Init(true);
        }

        public Card GetCard()
        {
            return card;
        }

        public void Run()
        {
            isRunning = true;
        }
    }
}
