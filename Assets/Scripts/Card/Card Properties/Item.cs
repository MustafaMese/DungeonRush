using DungeonRush.Cards;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush.Property
{
    public class Item : MonoBehaviour
    {
        public ItemType type;
        public int damage;
        public Sprite sprite;

        public Sprite GetSprite()
        {
            return sprite;
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