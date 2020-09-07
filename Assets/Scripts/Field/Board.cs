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
            public static Dictionary<int, Tile> tilesByListnumbers = new Dictionary<int, Tile>();
            public static Dictionary<Vector2, Tile> tilesByCoordinates = new Dictionary<Vector2, Tile>();
            public static bool touched;
            public CardManager cm;
            public BoardCreator bCreator;
            public int playerStartTile = 0;

            [SerializeField] Sprite darknessGray = null;
            [SerializeField] Sprite darknessBlack = null;

            private void Awake()
            {
                instance = this;

                bCreator = FindObjectOfType<BoardCreator>();
                RowLength = bCreator.rowLength;
            }

            private void Start()
            {
                if(cardPlaces.Count <= 0)
                {
                    for (int i = 0; i < transform.childCount; i++)
                    {
                        Tile t = transform.GetChild(i).GetComponent<Tile>();
                        if (t != null)
                            cardPlaces.Add(transform.GetChild(i).GetComponent<Tile>());
                    }
                }

                cm.cards = new List<Card>(FindObjectsOfType<Card>());
                bCreator.InitializeTiles(cardPlaces);
                SetCardTiles(cm.cards);
                DeterminePlayerTile();
                SetTileDarkness();
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
                for (int i = 0; i < cm.cards.Count; i++)
                {
                    Card card = cm.cards[i];
                    if (card.GetCardType() == CardType.PLAYER)
                        cm.instantPlayerTile = card.GetTile();
                }
            }
            private void AddStaticCards()
            {
                for (int i = 0; i < cardPlaces.Count; i++)
                {
                    if (cardPlaces[i].GetTrapCard() == null)
                    {
                        int number = UnityEngine.Random.Range(0, 101);
                        if (number < 3)
                            cm.AddCard(GiveRandomCard(cm.trapCards), cardPlaces[i], this, true);
                    }
                }
            }
            private void AddDynamicCards()
            {
                for (int i = 0; i < cardPlaces.Count; i++)
                {
                    if (cardPlaces[i].GetCard() == null)
                    {
                        int number = UnityEngine.Random.Range(0, 101);
                        if (number < 20)
                            cm.AddCard(GiveRandomCard(cm.enemyCards), cardPlaces[i], this, false);
                        else if (number < 28)
                            cm.AddCard(GiveRandomCard(cm.itemCards), cardPlaces[i], this, false);
                    }
                }
            }
            public Card GiveRandomCard(Card[] card)
            {
                int length = card.Length;
                return card[UnityEngine.Random.Range(0, length)];
            }
            #endregion

            #region TILES AND CARD PLACES
            public void CardPlacesToTiles()
            {
                tilesByListnumbers.Clear();
                tilesByCoordinates.Clear();
                foreach (var tile in cardPlaces)
                {
                    tilesByListnumbers.Add(tile.GetListNumber(), tile);
                    tilesByCoordinates.Add(tile.GetCoordinate(), tile);
                }
            }
            public void SetTileDarkness()
            {
                for (int i = 0; i < cardPlaces.Count; i++)
                    cardPlaces[i].SetDarkness(null);

                //Tile playerTile = cm.instantPlayerTile;
                //Vector3 coordinate = playerTile.GetCoordinate();

                //for (int i = 0; i < cardPlaces.Count; i++)
                //{
                //    float distance = (cardPlaces[i].transform.position - coordinate).sqrMagnitude;
                //    if (distance <= 3)
                //        cardPlaces[i].SetDarkness(null);
                //    else if (distance <= 6)
                //        cardPlaces[i].SetDarkness(darknessGray);
                //    else
                //        cardPlaces[i].SetDarkness(darknessBlack);
                //}
            }
            public void SetTiles(Dictionary<int, Tile> t)
            {
                tilesByListnumbers = t;
            }
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
                tilesByListnumbers.Clear();
                tilesByCoordinates.Clear();
                touched = false;
            }
        }
    }
}
