using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Spine;
using Spine.Unity;
using UnityEngine;

public class BerserkAbility : CharacterAbility
{
    private bool _on = false;
    public bool IsOn => _on;
    
    // 스킨
    private SkeletonAnimation _animation;

    // private const string BerserkSkinName = "01";
    // private const string DefaultSkinName = "default";

    // 이펙트 컨테이너
    private BerserkVFXContainer _berserkVFXContainer;
    
    // 광폭화 눈 트레일
    private Bone _eyeBone;

    private const string EyeBoneName = "eye";
    
    // 파티클
    private Transform _berserkEyeVFXTransform; 

    private bool _isEyesCached = false;
    
    
    public override void Init()
    {
        base.Init();

        _animation = GetComponentInChildren<SkeletonAnimation>();
        _berserkVFXContainer = GetComponentInChildren<BerserkVFXContainer>();
    }

    public void On()
    {
        _on = true;

        if (!_isEyesCached)
        {
            CacheBerserkEyes();
        }
        
        ChangeActivationBerserkVFX();
        //ChangeBerserkSkin();
    }

    public void Off()
    {
        _on = false;
        
        ChangeActivationBerserkVFX();
        //ChangeBerserkSkin();
    }

    private void ChangeActivationBerserkVFX()
    {
        _berserkVFXContainer.ChangeBerserkEffectActivation(_on);
    }

    
    private void CacheBerserkEyes()
    {
        _eyeBone = _animation.Skeleton.FindBone(EyeBoneName);

        _berserkEyeVFXTransform = _berserkVFXContainer.GetBerserkEyeTrailEffect();

        _isEyesCached = true;
    }

    public override void ProcessAbility(float deltaTime)
    {
        if (!_on)
        {
            return;
        }
        
        _berserkEyeVFXTransform.position = _eyeBone.GetWorldPosition(_onwerObject.transform);
    }

    public void SetActivationEyeTrail(bool active)
    {
        if (_isEyesCached && _on)
        {
            _berserkEyeVFXTransform.gameObject.SetActive(active);
        }        
    }
    
    
    // 스킨 바꾸는 방식이 아니라서 폐기
    // private void ChangeBerserkSkin()
    // {
    //     if (_on)
    //     {
    //         //_animation.Skeleton.SetSkin(DefaultSkinName);
    //     }
    //     else
    //     {
    //         //_animation.Skeleton.SetSkin(BerserkSkinName);
    //     }
    //     
    //     //_animation.Skeleton.SetSlotsToSetupPose();
    // }
}
