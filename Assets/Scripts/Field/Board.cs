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
            public List<Tile> cardPlaces = new List<Tile>();
            public static Dictionary<Vector2, Tile> tilesByCoordinates = new Dictionary<Vector2, Tile>();
            public static bool touched;
            public BoardCreator bCreator;
            public int playerStartTile = 0;

            private void Awake()
            {
                instance = this;

                bCreator = FindObjectOfType<BoardCreator>();
                RowLength = bCreator.rowLength;
            }

            private void Start()
            {
                if (cardPlaces.Count <= 0)
                {
                    for (int i = 0; i < transform.childCount; i++)
                    {
                        Tile t = transform.GetChild(i).GetComponent<Tile>();
                        if (t != null)
                            cardPlaces.Add(transform.GetChild(i).GetComponent<Tile>());
                    }
                }

                Initizalize();
            }

            public void Initizalize()
            {
                CardManager.Instance.cards = new List<Card>(FindObjectsOfType<Card>());
                bCreator.InitializeTiles(cardPlaces);
                SetCardTiles(CardManager.Instance.cards);
                DeterminePlayerTile();
            }

            private void SetCardTiles(List<Card> cards)
            {
                for (int i = 0; i < cards.Count; i++)
                {
                    Card c = cards[i];
                    Tile t = tilesByCoordinates[c.transform.position];
                    
                    if (c.GetCardType() == CardType.TRAP)
                        t.SetTrapCard(c);
                    else
                        t.SetCard(c);

                    if (c.GetCardType() != CardType.PLAYER)
                        c.transform.SetParent(transform);

                    c.SetTile(t);

                }
            }

            #region CARDS
            public void DeterminePlayerTile()
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

            #region TILES AND CARD PLACES

            public void SetCardPlaces(List<Tile> cardPlaces)
            {
                this.cardPlaces = cardPlaces;
            }
            public List<Tile> GetCardPlaces()
            {
                return this.cardPlaces;
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
