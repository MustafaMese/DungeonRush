using System.Collections;
using System.Collections.Generic;
using DungeonRush.Cards;
using DungeonRush.Field;
using DungeonRush.Managers;
using UnityEngine;

namespace DungeonRush.Controller
{
    public class ThornTrapController : AIController
    {
        private TrapController trapController;

        protected override void Stop()
        {
            isRunning = false;
            card.GetMove().Reset();
        }

        protected override void ChangeState()
        {
            return;
        }

        protected override void ChooseController()
        {
            trapController = FindObjectOfType<TrapController>();
            TrapController.subscribedTraps.Add(this);
        }

        protected override void Notify()
        {
            trapController.OnNotify();
        }

        protected override Swipe SelectTileForSwipe(Card attacker)
        {
            isMoving = true;
            return Swipe.NONE;
        }
    }
}
