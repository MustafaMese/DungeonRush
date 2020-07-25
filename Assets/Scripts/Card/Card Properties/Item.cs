using DungeonRush.Cards;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush.Property
{
    public class Item : MonoBehaviour
    {
        [SerializeField] SpriteRenderer itemSpriteRenderer = null;

        public ItemType type;
        public int power = 0;
        public Sprite itemSprite = null;

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

        public int GetPower()
        {
            return power;
        }
    }

    public enum ItemType
    {
        WEAPON,
        POTION,
        POSION,
        ARMOR,
        NONE = -1
    }
}