using DungeonRush.Cards;
using DungeonRush.Data;
using DungeonRush.Field;
using DungeonRush.Managers;
using DungeonRush.Property;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush.Attacking
{
    public abstract class AttackStyle : ScriptableObject
    {
        [SerializeField] protected GameObject effectObject;
        [SerializeField] protected float animationTime;
        [SerializeField] protected int power;

        public abstract void Attack(Move move, int damage);
        public abstract void SetEffectPosition(GameObject effect, Vector3 tPos, Transform card = null);
        public virtual Dictionary<Tile, Swipe> GetAvaibleTiles(Card card)
        {
            if (card == null) return null;

            int rL = Board.RowLength;
            Vector2 coordinate = card.transform.position;
            Dictionary<Tile, Swipe> avaibleTiles = new Dictionary<Tile, Swipe>();

            if (coordinate.y < rL - 1)
            {
                var targetCoordinate = new Vector2(coordinate.x, coordinate.y + 1);
                var upperTile = Board.tilesByCoordinates[targetCoordinate];
                if (upperTile.GetCard() != null && card.GetCharacterType().IsEnemy(upperTile.GetCard().GetCharacterType()))
                {
                    avaibleTiles.Add(upperTile, Swipe.UP);
                }
            }

            if (coordinate.y > 0)
            {
                var targetCoordinate = new Vector2(coordinate.x, coordinate.y - 1);
                var lowerTile = Board.tilesByCoordinates[targetCoordinate];
                if (lowerTile.GetCard() != null && card.GetCharacterType().IsEnemy(lowerTile.GetCard().GetCharacterType()))
                {
                    avaibleTiles.Add(lowerTile, Swipe.DOWN);
                }
            }

            if (coordinate.x > 0)
            {
                var targetCoordinate = new Vector2(coordinate.x - 1, coordinate.y);
                var leftTile = Board.tilesByCoordinates[targetCoordinate];
                if (leftTile.GetCard() != null && card.GetCharacterType().IsEnemy(leftTile.GetCard().GetCharacterType()))
                {
                    avaibleTiles.Add(leftTile, Swipe.LEFT);
                }
            }

            if (coordinate.x < rL - 1)
            {
                var targetCoordinate = new Vector2(coordinate.x + 1, coordinate.y);
                var rightTile = Board.tilesByCoordinates[targetCoordinate];
                if (rightTile.GetCard() != null && card.GetCharacterType().IsEnemy(rightTile.GetCard().GetCharacterType()))
                {
                    avaibleTiles.Add(rightTile, Swipe.RIGHT);
                }
            }

            return avaibleTiles;
        }
        public virtual bool Define(Card card, Swipe swipe)
        {
            int rL = Board.RowLength;
            Vector2 coordinate = card.GetTile().transform.position;
            switch (swipe)
            {
                case Swipe.NONE:
                    break;
                case Swipe.UP:
                    if (coordinate.y < rL - 1)
                    {
                        Vector2 targetPos = new Vector2(coordinate.x, coordinate.y + 1);
                        Tile targetTile = Board.tilesByCoordinates[targetPos];
                        if (targetTile.GetCard() != null && targetTile.GetCard().GetCardType() == CardType.WALL)
                            break;
                        ConfigureCardMove(card, targetTile);
                        return true;
                    }
                    break;
                case Swipe.DOWN:
                    if (coordinate.y > 0)
                    {
                        Vector2 targetPos = new Vector2(coordinate.x, coordinate.y - 1);
                        Tile targetTile = Board.tilesByCoordinates[targetPos];
                        if (targetTile.GetCard() != null && targetTile.GetCard().GetCardType() == CardType.WALL)
                            break;
                        ConfigureCardMove(card, targetTile);
                        return true;
                    }
                    break;
                case Swipe.LEFT:
                    if (coordinate.x > 0)
                    {
                        Vector2 targetPos = new Vector2(coordinate.x - 1, coordinate.y);
                        Tile targetTile = Board.tilesByCoordinates[targetPos];
                        if (targetTile.GetCard() != null && targetTile.GetCard().GetCardType() == CardType.WALL)
                            break;
                        ConfigureCardMove(card, targetTile);
                        return true;
                    }
                    break;
                case Swipe.RIGHT:
                    if (coordinate.x < rL - 1)
                    {
                        Vector2 targetPos = new Vector2(coordinate.x + 1, coordinate.y);
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
        public virtual List<Card> GetAttackedCards(Move move)
        {
            return null;
        }
        public int GetPower()
        {
            return power;
        }
        public GameObject GetEffect()
        {
            return effectObject;
        }
        public float GetAnimationTime()
        {
            return animationTime;
        }
        protected void ConfigureCardMove(Card card, Tile targetTile)
        {
            Move move = new Move(targetTile, card, MoveType.ATTACK, false);
            card.SetMove(move);
        }
    }
}
