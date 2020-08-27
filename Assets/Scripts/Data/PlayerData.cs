using DungeonRush.Cards;
using DungeonRush.Items;
using DungeonRush.Property;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int maxHealth;
    public int currentHealth;
    public int gold;
    public int xp;

    public int[] uniqueIDs;

    public PlayerData(PlayerCard player)
    {
        maxHealth = player.GetMaxHealth(); 
        currentHealth = player.GetHealth();
        gold = player.Coins;
        xp = player.Experience;

        uniqueIDs = player.GetComponent<ItemUser>().GetItemsIDs().ToArray();
    }
}
