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
        protected override void Stop()
        {
            isRunning = false;
            card.GetMove().Reset();
        }

    }
}
