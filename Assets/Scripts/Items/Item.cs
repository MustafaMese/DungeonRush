using DungeonRush.Attacking;
using DungeonRush.Cards;
using DungeonRush.Property;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush.Items
{
    public class Item : MonoBehaviour
    {
        [SerializeField] SpriteRenderer itemSpriteRenderer = null;
        [SerializeField] CardProperties properties = null;
        [SerializeField] ItemType type;
        [SerializeField] int power = 0;
        [SerializeField] Sprite itemSmallSprite = null;
        [SerializeField] Sprite itemBigSprite = null;
        [SerializeField] int id = 0;

        [Header("For weapons")]
        [SerializeField] AttackStyle attackStyle;

        public string GetItemName()
        {
            return properties.cardName;
        }

        public AttackStyle GetAttackStyle()
        {
            return attackStyle;
        }

        public Sprite GetSmallSprite()
        {
            return itemSmallSprite;
        }

        public Sprite GetBigSprite()
        {
            return itemBigSprite;
        }

        public int GetId()
        {
            return id;
        }

        public SpriteRenderer GetRenderer()
        {
            return itemSpriteRenderer;
        }

        public ItemType GetItemType()
        {
            return type;
        }

        public int GetPower()
        {
            return power;
        }
    }

    public enum ItemType
    {
        WEAPON,
        POTION,
        POISON,
        ARMOR,
        MAX_HEALTH_INCREASER,
        COIN,
        NONE = -1
    }
}