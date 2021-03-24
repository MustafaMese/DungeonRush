using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerProperties
{
    public int str;
    public int agi;
    public int luck;

    public PlayerProperties(int str, int agi, int luck)
    {
        this.str = str;
        this.agi = agi;
        this.luck = luck;
    }

    public static void CalculateStr(int str, out int maxHealth, out int damage)
    {
        maxHealth = 10 + str * 3;
        damage = 2 + str * 1;
    }

    public static void CalculateAgi(int agi, out int critic, out int dodge)
    {
        critic = agi * 3;
        dodge = agi * 3;
    }

    public static void CalculateLuck(int luck, out int lootChance)
    {
        lootChance = luck * 5;
    }
}
