using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush.States
{
    [CreateAssetMenu(menuName = "ScriptableObjects/ActionScheme/WAM")]
    public class WAM : ActionState
    {
        public override State ChangeState(State state, GameObject exclamation)
        {
            switch (state)
            {
                case State.NONE:
                    exclamation.SetActive(true);
                    return State.ATTACK;
                case State.WAIT:
                    return State.MOVE;
                case State.ATTACK:
                    exclamation.SetActive(false);
                    return State.WAIT;
                case State.MOVE:
                    exclamation.SetActive(true);
                    return State.ATTACK;
                default:
                    return State.NONE;
            }
        }
    }
}