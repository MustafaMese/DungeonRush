using DungeonRush.Cards;
using DungeonRush.Data;
using DungeonRush.Field;
using DungeonRush.Managers;
using DungeonRush.Property;
using DungeonRush.Traits;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush.Attacking
{
    public abstract class AttackStyle : ScriptableObject
    {
        [SerializeField] protected AudioClip clip;
        [SerializeField] protected GameObject effectObject;
        [SerializeField] protected float animationTime;
        [SerializeField] protected int power;

        [SerializeField] protected List<StatusObject> impacts = new List<StatusObject>();

        public abstract void Attack(Move move, int damage);
        public abstract void SetEffectPosition(GameObject effect, Vector3 tPos, Transform card = null);
       
        public virtual Dictionary<Tile, Swipe> GetAvaibleTiles(Card card)
        {
            if (card == null) return null;

            Vector2 coordinate = card.GetTile().GetCoordinate();
            Dictionary<Tile, Swipe> avaibleTiles = new Dictionary<Tile, Swipe>();
            Vector2 targetCoordinate;
            targetCoordinate = new Vector2(coordinate.x, coordinate.y + 1);
            if(Board.tilesByCoordinates.ContainsKey(targetCoordinate))
            {
                var upperTile = Board.tilesByCoordinates[targetCoordinate];
                if (upperTile.GetCard() != null && card.GetCharacterType().IsEnemy(upperTile.GetCard().GetCharacterType()))
                    avaibleTiles.Add(upperTile, Swipe.UP);
            }

            targetCoordinate = new Vector2(coordinate.x, coordinate.y - 1);
            if(Board.tilesByCoordinates.ContainsKey(targetCoordinate))
            {
                var lowerTile = Board.tilesByCoordinates[targetCoordinate];
                if (lowerTile.GetCard() != null && card.GetCharacterType().IsEnemy(lowerTile.GetCard().GetCharacterType()))
                    avaibleTiles.Add(lowerTile, Swipe.DOWN);
            }

            targetCoordinate = new Vector2(coordinate.x - 1, coordinate.y);
            if(Board.tilesByCoordinates.ContainsKey(targetCoordinate))
            {
                var leftTile = Board.tilesByCoordinates[targetCoordinate];
                if (leftTile.GetCard() != null && card.GetCharacterType().IsEnemy(leftTile.GetCard().GetCharacterType()))
                    avaibleTiles.Add(leftTile, Swipe.LEFT);
            }

            targetCoordinate = new Vector2(coordinate.x + 1, coordinate.y);
            if(Board.tilesByCoordinates.ContainsKey(targetCoordinate))
            {
                var rightTile = Board.tilesByCoordinates[targetCoordinate];
                if (rightTile.GetCard() != null && card.GetCharacterType().IsEnemy(rightTile.GetCard().GetCharacterType()))
                    avaibleTiles.Add(rightTile, Swipe.RIGHT);
            }

            return avaibleTiles;
        }
        public virtual bool Define(Card card, Swipe swipe)
        {
            Vector2 coordinate = card.GetTile().transform.position;
            Vector2 targetPos = Vector2.zero;
            switch (swipe)
            {
                case Swipe.NONE:
                    break;
                case Swipe.UP:
                    targetPos = new Vector2(coordinate.x, coordinate.y + 1);
                    if (Board.tilesByCoordinates.ContainsKey(targetPos))
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
                    if (Board.tilesByCoordinates.ContainsKey(targetPos))
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
                    if (Board.tilesByCoordinates.ContainsKey(targetPos))
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
                    if (Board.tilesByCoordinates.ContainsKey(targetPos))
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

        public List<StatusObject> GetImpacts() 
        {
            return impacts; 
        }
        
        public void ExecuteImpacts(Card target)
        {
            for (var i = 0; i < impacts.Count; i++)
            {
                target.GetStatusController().AddStatus(impacts[i]);
            }
        }
    }
}
