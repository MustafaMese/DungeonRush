using DungeonRush.Cards;
using DungeonRush.Items;
using DungeonRush.Skills;
using UnityEngine;

[System.Serializable]
public class PlayerData
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

    public string[] uniqueItemIDs;
    public string[] uniqueSkillIDs;

    public PlayerData(PlayerCard player)
    {
        maxHealth = player.GetMaxHealth(); 
        currentHealth = player.GetHealth();
        gold = player.Coins;
        xp = player.Experience;

        criticChance = player.CriticChance;
        dodgeChance = player.DodgeChance;
        lifeCount = player.LifeCount;
        moveCount = player.TotalMoveCount;
        lootChance = player.LootChance;

        Debug.Log("1");

        uniqueItemIDs = player.GetComponent<ItemUser>().GetItemsIDs().ToArray();
        uniqueSkillIDs = player.GetComponent<SkillUser>().GetSkillIDs().ToArray();

        Debug.Log("2");
    }
}
