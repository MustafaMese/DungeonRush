﻿using DungeonRush.Cards;
using DungeonRush.Data;
using DungeonRush.Property;
using UnityEngine;

namespace DungeonRush.Attacking
{
    [CreateAssetMenu(menuName = "Attack/OneBoxAttack", order = 1)]
    public class OneBoxAttacking : AttackStyle
    {
        public override void Attack(Move move, int damage)
        {
            Card targetCard = move.GetTargetTile().GetCard();
            if(targetCard != null)
                targetCard.DecreaseHealth(damage);
        }

        public override void SetEffectPosition(GameObject effect, Vector3 tPos, Transform card)
        {
            effect.transform.position = tPos;
            effect.transform.SetParent(card);
        }
    }
}