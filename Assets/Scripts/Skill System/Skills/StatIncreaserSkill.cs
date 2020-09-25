using DungeonRush.Cards;
using DungeonRush.Data;
using DungeonRush.Skills;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Skill/StatIncreaser")]
public class StatIncreaserSkill : Skill
{
    [SerializeField] StatType statType;

    public override void Execute(Move move)
    {
        Card card = move.GetCard();
      
        switch (statType)
        {
            case StatType.MAXIMUM_HEALTH:
                card.MaximumHealth++;
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


    public override void PositionEffect(GameObject effect, Move move)
    {
        return;
    }

    public override Vector3 PositionTextPopup(GameObject textPopup, Move move)
    {
        return Vector3.zero;
    }
}
