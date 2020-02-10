using UnityEngine;
using System.Collections;
using DungeonRush.Cards;
using DungeonRush.Settings;
using DungeonRush.Element;
using DungeonRush.Moves;
using System;
using TMPro;
using DungeonRush.Property;

namespace DungeonRush
{
    namespace Managers
    {
        public class GameManager : MonoBehaviour
        {
            public static MoveMaker moveMaker;
            public static CardManager cardManager;
            public TourManager tourManager;
            public CoinCounter coinCounter;
            public CardController cardController;

            [Header("Board")]
            public Board board;

            [Header("Characters")]
            public PlayerCard playerCard;
            public EnemyCard[] enemyCards;
            public ItemCard[] itemCards;
            public CoinCard[] coinCards;

            private void Awake()
            {
                moveMaker = FindObjectOfType<MoveMaker>();
                cardManager = FindObjectOfType<CardManager>();
                tourManager = FindObjectOfType<TourManager>();
                coinCounter = FindObjectOfType<CoinCounter>();
                cardController = FindObjectOfType<CardController>();
            }

            private void Update()
            {
                if (!Board.touched && tourManager.IsTourNumbersEqual())
                {
                    if (SwipeManager.swipeDirection != Swipe.None)
                    {
                        AssignPlayerMove();
                    }
                }
            }

            private void AssignPlayerMove()
            {
                int listnumber = cardManager.GetInstantPlayerTile().GetListNumber();
                Tile targetTile = null;
                Tile targetTile2 = null;
                Tile targetTile3 = null;
                Tile targetTile4 = null;
                try
                {
                    cardController.AssignTiles(listnumber, ref targetTile, ref targetTile2, ref targetTile3, ref targetTile4);
                }
                catch (Exception)
                {
                    throw new Exception("Dictionary'de sınırı aştın. No problema. Error 31");
                }
                cardController.AssignMoves(targetTile, targetTile2, targetTile3, targetTile4);
            }

            public Card AddCard(Card piece, Tile tile, bool playerCard, Board board, bool inGame)
            {
                Card newPiece = Instantiate(piece, tile.transform.position, Quaternion.identity, board.transform);
                tile.SetCard(newPiece);
                newPiece.SetTile(tile);
                cardManager.AddToCards(newPiece, inGame);
                // Card informations bu if ile beraber oyundan fiilen çıkarılmış oldu.
                if(!inGame)
                    cardManager.cardInformations.Add(CardManager.CreateCardInformation(tile, newPiece, playerCard));
                return newPiece;
            }

            public static void RemoveCard(Tile tile)
            {
                foreach (var card in cardManager.cards)
                {
                    if(card.GetTile() == tile)
                    {
                        tile.SetCard(null);
                        Destroy(card.transform.gameObject);
                        return;
                    }
                }
            }

            public static void RemoveCard(Move move, bool isPlayerCard)
            {
                if (!isPlayerCard)
                {
                    if (!move.GetTargetTile().IsTileOccupied())
                        return;

                    foreach (var card in cardManager.cards)
                    {
                        if (card.GetTile() == move.GetTargetTile())
                        {
                            move.GetTargetTile().SetCard(null);
                            Destroy(card.transform.gameObject);
                            return;
                        }
                    }
                    throw new Exception("Error 31");
                }
                else
                {
                    foreach (var card in cardManager.cards)
                    {
                        if (card == move.GetCard())
                        {
                            Destroy(card.transform.gameObject);
                            return;
                        }
                    }
                    throw new Exception("Error 31");
                }
            }

            public static CardManager GetCardManager()
            {
                return cardManager;
            }

            public static MoveMaker GetMoveMaker()
            {
                return moveMaker;
            }

            public void NullControlOnTiles()
            {
                for (int i = 0; i < Board.tiles.Count; i++)
                {
                    if (Board.tiles[i].GetCard() == null)
                    {
                        foreach (var card in GameManager.GetCardManager().cards)
                        {
                            if (card.GetTile() == Board.tiles[i])
                            {
                                Board.tiles[i].SetCard(card);
                                return;
                            }
                        }
                        AddCardImmediately(Board.tiles[i]);
                        throw new Exception("Bişeyler ekledik.");
                    }
                }
            }

            public void AddCardImmediately(Tile tile)
            {
                int number = UnityEngine.Random.Range(0, 101);
                if (number < 70)
                    AddCard(GiveRandomCard(enemyCards), tile, false, board, true);
                else if (number < 95)
                    AddCard(GiveRandomCard(itemCards), tile, false, board, true);
                else
                    AddCard(GiveRandomCard(coinCards), tile, false, board, true);
            }

            private Card GiveRandomCard(Card[] card)
            {
                int length = card.Length;
                return card[UnityEngine.Random.Range(0, length)];
            }
        }
    }
}
