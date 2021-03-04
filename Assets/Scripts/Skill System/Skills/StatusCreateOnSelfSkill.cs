using DungeonRush.Cards;
using DungeonRush.Data;
using DungeonRush.Traits;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DungeonRush.Skills
{
    public class StatusCreateOnSelfSkill : PassiveSkill
    {
        [SerializeField] StatusObject status;

        private StatusController statusController;

        public override void Initialize(Card card)
        {
            base.Initialize(card);
            statusController = card.GetComponent<StatusController>();
        }

        public override void Execute(Move move)
        {
            Card card = move.GetCard();
            if (card != null)
                statusController.AddStatus(status);
        }
    }
}