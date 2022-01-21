using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttackState : CoroutineState
{
    private SpriteAnimationAbility _animationAbility;
    private MonsterAttackAbility _monsterAttackAbility;
    private PlayerDetectAbility _playerDetectAbility;
    
    private PlayerObject _playerObject;    
    private const string DeathAnimationName = "death";

    
    public MonsterAttackState(CharacterObject owner, StateMachine stateMachine) : base(owner, stateMachine)
    {
        
    }

    public override void Init()
    {
        base.Init();

        _animationAbility = _owner.GetAbility<SpriteAnimationAbility>();
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

        var duration = _animationAbility.PlayAttackAnimation();
        
        var waitForHalf = new WaitForSeconds(duration / 2f);

        yield return waitForHalf;

        var attackResult = _monsterAttackAbility.Attack();

        yield return waitForHalf;

        _monsterAttackAbility.SetAttackCoolTime(1f);

        _owner.GetAbility<FSMAbility>().ChangeState(Enum_MonsterStateType.Idle);
    }

    public override void LogicUpdate(float deltaTime)
    {
    }
}
