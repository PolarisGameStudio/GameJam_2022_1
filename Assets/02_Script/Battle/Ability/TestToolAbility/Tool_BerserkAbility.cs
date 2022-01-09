using System.Collections;
using System.Collections.Generic;
using Spine;
using Spine.Unity;
using UnityEngine;


/// <summary>
/// 버서커 상태일 때 캐릭터 스켈레톤 스킨 변경시킬 ability
/// </summary>
public class Tool_BerserkAbility : CharacterAbility
{
    private SkeletonAnimation _animation;

    private const string BerserkSkinName = "01";
    private const string DefaultSkinName = "default";
    
    private const string LeftEyeBoneName = "berserk_eye_L";
    private const string RightEyeBoneName = "berserk_eye_R";

    private Bone _leftEyeBone;
    private Bone _rightEyeBone;

    public override void Init()
    {
        base.Init();

        _animation = GetComponentInChildren<SkeletonAnimation>();
        
        CacheBerserkEyeBones();
    }

    public void ChangeBerserkSkin(bool isBerserkMode)
    {
        if (isBerserkMode)
        {
            _animation.Skeleton.SetSkin(BerserkSkinName);
        }
        else
        {
            _animation.Skeleton.SetSkin(DefaultSkinName);
        }

        _animation.Skeleton.SetSlotsToSetupPose();
    }


    private void CacheBerserkEyeBones()
    {
        _leftEyeBone = _animation.Skeleton.FindBone("LeftEyeBoneName");
        _rightEyeBone = _animation.Skeleton.FindBone("RightEyeBoneName");
    }
}