using DungeonRush.Cards;
using UnityEngine;

namespace DungeonRush.Items
{
    public interface IItem
    {
        string GetName();
        Sprite GetPrimarySprite();
        Sprite GetUISprite();
        string GetID();
        ItemType GetItemType();
        int GetPower();
        void Execute(Card card);
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