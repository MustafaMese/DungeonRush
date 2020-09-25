using DungeonRush.Cards;
using DungeonRush.Data;
using DungeonRush.Property;
using DungeonRush.Skills;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Skill/DoubleAttack")]
public class DoubleAttack : Skill
{
    public override void Execute(Move move)
    {
        Card card = move.GetCard();

        if (card != null)
            card.GetComponent<Attacker>().AttackAction(move);
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
