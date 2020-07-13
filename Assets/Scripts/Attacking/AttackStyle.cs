using DungeonRush.Data;
using DungeonRush.Effects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush.Attacking
{
    public abstract class AttackStyle : ScriptableObject
    {
        [SerializeField] protected EffectObject effectPrefab;

        public abstract void Attack(Move move, int damage);
    }
}
