using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionState : ScriptableObject
{
    public abstract State ChangeState(State state, GameObject exclamation);
}
