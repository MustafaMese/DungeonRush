using System.Collections;
using System.Collections.Generic;
using DungeonRush.Cards;
using DungeonRush.Data;
using UnityEngine;
namespace DungeonRush.Skills
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Skill/InstantKill")]
    public class InstantKill : Skill
    {
        public override void Execute(Move move)
        {
            Card card = move.GetTargetTile().GetCard();
            if (card != null)
                card.DecreaseHealth(10000);
        }

        public override void PositionEffect(GameObject effect, Move move)
        {
            Transform t = move.GetCard().transform;
            effect.transform.SetParent(t);
            effect.transform.position = t.position;
        }

        public override Vector3 PositionTextPopup(GameObject textPopup, Move move)
        {
            throw new System.NotImplementedException();
        }
    }
}