using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Spine;
using Spine.Unity;
using UnityEngine;
using UnityEngine.Serialization;

public class MultiSkinAnimationAbility : AnimationAbility
{
    public SkeletonAnimation _characterAnim;
    public SkeletonAnimation _slashAnim;
    

    public override void Init()
    {
        base.Init();
    }

    public void RefreshSkins()
    {
        Skeleton skeleton = _characterAnim.skeleton;
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
        
        _characterAnim.skeleton.SetSkin(newSkin);
        _characterAnim.skeleton.SetSlotsToSetupPose();
    }

    public void PlaySlashAnimation(float timeScale)
    {
        _slashAnim.gameObject.SetActive(true);
        _slashAnim.AnimationState.TimeScale = timeScale;
        _slashAnim.AnimationState.SetAnimation(0, $"eff_attack1_{(char)('A' + (DataManager.PromotionData.CurrentPromotionIndex))}", false);
    }
}
