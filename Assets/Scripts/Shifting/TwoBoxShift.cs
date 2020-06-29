using System;
using System.Collections;
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
            upperBorder = (Board.RowLength * 2) - 1;
            lowerBorder = Board.RowLength * (Board.RowLength - 2);
            leftBorder = 1;
            rightBorder = Board.RowLength - 2;

            int listnumber = card.GetTile().GetListNumber();
            int length = Board.RowLength;

            switch (swipe)
            {
                case Swipe.NONE:
                    break;
                case Swipe.UP:
                    if (listnumber > upperBorder)
                    {
                        int targetListnumber = listnumber - (length * 2);
                        Tile targetTile = Board.tilesByListnumbers[targetListnumber];
                        ConfigureCardMove(card, targetTile);
                        return true;
                    }
                    break;
                case Swipe.DOWN:
                    if (listnumber < lowerBorder)
                    {
                        int targetListnumber = listnumber + (length * 2);
                        Tile targetTile = Board.tilesByListnumbers[targetListnumber];
                        ConfigureCardMove(card, targetTile);
                        return true;
                    }
                    break;
                case Swipe.LEFT:
                    if (listnumber % length != leftBorder)
                    {
                        int targetListnumber = listnumber - 2;
                        Tile targetTile = Board.tilesByListnumbers[targetListnumber];
                        ConfigureCardMove(card, targetTile);
                        return true;
                    }
                    break;
                case Swipe.RIGHT:
                    if (listnumber % length != rightBorder)
                    {
                        int targetListnumber = listnumber + 2;
                        Tile targetTile = Board.tilesByListnumbers[targetListnumber];
                        ConfigureCardMove(card, targetTile);
                        return true;
                    }
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
        public override Dictionary<Tile, Swipe> GetAvaibleTiles(Card card)
        {
            throw new System.NotImplementedException();

            if (card == null) return null;

            int listnumber = card.GetTile().GetListNumber();
            int length = Board.RowLength;
            Dictionary<Tile, Swipe> avaibleTiles = new Dictionary<Tile, Swipe>();

            // TODO Sadece bir kare de gidebilmeli
            if(listnumber > upperBorder) 
            {
                var twoUpperTile = Board.tilesByListnumbers[listnumber - (length * 2)];
                if (twoUpperTile.GetCard() == null || card.GetCharacterType().IsEnemy(twoUpperTile.GetCard().GetCharacterType()))
                {
                    avaibleTiles.Add(twoUpperTile, Swipe.UP);
                }
                
            }
        }
        public override Swipe SelectTileToAttack(Dictionary<Tile, Swipe> tiles)
        {
            throw new System.NotImplementedException();
        }
    }
}
