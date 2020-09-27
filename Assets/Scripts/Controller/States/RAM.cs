using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush.States
{
    [CreateAssetMenu(menuName = ("ScriptableObjects/ActionScheme/RAM"))]
    public class RAM : ActionState
    {
        public override State ChangeState(State state, GameObject exclamation)
        {
            switch (state)
            {
                case State.RANGE:
                    exclamation.SetActive(true);
                    Debug.Log("Nişan alıyorum amcık");
                    return State.RANGE_ATTACK;
                case State.RANGE_ATTACK:
                    exclamation.SetActive(false);
                    Debug.Log("Vurdum gitti ammına");
                    return State.MOVE;
                case State.MOVE:
                    exclamation.SetActive(false);
                    return State.RANGE;
                case State.NONE:
                    exclamation.SetActive(false);
                    return State.RANGE;
                default:
                    return State.NONE;
            }
        }
    }
}