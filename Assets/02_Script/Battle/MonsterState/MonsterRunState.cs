using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterRunState : NormalState
{
    private AnimationAbility _animationAbility;
    private MovementAbility _movementAbility;
    private PlayerDetectAbility _playerDetectAbility;
    
    public MonsterRunState(CharacterObject owner, StateMachine stateMachine) : base(owner, stateMachine)
    {
    }

    public override void Init()
    {
        base.Init();

        _animationAbility = _owner.GetAbility<AnimationAbility>();
        _movementAbility  = _owner.GetAbility<MovementAbility>();
        _playerDetectAbility = _owner.GetAbility<PlayerDetectAbility>();
    }

    public override void Enter()
    {
        _animationAbility.PlayAnimation("walk", true);
    }

    public override void LogicUpdate(float deltaTime)
    {
        if (_playerDetectAbility.HaveTarget)
        {
            _owner.GetAbility<FSMAbility>().ChangeState(Enum_MonsterStateType.Idle);
            return;
        }
        
        _movementAbility.MoveToLeft(deltaTime);
    }

    public override void Exit()
    {
    }
}
