using DungeonRush.Cards;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush
{
    namespace Property
    {
        [System.Serializable]
        public struct Item
        {
            public int health;
            public Sprite sprite;
            public bool exist;

            public void Reset()
            {
                health = 0;
                sprite = null;
                exist = false;
            }

            public void Set(Card item)
            {
                Reset();
                exist = true;
                health = item.GetHealth();
            }

            public int GetHealth()
            {
                return health;
            }

            public void DecreaseHealth(int damage)
            {
                health -= damage;
                health = Mathf.Max(0, health);
            }
        }

        public class CardUtils : MonoBehaviour
        {
            public Card card;

            /// <summary>
            /// Düşmanlar için Çözüm: 1.5x1.5, 2x2, 2.5x2.5, 3x3, ... Mathf.Floor ile!
            /// </summary>
            private const int elev1 = 2, elev2 = 4, elev3 = 6, elev4 = 9, elev5 = 12, elev6 = 16, elev7 = 20;

            /// <summary>
            /// İtemler için Çözüm. 2x1, 2x2, 2x3, ...
            /// </summary>
            private const int ilev1 = 2, ilev2 = 4, ilev3 = 6, ilev4 = 8, ilev5 = 10, ilev6 = 14;  

            private void Start()
            {
                card = GetComponent<Card>();
                SetLevel();
            }

            public void SetLevel()
            {
                if (card == null)
                    card = GetComponent<Card>();

                if (card.GetCardType() == CardType.ENEMY)
                {
                    SetEnemyLevel(card);
                }
                else if (card.GetCardType() == CardType.ITEM)
                {
                    SetItemLevel(card);
                }
                else if (card.GetCardType() == CardType.COIN)
                {
                    SetCoinLevel(card);
                }
            }

            private void SetCoinLevel(Card card)
            {
                float randomValue = Random.Range(0, 100);
                if (randomValue < 50f)
                {
                    card.SetHealth(ilev1);
                    card.SetLevel(1);
                }
                else if (randomValue < 80f)
                {
                    card.SetHealth(ilev2);
                    card.SetLevel(2);
                }
                else if (randomValue < 89.5f)
                {
                    card.SetHealth(ilev3);
                    card.SetLevel(3);
                }
                else if (randomValue < 94.75f)
                {
                    card.SetHealth(ilev4);
                    card.SetLevel(4);
                }
                else if (randomValue < 98.875f)
                {
                    card.SetHealth(ilev5);
                    card.SetLevel(5);
                }
                else if (randomValue < 99.875f)
                {
                    card.SetHealth(ilev6);
                    card.SetLevel(6); 
                    
                }
                card.isBossCard = false;
            }

            private void SetItemLevel(Card card)
            {
                if (card.GetLevel() != 0)
                {
                    card.isBossCard = true;
                }
                else
                {
                    float randomValue = Random.Range(0, 100);
                    if (randomValue < 50f)
                    {
                        card.SetHealth(ilev1);
                        card.SetLevel(1);
                        card.isBossCard = false;
                    }
                    else if (randomValue < 80f)
                    {
                        card.SetHealth(ilev2);
                        card.SetLevel(2);
                        card.isBossCard = false;
                    }
                    else if (randomValue < 89.5f)
                    {
                        card.SetHealth(ilev3);
                        card.SetLevel(3);
                        card.isBossCard = false;
                    }
                    else if (randomValue < 94.75f)
                    {
                        card.SetHealth(ilev4);
                        card.SetLevel(4);
                        card.isBossCard = false;
                    }
                    else if (randomValue < 98.875f)
                    {
                        card.SetHealth(ilev5);
                        card.SetLevel(5);
                        card.isBossCard = false;
                    }
                    else if (randomValue < 99.875f)
                    {
                        card.SetHealth(ilev6);
                        card.SetLevel(6);
                        card.isBossCard = true;
                        return;
                    }
                }
            }

            public void SetEnemyLevel(Card card)
            {
                if (card.GetLevel() != 0)
                {
                    card.isBossCard = true;
                }
                else
                {
                    float randomValue = Random.Range(0, 100);
                    if (randomValue < 50f)
                    {
                        card.SetHealth(elev1);
                        card.SetLevel(1);
                        card.isBossCard = false;
                    }
                    else if (randomValue < 75f)
                    {
                        card.SetHealth(elev2);
                        card.SetLevel(2);
                        card.isBossCard = false;
                    }
                    else if (randomValue < 87.5f)
                    {
                        card.SetHealth(elev3);
                        card.SetLevel(3);
                        card.isBossCard = false;
                    }
                    else if (randomValue < 93.75f)
                    {
                        card.SetHealth(elev4);
                        card.SetLevel(4);
                        card.isBossCard = false;
                    }
                    else if (randomValue < 96.875f)
                    {
                        card.SetHealth(elev5);
                        card.SetLevel(5);
                        card.isBossCard = false;
                    }
                    else if (randomValue < 98.4375f)
                    {
                        card.SetHealth(elev6);
                        card.SetLevel(6);
                        card.isBossCard = false;
                    }
                    else
                    {
                        card.SetHealth(elev7);
                        card.SetLevel(7);
                        card.isBossCard = true;
                    }
                }
            }
        }
    }
}