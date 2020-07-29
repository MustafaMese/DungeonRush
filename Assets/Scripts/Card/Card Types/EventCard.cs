using System.Collections;
using System.Collections.Generic;
using DungeonRush.Property;
using UnityEngine;

namespace DungeonRush.Cards
{
    public class EventCard : Card, IAcquirable
    {
        [SerializeField] ItemCard item;

        public Item GetAcquirable()
        {
            return item.GetComponent<Item>();
        }
    }
}
