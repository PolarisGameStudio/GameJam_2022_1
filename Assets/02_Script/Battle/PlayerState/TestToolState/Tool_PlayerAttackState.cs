using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// todo: 스킬툴 공격연출을 위한 땜빵용
public class Tool_PlayerAttackState : CoroutineState
{
    private AnimationAbility _animationAbility;
    private ToolAbility _toolAbility;
    private PlayerAttackAbility _playerAttackAbility;
    private MonsterDetectAbility _detectAbility;
    private WeaponAbility _weaponAbility;

    private List<AttackPreset> _attackPresets;

    public Tool_PlayerAttackState(CharacterObject owner, StateMachine stateMachine) : base(owner, stateMachine)
    {
    }

    public override void Init()
    {
        base.Init();

        _animationAbility = _owner.GetAbility<AnimationAbility>();
        _toolAbility = _owner.GetAbility<ToolAbility>();
        _playerAttackAbility = _owner.GetAbility<PlayerAttackAbility>();
        _detectAbility = _owner.GetAbility<MonsterDetectAbility>();
        _weaponAbility = _owner.GetAbility<WeaponAbility>();
    }

    public override void Enter()
    {
    }

    public override IEnumerator Enter_Coroutine()
    {
        bool isFirst = true;
        while (_toolAbility.ContinuousAtack || isFirst)
        {
            isFirst = false;

            _attackPresets = _weaponAbility.GetAttackPresetList();

            int attackPresetIndex = 0;

            if (_toolAbility.ContinuousAtack)
            {
                if (_toolAbility.RandomAttack)
                {
                    attackPresetIndex = UnityEngine.Random.Range(0, _weaponAbility.AttackPresetCount);
                }
                else
                {
                    attackPresetIndex = _toolAbility.GetAttackCount();
                    attackPresetIndex %= _weaponAbility.AttackPresetCount;
                }
            }
            else
            {
                attackPresetIndex = _attackPresets.FindIndex(x => x == _toolAbility.SelectedAttackPreset);
            }

            AttackPreset preset = _attackPresets[attackPresetIndex];

            var animationName = preset.AnimtaionName;

            var timeScale = 1f;
            var realTimeScale = 1f;

            var attackSpeed = (float) _owner.Stat[Enum_StatType.AttackSpeed];
            
            var duration = _animationAbility.GetDuration(animationName, timeScale);

            float diff = (1f / attackSpeed) - duration;
            if (diff < 0) // 공속이 애니메이션 속도보다 빠르다면 애니메이션 속도를 빠르게한다.
            {
                realTimeScale = duration / (1f / attackSpeed);
                timeScale = Mathf.Min(realTimeScale, _animationAbility.MaxAttackAnimationSpeedScale);
            
                duration = _animationAbility.GetDuration(animationName, realTimeScale);
            }

            _animationAbility.PlayAnimation(animationName, false, timeScale);
            _weaponAbility.PlaySlashVFX(attackPresetIndex, timeScale);

            preset.PlaySlow(attackSpeed);
            preset.PlayBlackout(attackSpeed);

            if (preset.ShakeImmediately)
            {
                preset.PlayShake();
            }

            var damageDelay = preset.DamageDelay / attackSpeed;

            yield return new WaitForSeconds(damageDelay);

            _playerAttackAbility.Attack(_detectAbility.Target);
            _weaponAbility.PlaySlashVFX(attackPresetIndex, attackSpeed);

            if (!preset.ShakeImmediately)
            {
                preset.PlayShake();
            }

            var animationRemainTime = duration - damageDelay;

            if (animationRemainTime > 0)
            {
                yield return new WaitForSeconds(animationRemainTime);
            }
        }

        _owner.GetAbility<FSMAbility>().ChangeState(Enum_PlayerStateType.Idle);
    }

    public override void LogicUpdate(float deltaTime)
    {
        Debug.LogError("아응");
    }

    public override void Exit()
    {
    }
}