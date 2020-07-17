using DungeonRush.Cards;
using System;
using System.Collections.Generic;
using UnityEngine;
using DungeonRush.Field;
using DungeonRush.Property;
using DungeonRush.Controller;

namespace DungeonRush
{
    namespace Managers
    {
        public class CardManager : MonoBehaviour
        {
            private static CardManager instance = null;
            // Game Instance Singleton
            public static CardManager Instance
            {
                get { return instance; }
                set { instance = value; }
            }

            [Header("Characters")]
            public PlayerCard playerCard;
            public EnemyCard[] enemyCards;
            public ItemCard[] itemCards;
            public CoinCard[] coinCards;
            public EnemyCard[] trapCards;

            public List<Card> cards = new List<Card>();
            public Tile instantPlayerTile;

            public Board board;


            private void Awake()
            {
                Instance = this;
            }

            private void Start()
            {
                board = FindObjectOfType<Board>();
            }

            public void ReshuffleCards() 
            {
                cards.Clear();
                foreach (var card in FindObjectsOfType<Card>())
                {
                    cards.Add(card);
                }
            }

            public PlayerCard GetPlayerCard()
            {
                return this.playerCard;
            }

            public Card GetCard(Tile tile)
            {
                for (int i = 0; i < cards.Count; i++)
                {
                    Card card = cards[i];
                    if (card.GetTile() == tile)
                    {
                        return card;
                    }
                }
                throw new Exception("Error 31");
            }

            public Tile GetInstantPlayerTile()
            {
                return this.instantPlayerTile;
            }

            public void SetInstantPlayerTile(Tile tile)
            {
                this.instantPlayerTile = tile;
            }

            public void AddToCards(Card m_card, bool inGame)
            {
                if(inGame)
                {
                    for (int i = 0; i < cards.Count; i++)
                    {
                        if (cards[i] == null)
                        {
                            cards[i] = m_card;
                            break;
                        }
                    }
                }
                else
                {
                    cards.Add(m_card);
                }
            }

            public Card AddCard(Card piece, Tile tile, Board board, bool inGame, bool isTrapCard)
            {
                Card newPiece = Instantiate(piece, tile.transform.position, Quaternion.identity);
                if(newPiece.GetCardType() != CardType.PLAYER)
                    newPiece.transform.SetParent(board.transform);

                if (isTrapCard)
                    tile.SetTrapCard(newPiece);
                else
                    tile.SetCard(newPiece);

                newPiece.SetTile(tile);
                AddToCards(newPiece, inGame);
                return newPiece;
            }

            public void AddCard(Tile tile)
            {
                int number = UnityEngine.Random.Range(0, 101);
                if (number < 70)
                    AddCard(GiveRandomCard(enemyCards), tile, board, true, false);
                else if (number < 95)
                    AddCard(GiveRandomCard(itemCards), tile, board, true, false);
                else
                    AddCard(GiveRandomCard(coinCards), tile, board, true, false);
            }

            private Card GiveRandomCard(Card[] card)
            {
                int length = card.Length;
                return card[UnityEngine.Random.Range(0, length)];
            }

            public static void RemoveCard(Tile tile, bool isPlayerCard)
            {
                Card card = tile.GetCard();
                if(card.GetCardType() == CardType.ENEMY)
                {
                    EnemyController.UnsubscribeCard((AIController)card.Controller);
                }
                else if(card.GetCardType() == CardType.TRAP)
                {
                    TrapController.UnsubscribeCard((AIController)card.Controller);
                }

                Destroy(card.transform.gameObject);
                tile.SetCard(null);


            }

            public static void RemoveCardForAttacker(int listnumber, bool isPlayer)
            {
                RemoveCard(Board.tilesByListnumbers[listnumber], isPlayer);
            }
        }
    }
}
