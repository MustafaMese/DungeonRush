using DungeonRush.Data;
using DungeonRush.Property;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush.Attacking
{
    public abstract class AttackStyle : ScriptableObject
    {
        [SerializeField] protected GameObject effectObject;
        [SerializeField] protected float animationTime;

        public abstract void Attack(Move move, int damage);
        public abstract void SetEffectPosition(GameObject effect, Vector3 tPos, Transform card = null);
        public GameObject GetEffect()
        {
            return effectObject;
        }

        public float GetAnimationTime()
        {
            return animationTime;
        }
    }
}
