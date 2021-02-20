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

        criticChance = player.GetStats().CriticChance;
        dodgeChance = player.GetStats().DodgeChance;
        lifeCount = player.GetStats().LifeCount;
        moveCount = player.GetStats().TotalMoveCount;
        lootChance = player.GetStats().LootChance;

        uniqueItemIDs = player.GetComponent<ItemUser>().GetItemsIDs().ToArray();
        uniqueSkillIDs = player.GetComponent<SkillUser>().GetSkillIDs().ToArray();

    }
}
