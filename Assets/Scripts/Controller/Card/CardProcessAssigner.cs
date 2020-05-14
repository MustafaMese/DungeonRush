using DungeonRush.Cards;
using DungeonRush.Field;
using DungeonRush.Data;
using DungeonRush.Property;
using UnityEngine;

namespace DungeonRush
{
    namespace Managers
    {
        public class CardProcessAssigner : MonoBehaviour
        {
            private static CardProcessAssigner instance = null;
            // Game Instance Singleton
            public static CardProcessAssigner Instance  
            {
                get { return instance; } set { instance = value; }
            }

            private bool attackingMove;

            private void Awake()
            {
                Instance = this;
            }

            // TODO Yeni kart sırasını ekle.
            /// <summary>
            /// targetTile is actual target, targetTile2 is our tile.
            /// </summary>
            public void AssignTiles(int listnumber, ref Tile targetTile, ref Tile targetTile2, ref Tile targetTile3, ref Tile targetTile4, Swipe swipe)
            {
                if (swipe != Swipe.PLAYER)
                    SwipeManager.swipeDirection = swipe;    

                switch (SwipeManager.swipeDirection)
                {
                    case Swipe.NONE:
                        targetTile = null;
                        targetTile2 = null;
                        targetTile3 = null;
                        targetTile4 = null;
                        break;
                    case Swipe.UP:
                        if (listnumber > 3)
                        {
                            targetTile = Board.tiles[listnumber - 4];
                            targetTile2 = Board.tiles[listnumber];
                            MoveMaker.Instance.moveNumber = 1;
                            if (targetTile.GetCoordinate().y == 1)
                            {
                                targetTile3 = Board.tiles[listnumber + 4];
                                //if (targetTile3.GetCard().GetCharacterType().cT != CharacterType.WALL)
                                MoveMaker.Instance.moveNumber = 2;
                            }
                            else if (targetTile.GetCoordinate().y == 0)
                            {
                                targetTile3 = Board.tiles[listnumber + 4];
                                targetTile4 = Board.tiles[listnumber + 8];
                                //if (targetTile3.GetCard().GetCharacterType().cT != CharacterType.WALL)
                                MoveMaker.Instance.moveNumber = 3;
                            }
                        }
                        break;
                    case Swipe.DOWN:
                        if (listnumber < 12)
                        {
                            targetTile = Board.tiles[listnumber + 4];
                            targetTile2 = Board.tiles[listnumber];
                            MoveMaker.Instance.moveNumber = 1;
                            if (targetTile.GetCoordinate().y == 2)
                            {
                                targetTile3 = Board.tiles[listnumber - 4];
                                //if (targetTile3.GetCard().GetCharacterType().cT != CharacterType.WALL)
                                MoveMaker.Instance.moveNumber = 2;
                            }
                            else if (targetTile.GetCoordinate().y == 3)
                            {
                                targetTile3 = Board.tiles[listnumber - 4];
                                targetTile4 = Board.tiles[listnumber - 8];
                                //if (targetTile3.GetCard().GetCharacterType().cT != CharacterType.WALL)
                                MoveMaker.Instance.moveNumber = 3;
                            }
                        }
                        break;
                    case Swipe.LEFT:
                        if (listnumber % 4 != 0)
                        {
                            targetTile = Board.tiles[listnumber - 1];
                            targetTile2 = Board.tiles[listnumber];
                            MoveMaker.Instance.moveNumber = 1;
                            if (targetTile.GetCoordinate().x == 1)
                            {
                                targetTile3 = Board.tiles[listnumber + 1];
                                //if (targetTile3.GetCard().GetCharacterType().cT != CharacterType.WALL)
                                MoveMaker.Instance.moveNumber = 2;
                            }
                            else if (targetTile.GetCoordinate().x == 0)
                            {
                                targetTile3 = Board.tiles[listnumber + 1];
                                targetTile4 = Board.tiles[listnumber + 2];
                                //if (targetTile4.GetCard().GetCharacterType().cT != CharacterType.WALL)
                                MoveMaker.Instance.moveNumber = 3;
                            }
                        }
                        break;
                    case Swipe.RIGHT:
                        if (listnumber % 4 != 3)
                        {
                            targetTile = Board.tiles[listnumber + 1];
                            targetTile2 = Board.tiles[listnumber];
                            MoveMaker.Instance.moveNumber = 1;
                            if (targetTile.GetCoordinate().x == 2)
                            {
                                targetTile3 = Board.tiles[listnumber - 1];
                                //if (targetTile3.GetCard().GetCharacterType().cT != CharacterType.WALL)
                                MoveMaker.Instance.moveNumber = 2;
                            }
                            else if (targetTile.GetCoordinate().x == 3)
                            {
                                targetTile3 = Board.tiles[listnumber - 1];
                                targetTile4 = Board.tiles[listnumber - 2];
                                //if (targetTile4.GetCard().GetCharacterType().cT != CharacterType.WALL)
                                MoveMaker.Instance.moveNumber = 3;
                            }
                        }
                        break;
                    default:
                        break;
                }
            }

            public bool AssignMoves(Tile targetTile, Tile targetTile2, Tile targetTile3, Tile targetTile4, out MoveType type)
            {
                type = SelectMoveAction(targetTile, targetTile2, targetTile3, targetTile4);
                if (targetTile != null)
                {
                    Card moverCard = targetTile2.GetCard();
                    // Movemaker update'i tetiklendi
                    Board.touched = true;
                    if (type == MoveType.ATTACK)
                    {
                        bool canAttack = moverCard.CanAttack(targetTile.GetCard());
                        if (canAttack)
                            ConfigureMoves(targetTile, targetTile2, targetTile3, targetTile4, MoveType.ATTACK);
                        else
                        {
                            MoveMaker.Instance.moveNumber = 0;
                            ConfigureJustAttackMove(targetTile, targetTile2);
                            return false;
                        }
                    }
                    else
                        ConfigureMoves(targetTile, targetTile2, targetTile3, targetTile4, type);
                }
                else
                    MoveMaker.Instance.moveNumber = -1;
                attackingMove = false;
                return true;  
            }

            public MoveType SelectMoveAction(Tile targetTile, Tile targetTile2, Tile targetTile3, Tile targetTile4)
            {
                if (targetTile == null || targetTile2 == null)
                    return MoveType.NONE;

                Card attackerCard = targetTile2.GetCard();
                Card targetCard = targetTile.GetCard();

                if (attackerCard.GetCardType() == CardType.PLAYER)
                {
                    if (targetCard.GetCardType() == CardType.ENEMY)
                        return MoveType.ATTACK;
                    else if (targetCard.GetCardType() == CardType.ITEM)
                        return MoveType.ITEM;
                    else if (targetCard.GetCardType() == CardType.COIN)
                        return MoveType.COIN;
                    else
                        return MoveType.EMPTY;
                }
                else if (attackerCard.GetCardType() == CardType.ENEMY)
                {
                    if (attackerCard.IsAttacker())
                        return MoveType.ATTACK;
                    else if (attackerCard.IsItemUser())
                        return MoveType.ITEM;
                    return MoveType.NONE;
                }
                else
                    return MoveType.NONE;
            }

            // Kötü tasarım ama iyi çalışıyo?
            public bool StartMoves()
            {
                if (MoveMaker.Instance.moveNumber == 0)
                    return false;

                if (!attackingMove) 
                {
                    MoveMaker.Instance.InstantMove.GetCard().isMoving = true;
                    if (MoveMaker.Instance.moveNumber == 2)
                    {
                        MoveMaker.Instance.InstantMove2.GetCard().isMoving = true;
                    }
                    else if (MoveMaker.Instance.moveNumber == 3)
                    {
                        MoveMaker.Instance.InstantMove2.GetCard().isMoving = true;
                        MoveMaker.Instance.InstantMove3.GetCard().isMoving = true;
                    } 
                }
                MoveMaker.Instance.InstantMove.GetTargetTile().GetCard().Disappear();
                return true;
            }

            public void JustAttack()
            {
                Move move = MoveMaker.Instance.InstantMove;
                move.GetCard().Attack(move.GetTargetTile().GetCard());
            }

            private void ConfigureJustAttackMove(Tile targetTile, Tile targetTile2)
            {
                Move move = new Move(targetTile2, targetTile, targetTile2.GetCard(), MoveType.ATTACK, true);
                targetTile2.GetCard().SetMove(move);
                MoveMaker.Instance.SetInstantMove(move);
                attackingMove = true;
            }

            private void ConfigureMoves(Tile targetTile, Tile targetTile2, Tile targetTile3, Tile targetTile4, MoveType type)
            {
                var moveNumber = MoveMaker.Instance.moveNumber;

                if (moveNumber == 1)
                {
                    Move move = new Move(targetTile2, targetTile, targetTile2.GetCard(), type, true);
                    targetTile2.GetCard().SetMove(move);
                    MoveMaker.Instance.SetInstantMove(move);
                }
                else if (moveNumber == 2)
                {
                    Move move = new Move(targetTile2, targetTile, targetTile2.GetCard(), type, false);
                    targetTile2.GetCard().SetMove(move);
                    Move move2 = new Move(targetTile3, targetTile2, targetTile3.GetCard(), MoveType.EMPTY, true);
                    targetTile3.GetCard().SetMove(move2);
                    MoveMaker.Instance.SetInstantMove(move, move2);
                }
                else if (moveNumber == 3)
                {
                    Move move = new Move(targetTile2, targetTile, targetTile2.GetCard(), type, false);
                    targetTile2.GetCard().SetMove(move);
                    Move move2 = new Move(targetTile3, targetTile2, targetTile3.GetCard(), MoveType.EMPTY, false);
                    targetTile3.GetCard().SetMove(move2);
                    Move move3 = new Move(targetTile4, targetTile3, targetTile4.GetCard(), MoveType.EMPTY, true);
                    targetTile4.GetCard().SetMove(move3);
                    MoveMaker.Instance.SetInstantMove(move, move2, move3);
                }
            }

        }
    }
}