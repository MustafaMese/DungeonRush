using System.Collections.Generic;
using DungeonRush.Cards;
using DungeonRush.Data;
using DungeonRush.Field;
using DungeonRush.Managers;
using DungeonRush.Traits;
using UnityEngine;

namespace DungeonRush.Attacking
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Attack/TrapAttack")]
    public class TrapAttacking : AttackStyle
    {
        public override void Attack(Move move, int damage)
        {
            Card targetCard = move.GetCardTile().GetCard();
            if (targetCard != null)
                targetCard.GetDamagable().DecreaseHealth(damage);
        }

        public override void SetEffectPosition(GameObject effect, Vector3 tPos, Transform card = null)
        {
            effect.transform.position = tPos;
        }

        public override bool Define(Card card, Swipe swipe)
        {
            ConfigureCardMove(card, card.GetTile());
            return card.GetTile().GetCard() != null;
        }
    }
}
