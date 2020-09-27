using UnityEngine;

namespace DungeonRush.States
{
    public abstract class ActionState : ScriptableObject
    {
        public abstract State ChangeState(State state, GameObject exclamation);
    }
}