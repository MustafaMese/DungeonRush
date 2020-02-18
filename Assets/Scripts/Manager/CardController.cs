using DungeonRush.Cards;
using DungeonRush.Element;
using DungeonRush.Moves;
using DungeonRush.Property;
using UnityEngine;

namespace DungeonRush
{
    namespace Managers
    {
        public class CardController : MonoBehaviour
        {
            private CardManager cardManager;
            private TourManager tourManager;
            private MoveMaker moveMaker;

            private bool attackingMove;

            private void Start()
            {
                cardManager = FindObjectOfType<CardManager>();
                tourManager = FindObjectOfType<TourManager>();
                moveMaker = FindObjectOfType<MoveMaker>();
            }

            public void AssignTiles(int listnumber, ref Tile targetTile, ref Tile targetTile2, ref Tile targetTile3, ref Tile targetTile4)
            {
                switch (SwipeManager.swipeDirection)
                {
                    case Swipe.None:
                        break;
                    case Swipe.Up:
                        if (listnumber > 3)
                        {
                            targetTile = Board.tiles[listnumber - 4];
                            targetTile2 = Board.tiles[listnumber];
                            moveMaker.moveNumber = 1;
                            if (targetTile.GetCoordinate().y == 0)
                            {
                                targetTile3 = Board.tiles[listnumber + 4];
                                moveMaker.moveNumber = 2;
                            }
                        }
                        break;
                    case Swipe.Down:
                        if (listnumber < 8)
                        {
                            targetTile = Board.tiles[listnumber + 4];
                            targetTile2 = Board.tiles[listnumber];
                            moveMaker.moveNumber = 1;
                            if (targetTile.GetCoordinate().y == 2)
                            {
                                targetTile3 = Board.tiles[listnumber - 4];
                                moveMaker.moveNumber = 2;
                            }
                        }
                        break;
                    case Swipe.Left:
                        if (listnumber % 4 != 0)
                        {
                            targetTile = Board.tiles[listnumber - 1];
                            targetTile2 = Board.tiles[listnumber];
                            moveMaker.moveNumber = 1;
                            if (targetTile.GetCoordinate().x == 1)
                            {
                                targetTile3 = Board.tiles[listnumber + 1];
                                moveMaker.moveNumber = 2;
                            }
                            else if (targetTile.GetCoordinate().x == 0)
                            {
                                targetTile3 = Board.tiles[listnumber + 1];
                                targetTile4 = Board.tiles[listnumber + 2];
                                moveMaker.moveNumber = 3;
                            }
                        }
                        break;
                    case Swipe.Right:
                        if (listnumber % 4 != 3)
                        {
                            targetTile = Board.tiles[listnumber + 1];
                            targetTile2 = Board.tiles[listnumber];
                            moveMaker.moveNumber = 1;
                            if (targetTile.GetCoordinate().x == 2)
                            {
                                targetTile3 = Board.tiles[listnumber - 1];
                                moveMaker.moveNumber = 2;
                            }
                            else if (targetTile.GetCoordinate().x == 3)
                            {
                                targetTile3 = Board.tiles[listnumber - 1];
                                targetTile4 = Board.tiles[listnumber - 2];
                                moveMaker.moveNumber = 3;
                            }
                        }
                        break;
                    default:
                        break;
                }
            }

            public bool AssignMoves(Tile targetTile, Tile targetTile2, Tile targetTile3, Tile targetTile4)
            {
                Card moverCard = targetTile2.GetCard();
                if(targetTile != null)
                {
                    // Movemaker update'i tetiklendi
                    Board.touched = true;
                    // Hamle yapılan Tile'da card var mı?
                    CardType targetCardType = targetTile.GetCard().GetCardType();
                    if (targetTile.IsTileOccupied())
                    {
                        // TODO BURALARIN İÇİ DEĞİL DE İFLER DEĞİŞECEK
                        if(targetCardType == CardType.ENEMY)
                        {
                            bool canAttack = moverCard.GetComponent<Attacker>().CanAttack((EnemyCard)targetTile.GetCard());
                            if (canAttack)
                            {
                                ConfigureMoves(targetTile, targetTile2, targetTile3, targetTile4, MoveType.Attack);
                            }
                            else
                            {
                                moveMaker.moveNumber = 0;
                                ConfigureJustAttackMove(targetTile, targetTile2);
                                return false;
                            }
                        }
                        else if(targetCardType == CardType.ITEM)
                        {
                            ConfigureMoves(targetTile, targetTile2, targetTile3, targetTile4, MoveType.Item);
                        }
                        else if(targetCardType == CardType.COIN)
                        {
                            ConfigureMoves(targetTile, targetTile2, targetTile3, targetTile4, MoveType.Coin);
                        }
                    }
                    else
                    {
                        ConfigureMoves(targetTile, targetTile2, targetTile3, targetTile4, MoveType.Empty);
                    }
                }
                attackingMove = false;
                return true;
            }

            public void StartMoves()
            {
                if (!attackingMove) 
                {
                    moveMaker.instantMove.GetCard().isMoving = true;
                    if (moveMaker.moveNumber == 2)
                    {
                        moveMaker.instantMove2.GetCard().isMoving = true;
                    }
                    else if (moveMaker.moveNumber == 3)
                    {
                        moveMaker.instantMove2.GetCard().isMoving = true;
                        moveMaker.instantMove3.GetCard().isMoving = true;
                    } 
                }
                moveMaker.instantMove.GetTargetTile().GetCard().Disappear();
            }

            public void JustAttack()
            {
                Move move = moveMaker.instantMove;
                move.GetCard().GetComponent<Attacker>().Attack((EnemyCard)move.GetTargetTile().GetCard());
            }

            private void ConfigureJustAttackMove(Tile targetTile, Tile targetTile2)
            {
                Move move = new Move(targetTile2, targetTile, targetTile2.GetCard(), MoveType.Attack, true);
                targetTile2.GetCard().SetMove(move);
                moveMaker.instantMove = move;
                attackingMove = true;
            }

            private void ConfigureMoves(Tile targetTile, Tile targetTile2, Tile targetTile3, Tile targetTile4, MoveType type)
            {
                if (moveMaker.moveNumber == 1)
                {
                    Move move = new Move(targetTile2, targetTile, targetTile2.GetCard(), type, true);
                    targetTile2.GetCard().SetMove(move);
                    moveMaker.SetInstantMove(move);
                }
                else if (moveMaker.moveNumber == 2)
                {
                    Move move = new Move(targetTile2, targetTile, targetTile2.GetCard(), type, false);
                    targetTile2.GetCard().SetMove(move);
                    Move move2 = new Move(targetTile3, targetTile2, targetTile3.GetCard(), MoveType.Empty, true);
                    targetTile3.GetCard().SetMove(move2);
                    moveMaker.SetInstantMove(move, move2);
                }
                else if (moveMaker.moveNumber == 3)
                {
                    Move move = new Move(targetTile2, targetTile, targetTile2.GetCard(), type, false);
                    targetTile2.GetCard().SetMove(move);
                    Move move2 = new Move(targetTile3, targetTile2, targetTile3.GetCard(), MoveType.Empty, false);
                    targetTile3.GetCard().SetMove(move2);
                    move2.GetCard().isMoving = true;
                    Move move3 = new Move(targetTile4, targetTile3, targetTile4.GetCard(), MoveType.Empty, true);
                    targetTile4.GetCard().SetMove(move3);
                    moveMaker.SetInstantMove(move, move2, move3);
                }
            }
        }
    }
}