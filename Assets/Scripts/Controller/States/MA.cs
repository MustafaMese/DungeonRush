using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ActionScheme/MA")]
public class MA : ActionState
{
    public override State ChangeState(State state, GameObject exclamation)
    {
        switch (state)
        {
            case State.NONE:
                exclamation.SetActive(true);
                return State.ATTACK;
            case State.ATTACK:
                exclamation.SetActive(false);
                return State.MOVE;
            case State.MOVE:
                exclamation.SetActive(true);
                return State.ATTACK;
            default:
                return State.NONE;
        }
    }
}
