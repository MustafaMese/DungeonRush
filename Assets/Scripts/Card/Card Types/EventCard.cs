using System.Collections;
using System.Collections.Generic;
using DungeonRush.Property;
using UnityEngine;

namespace DungeonRush.Cards
{
    public class EventCard : Card, IGameEvent
    {
        [SerializeField] ItemCard item = null;
        [SerializeField] EventType eventType;

        public Item GetItem()
        {
            return item.GetComponent<Item>();
        }

        public EventType GetEventType()
        {
            return eventType;
        }
    }
}
