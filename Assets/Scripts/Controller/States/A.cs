using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush.States
{
    [CreateAssetMenu(menuName = "ScriptableObjects/ActionScheme/A")]
    public class A : ActionState
    {
        public override State ChangeState(State state, GameObject exclamation)
        {
            return State.ATTACK;
        }
    }
}