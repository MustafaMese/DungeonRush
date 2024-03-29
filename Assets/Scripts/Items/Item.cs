﻿using DungeonRush.Cards;
using DungeonRush.Traits;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush.Items
{
    public abstract class Item : ScriptableObject, IItem
    {
        string ID = Guid.NewGuid().ToString("N");

        [SerializeField] string itemName;
        [SerializeField] ItemType type;
        [SerializeField] Sprite UISprite = null;
        [SerializeField] Sprite primarySprite = null;
        [SerializeField] bool purchased;
        [SerializeField] int price;

        public abstract int GetPower();
        public abstract void Execute(Card card);

        public virtual BoneType GetBoneType() { return BoneType.NONE; }
        public virtual Sprite GetSecondarySprite() { return null; }
        public virtual string GetExplanation() { return " "; }

        public bool Purchased { get => purchased; set => purchased = value; }
        public int Price { get => price; set => price = value; }

        public ItemType GetItemType()
        {
            return type;
        }

        public string GetName()
        {
            return itemName;
        }

        public Sprite GetPrimarySprite()
        {
            return primarySprite;
        }

        public Sprite GetUISprite()
        {
            return UISprite;
        }

        public string GetID()
        {
            return ID;
        }
    }
}

