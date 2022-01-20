﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDeathState : CoroutineState
{
    private AnimationAbility _animationAbility;

    private const string DeathAnimationName = "death";

    private float _deathAnimationDuration = 0f;

    private WaitForSeconds _deathAnimationWaitForSeconds;
    
    public MonsterDeathState(CharacterObject owner, StateMachine stateMachine) : base(owner, stateMachine)
    {
    }

    public override void Init()
    {
        base.Init();

        _animationAbility = _owner.GetAbility<AnimationAbility>();

        _deathAnimationDuration = _animationAbility.GetDuration(DeathAnimationName);
        _deathAnimationWaitForSeconds = new WaitForSeconds(_deathAnimationDuration);
    }
    
    public override IEnumerator Enter_Coroutine()
    {
        _owner.SetAlive(false);

        _animationAbility.PlayAnimation(DeathAnimationName, false);

        yield return _deathAnimationWaitForSeconds;

        _owner.Kill();
    }

    public override void LogicUpdate(float deltaTime)
    {
    }
}
