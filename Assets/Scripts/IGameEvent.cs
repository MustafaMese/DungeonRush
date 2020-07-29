using DungeonRush.Property;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameEvent
{
    Item GetItem();
    EventType GetEventType();
}
