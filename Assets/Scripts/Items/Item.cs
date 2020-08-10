using DungeonRush.Attacking;
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
        [SerializeField] Sprite itemSprite = null;
        [SerializeField] int id = 0;
        [SerializeField] AttackStyle attackStyle;

        public AttackStyle GetAttackStyle()
        {
            return attackStyle;
        }

        public Sprite GetSprite()
        {
            return itemSprite;
        }

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