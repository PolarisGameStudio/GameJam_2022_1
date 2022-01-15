using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : NormalState
{
    public const string NormalRunAnimationName = "move";
    
    private AnimationAbility _animationAbility;
    private MovementAbility _movementAbility;
    private MonsterDetectAbility _monsterDetectAbility;
    private PlayerAttackAbility _playerAttackAbility;
    
    public PlayerRunState(CharacterObject owner, StateMachine stateMachine) : base(owner, stateMachine)
    {
    }

    public override void Init()
    {
        base.Init();

        _animationAbility = _owner.GetAbility<AnimationAbility>();
        _movementAbility  = _owner.GetAbility<MovementAbility>();
        _monsterDetectAbility = _owner.GetAbility<MonsterDetectAbility>();
        _playerAttackAbility = _owner.GetAbility<PlayerAttackAbility>();
    }

    public override void Enter()
    {
        float moveSpeed = (float)_owner.Stat[Enum_StatType.MoveSpeed];

        //float moveAnimationSpeed = Mathf.Min(moveSpeed, _animationAbility.MaxAnimationSpeedScale);

        if (_animationAbility.UseMovmentSpeed && moveSpeed > _animationAbility.StartMovementAnimationSpeed)
        {
            var diff = moveSpeed - _animationAbility.StartMovementAnimationSpeed;
            var moveScale = 1 + (diff * _animationAbility.MovementAnimationSpeedFactor);
            
            moveScale = Mathf.Min(moveScale, _animationAbility.MaxMovementAnimationSpeedScale);
            
            _animationAbility.PlayAnimation( NormalRunAnimationName, true, moveScale);
        }
        else
        {
            _animationAbility.PlayAnimation(NormalRunAnimationName, true, 1f);
        }
    }

    public override void LogicUpdate(float deltaTime)
    {
        if (_monsterDetectAbility.IsEnableAttack)
        {
            if (_playerAttackAbility.IsAttackPossible)
            {
                _owner.GetAbility<FSMAbility>().ChangeState(Enum_PlayerStateType.Attack);
            }
            else
            {
                _owner.GetAbility<FSMAbility>().ChangeState(Enum_PlayerStateType.Idle);
            }
            
            return;
        }
        
        _movementAbility.MoveToRight(deltaTime);
    }

    public override void Exit()
    {
    }
}
