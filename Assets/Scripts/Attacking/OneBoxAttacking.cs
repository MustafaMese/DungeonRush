using System.Collections;
using System.Collections.Generic;
using DungeonRush.Cards;
using DungeonRush.Data;
using UnityEngine;

namespace DungeonRush.Attacking
{
    [CreateAssetMenu(menuName = "Attack/OneBoxAttack", order = 1)]
    public class OneBoxAttacking : AttackStyle
    {
        [SerializeField] float effectTime;

        public override void Attack(Move move, int damage)
        {
            Card targetCard = move.GetTargetTile().GetCard();
            if(targetCard != null)
                targetCard.DecreaseHealth(damage);

            Transform card = move.GetCard().transform;
            Vector3 tPos = move.GetTargetTile().GetCoordinate();
            SetEffectPosition(tPos, card);
        }

        private void SetEffectPosition(Vector3 tPos, Transform card)
        {
            if (effectPrefab.prefab == null)
                effectPrefab.InitializeObject(effectTime, tPos, card, true);
            else
                effectPrefab.EnableObject(effectTime, tPos);
        }
    }
}
