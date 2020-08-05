using DungeonRush.Cards;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush.Controller
{
    public interface IMoveController
    {
        void MakeMove();
        Card GetCard();
    }
}