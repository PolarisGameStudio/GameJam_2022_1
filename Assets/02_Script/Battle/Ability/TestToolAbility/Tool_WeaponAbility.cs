using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Spine.Unity;
using UnityEngine;

public class Tool_WeaponAbility : CharacterAbility
{
    private SkeletonAnimation _animation;

    private const string LeftWeaponSlotName  = "vfx/slash_0";
    private const string RightWeaponSlotName = "RightWeaponSlot";
    
    public override void Init()
    {
        base.Init();
        
        _animation = GetComponentInChildren<SkeletonAnimation>();
    }

    public void SetLeftWeapon(string weaponName)
    {
        _animation.skeleton.SetAttachment(LeftWeaponSlotName, weaponName);
    }

    public void SetRightWeapon(string weaponName)
    {
        _animation.skeleton.SetAttachment(RightWeaponSlotName, weaponName);
    }

    public void RefreshWeapon()
    {
        Debug.Log("무기 교체!: " + Tool_WeaponManager.Instance.GetCurrentCombination());

        switch (Tool_WeaponManager.Instance.LeftHand)
        {
            case Enum_WeaponType.Axe:
                SetLeftWeapon("vfx/slash_0");
                break;
            case Enum_WeaponType.Sword:
                SetLeftWeapon("vfx/slash_1");
                break;
            case Enum_WeaponType.Blunt:
                SetLeftWeapon("vfx/slash_2");
                break;
        }
    }
}
