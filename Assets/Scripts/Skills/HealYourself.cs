using DungeonRush.Cards;
using DungeonRush.Data;
using DungeonRush.Effects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush.Skills
{
    [CreateAssetMenu(menuName = "Skill/Healing")]
    public class HealYourself : Skill
    {
        public int healPower = 2;
        public float effectTime = 0.3f;
        public EffectObject healAnimationPrefab;

        public override void Initialize(Move move)
        {
            
        }

        public override void Execute(Move move)
        {
            move.GetCard().IncreaseHealth(healPower);
            healAnimationPrefab.InitializeObject(effectTime, move.GetCard().transform);
            Debug.Log("Healladım seni");
        }

    }
}
