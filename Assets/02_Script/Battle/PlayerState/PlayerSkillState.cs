using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// todo: 스킬 애니메이션 있으면 State상태에서 애니메이션 실행 후 Idle로 (임모탈)
// todo: 없으면 State없이 PlayerSkillAbility 자체에서 쿨타임 체크 후 자동사용  (창술사)
public class PlayerSkillState : CoroutineState
{
    private PlayerSkillAbility _playerSkillAbility;
    private AnimationAbility _animationAbility;

    private const string SkillAnimation = "attack";

    public PlayerSkillState(CharacterObject owner, StateMachine stateMachine) : base(owner, stateMachine)
    {
    }

    public override void Init()
    {
        base.Init();

        _playerSkillAbility = _owner.GetAbility<PlayerSkillAbility>();
        _animationAbility = _owner.GetAbility<AnimationAbility>();
    }

    
    // todo: 임시 , 아무것도 안하는 상태
    public override IEnumerator Enter_Coroutine()
    {
        yield return new WaitForSeconds(0.1f);
    }

    public override void LogicUpdate(float deltaTime)
    {
    }
}