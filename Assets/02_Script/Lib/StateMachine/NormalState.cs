using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NormalState : State
{
    protected NormalState(CharacterObject owner, StateMachine stateMachine) : base(owner, stateMachine)
    {
    }

    public override IEnumerator Enter_Coroutine()
    {
        yield return null;
    }
}
