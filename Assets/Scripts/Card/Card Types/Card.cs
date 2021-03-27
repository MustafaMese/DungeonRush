using DungeonRush.Attacking;
using DungeonRush.Controller;
using DungeonRush.Data;
using DungeonRush.Field;
using DungeonRush.Property;
using DungeonRush.Shifting;
using DungeonRush.Traits;
using System.Collections;
using TMPro;
using UnityEngine;

namespace DungeonRush
{
    namespace Cards
    {
        public abstract class Card : MonoBehaviour
        {
            protected Tile tile = null;
            
            protected Move move;
            protected string cardName;

            [Header("General Properties")]
            [SerializeField] protected CardProperties cardProperties = null;
            [SerializeField] Character characterType;

            public void Start()
            {
                Initialize();
            }

            public virtual IMovable GetMovable() { return null; }
            public virtual IFighter GetFighter() { return null; }
            public virtual IDamagable GetDamagable() { return null; }
            public virtual Stats GetStats() { return null; }
            public virtual IMoveController GetController() { return null; }
            public virtual StatusController GetStatusController() { return null; }
            public virtual Animator GetAnimator() { return null; }

            protected virtual void Initialize()
            {
                cardName = cardProperties.cardName;
            }
            
            public int GetLevel()
            {
                return cardProperties.level;
            }

            public Sprite GetCharacterIcon()
            {
                return cardProperties.characterIcon;
            }

            public string GetCardName()
            {
                return cardName;
            }

            public Tile GetTile()
            {
                return tile;
            }

            public CardType GetCardType()
            {
                return cardProperties.cardType;
            }

            public void SetTile(Tile coordinate)
            {
                this.tile = coordinate;
            }

            public void SetCardType(CardType type)
            {
                cardProperties.cardType = type;
            }

            public Character GetCharacterType()
            {
                return characterType;
            }

            public Move GetMove()
            {
                return move;
            }

            public void SetMove(Move move)
            {
                this.move = move;
            }
        }
    }
}
