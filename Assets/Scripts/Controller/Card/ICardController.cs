using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush.Controller
{
    public interface ICardController
    {
        void InitProcessHandlers();
        bool IsRunning();
        void Run();
        void Stop();
        void Begin();
    }
}
