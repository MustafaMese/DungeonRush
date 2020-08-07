using DungeonRush.Cards;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush.Items
{
    public class Item : MonoBehaviour
    {
        [SerializeField] SpriteRenderer itemSpriteRenderer = null;

        [SerializeField] ItemType type;
        [SerializeField] int power = 0;
        public Sprite itemSprite = null;
        [SerializeField] int id = 0;

        public int GetId()
        {
            return id;
        }

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
        POISON,
        ARMOR,
        MAX_HEALTH_INCREASER,
        NONE = -1
    }
}