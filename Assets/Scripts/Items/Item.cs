using DungeonRush.Attacking;
using DungeonRush.Cards;
using DungeonRush.Property;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace DungeonRush.Items
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Item")]
    public class Item : ScriptableObject
    {
        [SerializeField] CardProperties properties = null;
        [SerializeField] ItemType type;
        [SerializeField] int power = 0;
        [SerializeField] Sprite itemSmallSprite = null;
        [SerializeField] Sprite itemBigSprite = null;

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

        public string GetId()
        {
            return properties.cardName;
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