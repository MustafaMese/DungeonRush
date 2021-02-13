using DungeonRush.Cards;
using DungeonRush.Data;
using DungeonRush.Skills;
using DungeonRush.Traits;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush.Skills
{
    public class StatIncreaserSkill : OneShotSkill
    {
        [SerializeField] StatType statType;

        public override void Execute(Move move)
        {
            Card card = move.GetCard();

            switch (statType)
            {
                case StatType.MAXIMUM_HEALTH:
                    card.GetStats().MaximumHealth += 10;
                    break;
                case StatType.CRITIC_CHANCE:
                    card.GetStats().CriticChance++;
                    break;
                case StatType.DODGE_CHANCE:
                    card.GetStats().DodgeChance++;
                    break;
                case StatType.LIFE_COUNT:
                    card.GetStats().LifeCount++;
                    break;
                case StatType.MOVE_COUNT:
                    card.GetStats().TotalMoveCount++;
                    break;
                case StatType.LOOT_CHANCE:
                    card.GetStats().LootChance++;
                    break;
                default:
                    break;
            }
        }
    }
}
