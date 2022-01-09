using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Tool_PlayerSkillState : CoroutineState
{
    private AnimationAbility _animationAbility;
    private ToolAbility _toolAbility;
    private MonsterDetectAbility _detectAbility;

    public Tool_PlayerSkillState(CharacterObject owner, StateMachine stateMachine) : base(owner, stateMachine)
    {
    }

    public override void Init()
    {
        base.Init();

        _animationAbility = _owner.GetAbility<AnimationAbility>();
        _toolAbility = _owner.GetAbility<ToolAbility>();
        _detectAbility = _owner.GetAbility<MonsterDetectAbility>();
    }

    public override void Enter()
    {
    }
    
    public override IEnumerator Enter_Coroutine()
    {
       //  var attackSpeed = (float)_owner.Stat[Enum_StatType.AttackSpeed];
       //  
       //  var skill = Tool_SkillManager.Instance.GetShadowSkill(_toolAbility.SkillPresetIndex);
       //
       // // var animationName = skill.GetAnimationName();
       //
       //  float duration = 0;
       //
       //  if (!string.IsNullOrEmpty(animationName))
       //  {
       //      _animationAbility.PlayAnimation(animationName, false, attackSpeed);
       //      duration = _animationAbility.GetDuration(animationName, attackSpeed);
       //  }
       //  
       // // skill.PlaySkillEffect();
       //
       // // var damageDelay = skill.GetDamageDelay() / attackSpeed;
       //
       //  yield return new WaitForSeconds(damageDelay);
       //  
       //  skill.Active(_detectAbility.Target);
       //
       //  var animationRemainTime = duration - damageDelay;
       //  
       //  if (animationRemainTime > 0)
       //  {
       //      yield return new WaitForSeconds(animationRemainTime);
       //  }
       //  
       //  _owner.GetAbility<FSMAbility>().ChangeState(Enum_PlayerStateType.Idle);

       yield return null;
    }

    public override void LogicUpdate(float deltaTime)
    {
        Debug.LogError("아응");
    }

    public override void Exit()
    {
    }
}
