using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    protected CharacterObject _owner;
    
    protected StateMachine _stateMachine;

    private bool _inited = false;
    public bool Inited => _inited;
    
    protected State(CharacterObject owner, StateMachine stateMachine)
    {
        _owner = owner;

        _stateMachine = stateMachine;
    }

    public virtual void Init()
    {
        _inited = true;
    }

    public abstract void Enter();

    public abstract IEnumerator Enter_Coroutine();

    public abstract void LogicUpdate(float deltaTime);
    public abstract void Exit();
}