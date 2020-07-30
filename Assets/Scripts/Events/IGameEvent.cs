using DungeonRush.Cards;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameEvent
{
    void GetEvent(Card card);
    EventType GetEventType();
}
