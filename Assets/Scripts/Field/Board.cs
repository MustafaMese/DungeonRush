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
                InitializeTiles(new List<Tile>(FindObjectsOfType<Tile>()));
                SetCardTiles(new List<Card>(FindObjectsOfType<Card>()));
            }

            private void InitializeTiles(List<Tile> cardPlaces)
            {
                for (int i = 0; i < cardPlaces.Count; i++)
                {
                    Tile pos = cardPlaces[i];
                    if(pos.tileType != TileType.TILE) continue;
                    
                    pos.SetCoordinate(pos.transform.position);
                    pos.SetCard(null);
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

                    c.SetTile(t);

                }
            }

            private void OnDestroy()
            {
                tilesByCoordinates.Clear();
                touched = false;
            }
        }
    }
}
