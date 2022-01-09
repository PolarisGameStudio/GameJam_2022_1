using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// todo: 스킬툴 공격연출을 위한 땜빵용
public class PlayerAttackState : CoroutineState
{
    private AnimationAbility _animationAbility;
    private PlayerAttackAbility _playerAttackAbility;
    private MonsterDetectAbility _monsterDetectAbility;
    private WeaponAbility _weaponAbility;
    private BerserkAbility _berserkAbility;

    public PlayerAttackState(CharacterObject owner, StateMachine stateMachine) : base(owner, stateMachine)
    {
    }

    public override void Init()
    {
        base.Init();

        _animationAbility = _owner.GetAbility<AnimationAbility>();
        _playerAttackAbility = _owner.GetAbility<PlayerAttackAbility>();
        _monsterDetectAbility = _owner.GetAbility<MonsterDetectAbility>();
        _weaponAbility = _owner.GetAbility<WeaponAbility>();
        _berserkAbility = _owner.GetAbility<BerserkAbility>();
    }
    
    
    public override IEnumerator Enter_Coroutine()
    {
        _berserkAbility.SetActivationEyeTrail(false);

        var targets = _monsterDetectAbility.AttackableTargets;
        if (targets == null || targets.Count == 0)
        {
            _owner.GetAbility<FSMAbility>().ChangeState(Enum_PlayerStateType.Run);
            yield break;
        }

        var timeScale = 1f;
        var realTimeScale = 1f;

        var attackSpeed = (float) _owner.Stat[Enum_StatType.AttackSpeed];


        //var animationName = UnityEngine.Random.Range(0, 2) == 1 ? "attack1" : "attack2";
        var randomIndex = UnityEngine.Random.Range(0, _weaponAbility.AttackPresetCount);
        var attackPreset = _weaponAbility.GetAttackPreset(randomIndex);
        var animationName = attackPreset.AnimtaionName;
        var duration = _animationAbility.GetDuration(animationName, timeScale);

        float diff = (1f / attackSpeed) - duration;
        if (diff < 0) // 공속이 애니메이션 속도보다 빠르다면 애니메이션 속도를 빠르게한다.
        {
            realTimeScale = duration / (1f / (float) _owner.Stat[Enum_StatType.AttackSpeed]);
            timeScale = Mathf.Min(realTimeScale, _animationAbility.MaxAttackAnimationSpeedScale);

            duration = _animationAbility.GetDuration(animationName, realTimeScale);
        }

        _animationAbility.PlayAnimation(animationName, false, timeScale);
        _weaponAbility.PlaySlashVFX(randomIndex, timeScale);

        attackPreset.PlayBlackout();
        attackPreset.PlaySlow();

        var damageDelay = attackPreset.DamageDelay / realTimeScale;
        var waitForDamageDelay = new WaitForSeconds(damageDelay);

        yield return waitForDamageDelay;
        
        TryStateExit();

        var intervalTime = 0.02f / realTimeScale;

        int targetCount = targets.Count;

        for (int i = 0; i < targetCount; ++i)
        {
            _playerAttackAbility.DelayAttack(targets[i], i * intervalTime);
        }

        attackPreset.PlayShake();

        BerserkManager.Instance.OnPlayerAttack();

        yield return new WaitForSeconds(duration - damageDelay);

        _playerAttackAbility.SetAttackCoolTime(diff < 0 ? 0 : (1f / attackSpeed) - duration);

        _owner.GetAbility<FSMAbility>().ChangeState(Enum_PlayerStateType.Idle);
    }

    public override void LogicUpdate(float deltaTime)
    {
        Debug.LogError("Attack_Update");
    }

    public override void Exit()
    {
        _weaponAbility.StopSlashVFX();
    }

    private void TryStateExit()
    {
        if (!_monsterDetectAbility.HaveTarget)
        {
            _owner.GetAbility<FSMAbility>().ChangeState(Enum_PlayerStateType.Idle);
        }
    }
}