﻿using DungeonRush.Element;
using DungeonRush.Moves;
using DungeonRush.Property;
using System.Collections;
using TMPro;
using UnityEngine;

namespace DungeonRush
{
    namespace Cards
    {
        // TODO Card sınıfını alt yapılara bölebilirsin.
        // Örneğin Move'la alakalı her şeyi Mover classına atabilirsin.
        // Health ile alakalı her şeyi Health classına atabilirsin.
        // Item almayla alakalı her şeyi Item classına atabilirsin.
        [RequireComponent(typeof(CardUtils), typeof(Mover))]
        public abstract class Card : MonoBehaviour
        {
            protected Tile coordinate;
            protected Health health;
            protected string cardName;
            protected Move move;
            protected int cardLevel;
            protected float timeLeft;
            protected Mover mover;
            protected bool isAlive;

            [SerializeField, Range(0, 1)] float disappearing = 0.1f;

            [Header("General Properties")]
            public CardTemplate cardProperties;
            public bool isMoving;
            public bool isBossCard;
            public CardUtils cardUtils;

            [Header("General Components")]
            public SpriteRenderer frameColor;
            public SpriteRenderer characterSprite;
            public TextMeshPro textMeshHealth;
            public TextMeshPro textMeshName;

            public void Start()
            {
                isAlive = true;
                isMoving = false;
                isBossCard = false;
                timeLeft = 2f;
                cardLevel = cardProperties.level;
                health.Set(cardProperties.health);
                cardName = cardProperties.cardName;
                characterSprite.sprite = cardProperties.sprite;
                textMeshHealth.text = health.Get().ToString();
                
                // TODO Burayı sonradan toparlamayı düşünebilirsin.
                if(CardType.COIN != GetCardType())
                    textMeshName.text = cardName;
                else
                    cardUtils.SetLevel();
                mover = GetComponent<Mover>();
                move.Reset();
                StartCoroutine(FadeImage(false));
            }

            // Color da sıkıntılar mevcut
            public void Update()
            {
                if(!isAlive)
                {
                    transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(0, 0, 0), disappearing);
                }
                if (isBossCard)
                {
                    RainbowColorTimer();
                }

            }

            public abstract string GetCardName();
            public abstract int GetHealth();
            protected abstract void UpdateHealthText();
            public abstract void DecreaseHealth(int damage);
            public abstract void IncreaseHealth(int health);
            public abstract void SetHealth(int health);
            public abstract Sprite GetImage();
            public abstract CardType GetCardType();
            public abstract void SetCardType(CardType type);
            public abstract Tile GetTile();
            public abstract void SetTile(Tile coordinate);
            public abstract Move GetMove();
            public abstract void SetMove(Move move);
            public abstract void ExecuteMove();
            public abstract void SetLevel(int level);
            public abstract int GetLevel();
            public abstract SpriteRenderer GetFrameColor();
            public abstract void SetCardFrameColor(Color32 color);
            public void RainbowColorTimer()
            {
                timeLeft -= Time.deltaTime;
                if (timeLeft < 0)
                {
                    cardUtils.SetRainbowColorFrame();
                    timeLeft = 2f;
                }
            }
            public void Disappear()
            {
                isAlive = false;
            }
            public IEnumerator FadeImage(bool fadeAway)
            {
                // fade from opaque to transparent
                if (fadeAway)
                {
                    // loop over 1 second backwards
                    for (float i = 1; i >= 0; i -= Time.deltaTime)
                    {
                        // set color with i as alpha
                        characterSprite.color = new Color(255, 255, 255, i);
                        yield return null;
                    }
                }
                // fade from transparent to opaque
                else
                {
                    // loop over 1 second
                    for (float i = 0; i <= 1; i += Time.deltaTime)
                    {
                        // set color with i as alpha
                        characterSprite.color = new Color(255, 255, 255, i);
                        yield return null;
                    }
                }
            }
            public ItemType GetItemType()
            {
                return cardProperties.itemType;
            }
            public CharacterType GetCharacterType()
            {
                return cardProperties.characterType;
            }
        }
    }
}