using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 버서커 애니메이션 있다고 가정하고
/// 버서커 스킬 사용했을 때
/// BerserkEnterState 전환 -> 애니메이션 실행 -> IdleState or BerserkIdle State 로 전환시킬 State
/// 버서커 애니메이션이 없다면 해당 State 제거하고 상태변환없이 파티클, 스켈레톤 스킨만 변경
/// </summary>
public class Tool_PlayerBerserkEnterState : CoroutineState
{
    private const string _berserkEnterAnimationName = "death";

    private AnimationAbility _animationAbility;
    private BerserkAbility _berserkAbility;

    public override void Init()
    {
        base.Init();

        _animationAbility = _owner.GetAbility<AnimationAbility>();
        _berserkAbility = _owner.GetAbility<BerserkAbility>();
    }

    public Tool_PlayerBerserkEnterState(CharacterObject owner, StateMachine stateMachine) : base(owner, stateMachine)
    {
    }

    public override IEnumerator Enter_Coroutine()
    {
        var duration = _animationAbility.GetDuration(_berserkEnterAnimationName);
        _animationAbility.PlayAnimation(_berserkEnterAnimationName, false);

        float effectDelay = 1f;

        yield return new WaitForSecondsRealtime(effectDelay);

        _berserkAbility.On();

        yield return new WaitForSecondsRealtime(duration - effectDelay);
        
        _owner.GetAbility<FSMAbility>().ChangeState(Enum_MonsterStateType.Idle);
    }

    public override void LogicUpdate(float deltaTime)
    {
    }
}