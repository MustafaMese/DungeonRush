using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush.Controller
{
    public interface ICardController
    {
        void InitProcessHandlers();
        void Run();
        void Begin();
    }
}
