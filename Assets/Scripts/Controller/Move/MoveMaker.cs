using DungeonRush.Data;
using DungeonRush.Field;
using UnityEngine;

namespace DungeonRush
{
    namespace Managers
    {
        public class MoveMaker : MonoBehaviour
        {
            private static MoveMaker instance = null;
            // Game Instance Singleton
            public static MoveMaker Instance
            {
                get { return instance; } set { instance = value; }
            }

            // Çoklu ilerleme için.
            public Move instantMove;
            public Move instantMove2;
            public Move instantMove3;

            public Move InstantMove { get { return instantMove; } }
            public Move InstantMove2 { get { return instantMove2; } }
            public Move InstantMove3 { get { return instantMove3; } }

            public int moveNumber;

            public static bool movesFinished = false;

            // Card ekleme için.
            public Tile targetTileForAddingCard;

            private void Awake()
            {
                Instance = this;
            }

            private void Start()
            {
                moveNumber = 0;
            }

            public void ResetMoves()
            {
                if (moveNumber == 1 && instantMove.GetMoveType() != MoveType.NONE)
                    ResetInstantMove();
                else if (moveNumber == 2 && instantMove.GetMoveType() != MoveType.NONE && instantMove2.GetMoveType() != MoveType.NONE)
                    ResetInstantMove();
                else if (moveNumber == 3 && instantMove.GetMoveType() != MoveType.NONE && instantMove2.GetMoveType() != MoveType.NONE && instantMove3.GetMoveType() != MoveType.NONE)
                    ResetInstantMove();
            }

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

            private void OnDestroy()
            {
                movesFinished = false;
            }
        }
    }
}