using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush.Data
{
    [System.Serializable]
    public class PlayerUtility
    {
        private const int LEVEL_TRESHOLD = 7;
        
        public int totalXp;
        public int gold;

        public PlayerUtility(int xp, int gold)
        {
            this.totalXp = xp;
            this.gold = gold;
        }

        public int CalculateLevel()
        {
            return (int)((Mathf.Sqrt(1 + 8 * totalXp / LEVEL_TRESHOLD)) / 2);
        }
    }
}


