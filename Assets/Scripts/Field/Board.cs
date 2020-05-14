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
            public static Board instance;

            public static int RowLength;
            public List<Tile> cardPlaces = new List<Tile>();
            public static Dictionary<int, Tile> tiles = new Dictionary<int, Tile>();
            public static bool touched;
            public CardManager cm;
            public BoardCreator bCreator;

            private void Awake()
            {
                instance = this;

                bCreator = FindObjectOfType<BoardCreator>();
                RowLength = bCreator.rowLength;
            }

            private void Start()
            {
                bCreator.InitializeTiles(cardPlaces);

                cm.AddCard(cm.playerCard, cardPlaces[28], this, false);

                for (int i = 0; i < cardPlaces.Count; i++)
                {
                    if (cardPlaces[i].GetCard() == null)
                    {
                        int number = Random.Range(0, 101);
                        if(number < 70)
                            cm.AddCard(GiveRandomCard(cm.enemyCards), cardPlaces[i], this, false);
                        else if (number < 95)
                            cm.AddCard(GiveRandomCard(cm.itemCards), cardPlaces[i], this, false);
                        else
                            cm.AddCard(GiveRandomCard(cm.coinCards), cardPlaces[i], this, false);
                    }
                }

                foreach (var card in cm.cards)
                {
                    if(card.GetCardType() == CardType.PLAYER)
                    {
                        cm.instantPlayerTile = card.GetTile();
                    }
                }
            }

            public void SetTiles(Dictionary<int, Tile> t)
            {
                tiles = t;
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
                tiles.Clear();
                touched = false;
            }
        }
    }
}
