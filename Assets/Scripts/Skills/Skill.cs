using DungeonRush.Cards;
using DungeonRush.Data;
using UnityEngine;

namespace DungeonRush.Skills
{
    public abstract class Skill : ScriptableObject
    {
        public bool isMover;
        public bool isAttacker;
        [SerializeField] protected int cooldown;
        [SerializeField] protected float effectTime;

        public abstract void Execute(Move move);
        public abstract void DisableObject();

        public int GetCooldown()
        {
            return cooldown;
        }

        public float GetEffectTime()
        {
            return effectTime;
        }
    }
}
