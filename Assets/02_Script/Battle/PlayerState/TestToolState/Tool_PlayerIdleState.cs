using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// todo: 스킬툴 공격연출을 위한 땜빵용
public class Tool_PlayerIdleState : CoroutineState
{ 
    private AnimationAbility _animationAbility;
    
    public Tool_PlayerIdleState(CharacterObject owner, StateMachine stateMachine) : base(owner, stateMachine)
    {
    }

    public override void Init()
    {
        base.Init();

        _animationAbility = _owner.GetAbility<AnimationAbility>();
    }

    public override void Enter()
    {
        //_animationAbility.PlayAnimation("idle", true);
    }

    public override IEnumerator Enter_Coroutine()
    {
        //_animationAbility.PlayAnimation("idle", true);

        yield return new WaitForSeconds(2f);
        
        //_owner.GetAbility<FSMAbility>().ChangeState(Enum_PlayerStateType.Run);
    }

    public override void LogicUpdate(float deltaTime)
    {
        //Debug.LogError("아응");
    }

    public override void Exit()
    {
    }
}
