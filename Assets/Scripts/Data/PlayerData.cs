using System;
using System.Collections.Generic;
using DungeonRush.Cards;
using DungeonRush.Items;
using DungeonRush.Property;
using DungeonRush.Skills;
using UnityEngine;

//[System.Serializable]
[CreateAssetMenu(menuName = "ScritableObject/SaveItem")]
public class PlayerData : ScriptableObject
{
    public int maxHealth;
    public int currentHealth;
    public int gold;
    public int xp;

    public int criticChance;
    public int dodgeChance;
    public int lifeCount;
    public int moveCount;
    public int lootChance;

    public List<string> uniqueItemIDs;
    public List<string> uniqueSkillIDs;

    // public PlayerData(PlayerCard player)
    // {
    //     maxHealth = player.GetMaxHealth(); 
    //     currentHealth = player.GetHealth();
    //     gold = player.Gold;
    //     xp = player.Experience;

    //     criticChance = player.GetStats().CriticChance;
    //     dodgeChance = player.GetStats().DodgeChance;
    //     lifeCount = player.GetStats().LifeCount;
    //     moveCount = player.GetStats().TotalMoveCount;
    //     lootChance = player.GetStats().LootChance;

    //     uniqueItemIDs = player.GetComponent<ItemUser>().GetItemsIDs().ToArray();
    //     uniqueSkillIDs = player.GetComponent<SkillUser>().GetSkillIDs().ToArray();

    // }

    public void Save(PlayerCard player)
    {
        maxHealth = player.GetMaxHealth();
        currentHealth = player.GetHealth();
        gold = player.Gold;
        xp = player.Experience;

        criticChance = player.GetStats().CriticChance;
        dodgeChance = player.GetStats().DodgeChance;
        lifeCount = player.GetStats().LifeCount;
        moveCount = player.GetStats().TotalMoveCount;
        lootChance = player.GetStats().LootChance;

        uniqueItemIDs = player.GetComponent<ItemUser>().GetItemsIDs();
        uniqueSkillIDs = player.GetComponent<SkillUser>().GetSkillIDs();
    }

    public void Load(PlayerCard player)
    {
        player.SetMaxHealth(maxHealth);
        player.SetCurrentHealth(currentHealth);
        player.GetComponent<Health>().InitializeBar();
        player.Gold = gold;
        player.Experience = xp;
        player.GetStats().CriticChance = criticChance;
        player.GetStats().DodgeChance = dodgeChance;
        player.GetStats().LifeCount = lifeCount;
        player.GetStats().TotalMoveCount = moveCount;
        player.GetStats().LootChance = lootChance;

        for (int i = 0; i < uniqueItemIDs.Count; i++)
        {
            Item item = ItemDB.Instance.GetItem(uniqueItemIDs[i]);
            player.GetComponent<ItemUser>().TakeItem(item, false);
        }
        for (int i = 0; i < uniqueSkillIDs.Count; i++)
        {
            SkillObject skillObject = ItemDB.Instance.GetSkill(uniqueSkillIDs[i]);
        
            player.GetComponent<SkillUser>().AddSkill(skillObject, false);
        }
    }

    public void Reset()
    {
        maxHealth = 0;
        currentHealth = 0;
        gold = 0;
        xp = 0;
        criticChance = 0;
        dodgeChance = 0;
        lifeCount = 0;
        moveCount = 0;
        lootChance = 0;
        uniqueItemIDs.Clear();
        uniqueSkillIDs.Clear();
    }

    
}
