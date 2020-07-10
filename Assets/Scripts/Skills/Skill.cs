using DungeonRush.Cards;
using DungeonRush.Data;
using UnityEngine;

namespace DungeonRush.Skills
{
    public abstract class Skill : ScriptableObject
    {
        public bool isMover;
        public bool isAttacker;
        public int cooldown;

        public abstract void Initialize(Move move);

        public abstract void Execute(Move move);
    }
}
