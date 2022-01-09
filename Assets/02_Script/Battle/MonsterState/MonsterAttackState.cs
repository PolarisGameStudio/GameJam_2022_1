using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttackState : CoroutineState
{
    private AnimationAbility _animationAbility;
    private MonsterAttackAbility _monsterAttackAbility;
    private PlayerDetectAbility _playerDetectAbility;

    private const string AttackAnimationName = "attack";
    
    private PlayerObject _playerObject;
    
    public MonsterAttackState(CharacterObject owner, StateMachine stateMachine) : base(owner, stateMachine)
    {
        
    }

    public override void Init()
    {
        base.Init();

        _animationAbility = _owner.GetAbility<AnimationAbility>();
        _monsterAttackAbility = _owner.GetAbility<MonsterAttackAbility>();
        _playerDetectAbility = _owner.GetAbility<PlayerDetectAbility>();

        _playerObject = BattleManager.Instance.PlayerObject;
    }
    
    public override IEnumerator Enter_Coroutine()
    {
        if (_owner.IsDeath)
        {
            yield break;
        }
        
        if (!_playerDetectAbility.HaveTarget)
        {
            _owner.GetAbility<FSMAbility>().ChangeState(Enum_MonsterStateType.Idle);
            yield break;
        }

        var timeScale = 1f;
        var realTimeScale = 1f;

        var attackSpeed = (float) _owner.Stat[Enum_StatType.AttackSpeed];
        var duration = _animationAbility.GetDuration(AttackAnimationName, timeScale);

        float diff = (1f / attackSpeed) - duration;
        if (diff < 0) // 공속이 애니메이션 속도보다 빠르다면 애니메이션 속도를 빠르게한다.
        {
            realTimeScale = duration / (1f / attackSpeed);
            timeScale = Mathf.Min(realTimeScale, _animationAbility.MaxAttackAnimationSpeedScale);
            
            duration = _animationAbility.GetDuration(AttackAnimationName, realTimeScale);
        }

        _animationAbility.PlayAnimation(AttackAnimationName, false, realTimeScale);

        var waitForHalf = new WaitForSeconds(duration / 2f);

        yield return waitForHalf;

        var attackResult = _monsterAttackAbility.Attack();

        yield return waitForHalf;

        _monsterAttackAbility.SetAttackCoolTime(diff < 0 ? 0 : (1f / attackSpeed) - duration);

        _owner.GetAbility<FSMAbility>().ChangeState(Enum_MonsterStateType.Idle);
    }

    public override void LogicUpdate(float deltaTime)
    {
    }
}
