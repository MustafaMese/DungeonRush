using DungeonRush.Customization;
using DungeonRush.Events;
using UnityEngine;

namespace DungeonRush.Cards
{
    public abstract class EventCard : Card, IGameEvent
    {
        [SerializeField] protected MoveEventType eventType;

        private ICustomization customization;

        protected override void Initialize()
        {
            customization = GetComponent<ICustomization>();

            if (transform.position.y < 0)
                customization.ChangeLayer(false, (int)transform.position.y);
            else if (transform.position.y > 0)
                customization.ChangeLayer(true, (int)transform.position.y);
        }

        public abstract void GetEvent(Card card);
        public MoveEventType GetEventType()
        {
            return eventType;    
        }
    }
}
