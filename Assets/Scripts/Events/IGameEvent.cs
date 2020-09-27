using DungeonRush.Cards;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush.Events
{
    public interface IGameEvent
    {
        void GetEvent(Card card);
        MoveEventType GetEventType();
    }
}