using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush.Traits
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Card/Stat")]
    public class StatData : ScriptableObject
    {
        public int maximumHealth;
        public int criticChance;
        public int dodgeChance;
        public int lifeCount;
        public int moveCount;
        public int lootChance;
    }

    public enum StatType
    {
        MAXIMUM_HEALTH,
        CRITIC_CHANCE,
        DODGE_CHANCE,
        LIFE_COUNT,
        MOVE_COUNT,
        LOOT_CHANCE
    }
}
