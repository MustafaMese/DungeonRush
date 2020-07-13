using System.Collections;
using System.Collections.Generic;
using DungeonRush.Cards;
using UnityEngine;

namespace DungeonRush.Controller
{
    public class TrapController : MonoBehaviour, ICardController
    {
        public static List<AIController> subscribedTraps = new List<AIController>();

        public void Begin()
        {
            throw new System.NotImplementedException();
        }

        public void InitProcessHandlers()
        {
            throw new System.NotImplementedException();
        }

        public bool IsRunning()
        {
            throw new System.NotImplementedException();
        }

        public void Run()
        {
            throw new System.NotImplementedException();
        }

        public void Stop()
        {
            throw new System.NotImplementedException();
        }

        public static void UnsubscribeCard(AIController controller)
        {
            subscribedTraps.Remove(controller);
        }
    }
}
