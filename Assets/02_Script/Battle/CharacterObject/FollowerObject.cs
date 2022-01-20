using System;
using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

public class FollowerObject : MonoBehaviour
{
    private SkeletonAnimation _skeletonAnimation;
    
    public const string MoveAnimationName = "move";
    public const string IdleAnimationName = "idle";

    private void Awake()
    {
        _skeletonAnimation = GetComponent<SkeletonAnimation>();
    }

    public void ChangeFollowerModel(int index)
    {
        if (index == -1)
        {
            gameObject.SetActive(false);
            return;
        }
        
        gameObject.SetActive(true);
        
        //_skeletonAnimation.skeletonDataAsset =; 
        _skeletonAnimation.Initialize(true);
    }
    
    public void PlayMoveAnimation(float moveScale = 1f)
    {
        if (_skeletonAnimation.AnimationName == MoveAnimationName)
        {
            return;
        }
        _skeletonAnimation.timeScale = moveScale;
        _skeletonAnimation.AnimationState.SetAnimation(0, MoveAnimationName, true);
    }

    public void PlayIdleAnimation()
    {        
        if (_skeletonAnimation.AnimationName == IdleAnimationName)
        {
            return;
        }
        _skeletonAnimation.timeScale = 1;
        _skeletonAnimation.AnimationState.SetAnimation(0, IdleAnimationName, true);
    }
}
