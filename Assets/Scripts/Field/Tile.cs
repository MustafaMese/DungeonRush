﻿using DungeonRush.Cards;
using DungeonRush.Managers;
using DungeonRush.Data;
using DungeonRush.Settings;
using UnityEngine;

namespace DungeonRush
{
    namespace Field 
    {
        [ExecuteAlways]
        public class Tile : MonoBehaviour
        {
            public Vector2Int coordinate;
            public Card card;
            public int listNumber;


            private void Start()
            {
                if (card != null)
                {
                    CardManager.Instance.AddCard(card, this, FindObjectOfType<Board>(), false);
                }
            }

            private void Update()
            {
                #if UNITY_EDITOR
                #endif
            }

            public Card GetCard()
            {
                return card;
            }

            public Vector2Int GetCoordinate()
            {
                return Geometry.GridPoint(coordinate.x, coordinate.y);
            }

            public void SetCoordinate(Vector3 pos)
            {
                this.coordinate = Geometry.GridFromPoint(pos);
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

            public int GetListNumber()
            {
                return listNumber;
            }

            public void SetListNumber(int listNumber)
            {
                this.listNumber = listNumber;
            }

            public static void ChangeTile(Move move, bool isEmpty, bool isPlayer)
            {
                if (!isEmpty)
                    CardManager.RemoveCard(move.GetTargetTile(), false);
                if (isPlayer)
                    CardManager.Instance.SetInstantPlayerTile(move.GetTargetTile());
                move.GetTargetTile().SetCard(move.GetCard());
                move.GetCardTile().SetCard(null);
                move.GetCard().SetTile(move.GetTargetTile());
            }
        }
    }
}
