using System.Collections;
using System.Collections.Generic;
using DungeonRush.Property;
using DungeonRush.Managers;
using UnityEngine;

namespace DungeonRush.Cards
{
    public abstract class EventCard : Card, IGameEvent
    {
        [SerializeField] protected EventType eventType;

        public abstract void GetEvent(Card card);
        public EventType GetEventType()
        {
            return eventType;    
        }
    }
}
