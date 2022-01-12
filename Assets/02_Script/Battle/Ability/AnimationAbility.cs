using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Spine.Unity;
using UnityEngine;
using UnityEngine.Serialization;

public class AnimationAbility : CharacterAbility
{
    private SkeletonAnimation _animation;

    private float _height;
    public float Height => _height;

    public float _width;
    public float Width => _width;

    [Header("공격 애니메이션")]
    public float MaxAttackAnimationSpeedScale = 5;
    
    [Header("이동 애니메이션")]
    public bool UseMovmentSpeed;
    public float StartMovementAnimationSpeed = 2f;
    public float MovementAnimationSpeedFactor = 1.1f;
    public float MaxMovementAnimationSpeedScale = 3f;


    private string _currentAnimationName = string.Empty;
    private bool _currentAnimationLoop;
    private float _currentAnimationSpeed;
    

    public override void Init()
    {
        base.Init();
        
        _animation = GetComponentInChildren<SkeletonAnimation>();

        var data = _animation.skeletonDataAsset.GetSkeletonData(false);
        var scale = _animation.skeletonDataAsset.scale;
        _height = data.Height * scale;
        _width = data.Width * scale;
    }

    public void SetSkeletonDataAsset(SkeletonDataAsset skeletonDataAsset)
    {
        if (_animation.skeletonDataAsset == skeletonDataAsset)
        {
            return;
        }

        _animation.initialSkinName = null;
        _animation.ClearState();
        _animation.skeletonDataAsset = skeletonDataAsset;
        _animation.Initialize(true);
    }
    
    public void PlayAnimation(string name, bool loop, float speed = 1f)
    {
        _animation.AnimationState.TimeScale = speed;
        _animation.AnimationState.SetAnimation(0, name, loop);

        _currentAnimationName = name;
        _currentAnimationLoop = loop;
        _currentAnimationSpeed = speed;
    }

    public float GetDuration(string name, float speed = 1f)
    { 
        var animation = _animation.skeleton.Data.FindAnimation(name);

        if (animation == null)
        {
            Debug.LogError($"[{name}] 애니메이션이 없습니다.");
            return 1f;
        }

        return animation.Duration / speed;
    }
}
