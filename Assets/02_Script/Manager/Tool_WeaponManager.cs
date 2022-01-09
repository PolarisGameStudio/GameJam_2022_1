using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//todo : 스킬툴 장비 장착 시뮬레이션을 위한 임시 매니저, 후에 새로 만들거나, 수정필요
public class Tool_WeaponManager : SingletonBehaviour<Tool_WeaponManager>
{
    private Enum_WeaponType _leftHand = Enum_WeaponType.Sword;
    public Enum_WeaponType LeftHand => _leftHand;
    
    private Enum_WeaponType _rightHand = Enum_WeaponType.Sword;
    public Enum_WeaponType RightHand => _rightHand;

    private Enum_WeaponCombinationType _currentCombinationType = Enum_WeaponCombinationType.Sword_Sword;

    private WeaponAnimationContainers _weaponAnimationContainers;

    protected override void Awake()
    {
        base.Awake();
        _weaponAnimationContainers = GetComponentInChildren<WeaponAnimationContainers>();
    }

    public Enum_WeaponCombinationType GetCurrentCombination()
    {
        return _currentCombinationType;
    }

    public void ChangeWeapon(bool isLeft, Enum_WeaponType weaponType)
    {
        if (isLeft)
        {
            _leftHand = weaponType;
        }
        else
        {
            _rightHand = weaponType;
        }

        _currentCombinationType = GetWeaponCombination(_leftHand, _rightHand);
        
        RefreshEvent.Trigger(Enum_RefreshEventType.Weapon);
    }
    
    public Enum_WeaponCombinationType GetWeaponCombination(Enum_WeaponType leftHand, Enum_WeaponType rightHand)
    {
        bool isSwordEquipped = leftHand == Enum_WeaponType.Sword || rightHand == Enum_WeaponType.Sword;
        bool isAxeEquipped = leftHand == Enum_WeaponType.Axe || rightHand == Enum_WeaponType.Axe;
        bool isBluntEquipped = leftHand == Enum_WeaponType.Blunt || rightHand == Enum_WeaponType.Blunt;
        
        if (isSwordEquipped)
        {
            if (isAxeEquipped)
            {
                return Enum_WeaponCombinationType.Sword_Axe;
            }
            else if (isBluntEquipped)
            {
                return Enum_WeaponCombinationType.Sword_Blunt;
            }
            else
            {
                return Enum_WeaponCombinationType.Sword_Sword;
            }
        }
        else if (isAxeEquipped)
        {
            if (isBluntEquipped)
            {
                return Enum_WeaponCombinationType.Axe_Blunt;
            }
            else
            {
                return Enum_WeaponCombinationType.Axe_Axe;
            }
        }
        else
        {
            return Enum_WeaponCombinationType.Blunt_Blunt;
        }
    }
}
