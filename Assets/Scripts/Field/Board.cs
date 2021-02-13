using System.Collections.Generic;
using UnityEngine;
using DungeonRush.Cards;
using DungeonRush.Managers;
using DungeonRush.Property;
using System;
using UnityEditor;

namespace DungeonRush
{
    namespace Field
    {
        public class Board : MonoBehaviour
        {
            // Touched moveschedulara veya nonplayera falan alınabilinir.
            public static Board instance;

            public static int RowLength;
            public static Dictionary<Vector2, Tile> tilesByCoordinates = new Dictionary<Vector2, Tile>();
            public static bool touched;

            private void Awake()
            {
                instance = this;
            }

            private void Start()
            {

                Initizalize();
            }

            public void Initizalize()
            {
                CardManager.Instance.cards = new List<Card>(FindObjectsOfType<Card>());
                InitializeTiles(new List<Tile>(FindObjectsOfType<Tile>()));
                SetCardTiles(CardManager.Instance.cards);
                DeterminePlayerTile();
            }

            private void InitializeTiles(List<Tile> cardPlaces)
            {
                for (int i = 0; i < cardPlaces.Count; i++)
                {
                    Tile pos = cardPlaces[i];
                    if(pos.tileType != TileType.TILE) continue;
                    
                    pos.SetCoordinate(pos.transform.position);
                    pos.SetCard(null);
                    //pos.transform.SetParent(null);
                    tilesByCoordinates.Add(pos.transform.position, pos);
                }
            }

            private void SetCardTiles(List<Card> cards)
            {
                for (int i = 0; i < cards.Count; i++)
                {
                    Card c = cards[i];
                    Tile t = tilesByCoordinates[c.transform.position];
                    
                    if (c.GetCardType() == CardType.TRAP)
                        t.SetEnvironmentCard((EnvironmentCard)c);
                    else
                        t.SetCard(c);

                    c.transform.SetParent(null);
                    // if (c.GetCardType() != CardType.PLAYER)
                    //     c.transform.SetParent(transform);

                    c.SetTile(t);

                }
            }

            #region CARDS
            private void DeterminePlayerTile()
            {
                int count = CardManager.Instance.cards.Count;
                List<Card> cmCards = new List<Card>(CardManager.Instance.cards);
                for (int i = 0; i < count; i++)
                {
                    Card card = cmCards[i];
                    if (card.GetCardType() == CardType.PLAYER)
                        CardManager.Instance.instantPlayerTile = card.GetTile();
                }
            }

            public Card GiveRandomCard(Card[] card)
            {
                int length = card.Length;
                return card[UnityEngine.Random.Range(0, length)];
            }
            #endregion

            private void OnDestroy()
            {
                tilesByCoordinates.Clear();
                touched = false;
            }
        }
    }
}
