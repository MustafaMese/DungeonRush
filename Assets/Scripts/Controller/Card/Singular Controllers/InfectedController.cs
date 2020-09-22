using System.Collections.Generic;
using DungeonRush.Cards;
using DungeonRush.Field;
using DungeonRush.Managers;
using UnityEngine;

namespace DungeonRush.Controller
{
    public class InfectedController : AIController
    {
        protected override void Initialize()
        {
            base.Initialize();
            exclamation.SetActive(false);
        }
    }
}