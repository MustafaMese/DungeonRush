using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush.Data
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Card/Stat")]
    public class Stats : ScriptableObject
    {
        public int maximumHealth;
        public int criticChance;
        public int dodgeChance;
        public int lifeCount;
        public int moveCount;
        public int lootChance;
    }
}
