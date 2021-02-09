using DungeonRush.Cards;
using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using DungeonRush.Field;
using DungeonRush.Property;
using DungeonRush.Controller;
using DG.Tweening;

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

            #endregion

            #region REMOVE METHODS

            // TODO Karakter rasgele yok oluyo
            public static void RemoveCard(Tile tile)
            {
                Card card = tile.GetCard();


                if (card != null)
                {
                    if (card.GetCardType() == CardType.PLAYER && card.GetComponent<Health>().GetCurrentHealth() > 0) return;

                    tile.SetCard(null);
                    Instance.StartCoroutine(LateDestroy(card.gameObject));
                }
            }

            public static void Unsubscribe(Card card)
            {
                if (card.GetCardType() == CardType.ENEMY)
                {
                    EnemyManager.UnsubscribeCard((EnemyAIController)card.Controller);
                }
                else if (card.GetCardType() == CardType.TRAP)
                {
                    TrapManager.UnsubscribeCard((TrapAIController)card.Controller);
                }
            }

            private static IEnumerator LateDestroy(GameObject obj)
            {
                yield return new WaitForSeconds(0.6f);
                if (obj != null)
                {
                    var c = obj.GetComponent<Card>().transform.DOKill();
                    Destroy(obj);
                }
            }

            public static void RemoveCardForAttacker(Vector2 coordinate)
            {
                RemoveCard(Board.tilesByCoordinates[coordinate]);
            }
            #endregion 
        }
    }
}
