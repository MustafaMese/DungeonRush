using DungeonRush.Cards;
using DungeonRush.Data;
using DungeonRush.Field;
using DungeonRush.Managers;
using DungeonRush.Property;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DungeonRush.Shifting
{
    [CreateAssetMenu(menuName = "Shift/OneBoxShift", order = 1)]
    public class OneBoxShift : Shift
    {
        public override bool Define(Card card, Swipe swipe)
        {
            int rL = Board.RowLength;

            Vector2 coordinate = card.GetTile().transform.position;
            switch (swipe)
            {
                case Swipe.NONE:
                    break;
                case Swipe.UP:
                    if(coordinate.y < rL - 1) 
                    {
                        Vector2 targetPos = new Vector2(coordinate.x, coordinate.y + 1);
                        Tile targetTile = Board.tilesByCoordinates[targetPos];
                        ConfigureCardMove(card, targetTile);
                        return true;
                    }
                    break;
                case Swipe.DOWN:
                    if(coordinate.y > 0) 
                    {
                        Vector2 targetPos = new Vector2(coordinate.x, coordinate.y - 1);
                        Tile targetTile = Board.tilesByCoordinates[targetPos];
                        ConfigureCardMove(card, targetTile);
                        return true;
                    }
                    break;
                case Swipe.LEFT:
                    if(coordinate.x > 0) 
                    {
                        Vector2 targetPos = new Vector2(coordinate.x - 1, coordinate.y);
                        Tile targetTile = Board.tilesByCoordinates[targetPos];
                        ConfigureCardMove(card, targetTile);
                        return true;
                    }
                    break;
                case Swipe.RIGHT:
                    if(coordinate.x < rL - 1) 
                    {
                        Vector2 targetPos = new Vector2(coordinate.x + 1, coordinate.y);
                        Tile targetTile = Board.tilesByCoordinates[targetPos];
                        ConfigureCardMove(card, targetTile);
                        return true;
                    }
                    break;
                default:
                    break;
            }

            return false;
        }
        private void ConfigureCardMove(Card card, Tile targetTile)
        {
            Board.touched = true;
            MoveType moveType = FindMoveType(targetTile);
            bool canMove;
            if (moveType == MoveType.EMPTY)
                canMove = true;
            else
                canMove = card.CanAttack(targetTile.GetCard());
            Move move = new Move(targetTile, card, moveType, canMove);
            card.SetMove(move);
        }
        public override Swipe SelectTileToAttack(Dictionary<Tile, Swipe> tiles)
        {
            var number = tiles.Count;
            number = Random.Range(0, number);

            List<Tile> keys = Enumerable.ToList(tiles.Keys);
            if (keys.Count <= 0)
                return Swipe.NONE;
            else
            {
                Tile tile = keys[number];
                return tiles[tile];
            }
        }
        public override Dictionary<Tile, Swipe> GetAvaibleTiles(Card card)
        {
            if (card == null) return null;

            int listnumber = card.GetTile().GetListNumber();
            int length = Board.RowLength;
            Dictionary<Tile, Swipe> avaibleTiles = new Dictionary<Tile, Swipe>();

            if (listnumber > upperBorder)
            {
                var upperTile = Board.tilesByListnumbers[listnumber - length];
                if (upperTile.GetCard() == null || card.GetCharacterType().IsEnemy(upperTile.GetCard().GetCharacterType()))
                {
                    avaibleTiles.Add(upperTile, Swipe.UP);
                }
            }
            if (listnumber < lowerBorder)
            {
                var lowerTile = Board.tilesByListnumbers[listnumber + length];
                if (lowerTile.GetCard() == null || card.GetCharacterType().IsEnemy(lowerTile.GetCard().GetCharacterType()))
                {
                    avaibleTiles.Add(lowerTile, Swipe.DOWN);
                }
            }
            if (listnumber % length != rightBorder)
            {
                var rightTile = Board.tilesByListnumbers[listnumber + 1];
                if (rightTile.GetCard() == null || card.GetCharacterType().IsEnemy(rightTile.GetCard().GetCharacterType()))
                {
                    avaibleTiles.Add(rightTile, Swipe.RIGHT);
                }
            }
            if (listnumber % length != leftBorder)
            {
                var leftTile = Board.tilesByListnumbers[listnumber - 1];
                if (leftTile.GetCard() == null || card.GetCharacterType().IsEnemy(leftTile.GetCard().GetCharacterType()))
                {
                    avaibleTiles.Add(leftTile, Swipe.LEFT);
                }
            }
            return avaibleTiles;
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
                case CardType.ENEMY:
                    return MoveType.ATTACK;
                case CardType.ITEM:
                    return MoveType.ITEM;
                case CardType.COIN:
                    return MoveType.COIN;
                default:
                    return MoveType.EMPTY;
            }
        }
    }
}
