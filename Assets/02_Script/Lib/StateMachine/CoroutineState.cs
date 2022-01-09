using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CoroutineState : State
{
    protected CoroutineState(CharacterObject owner, StateMachine stateMachine) : base(owner, stateMachine)
    {
    }

    public override void Enter()
    {
        
    }

    public override void Exit()
    {
        
    }
}
