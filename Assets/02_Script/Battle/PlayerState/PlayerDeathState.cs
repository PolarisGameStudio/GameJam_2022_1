using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathState : CoroutineState
{
    private AnimationAbility _animationAbility;
    private MeshRendererAbility _meshRendererAbility;

    private const string DeathAnimationName = "death";

    private float _deathAnimationDuration = 0f;

    private WaitForSeconds _deathAnimationWaitForSeconds;
    
    public PlayerDeathState(CharacterObject owner, StateMachine stateMachine) : base(owner, stateMachine)
    {
    }
    
    public override void Init()
    {
        base.Init();

        _animationAbility = _owner.GetAbility<AnimationAbility>();
        _meshRendererAbility = _owner.GetAbility<MeshRendererAbility>();
        
        // 애니메이션용
        //_deathAnimationDuration = _animationAbility.GetDuration(DeathAnimationName);
        //_deathAnimationWaitForSeconds = new WaitForSeconds(_deathAnimationDuration);

        _deathAnimationWaitForSeconds = new WaitForSeconds(1.4f);
        // 블랙용
    }

    public override IEnumerator Enter_Coroutine()
    {
        _owner.SetAlive(false);

        
        // 애니메이션용
        //_animationAbility.PlayAnimation(DeathAnimationName, false);
        // yield return _deathAnimationWaitForSeconds;

        // 블랙용
        _meshRendererAbility.FillBlack(1.4f);
        yield return _deathAnimationWaitForSeconds;

        _owner.Kill();

        BattleManager.Instance.BattleOver();
    }

    public override void LogicUpdate(float deltaTime)
    {
    }
}
