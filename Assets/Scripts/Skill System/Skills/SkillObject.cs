using System;
using DungeonRush.Skills;
using UnityEngine;

namespace DungeonRush.Skills
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Skill Object")]
    public class SkillObject : ScriptableObject
    {
        public Skill skillPrefab;
        public string ID = Guid.NewGuid().ToString("N");

        public Skill Create(Transform t)
        {
            Skill s = Instantiate(skillPrefab, t);
            return s;
        }

        public string GetID()
        {
            return ID;
        }
    }
}
