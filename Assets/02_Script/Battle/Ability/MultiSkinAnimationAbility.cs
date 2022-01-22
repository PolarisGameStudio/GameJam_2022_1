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
    

    public override void Init()
    {
        base.Init();
        
        _animation = GetComponentInChildren<SkeletonAnimation>();
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
        if (rightSkin != null)
        {
            newSkin.AddSkin(rightSkin); 
        }
        if (leftSkin != null)
        {
            newSkin.AddSkin(leftSkin); 
        }  
        
        _animation.skeleton.SetSkin(newSkin);
        _animation.skeleton.SetSlotsToSetupPose();
    }

}
