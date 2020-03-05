using DungeonRush.Cards;
using System;
using System.Collections.Generic;
using UnityEngine;
using DungeonRush.Element;

namespace DungeonRush
{
    namespace Managers
    {
        public class CardManager : MonoBehaviour
        {
            public List<CardInformation> cardInformations = new List<CardInformation>();
            public List<Card> cards = new List<Card>();
            public Tile instantPlayerTile;

            private Card playerCard;

            public List<Card> GetHighLevelCards()
            {
                List<Card> cleverCards = new List<Card>();
                foreach (var card in cards)
                {
                    if (card.GetLevel() >= 5)
                        cleverCards.Add(card);
                }
                return cleverCards;
            }

            public void SetPlayerCardFromCards()
            {
                foreach (var card in cardInformations)
                {
                    if (card.playerCard)
                    {
                        this.playerCard = card.card;
                        return;
                    }
                }
                throw new Exception("Error 31");
            }

            public PlayerCard GetPlayerCard()
            {
                return (PlayerCard)this.playerCard;
            }

            public void SetInstantPlayerTileFromCards()
            {
                foreach (var card in cardInformations)
                {
                    if (card.playerCard)
                    {
                        this.instantPlayerTile = card.tile;
                        return;
                    }
                }
                throw new Exception("Error 31");
            }

            public Card GetCard(Tile tile)
            {
                foreach (var card in cards)
                {
                    if(card.GetTile() == tile)
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

            public static CardInformation CreateCardInformation(Tile tile, Card card, bool playerCard)
            {
                CardInformation ci = new CardInformation();
                ci.tile = tile;
                ci.card = card;
                ci.playerCard = playerCard;
                return ci;
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

            public void AddToCardInfos(CardInformation cardInfo, bool inGame)
            {
                if (inGame)
                {
                    for (int i = 0; i < cardInformations.Count; i++)
                    {
                        if(cardInformations[i].tile == cardInfo.tile)
                        {

                        }
                    }
                }
                else
                {
                    cardInformations.Add(cardInfo);
                }
            }
        }

        [Serializable]
        public struct CardInformation
        {
            public Tile tile;
            public Card card;
            public bool playerCard;
        }
    }
}
