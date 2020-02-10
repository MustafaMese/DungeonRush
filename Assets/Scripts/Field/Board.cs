using System.Collections.Generic;
using UnityEngine;
using DungeonRush.Cards;
using DungeonRush.Managers;

namespace DungeonRush
{
    namespace Element
    {
        public class Board : MonoBehaviour
        {
            public static readonly int CARD_NUM = 12;
            public Tile[] cardPlaces = new Tile[CARD_NUM];
            public static Dictionary<int, Tile> tiles = new Dictionary<int, Tile>();
            public static bool touched;
            public GameManager gm;

            private void Start()
            {
                InitializeTiles();
                gm.AddCard(gm.playerCard, cardPlaces[0], true, this, false);
                gm.AddCard(gm.enemyCards[0], cardPlaces[1], false, this, false);

                for (int i = 0; i < cardPlaces.Length; i++)
                {
                    if (cardPlaces[i].GetCard() == null)
                    {
                        int number = Random.Range(0, 101);
                        if(number < 70)
                            gm.AddCard(GiveRandomCard(gm.enemyCards), cardPlaces[i], false, this, false);
                        else if (number < 95)
                            gm.AddCard(GiveRandomCard(gm.itemCards), cardPlaces[i], false, this, false);
                        else
                            gm.AddCard(GiveRandomCard(gm.coinCards), cardPlaces[i], false, this, false);
                    }
                }

                GameManager.GetCardManager().SetInstantPlayerTileFromCards();
                GameManager.GetCardManager().SetPlayerCardFromCards();
            }

            private void InitializeTiles()
            {
                int i = 0;
                foreach (var pos in cardPlaces)
                {
                    pos.SetCoordinate(pos.transform.position);
                    pos.SetCard(null);
                    pos.SetListNumber(i);
                    tiles.Add(i, pos);
                    i++;
                }
            }

            public Tile[] GetCardPlaces()
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
