using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMAbility : CharacterAbility
{
    private StateMachine _stateMachine;
    public StateMachine StateMachine => _stateMachine;
    
    
    private Dictionary<int, State> _states = new Dictionary<int, State>(8);
    
    
    public override void Init()
    {
        base.Init();
        
        _stateMachine = GetComponent<StateMachine>();

        if (_stateMachine == null)
        {
            _stateMachine = gameObject.AddComponent<StateMachine>();
        }
    }

    public void Register(Enum_PlayerStateType type, State state)
    {
        _states.Add((int) type, state);
    }
    public void Register(Enum_MonsterStateType type, State state)
    {
        _states.Add((int) type, state);
    }
    
    public void Initialize(Enum_PlayerStateType type)
    {
        _stateMachine.Initialize(_states[(int)type]);    
    }
    public void Initialize(Enum_MonsterStateType type)
    {
        _stateMachine.Initialize(_states[(int)type]);    
    }
    
    public void ChangeState(Enum_PlayerStateType type)
    {
        _stateMachine.ChangeState(_states[(int)type]);
    }
    public void ChangeState(Enum_MonsterStateType type)
    {
        _stateMachine.ChangeState(_states[(int)type]);
    }

    public override void ProcessAbility(float deltaTime)
    {
        _stateMachine.LogicUpdate(deltaTime);
    }
}
