using System.Collections.Generic;
using DungeonRush.Cards;
using DungeonRush.Data;
using DungeonRush.Field;
using DungeonRush.Managers;
using DungeonRush.Property;
using UnityEngine;

namespace DungeonRush.Shifting
{
    [CreateAssetMenu(menuName = "Shift/TwoBoxShift", order = 2)]
    public class TwoBoxShift : Shift
    {
        public override bool Define(Card card, Swipe swipe)
        {
            int rL = Board.RowLength;
            Vector2 coordinate = card.GetTile().transform.position;
            Debug.Log("c " + coordinate);
            switch (swipe)
            {
                case Swipe.NONE:
                    break;
                case Swipe.UP:
                    if (coordinate.y < rL - 2)
                    {
                        Debug.Log("1");
                        Vector2 targetPos = new Vector2(coordinate.x, coordinate.y + 2);
                        Tile targetTile = Board.tilesByCoordinates[targetPos];
                        if (targetTile.GetCard() == null)
                        {
                            ConfigureCardMove(card, targetTile);
                            return true;
                        }
                    }

                    if (coordinate.y < rL - 1)
                    {
                        Vector2 targetPos = new Vector2(coordinate.x, coordinate.y + 1);
                        Tile targetTile = Board.tilesByCoordinates[targetPos];
                        if (targetTile.GetCard() == null)
                        {
                            ConfigureCardMove(card, targetTile);
                            return true;
                        }
                    }
                    Debug.Log("gobba");
                    break;
                case Swipe.DOWN:
                    if (coordinate.y > 1)
                    {
                        Vector2 targetPos = new Vector2(coordinate.x, coordinate.y - 2);
                        Tile targetTile = Board.tilesByCoordinates[targetPos];
                        if (targetTile.GetCard() == null)
                        {
                            ConfigureCardMove(card, targetTile);
                            return true;
                        }
                    }

                    if (coordinate.y > 0)
                    {
                        Vector2 targetPos = new Vector2(coordinate.x, coordinate.y - 1);
                        Tile targetTile = Board.tilesByCoordinates[targetPos];
                        if (targetTile.GetCard() == null)
                        {
                            ConfigureCardMove(card, targetTile);
                            return true;
                        }
                    }
                    break;
                case Swipe.LEFT:
                    if (coordinate.x > 1)
                    {
                        Vector2 targetPos = new Vector2(coordinate.x - 2, coordinate.y);
                        Tile targetTile = Board.tilesByCoordinates[targetPos];
                        if (targetTile.GetCard() == null)
                        {
                            ConfigureCardMove(card, targetTile);
                            return true;
                        }
                    }

                    if (coordinate.x > 0)
                    {
                        Vector2 targetPos = new Vector2(coordinate.x - 1, coordinate.y);
                        Tile targetTile = Board.tilesByCoordinates[targetPos];
                        if (targetTile.GetCard() == null)
                        {
                            ConfigureCardMove(card, targetTile);
                            return true;
                        }
                    }
                    break;
                case Swipe.RIGHT:
                    if (coordinate.x < rL - 2)
                    {
                        Vector2 targetPos = new Vector2(coordinate.x + 2, coordinate.y);
                        Tile targetTile = Board.tilesByCoordinates[targetPos];
                        if (targetTile.GetCard() == null)
                        {
                            ConfigureCardMove(card, targetTile);
                            return true;
                        }
                    }

                    if (coordinate.x < rL - 1)
                    {
                        Vector2 targetPos = new Vector2(coordinate.x + 1, coordinate.y);
                        Tile targetTile = Board.tilesByCoordinates[targetPos];
                        if (targetTile.GetCard() == null)
                        {
                            ConfigureCardMove(card, targetTile);
                            return true;
                        }
                    }
                    break;
                default:
                    break;
            }
            return false;
        }
        private void ConfigureCardMove(Card card, Tile targetTile)
        {
            MoveType moveType = FindMoveType(targetTile);
            bool canMove;
            if (moveType == MoveType.EMPTY)
                canMove = true;
            else
                canMove = card.CanAttack(targetTile.GetCard());
            Move move = new Move(targetTile, card, moveType, canMove);
            card.SetMove(move);
        }
        private MoveType FindMoveType(Tile t)
        {
            if (t == null)
                return MoveType.NONE;
            if (t.GetCard() == null)
                return MoveType.EMPTY;

            CardType type = t.GetCard().GetCardType();
            switch (type)
            {
                case CardType.PLAYER:
                    return MoveType.ATTACK;
                case CardType.ENEMY:
                    return MoveType.ATTACK;
                case CardType.ITEM:
                    return MoveType.ITEM;
                case CardType.COIN:
                    return MoveType.COIN;
                case CardType.EVENT:
                    return MoveType.EVENT;
                default:
                    return MoveType.EMPTY;
            }
        }
        public override Dictionary<Tile, Swipe> GetAvaibleTiles(Card card)
        {
            if (card == null) return null;

            Vector2 coordinate = card.GetTile().transform.position;
            int rL = Board.RowLength;
            Dictionary<Tile, Swipe> avaibleTiles = new Dictionary<Tile, Swipe>();

            bool upper = false;
            bool lower = false;
            bool left = false;
            bool right = false;

            if(coordinate.y < rL - 2) 
            {
                upper = true;
                var targetCoordinate = new Vector2(coordinate.x, coordinate.y + 2);
                var upperTile = Board.tilesByCoordinates[targetCoordinate];
                if(upperTile.GetCard() == null || card.GetCharacterType().IsEnemy(upperTile.GetCard().GetCharacterType()))
                {
                    avaibleTiles.Add(upperTile, Swipe.UP);
                }
            }
            if(!upper & coordinate.y < rL - 1) 
            {
                var targetCoordinate = new Vector2(coordinate.x, coordinate.y + 1);
                var upperTile = Board.tilesByCoordinates[targetCoordinate];
                if (upperTile.GetCard() == null || card.GetCharacterType().IsEnemy(upperTile.GetCard().GetCharacterType()))
                {
                    avaibleTiles.Add(upperTile, Swipe.UP);
                }
            }

            if(coordinate.y > 1)
            {
                lower = true;
                var targetCoordinate = new Vector2(coordinate.x, coordinate.y - 2);
                var lowerTile = Board.tilesByCoordinates[targetCoordinate];
                if (lowerTile.GetCard() == null || card.GetCharacterType().IsEnemy(lowerTile.GetCard().GetCharacterType()))
                {
                    avaibleTiles.Add(lowerTile, Swipe.DOWN);
                }
            }
            if(!lower && coordinate.y > 0)
            {
                var targetCoordinate = new Vector2(coordinate.x, coordinate.y - 1);
                var lowerTile = Board.tilesByCoordinates[targetCoordinate];
                if (lowerTile.GetCard() == null || card.GetCharacterType().IsEnemy(lowerTile.GetCard().GetCharacterType()))
                {
                    avaibleTiles.Add(lowerTile, Swipe.DOWN);
                }
            }

            if(coordinate.x > 1)
            {
                left = true;
                var targetCoordinate = new Vector2(coordinate.x - 2, coordinate.y);
                var leftTile = Board.tilesByCoordinates[targetCoordinate];
                if (leftTile.GetCard() == null || card.GetCharacterType().IsEnemy(leftTile.GetCard().GetCharacterType()))
                {
                    avaibleTiles.Add(leftTile, Swipe.LEFT);
                }
            }
            if(!left && coordinate.x > 0)
            {
                var targetCoordinate = new Vector2(coordinate.x - 1, coordinate.y);
                var leftTile = Board.tilesByCoordinates[targetCoordinate];
                if (leftTile.GetCard() == null || card.GetCharacterType().IsEnemy(leftTile.GetCard().GetCharacterType()))
                {
                    avaibleTiles.Add(leftTile, Swipe.LEFT);
                }
            }

            if(coordinate.x < rL - 2)
            {
                right = true;
                var targetCoordinate = new Vector2(coordinate.x + 2, coordinate.y);
                var rightTile = Board.tilesByCoordinates[targetCoordinate];
                if (rightTile.GetCard() == null || card.GetCharacterType().IsEnemy(rightTile.GetCard().GetCharacterType()))
                {
                    avaibleTiles.Add(rightTile, Swipe.RIGHT);
                }
            }
            if(!right && coordinate.x < rL - 1)
            {
                var targetCoordinate = new Vector2(coordinate.x + 1, coordinate.y);
                var rightTile = Board.tilesByCoordinates[targetCoordinate];
                if (rightTile.GetCard() == null || card.GetCharacterType().IsEnemy(rightTile.GetCard().GetCharacterType()))
                {
                    avaibleTiles.Add(rightTile, Swipe.RIGHT);
                }
            }

            return avaibleTiles;
        }
    }
}
