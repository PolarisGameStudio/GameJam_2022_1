using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimationAbility : CharacterAbility
{
    private Animator _animator;

    public float Height = 0;

    public override void Init()
    {
        base.Init();

        _animator = GetComponentInChildren<Animator>();
    }

    public float PlayDeathAnimation()
    {
        _animator.SetTrigger("Death");

        return 0.8f;
    }

    public float PlayAttackAnimation()
    {
         _animator.SetTrigger("Attack");

         return 1f;
    }
}