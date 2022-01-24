using System;
using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

public class FollowerObject : MonoBehaviour
{
    private Animator _animator;
    
    public const string MoveAnimationPostFix = "_move";
    public const string IdleAnimationPostFix = "_idle";

    private string MoveAnimationName => $"{_currentFollowerIndex+1:D3}{MoveAnimationPostFix}";
    private string IdleAnimationName => $"{_currentFollowerIndex+1:D3}{IdleAnimationPostFix}";

    private int _currentFollowerIndex;

    private void Awake()
    {
       // _skeletonAnimation = GetComponent<SkeletonAnimation>();
        _animator = GetComponentInChildren<Animator>();
    }

    public void ChangeFollowerModel(int index)
    {
        if (index == -1)
        {
            gameObject.SetActive(false);
            return;
        }
        
        gameObject.SetActive(true);

        _currentFollowerIndex = index;
    }
    
    public void PlayMoveAnimation(float moveScale = 1f)
    {
        // if (_animator.runtimeAnimatorController. == MoveAnimationName)
        // {
        //     return;
        // }
        _animator.speed = moveScale;
        
        _animator.SetTrigger(MoveAnimationName);
    }

    public void PlayIdleAnimation()
    {        
        _animator.speed = 1;
        
        _animator.SetTrigger(IdleAnimationName);
    }
}
