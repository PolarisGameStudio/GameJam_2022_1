using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// todo: 스킬툴 공격연출을 위한 땜빵용
public class PlayerAttackState_Backup : CoroutineState
{ 
    private AnimationAbility _animationAbility;
    private PlayerAttackAbility _playerAttackAbility;
    private MonsterDetectAbility _monsterDetectAbility;

    private List<AttackPreset> _attackPresets;
    
    public PlayerAttackState_Backup(CharacterObject owner, StateMachine stateMachine) : base(owner, stateMachine)
    {
    }

    public override void Init()
    {
        base.Init();

        _animationAbility = _owner.GetAbility<AnimationAbility>();
        _playerAttackAbility = _owner.GetAbility<PlayerAttackAbility>();
        _monsterDetectAbility = _owner.GetAbility<MonsterDetectAbility>();
    }
    

    public override IEnumerator Enter_Coroutine()
    {
        var targets = _monsterDetectAbility.Targets;
        if (targets == null || targets.Count == 0)
        {
            _owner.GetAbility<FSMAbility>().ChangeState(Enum_PlayerStateType.Run);
            yield break;
        }

        var timeScale = 1f;
        
        var attackSpeed = (float) _owner.Stat[Enum_StatType.AttackSpeed];
        var animationName = UnityEngine.Random.Range(0, 2) == 1 ? "attack1" : "attack2";
        var duration = _animationAbility.GetDuration(animationName, timeScale);

        float diff = (1f / attackSpeed) - duration;
        if (diff < 0)  // 공속이 애니메이션 속도보다 빠르다면 애니메이션 속도를 빠르게한다.
        {
            timeScale = duration / (1f / (float)_owner.Stat[Enum_StatType.AttackSpeed]);
            duration = _animationAbility.GetDuration(animationName, timeScale);
        }
        
        _animationAbility.PlayAnimation(animationName, false, timeScale);

        // Todo: 임시로 애니메이션 중간에 공격 들어가게 함
        var waitForHalf = new WaitForSeconds(duration / 2f);

        yield return waitForHalf;
        
        int targetCount = targets.Count;
        for (int i = 0; i < targetCount; ++i)
        {
            _playerAttackAbility.Attack(targets[i]);
        }
        
        yield return waitForHalf;
        
        _playerAttackAbility.SetAttackCoolTime(diff < 0 ? 0 : (1f / attackSpeed) - duration);
        
        _owner.GetAbility<FSMAbility>().ChangeState(Enum_PlayerStateType.Idle);
        
        
        
        /**
        var attackSpeed = (float)_owner.Stat[Enum_StatType.AttackSpeed];
        
        _attackPresets = Tool_WeaponManager.Instance.GetAttackPresetList();
        
        AttackPreset preset = _attackPresets[_toolAbility.AttackPresetIndex];

        var animationName = preset.AnimtaionName;
        float duration = 0;
        
        if (string.IsNullOrEmpty(animationName))
        {
            Debug.LogError("animationName이 없습니다.");
        }
        else
        {
            _animationAbility.PlayAnimation(animationName, false, attackSpeed);
            duration = _animationAbility.GetDuration(preset.AnimtaionName, attackSpeed);
        }

        preset.PlaySlow();
        preset.PlayBlackout();

        var damageDelay = preset.DamageDelay / attackSpeed;

        yield return new WaitForSeconds(damageDelay);

        //_attackAbility.Attack(_detectAbility.Target, Enum_DamageType.Normal);
        
        preset.PlayShake();

        var animationRemainTime = duration - damageDelay;
        
        if (animationRemainTime > 0)
        {
            yield return new WaitForSeconds(animationRemainTime);
        }
        
        _owner.GetAbility<FSMAbility>().ChangeState(Enum_PlayerStateType.Idle);
        **/
    }

    public override void LogicUpdate(float deltaTime)
    {
        Debug.LogError("Attack_Update");
    }

    public override void Exit()
    {
    }
}