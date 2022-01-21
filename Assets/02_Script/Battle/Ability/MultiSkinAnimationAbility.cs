using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Spine;
using Spine.Unity;
using UnityEngine;
using UnityEngine.Serialization;

public class MultiSkinAnimationAbility : AnimationAbility
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

    public void RefreshSkins()
    {
        Skeleton skeleton = _animation.skeleton;
        SkeletonData skeletonData = skeleton.Data;

        Skin newSkin = new Skin("");

        var mouthSkin = skeletonData.FindSkin($"swordC/swordC{DataManager.EquipmentData.GetSkinName(Enum_EquipmentType.Mouth):D2}");
        var leftSkin = skeletonData.FindSkin($"swordL/swordL{DataManager.EquipmentData.GetSkinName(Enum_EquipmentType.Left):D2}");
        var rightSkin =    skeletonData.FindSkin($"swordR/swordR{DataManager.EquipmentData.GetSkinName(Enum_EquipmentType.Right):D2}");

        if (mouthSkin != null)
        {
            newSkin.AddSkin(mouthSkin); 
        } 
        if (leftSkin != null)
        {
            newSkin.AddSkin(leftSkin); 
        }  
        if (rightSkin != null)
        {
            newSkin.AddSkin(rightSkin); 
        }
        
        _animation.skeleton.SetSkin(newSkin);
    }

}
