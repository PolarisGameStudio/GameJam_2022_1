using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimationAbility : CharacterAbility
{
    private Animation _animation;
   // private Animator _animator;

    public List<SpriteRenderer> _sprtieRenderer;

    public float Height = 0;

    public override void Init()
    {
        base.Init();

        _animation = GetComponentInChildren<Animation>();
    }

    public void SetMonsterModel(List<Sprite> sprites)
    {
        // if (_animation.skeletonDataAsset == skeletonDataAsset)
        // {
        //     return;
        // }

        for (var i = 0; i < _sprtieRenderer.Count; i++)
        {
            _sprtieRenderer[i].sprite = sprites[i];
        }
    }

    public void PlayMoveAnimation()
    {
        SpriteMonsterObject spriteMonsterObject = _onwerObject as SpriteMonsterObject;

        _animation.Stop();
        _animation.Play($"Zombie_{spriteMonsterObject.Monster._data.Index + 1:D3}_Move");
    }

    public float PlayDeathAnimation()
    {
        _animation.Stop();
        _animation.Play($"Zombie_Death");

        return 0.8f;
    }

    public void PlayAttackAnimation()
    {
        // _animator.Play($"Zombie_Attack");
    }
}