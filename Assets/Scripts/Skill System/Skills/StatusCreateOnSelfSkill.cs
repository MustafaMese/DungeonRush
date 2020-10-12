using DungeonRush.Cards;
using DungeonRush.Data;
using DungeonRush.Traits;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DungeonRush.Skills
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Skill/StatusCreateOnSelfSkill")]
    public class StatusCreateOnSelfSkill : Skill
    {
        [SerializeField] Status status;

        public override void Execute(Move move)
        {
            Card card = move.GetCard();
            if (card != null)
                card.GetComponent<StatusController>().AddStatus(status);
        }

        public override void PositionEffect(GameObject effect, Move move)
        {
            effect.transform.position = move.GetCard().transform.position;
            effect.transform.SetParent(move.GetCard().transform);
        }

        public override Vector3 PositionTextPopup(GameObject textPopup, Move move)
        {
            return Vector3.zero;
        }
    }
}