using System.Collections.Generic;
using DungeonRush.Cards;
using DungeonRush.Data;
using DungeonRush.Field;
using DungeonRush.Managers;
using UnityEngine;

namespace DungeonRush.Attacking
{
    [CreateAssetMenu(menuName = "Attack/ThornTrapAttack")]
    public class ThornTrapAttacking : AttackStyle
    {
        public override void Attack(Move move, int damage)
        {
            Card targetCard = move.GetCardTile().GetCard();
            if (targetCard != null)
                targetCard.DecreaseHealth(damage);
        }

        public override void SetEffectPosition(GameObject effect, Vector3 tPos, Transform card = null)
        {
            effect.transform.position = tPos;
        }
    }
}
