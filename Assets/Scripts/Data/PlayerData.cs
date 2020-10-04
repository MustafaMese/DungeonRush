using DungeonRush.Cards;
using DungeonRush.Items;
using DungeonRush.Property;
using DungeonRush.Skills;
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

    public string[] uniqueItemIDs;
    public string[] uniqueSkillIDs;

    public PlayerData(PlayerCard player)
    {
        maxHealth = player.GetMaxHealth(); 
        currentHealth = player.GetHealth();
        gold = player.Coins;
        xp = player.Experience;

        uniqueItemIDs = player.GetComponent<ItemUser>().GetItemsIDs().ToArray();
        uniqueSkillIDs = player.GetComponent<SkillUser>().GetSkillIDs().ToArray();
        foreach (var item in uniqueSkillIDs)
        {
            Debug.Log(item);
        }
    }
}
