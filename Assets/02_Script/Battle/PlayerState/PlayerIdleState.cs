using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : NormalState
{
    public const string NormalIdleAnimationName = "";
    private AnimationAbility _animationAbility;
    private MonsterDetectAbility _monsterDetectAbility;
    private PlayerAttackAbility _playerAttackAbility;

    public PlayerIdleState(CharacterObject owner, StateMachine stateMachine) : base(owner, stateMachine)
    {
    }

    public override void Init()
    {
        base.Init();

        _animationAbility = _owner.GetAbility<AnimationAbility>();
        _monsterDetectAbility = _owner.GetAbility<MonsterDetectAbility>();
        _playerAttackAbility = _owner.GetAbility<PlayerAttackAbility>();
    }

    public override void Enter()
    {
        if (_monsterDetectAbility.HaveTarget && _playerAttackAbility.IsAttackPossible)
        {
            _owner.GetAbility<FSMAbility>().ChangeState(Enum_PlayerStateType.Attack);
            
            return;
        }

        if (!_monsterDetectAbility.HaveTarget)
        {
            _owner.GetAbility<FSMAbility>().ChangeState(Enum_PlayerStateType.Run);

            return;
        }
        
        _animationAbility.PlayAnimation(NormalIdleAnimationName, true);
    }
    

    public override void LogicUpdate(float deltaTime)
    {
        if (_monsterDetectAbility.HaveTarget && _playerAttackAbility.IsAttackPossible)
        {
            _owner.GetAbility<FSMAbility>().ChangeState(Enum_PlayerStateType.Attack);
            
            return; 
        }

        if (!_monsterDetectAbility.HaveTarget)
        {
            _owner.GetAbility<FSMAbility>().ChangeState(Enum_PlayerStateType.Run);
        }
    }

    public override void Exit()
    {
    }
}
