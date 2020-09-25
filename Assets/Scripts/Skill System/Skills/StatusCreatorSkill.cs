﻿using DungeonRush.Cards;
using DungeonRush.Data;
using DungeonRush.Skills;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Skill/StatusCreator")]
public class StatusCreatorSkill : Skill
{
    [SerializeField] Status status;

    public override void Execute(Move move)
    {
        Card target = move.GetTargetTile().GetCard();

        if (target != null)
            target.GetComponent<StatusController>().AddStatus(status);

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
