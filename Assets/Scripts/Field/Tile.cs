using DungeonRush.Cards;
using DungeonRush.Managers;
using DungeonRush.Data;
using UnityEngine;
using System;

namespace DungeonRush
{
    namespace Field 
    {
        public class Tile : MonoBehaviour
        {
            public TileType tileType;

            private Vector2 coordinate = Vector2.zero;
            private Card card = null;
            [SerializeField] EnvironmentCard trapCard = null;

            [SerializeField] SpriteRenderer image;

            public void SetSortingLayer(Vector2 position, bool useOrder = false)
            {
                float posY = position.y;
                float posX = position.x;

                int layer = (int)Math.Truncate(posY);
                string sth = String.Concat("Row ", layer);
                image.sortingLayerName = sth;

                if(useOrder)
                    image.sortingOrder = (int)posX;
            }

            public EnvironmentCard GetEnvironmentCard()
            {
                return trapCard;
            }

            public Card GetCard()
            {
                return card;
            }

            public Vector2 GetCoordinate()
            {
                return coordinate;
            }

            public void SetCoordinate(Vector3 pos)
            {
                this.coordinate = pos;
            }

            public void SetEnvironmentCard(EnvironmentCard card)
            {
                this.trapCard = card;
            }

            public void SetCard(Card card)
            {
                this.card = card;
            }

            public bool IsTileOccupied()
            {
                if (this.card != null)
                    return true;
                return false;
            }

            public static void ChangeTile(Move move)
            {
                move.GetTargetTile().SetCard(move.GetCard());
                move.GetCardTile().SetCard(null);
                move.GetCard().SetTile(move.GetTargetTile());
            }
        }
    }
}

public enum TileType
{
    TILE,
    TOP_WALL,
    DOWN_WALL,
    RIGHT_WALL,
    LEFT_WALL,
    TOPLEFT_WALL,
    TOPRIGHT_WALL,
    DOWNLEFT_WALL,
    DOWNRIGHT_WALL,
}
