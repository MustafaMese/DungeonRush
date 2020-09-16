using DungeonRush.Cards;
using System;
using System.Collections.Generic;
using UnityEngine;
using DungeonRush.Field;
using DungeonRush.Property;
using DungeonRush.Controller;

namespace DungeonRush
{
    namespace Managers
    {
        [ExecuteAlways]
        public class CardManager : MonoBehaviour
        {
            private static CardManager instance = null;
            // Game Instance Singleton
            public static CardManager Instance
            {
                get { return instance; }
                set { instance = value; }
            }

            public List<Card> cards = new List<Card>();
            public Tile instantPlayerTile;

            private void Awake()
            {
                if (Instance != null)
                    Destroy(Instance);
                else
                    Instance = this;
            }

            public void ReshuffleCards() 
            {
                cards.Clear();
                foreach (var card in FindObjectsOfType<Card>())
                {
                    cards.Add(card);
                }
            }

            public void SetInstantPlayerTile(Tile tile)
            {
                this.instantPlayerTile = tile;
            }

            #region ADDING METHODS

            /// <summary>
            /// This method using for runtime.
            /// </summary>
            public Card AddCard(Card piece, Tile tile, Board board, bool isTrapCard)
            {
                Card newPiece = Instantiate(piece, tile.transform.position, Quaternion.identity);
                if(newPiece.GetCardType() != CardType.PLAYER)
                    newPiece.transform.SetParent(board.transform);

                if (isTrapCard)
                    tile.SetTrapCard(newPiece);
                else
                    tile.SetCard(newPiece);

                newPiece.SetTile(tile);
                cards.Add(newPiece);
                return newPiece;
            }

            /// <summary>
            /// In this method, cards are just instantiated. Using for editor.
            /// </summary>
            public void AddCard(Card piece, Vector3 pos)
            {
                Instantiate(piece, pos, Quaternion.identity);
            }

            #endregion

            #region REMOVE METHODS

            public static void RemoveCard(Tile tile)
            {
                Card card = tile.GetCard();
                if(card.GetCardType() == CardType.ENEMY)
                {
                    EnemyController.UnsubscribeCard((AIController)card.Controller);
                }
                else if(card.GetCardType() == CardType.TRAP)
                {
                    TrapController.UnsubscribeCard((AIController)card.Controller);
                }

                Destroy(card.transform.gameObject);
                tile.SetCard(null);
            }

            public static void RemoveCardForAttacker(Vector2 coordinate)
            {
                RemoveCard(Board.tilesByCoordinates[coordinate]);
            }
            #endregion 
        }
    }
}
