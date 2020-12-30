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
                    card.MaximumHealth += 10;
                    break;
                case StatType.CRITIC_CHANCE:
                    card.CriticChance++;
                    break;
                case StatType.DODGE_CHANCE:
                    card.DodgeChance++;
                    break;
                case StatType.LIFE_COUNT:
                    card.LifeCount++;
                    break;
                case StatType.MOVE_COUNT:
                    card.TotalMoveCount++;
                    break;
                case StatType.LOOT_CHANCE:
                    card.LootChance++;
                    break;
                default:
                    break;
            }
        }
    }
}
