using System.Collections;
using System.Collections.Generic;
using DungeonRush.Data;
using UnityEngine;

namespace DungeonRush.Attacking
{
    [CreateAssetMenu(menuName = "Attack/OneBoxAttack", order = 1)]
    public class OneBoxAttacking : AttackStyle
    {
        public override void Attack(Move move, int damage)
        {
            move.GetTargetTile().GetCard().DecreaseHealth(damage);
        }
    }
}
