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

    public int[] uniqueIDs;

    public PlayerData(Card player)
    {
        maxHealth = player.GetMaxHealth(); 
        currentHealth = player.GetHealth();

        uniqueIDs = player.GetComponent<ItemUser>().GetItemsIDs().ToArray();
    }
}
