using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

// todo: 스킬툴 공격연출을 위한 땜빵용
public class PlayerAttackState : CoroutineState
{
    public const string animationName = "attack1";
    
    private MultiSkinAnimationAbility _animationAbility;
    private PlayerAttackAbility _playerAttackAbility;
    private MonsterDetectAbility _monsterDetectAbility;

    public PlayerAttackState(CharacterObject owner, StateMachine stateMachine) : base(owner, stateMachine)
    {
    }

    public override void Init()
    {
        base.Init();

        _animationAbility = _owner.GetAbility<MultiSkinAnimationAbility>();
        _playerAttackAbility = _owner.GetAbility<PlayerAttackAbility>();
        _monsterDetectAbility = _owner.GetAbility<MonsterDetectAbility>();
    }
    
    
    public override IEnumerator Enter_Coroutine()
    {
        // var targets = _monsterDetectAbility.AttackableTargets;
        // if (targets == null || targets.Count == 0)
        // {
        //     _owner.GetAbility<FSMAbility>().ChangeState(Enum_PlayerStateType.Run);
        //     yield break;
        // }
        var target = _monsterDetectAbility.NearestAttackableTarget;
        
        if (target == null || target.IsDeath)
        {
            _owner.GetAbility<FSMAbility>().ChangeState(Enum_PlayerStateType.Run);
            yield break;
        }

        if (UtilCode.GetChance(50))
        {
            SoundManager.Instance.PlaySound("sfx_HeroAttack_3");
        }
        else
        {
            SoundManager.Instance.PlaySound("sfx_HeroAttack_1");
        }
        

        var timeScale = 1f;
        var realTimeScale = 1f;

        var attackSpeed = (float) _owner.Stat[Enum_StatType.AttackSpeed];


        //var animationName = UnityEngine.Random.Range(0, 2) == 1 ? "attack1" : "attack2";
        var duration = _animationAbility.GetDuration(animationName, timeScale);

        float diff = (1f / attackSpeed) - duration;
        if (diff < 0) // 공속이 애니메이션 속도보다 빠르다면 애니메이션 속도를 빠르게한다.
        {
            realTimeScale = duration / (1f / (float) _owner.Stat[Enum_StatType.AttackSpeed]);
            timeScale = Mathf.Min(realTimeScale, _animationAbility.MaxAttackAnimationSpeedScale);

            duration = _animationAbility.GetDuration(animationName, realTimeScale);
        }

        _animationAbility.PlayAnimation(animationName, false, timeScale);
        _animationAbility.PlaySlashAnimation(timeScale);

        var damageDelay = 0;
        //var damageDelay = attackPreset.DamageDelay / realTimeScale;
        var waitForDamageDelay = new WaitForSeconds(damageDelay);

        yield return waitForDamageDelay;
        
        TryStateExit();

        //var intervalTime = 0.02f / realTimeScale;

        // int targetCount = targets.Count;
        //
        // for (int i = 0; i < targetCount; ++i)
        // {
        //     _playerAttackAbility.DelayAttack(targets[i], i * intervalTime);
        // }

        _playerAttackAbility.Attack(target);

        yield return new WaitForSeconds(duration - damageDelay);

        _playerAttackAbility.SetAttackCoolTime(diff < 0 ? 0 : (1f / attackSpeed) - duration);

        _owner.GetAbility<FSMAbility>().ChangeState(Enum_PlayerStateType.Idle);
    }

    public override void LogicUpdate(float deltaTime)
    {
        
    }

    private void TryStateExit()
    {
        if (!_monsterDetectAbility.HaveTarget)
        {
            _owner.GetAbility<FSMAbility>().ChangeState(Enum_PlayerStateType.Idle);
        }
    }
}