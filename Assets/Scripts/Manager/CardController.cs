using DungeonRush.Cards;
using DungeonRush.DataPackages;
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

            // TODO Yeni kart sırasını ekle.
            /// <summary>
            /// targetTile is actual target, targetTile2 is our tile.
            /// </summary>
            public void AssignTiles(int listnumber, ref Tile targetTile, ref Tile targetTile2, ref Tile targetTile3, ref Tile targetTile4, Swipe swipe)
            {
                if (swipe != Swipe.None)
                    SwipeManager.swipeDirection = swipe;    

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
                            if (targetTile.GetCoordinate().y == 1)
                            {
                                targetTile3 = Board.tiles[listnumber + 4];
                                //if (targetTile3.GetCard().GetCharacterType().cT != CharacterType.WALL)
                                    moveMaker.moveNumber = 2;
                            }
                            else if (targetTile.GetCoordinate().y == 0)
                            {
                                targetTile3 = Board.tiles[listnumber + 4];
                                targetTile4 = Board.tiles[listnumber + 8];
                                //if (targetTile3.GetCard().GetCharacterType().cT != CharacterType.WALL)
                                    moveMaker.moveNumber = 3;
                            }
                        }
                        break;
                    case Swipe.Down:
                        if (listnumber < 12)
                        {
                            targetTile = Board.tiles[listnumber + 4];
                            targetTile2 = Board.tiles[listnumber];
                            moveMaker.moveNumber = 1;
                            if (targetTile.GetCoordinate().y == 2)
                            {
                                targetTile3 = Board.tiles[listnumber - 4];
                                //if (targetTile3.GetCard().GetCharacterType().cT != CharacterType.WALL)
                                    moveMaker.moveNumber = 2;
                            }
                            else if (targetTile.GetCoordinate().y == 3)
                            {
                                targetTile3 = Board.tiles[listnumber - 4];
                                targetTile4 = Board.tiles[listnumber - 8];
                                //if (targetTile3.GetCard().GetCharacterType().cT != CharacterType.WALL)
                                    moveMaker.moveNumber = 3;
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
                                //if (targetTile3.GetCard().GetCharacterType().cT != CharacterType.WALL)
                                    moveMaker.moveNumber = 2;
                            }
                            else if (targetTile.GetCoordinate().x == 0)
                            {
                                targetTile3 = Board.tiles[listnumber + 1];
                                targetTile4 = Board.tiles[listnumber + 2];
                                //if (targetTile4.GetCard().GetCharacterType().cT != CharacterType.WALL)
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
                                //if (targetTile3.GetCard().GetCharacterType().cT != CharacterType.WALL)
                                    moveMaker.moveNumber = 2;
                            }
                            else if (targetTile.GetCoordinate().x == 3)
                            {
                                targetTile3 = Board.tiles[listnumber - 1];
                                targetTile4 = Board.tiles[listnumber - 2];
                                //if (targetTile4.GetCard().GetCharacterType().cT != CharacterType.WALL)
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
                    MoveType type = SelectMoveAction(targetTile, targetTile2, targetTile3, targetTile4);
                    if(type == MoveType.Attack)
                    {
                        bool canAttack = moverCard.GetComponent<Attacker>().CanAttack((EnemyCard)targetTile.GetCard());
                        if (canAttack)
                            ConfigureMoves(targetTile, targetTile2, targetTile3, targetTile4, MoveType.Attack);
                        else
                        {
                            moveMaker.moveNumber = 0;
                            ConfigureJustAttackMove(targetTile, targetTile2);
                            return false;
                        }
                    }
                    else
                        ConfigureMoves(targetTile, targetTile2, targetTile3, targetTile4, type);
                }
                attackingMove = false;
                return true;  
            }


            public MoveType SelectMoveAction(Tile targetTile, Tile targetTile2, Tile targetTile3, Tile targetTile4)
            {
                Card attackerCard = targetTile2.GetCard();
                Card targetCard = targetTile.GetCard();

                if (attackerCard.GetCardType() == CardType.PLAYER)
                {
                    if (targetCard.GetCardType() == CardType.ENEMY)
                        return MoveType.Attack;
                    else if (targetCard.GetCardType() == CardType.ITEM)
                        return MoveType.Item;
                    else if (targetCard.GetCardType() == CardType.COIN)
                        return MoveType.Coin;
                    else
                        return MoveType.Empty;
                }
                else if (attackerCard.GetCardType() == CardType.ENEMY)
                {
                    if (attackerCard.GetComponent<Attacker>())
                        return MoveType.Attack;
                    else if (attackerCard.GetComponent<ItemUser>())
                        return MoveType.Item;
                    return MoveType.None;
                }
                else
                    return MoveType.None;
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
                    Move move3 = new Move(targetTile4, targetTile3, targetTile4.GetCard(), MoveType.Empty, true);
                    targetTile4.GetCard().SetMove(move3);
                    moveMaker.SetInstantMove(move, move2, move3);
                }
            }
        }
    }
}