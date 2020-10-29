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
            private Vector2 coordinate = Vector2.zero;
            private Card card = null;
            private Card trapCard = null;

            [SerializeField] SpriteRenderer image;

            public void SetSortingLayer(Vector2 position)
            {
                float posY = position.y;
                float posX = position.x;

                int layer = (int)Math.Truncate(posY);
                string sth = String.Concat("Row ", layer);
                image.sortingLayerName = sth;

                image.sortingOrder = (int)posX;
            }

            public Card GetTrapCard()
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

            public void SetTrapCard(Card card)
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

            public static void ChangeTile(Move move, bool isEmpty, bool isPlayer)
            {
                if (!isEmpty)
                    //CardManager.RemoveCard(move.GetTargetTile());
                if (isPlayer)
                    CardManager.Instance.SetInstantPlayerTile(move.GetTargetTile());
                move.GetTargetTile().SetCard(move.GetCard());
                move.GetCardTile().SetCard(null);
                move.GetCard().SetTile(move.GetTargetTile());
            }
        }
    }
}
