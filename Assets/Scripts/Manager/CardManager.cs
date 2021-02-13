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

            public EnemyCard AddCard(EnemyCard prefab, Tile tile)
            {
                EnemyCard enemy = Instantiate(prefab, tile.transform.position, Quaternion.identity);
                tile.SetCard(enemy);
                enemy.SetTile(tile);
                cards.Add(enemy);
                return enemy;
            }

            public EnvironmentCard AddCard(EnvironmentCard prefab, Tile tile)
            {
                EnvironmentCard trap = Instantiate(prefab, tile.transform.position, Quaternion.identity);
                tile.SetEnvironmentCard(trap);
                trap.SetTile(tile);
                cards.Add(trap);
                return trap;
            }

            #endregion

            #region REMOVE METHODS

            public static void RemoveCard(Vector2 coordinate, bool isTrap = false)
            {
                Tile tile = Board.tilesByCoordinates[coordinate];

                if(!isTrap)
                {
                    Card card = tile.GetCard();
                    if (card != null)
                    {
                        if (card.GetCardType() == CardType.PLAYER && card.GetComponent<Health>().GetCurrentHealth() > 0) return;

                        tile.SetCard(null);
                        Instance.StartCoroutine(LateDestroy(card.gameObject));
                    }
                }
                else
                {
                    EnvironmentCard card = tile.GetEnvironmentCard();
                    tile.SetEnvironmentCard(null);
                    Instance.StartCoroutine(LateDestroy(card.gameObject));
                }
            }

            public static void RemoveCard(Tile tile, bool isTrap = false)
            {
                if(!isTrap)
                {
                    Card card = tile.GetCard();
                    if (card != null)
                    {
                        if (card.GetCardType() == CardType.PLAYER && card.GetComponent<Health>().GetCurrentHealth() > 0) return;

                        tile.SetCard(null);
                        Instance.StartCoroutine(LateDestroy(card.gameObject));
                    }
                }
                else
                {
                    EnvironmentCard card = tile.GetEnvironmentCard();
                    tile.SetEnvironmentCard(null);
                    Instance.StartCoroutine(LateDestroy(card.gameObject));
                }
            }

            public static void Unsubscribe(Card card)
            {
                if (card.GetCardType() == CardType.ENEMY)
                {
                    EnemyManager.UnsubscribeCard((EnemyAIController)card.GetController());
                }
                else if (card.GetCardType() == CardType.TRAP)
                {
                    EnvironmentManager.UnsubscribeCard((EnvironmentAIController)card.GetController());
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
            
            #endregion 
        }
    }
}
