using DungeonRush.Cards;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush.Property
{
    public class Item : MonoBehaviour
    {
        [SerializeField] SpriteRenderer itemSpriteRenderer;

        public ItemType type;
        public int damage;
        public Sprite itemSprite;

        public SpriteRenderer GetRenderer()
        {
            return itemSpriteRenderer;
        }

        public Sprite GetItem()
        {
            return itemSprite;
        }

        public ItemType GetItemType()
        {
            return type;
        }

        public int GetDamage()
        {
            return damage;
        }
    }

    public enum ItemType
    {
        WEAPON,
        POTION,
        POSION,
        NONE = -1
    }
}