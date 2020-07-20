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

            [SerializeField] bool editedInRuntime = false;

            private void Awake()
            {
                instance = this;

                bCreator = FindObjectOfType<BoardCreator>();
                RowLength = bCreator.rowLength;
            }

            private void Start()
            {
                if (!editedInRuntime)
                {
                    bCreator.InitializeTiles(cardPlaces);
                    cm.AddCard(cm.playerCard, cardPlaces[0], this, false, false);
                    //for (int i = 0; i < cardPlaces.Count; i++)
                    //{
                    //    if (cardPlaces[i].GetCard() == null)
                    //    {
                    //        int number = Random.Range(0, 101);
                    //        if (number < 20)
                    //            cm.AddCard(GiveRandomCard(cm.enemyCards), cardPlaces[i], this, false, false);
                    //        else if(number < 28)
                    //            cm.AddCard(GiveRandomCard(cm.itemCards), cardPlaces[i], this, false, false);
                    //    }
                    //}

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
                else 
                {
                    CardPlacesToTiles();
                }

                for (int i = 0; i < cm.cards.Count; i++)
                {
                    Card card = cm.cards[i];
                    if(card.GetCardType() == CardType.PLAYER)
                        cm.instantPlayerTile = card.GetTile();
                }
            }

            public void CardPlacesToTiles() 
            {
                foreach (var tile in cardPlaces)
                {
                    tilesByListnumbers.Add(tile.listNumber, tile);
                    tilesByCoordinates.Add(tile.transform.position, tile);
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

            public Card GiveRandomCard(Card[] card)
            {
                int length = card.Length;
                return card[Random.Range(0, length)];
            }

            private void OnDestroy()
            {
                tilesByListnumbers.Clear();
                tilesByCoordinates.Clear();
                touched = false;
            }
        }
    }
}
