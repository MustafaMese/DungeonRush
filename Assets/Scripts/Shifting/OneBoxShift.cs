using DungeonRush.Cards;
using DungeonRush.Data;
using DungeonRush.Field;
using DungeonRush.Managers;
using DungeonRush.Property;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush.Shifting
{
    [CreateAssetMenu(menuName = "Shift/OneBoxShift", order = 1)]
    public class OneBoxShift : Shift
    {
        public override bool Define(Card card, Swipe swipe)
        {
            upperBorder = Board.RowLength - 1;
            lowerBorder = Board.RowLength * (Board.RowLength - 1);
            leftBorder = 0;
            rightBorder = Board.RowLength - 1;

            int listnumber = card.GetTile().GetListNumber();
            int length = Board.RowLength;
            switch (swipe)
            {
                case Swipe.NONE:
                    break;
                case Swipe.UP:
                    if (listnumber > upperBorder)
                    {
                        int targetListnumber = listnumber - length;
                        Tile targetTile = Board.tiles[targetListnumber];
                        ConfigureCardMove(card, targetTile);
                    }
                    return true;
                case Swipe.DOWN:
                    if (listnumber < lowerBorder)
                    {
                        int targetListnumber = listnumber + length;
                        Tile targetTile = Board.tiles[targetListnumber];
                        ConfigureCardMove(card, targetTile);
                    }
                    return true;
                case Swipe.LEFT:
                    if (listnumber % length != leftBorder)
                    {
                        int targetListnumber = listnumber - 1;
                        Tile targetTile = Board.tiles[targetListnumber];
                        ConfigureCardMove(card, targetTile);
                    }
                    return true;
                case Swipe.RIGHT:
                    if (listnumber % length != rightBorder)
                    {
                        int targetListnumber = listnumber + 1;
                        Tile targetTile = Board.tiles[targetListnumber];
                        ConfigureCardMove(card, targetTile);
                    }
                    return true;
            }
            return false;
        }
        private void ConfigureCardMove(Card card, Tile targetTile)
        {
            Board.touched = true;
            MoveType moveType = FindMoveType(targetTile);
            bool canMove = card.CanAttack(targetTile.GetCard());
            Move move = new Move(targetTile, card, moveType, canMove);
            card.SetMove(move);
        }
        private Swipe SelectTileToAttack(Dictionary<Tile, Swipe> tiles, Tile target)
        {
            foreach (var tile in tiles.Keys)
            {
                if (tile == target)
                    return tiles[tile];
            }
            return Swipe.NONE;
        }
        private Dictionary<Tile, Swipe> GetAvaibleTiles(Card card)
        {
            if (card == null) return null;

            int listnumber = card.GetTile().GetListNumber();
            int length = Board.RowLength;
            Dictionary<Tile, Swipe> avaibleTiles = new Dictionary<Tile, Swipe>();

            if (listnumber > upperBorder)
            {
                var upperTile = Board.tiles[listnumber - length];
                if (card.GetCharacterType().IsEnemy(upperTile.GetCard().GetCharacterType()))
                {
                    avaibleTiles.Add(upperTile, Swipe.UP);
                }
            }
            if (listnumber < lowerBorder)
            {
                var lowerTile = Board.tiles[listnumber + length];
                if (card.GetCharacterType().IsEnemy(lowerTile.GetCard().GetCharacterType()))
                {
                    avaibleTiles.Add(lowerTile, Swipe.DOWN);
                }
            }
            if (listnumber % length != rightBorder)
            {
                var rightTile = Board.tiles[listnumber + 1];
                if (card.GetCharacterType().IsEnemy(rightTile.GetCard().GetCharacterType()))
                {
                    avaibleTiles.Add(rightTile, Swipe.RIGHT);
                }
            }
            if (listnumber % length != leftBorder)
            {
                var leftTile = Board.tiles[listnumber - 1];
                if (card.GetCharacterType().IsEnemy(leftTile.GetCard().GetCharacterType()))
                {
                    avaibleTiles.Add(leftTile, Swipe.LEFT);
                }
            }
            return avaibleTiles;
        }
        private MoveType FindMoveType(Tile t)
        {
            var type = t.GetCard().GetCardType();

            switch (type)
            {
                case CardType.ENEMY:
                    return MoveType.ATTACK;
                case CardType.ITEM:
                    return MoveType.ITEM;
                case CardType.COIN:
                    return MoveType.COIN;
                default:
                    return MoveType.NONE;
            }
        }
    }
}
