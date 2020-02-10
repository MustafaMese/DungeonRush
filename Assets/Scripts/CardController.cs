using DungeonRush.Cards;
using DungeonRush.Element;
using DungeonRush.Moves;
using DungeonRush.Property;
using System.Collections;
using System.Collections.Generic;
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
                            moveMaker.moveNumber = 1;
                            if (targetTile.GetCoordinate().y == 0)
                            {
                                targetTile2 = Board.tiles[listnumber];
                                targetTile3 = Board.tiles[listnumber + 4];
                                moveMaker.moveNumber = 2;
                            }
                        }
                        break;
                    case Swipe.Down:
                        if (listnumber < 8)
                        {
                            targetTile = Board.tiles[listnumber + 4];
                            moveMaker.moveNumber = 1;
                            if (targetTile.GetCoordinate().y == 2)
                            {
                                targetTile2 = Board.tiles[listnumber];
                                targetTile3 = Board.tiles[listnumber - 4];
                                moveMaker.moveNumber = 2;
                            }
                        }
                        break;
                    case Swipe.Left:
                        if (listnumber % 4 != 0)
                        {
                            targetTile = Board.tiles[listnumber - 1];
                            moveMaker.moveNumber = 1;
                            if (targetTile.GetCoordinate().x == 1)
                            {
                                targetTile2 = Board.tiles[listnumber];
                                targetTile3 = Board.tiles[listnumber + 1];
                                moveMaker.moveNumber = 2;
                            }
                            else if (targetTile.GetCoordinate().x == 0)
                            {
                                targetTile2 = Board.tiles[listnumber];
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
                            moveMaker.moveNumber = 1;
                            if (targetTile.GetCoordinate().x == 2)
                            {
                                targetTile2 = Board.tiles[listnumber];
                                targetTile3 = Board.tiles[listnumber - 1];
                                moveMaker.moveNumber = 2;
                            }
                            else if (targetTile.GetCoordinate().x == 3)
                            {
                                targetTile2 = Board.tiles[listnumber];
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

            // TODO Player'a göre ayar vermeyi değiştir.
            public void AssignMoves(Tile targetTile, Tile targetTile2, Tile targetTile3, Tile targetTile4)
            {
                if (targetTile != null)
                {
                    // Player Card yürümeye hazır.
                    cardManager.GetPlayerCard().isMoving = true;
                    // Move maker updati'i tetiklendi.
                    Board.touched = true;
                    // PlayerCard'ının hedefinde card var mı?
                    if (targetTile.IsTileOccupied())
                    {
                        if (targetTile.GetCard().GetCardType() == CardType.ENEMY)
                        {
                            bool canAttack = cardManager.GetPlayerCard().GetComponent<Attacker>().CanAttack((EnemyCard)targetTile.GetCard());
                            if (canAttack)
                                ConfigureMoves(targetTile, targetTile2, targetTile3, targetTile4, MoveType.Attack);
                            else
                            {
                                tourManager.FinishTour(false);
                                return;
                            }
                        }
                        else if (targetTile.GetCard().GetCardType() == CardType.ITEM)
                            ConfigureMoves(targetTile, targetTile2, targetTile3, targetTile4, MoveType.Item);
                        else if (targetTile.GetCard().GetCardType() == CardType.COIN)
                            ConfigureMoves(targetTile, targetTile2, targetTile3, targetTile4, MoveType.Coin);
                    }
                    else
                        ConfigureMoves(targetTile, targetTile2, targetTile3, targetTile4, MoveType.Empty);
                    targetTile.GetCard().Disappear();
                }
            }

            private void ConfigureMoves(Tile targetTile, Tile targetTile2, Tile targetTile3, Tile targetTile4, MoveType type)
            {

                if (moveMaker.moveNumber == 1)
                {
                    Move move = new Move(cardManager.instantPlayerTile, targetTile, cardManager.GetPlayerCard(), type, true);
                    cardManager.GetPlayerCard().SetMove(move);
                    moveMaker.SetInstantMove(move);
                    tourManager.IncreaseTourNumber();
                }
                else if (moveMaker.moveNumber == 2)
                {
                    Move move = new Move(cardManager.instantPlayerTile, targetTile, cardManager.GetPlayerCard(), type, false);
                    cardManager.GetPlayerCard().SetMove(move);
                    Move move2 = new Move(targetTile3, targetTile2, targetTile3.GetCard(), MoveType.Empty, true);
                    targetTile3.GetCard().SetMove(move2);
                    move2.GetCard().isMoving = true;
                    moveMaker.SetInstantMove(move, move2);
                    tourManager.IncreaseTourNumber();
                }
                else if (moveMaker.moveNumber == 3)
                {
                    Move move = new Move(cardManager.instantPlayerTile, targetTile, cardManager.GetPlayerCard(), type, false);
                    cardManager.GetPlayerCard().SetMove(move);
                    Move move2 = new Move(targetTile3, targetTile2, targetTile3.GetCard(), MoveType.Empty, false);
                    targetTile3.GetCard().SetMove(move2);
                    move2.GetCard().isMoving = true;
                    Move move3 = new Move(targetTile4, targetTile3, targetTile4.GetCard(), MoveType.Empty, true);
                    targetTile4.GetCard().SetMove(move3);
                    move3.GetCard().isMoving = true;
                    moveMaker.SetInstantMove(move, move2, move3);
                    tourManager.IncreaseTourNumber();
                }
            }
        }
    }
}