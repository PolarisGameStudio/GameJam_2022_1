using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterIdleState : NormalState
{
   // private AnimationAbility _animationAbility;
    private SpriteAnimationAbility _animationAbility;
    private PlayerDetectAbility _playerDetectAbility;
    private MonsterAttackAbility _monsterAttackAbility;

    public MonsterIdleState(CharacterObject owner, StateMachine stateMachine) : base(owner, stateMachine)
    {
    }

    public override void Init()
    {
        base.Init();

        _animationAbility = _owner.GetAbility<SpriteAnimationAbility>();
        //_playerDetectAbility = _owner.GetAbility<PlayerDetectAbility>();
        //_monsterAttackAbility = _owner.GetAbility<MonsterAttackAbility>();
    }
    
    public override void Enter()
    {
        // if (!_playerDetectAbility.HaveTarget)
        // {
        //     _owner.GetAbility<FSMAbility>().ChangeState(Enum_MonsterStateType.Run);
        //     return;
        // }
        //
        // if (_monsterAttackAbility.IsAttackPossible && _owner.IsAlive)
        // {
        //     _owner.GetAbility<FSMAbility>().ChangeState(Enum_MonsterStateType.Attack);
        //     return;
        // }
        //
        //_animationAbility.PlayAnimation("idle", true);
        
      //  _animationAbility.PlayMoveAnimation();
    }

    public override void LogicUpdate(float deltaTime)
    {
        // if (!_playerDetectAbility.HaveTarget)
        // {
        //     _owner.GetAbility<FSMAbility>().ChangeState(Enum_MonsterStateType.Run);
        //     return;
        // }
        //
        // if (_monsterAttackAbility.IsAttackPossible)
        // {
        //     _owner.GetAbility<FSMAbility>().ChangeState(Enum_MonsterStateType.Attack);
        //     return;
        // }
    }

    public override void Exit()
    {
    }
}
