using System.Collections.Generic;
using UnityEngine;
using DungeonRush.Cards;
using DungeonRush.Managers;
using DungeonRush.Property;

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

            [SerializeField] Sprite darknessGray;
            [SerializeField] Sprite darknessBlack;

            private void Awake()
            {
                instance = this;

                bCreator = FindObjectOfType<BoardCreator>();
                RowLength = bCreator.rowLength;
            }

            private void Start()
            {
                bCreator.InitializeTiles(cardPlaces);
                cm.AddCard(cm.playerCard, cardPlaces[0], this, false, false);
                AddDynamicCards();
                AddStaticCards();
                DeterminePlayerTile();
                SetTileDarkness();
            }

            public void SetTileDarkness()
            {
                Tile playerTile = cm.instantPlayerTile;
                Vector3 coordinate = playerTile.GetCoordinate();

                for (int i = 0; i < cardPlaces.Count; i++)
                {
                    float distance = (cardPlaces[i].transform.position - coordinate).sqrMagnitude;
                    if (distance <= 3)
                        cardPlaces[i].SetDarkness(null);
                    else if (distance <= 6)
                        cardPlaces[i].SetDarkness(darknessGray);
                    else
                        cardPlaces[i].SetDarkness(darknessBlack);
                }
            }

            #region CARDS
            private void DeterminePlayerTile()
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
                        int number = Random.Range(0, 101);
                        if (number < 3)
                            cm.AddCard(GiveRandomCard(cm.trapCards), cardPlaces[i], this, false, true);
                    }
                }
            }
            private void AddDynamicCards()
            {
                for (int i = 0; i < cardPlaces.Count; i++)
                {
                    if (cardPlaces[i].GetCard() == null)
                    {
                        int number = Random.Range(0, 101);
                        if (number < 20)
                            cm.AddCard(GiveRandomCard(cm.enemyCards), cardPlaces[i], this, false, false);
                        else if (number < 28)
                            cm.AddCard(GiveRandomCard(cm.itemCards), cardPlaces[i], this, false, false);
                    }
                }
            }
            public Card GiveRandomCard(Card[] card)
            {
                int length = card.Length;
                return card[Random.Range(0, length)];
            }
            #endregion

            #region TILES AND CARD PLACES
            public void CardPlacesToTiles()
            {
                foreach (var tile in cardPlaces)
                {
                    tilesByListnumbers.Add(tile.GetListNumber(), tile);
                    tilesByCoordinates.Add(tile.GetCoordinate(), tile);
                }
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
