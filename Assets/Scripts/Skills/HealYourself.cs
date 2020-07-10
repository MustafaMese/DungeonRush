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
            Card card = move.GetCard();
            card.IncreaseHealth(healPower);

            if (healAnimationPrefab.prefab == null)
                healAnimationPrefab.InitializeObject(effectTime, card.transform.position, card.transform);
            else
                healAnimationPrefab.EnableObject(effectTime, card.transform.position);
        }

    }
}
