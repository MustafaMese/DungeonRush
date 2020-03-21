using DungeonRush.Cards;
using System.Collections;
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

        [System.Serializable]
        public struct Health
        {
            private int health;

            public void Set(int amount)
            {
                health = Mathf.Max(0, amount);
            }

            public int Get()
            {
                return health;
            }

            public void ChangeHealth(bool isDamage, int amount)
            {
                if (isDamage)
                    health -= amount;
                else
                    health += amount;

                health = Mathf.Max(0, health);
            }
        }

        public class CardUtils : MonoBehaviour
        {
            public Card card;
            private int count;
            private List<Color32> colors;

            /// <summary>
            /// Düşmanlar için Çözüm: 1.5x1.5, 2x2, 2.5x2.5, 3x3, ... Mathf.Floor ile!
            /// </summary>
            private const int elev1 = 2, elev2 = 4, elev3 = 6, elev4 = 9, elev5 = 12, elev6 = 16, elev7 = 20;

            /// <summary>
            /// İtemler için Çözüm. 2x1, 2x2, 2x3, ...
            /// </summary>
            private const int ilev1 = 2, ilev2 = 4, ilev3 = 6, ilev4 = 8, ilev5 = 10, ilev6 = 14;  

            private Color32 white;
            private Color32 green;
            private Color32 blue;
            private Color32 purple;
            private Color32 orange;
            private Color32 magenta;
            private Color32 pink;
            private Color32 cyan;
            private Color32 gold;

            private void Start()
            {
                card = GetComponent<Card>();
                white = new Color32(255, 255, 255, 255);
                green = new Color32(65, 210, 11, 255);
                blue = new Color32(47, 120, 255, 255);
                purple = new Color32(145, 50, 200, 255);
                orange = new Color32(255, 250, 0, 255);
                magenta = new Color32(207, 71, 71, 255);
                pink = new Color32(231, 105, 180, 255);
                cyan = new Color32(0, 255, 255, 255);
                gold = new Color32(207, 181, 59, 255);
                colors = new List<Color32>();
                colors.Add(blue);
                colors.Add(purple);
                colors.Add(orange);
                colors.Add(magenta);
                colors.Add(pink);
                colors.Add(cyan);
                SetLevel();
            }

            public void SetRainbowColorFrame()
            {
                if (count == colors.Count - 1)
                    count = 0;
                count++;
                card.SetCardFrameColor(colors[count]);
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
                card.SetCardFrameColor(green);
            }

            private void SetItemLevel(Card card)
            {
                if (card.GetLevel() != 0)
                {
                    card.isBossCard = true;
                    card.SetCardFrameColor(gold);
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
                        card.SetCardFrameColor(gold);
                        card.isBossCard = true;
                        return;
                    }
                    card.SetCardFrameColor(white);
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
                        card.SetCardFrameColor(blue);
                        card.isBossCard = false;
                    }
                    else if (randomValue < 75f)
                    {
                        card.SetHealth(elev2);
                        card.SetLevel(2);
                        card.SetCardFrameColor(purple);
                        card.isBossCard = false;
                    }
                    else if (randomValue < 87.5f)
                    {
                        card.SetHealth(elev3);
                        card.SetLevel(3);
                        card.SetCardFrameColor(orange);
                        card.isBossCard = false;
                    }
                    else if (randomValue < 93.75f)
                    {
                        card.SetHealth(elev4);
                        card.SetLevel(4);
                        card.SetCardFrameColor(magenta);
                        card.isBossCard = false;
                    }
                    else if (randomValue < 96.875f)
                    {
                        card.SetHealth(elev5);
                        card.SetLevel(5);
                        card.SetCardFrameColor(pink);
                        card.isBossCard = false;
                    }
                    else if (randomValue < 98.4375f)
                    {
                        card.SetHealth(elev6);
                        card.SetLevel(6);
                        card.SetCardFrameColor(cyan);
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