using System.Collections;
using System.Collections.Generic;
using DungeonRush.Cards;
using DungeonRush.Data;
using UnityEngine;

namespace DungeonRush.Attacking
{
    [CreateAssetMenu(menuName = "Attack/ThornTrapAttack")]
    public class ThornTrapAttacking : AttackStyle
    {
        [SerializeField] float effectTime;

        public override void Attack(Move move, int damage)
        {
            Card targetCard = move.GetCardTile().GetCard();
            if (targetCard != null)
                targetCard.DecreaseHealth(damage);

            Transform card = move.GetCard().transform;
            SetEffectPosition(targetCard.transform.position, card);
        }

        private void SetEffectPosition(Vector3 tPos, Transform card)
        {
            if (effectPrefab.prefab == null)
                effectPrefab.InitializeObject(effectTime, tPos, card.transform, true);
            else
                effectPrefab.EnableObject(effectTime, tPos);
        }
    }
}
