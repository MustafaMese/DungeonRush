using DungeonRush.Events;
using UnityEngine;

namespace DungeonRush.Cards
{
    public abstract class EventCard : Card, IGameEvent
    {
        [SerializeField] protected MoveEventType eventType;

        public abstract void GetEvent(Card card);
        public MoveEventType GetEventType()
        {
            return eventType;    
        }
    }
}
