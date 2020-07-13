using DungeonRush.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush.Attacking
{
    public abstract class AttackStyle : ScriptableObject
    {
        public abstract void Attack(Move move, int damage);
    }
}
