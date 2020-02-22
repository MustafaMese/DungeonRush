using DungeonRush.Moves;
using DungeonRush.Element;
using UnityEngine;
using DungeonRush.Cards;

namespace DungeonRush
{
    namespace Managers
    {
        public class MoveMaker : MonoBehaviour
        {
            public Board board;
            public GameManager gm;

            public static float speed = 10f;

            // Çoklu ilerleme için.
            public Move instantMove;
            public Move instantMove2;
            public Move instantMove3;
            public int moveNumber;

            [SerializeField, Range(0, 2)] float invokeAddingCardTime;
            [SerializeField, Range(0, 2)] float stoppingTime;

            public static bool movesFinished = false;

            // Card ekleme için.
            private Tile targetTileForAddingCard;

            private void Start()
            {
                invokeAddingCardTime = 0.1f;
                moveNumber = 0;
                stoppingTime = 0.2f;
            }

            private void Update()
            {
                //if (gm.tourManager.tourNumber - 1 == gm.tourManager.oldTourNumber)
                //    Move();
            }

            private void ResetMoves()
            {
                if (moveNumber == 1 && instantMove.GetMoveType() != MoveType.None)
                    ResetInstantMove();
                else if (moveNumber == 2 && instantMove.GetMoveType() != MoveType.None && instantMove2.GetMoveType() != MoveType.None)
                    ResetInstantMove();
                else if (moveNumber == 3 && instantMove.GetMoveType() != MoveType.None && instantMove2.GetMoveType() != MoveType.None && instantMove3.GetMoveType() != MoveType.None)
                    ResetInstantMove();
            }

            // TODO Buraya da bi ara el at...
            public void Move()
            {
                if (Board.touched && !movesFinished)
                {
                    if (moveNumber == 1)
                    {
                        if (instantMove.GetCard().GetMove().Equals(default(Move)))
                            instantMove.GetCard().SetMove(instantMove);

                        instantMove.GetCard().ExecuteMove();
                        if (targetTileForAddingCard == null)
                        {
                            targetTileForAddingCard = instantMove.GetCardTile();
                        }
                    }
                    else if (moveNumber == 2)
                    {
                        if (instantMove.GetCard() != null && instantMove.GetCard().GetMove().Equals(default(Move)))
                            instantMove.GetCard().SetMove(instantMove);
                        if (instantMove2.GetCard() != null && instantMove2.GetCard().GetMove().Equals(default(Move)))
                            instantMove2.GetCard().SetMove(instantMove2);

                        instantMove.GetCard().ExecuteMove();
                        instantMove2.GetCard().ExecuteMove();

                        if (targetTileForAddingCard == null)
                        {
                            targetTileForAddingCard = instantMove2.GetCardTile();
                        }
                    }
                    else if (moveNumber == 3)
                    {
                        if (instantMove.GetCard() != null && instantMove.GetCard().GetMove().Equals(default(Move)))
                            instantMove.GetCard().SetMove(instantMove);
                        if (instantMove2.GetCard() != null && instantMove2.GetCard().GetMove().Equals(default(Move)))
                            instantMove2.GetCard().SetMove(instantMove2);
                        if (instantMove3.GetCard() != null && instantMove3.GetCard().GetMove().Equals(default(Move)))
                            instantMove3.GetCard().SetMove(instantMove3);

                        instantMove.GetCard().ExecuteMove();
                        instantMove2.GetCard().ExecuteMove();
                        instantMove3.GetCard().ExecuteMove();
                        if (targetTileForAddingCard == null)
                        {
                            targetTileForAddingCard = instantMove3.GetCardTile();

                        }
                    }
                }
                // Stopping time methodunu ayarlamayı unutma.
                else if (movesFinished && !Board.touched)
                {
                    Invoke("AddCardByGameManager", invokeAddingCardTime);
                    Invoke("FinishTour", stoppingTime);
                    movesFinished = false;
                }
            }

            public void SetInstantMove(Move move)
            {
                this.instantMove = move;
            }

            public void SetInstantMove(Move move, Move move2)
            {
                this.instantMove = move;
                this.instantMove2 = move2;
            }

            public void SetInstantMove(Move move, Move move2, Move move3)
            {
                this.instantMove = move;
                this.instantMove2 = move2;
                this.instantMove3 = move3;
            }

            public Move GetInstantMove()
            {
                return this.instantMove;
            }

            public void ResetInstantMove()
            {
                if(moveNumber == 1)
                    instantMove.Reset();
                else if (moveNumber == 2)
                {
                    instantMove.Reset();
                    instantMove2.Reset();
                }
                else if (moveNumber == 3)
                {
                    instantMove.Reset();
                    instantMove2.Reset();
                    instantMove3.Reset();
                }

                moveNumber = 0;
            }

            public void AddCardByGameManager()
            {
                int number = Random.Range(0, 101);
                if (number < 70)
                    gm.AddCard(GiveRandomCard(gm.enemyCards), targetTileForAddingCard, false, board, true);
                else if (number < 95)
                    gm.AddCard(GiveRandomCard(gm.itemCards), targetTileForAddingCard, false, board, true);
                else
                    gm.AddCard(GiveRandomCard(gm.coinCards), targetTileForAddingCard, false, board, true);

                targetTileForAddingCard = null;
                gm.NullControlOnTiles();
            }

            private Card GiveRandomCard(Card[] card)
            {
                int length = card.Length;
                return card[Random.Range(0, length)];
            }

            public void FinishTour()
            {
                gm.tourManager.FinishTour(true);
                ResetMoves();
            }

            private void OnDestroy()
            {
                movesFinished = false;
            }
        }
    }
}