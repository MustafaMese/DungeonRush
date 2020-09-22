using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("ScriptableObjects/ActionScheme/MAA"))]
public class MAA : ActionState
{
    public override State ChangeState(State state, GameObject exclamation)
    {
        Debug.Log(state);

        switch (state)
        {
            case State.NONE:
                exclamation.SetActive(true);
                return State.ATTACK;
            case State.MOVE:
                exclamation.SetActive(true);
                return State.ATTACK;
            case State.ATTACK:
                exclamation.SetActive(true);
                return State.ATTACK2;
            case State.ATTACK2:
                exclamation.SetActive(false);
                return State.MOVE;
            default:
                return State.NONE;
        }
    }
}
