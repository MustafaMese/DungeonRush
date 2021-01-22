using DungeonRush.Cards;
using DungeonRush.Data;
using DungeonRush.Field;
using DungeonRush.Managers;
using DungeonRush.Property;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush.Shifting
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Shift/OneBoxShift")]
    public class OneBoxShift : Shift
    {
        public override bool Define(Card card, Swipe swipe)
        {
            Vector2 coordinate = card.GetTile().transform.position;
            Vector2 targetPos = Vector2.zero;
            switch (swipe)
            {
                case Swipe.NONE:
                    break;
                case Swipe.UP:
                    targetPos = new Vector2(coordinate.x, coordinate.y + 1);
                    if(Board.tilesByCoordinates.ContainsKey(targetPos)) 
                    {
                        Tile targetTile = Board.tilesByCoordinates[targetPos];
                        if (targetTile.GetCard() != null && targetTile.GetCard().GetCardType() == CardType.WALL)
                            break;
                        ConfigureCardMove(card, targetTile);
                        return true;
                    }
                    break;
                case Swipe.DOWN:
                    targetPos = new Vector2(coordinate.x, coordinate.y - 1);
                    if(Board.tilesByCoordinates.ContainsKey(targetPos)) 
                    {
                        Tile targetTile = Board.tilesByCoordinates[targetPos];
                        if (targetTile.GetCard() != null && targetTile.GetCard().GetCardType() == CardType.WALL)
                            break;
                        ConfigureCardMove(card, targetTile);
                        return true;
                    }
                    break;
                case Swipe.LEFT:
                    targetPos = new Vector2(coordinate.x - 1, coordinate.y);
                    if(Board.tilesByCoordinates.ContainsKey(targetPos)) 
                    {
                        Tile targetTile = Board.tilesByCoordinates[targetPos];
                        if (targetTile.GetCard() != null && targetTile.GetCard().GetCardType() == CardType.WALL)
                            break;
                        ConfigureCardMove(card, targetTile);
                        return true;
                    }
                    break;
                case Swipe.RIGHT:
                    targetPos = new Vector2(coordinate.x + 1, coordinate.y);
                    if(Board.tilesByCoordinates.ContainsKey(targetPos)) 
                    {
                        Tile targetTile = Board.tilesByCoordinates[targetPos];
                        if (targetTile.GetCard() != null && targetTile.GetCard().GetCardType() == CardType.WALL)
                            break;
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
            MoveType moveType = FindMoveType(targetTile);

            bool canMove;
            if (moveType != MoveType.ATTACK)
                canMove = true;
            else
                canMove = false;

            Move move = new Move(targetTile, card, moveType, canMove);
            card.SetMove(move);
        }
        
        public override Dictionary<Tile, Swipe> GetAvaibleTiles(Card card)
        {
            if (card == null) return null;

            Vector2 coordinate = card.transform.position;
            Dictionary<Tile, Swipe> avaibleTiles = new Dictionary<Tile, Swipe>();

            Vector2 targetCoordinate;
            targetCoordinate = new Vector2(coordinate.x, coordinate.y + 1);
            if (Board.tilesByCoordinates.ContainsKey(targetCoordinate)) 
            {
                var upperTile = Board.tilesByCoordinates[targetCoordinate];
                if (upperTile.GetCard() == null) 
                    avaibleTiles.Add(upperTile, Swipe.UP);
            }

            targetCoordinate = new Vector2(coordinate.x, coordinate.y - 1);
            if(Board.tilesByCoordinates.ContainsKey(targetCoordinate)) 
            {
                var lowerTile = Board.tilesByCoordinates[targetCoordinate];
                if (lowerTile.GetCard() == null)
                    avaibleTiles.Add(lowerTile, Swipe.DOWN);
            }

            targetCoordinate = new Vector2(coordinate.x - 1, coordinate.y);
            if(Board.tilesByCoordinates.ContainsKey(targetCoordinate)) 
            {
                var leftTile = Board.tilesByCoordinates[targetCoordinate];
                if (leftTile.GetCard() == null)
                    avaibleTiles.Add(leftTile, Swipe.LEFT);
            }

            targetCoordinate = new Vector2(coordinate.x + 1, coordinate.y);
            if(Board.tilesByCoordinates.ContainsKey(targetCoordinate)) 
            {
                var rightTile = Board.tilesByCoordinates[targetCoordinate];
                if (rightTile.GetCard() == null)
                    avaibleTiles.Add(rightTile, Swipe.RIGHT);
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
                case CardType.PLAYER:
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
    }
}
