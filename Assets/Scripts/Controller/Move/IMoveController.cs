using DungeonRush.Cards;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush.Controller
{
    public interface IMoveController
    {
        bool IsRunning();
        void MakeMove();
        Sprite GetCardIcon();
        void Stop();
        void Notify();
        void Run();
    }
}