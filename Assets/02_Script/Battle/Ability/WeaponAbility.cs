using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Spine.Unity;
using UnityEngine;

public class WeaponAbility : CharacterAbility
{
    // 무기 스킨
    private SkeletonAnimation _animation;

    private const string LeftWeaponSlotName = "LeftWeaponSlot";
    private const string RightWeaponSlotName = "RightWeaponSlot";

    // 무기 공격 애니메이션
    private WeaponAnimationContainers _weaponAnimationContainers;
    private List<AttackPreset> _attackPresets;

    public int AttackPresetCount => _attackPresets.Count;

    // 무기 검기 이펙트
    private SlashVFXContainers _slashVFXContainers;
    private SlashVFXContainer _currentSlashVFXContainer;

    public override void Init()
    {
        base.Init();

        _animation = GetComponentInChildren<SkeletonAnimation>();
        _weaponAnimationContainers = GetComponentInChildren<WeaponAnimationContainers>();
        _slashVFXContainers = GetComponentInChildren<SlashVFXContainers>();
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
        var weaponCombination = WeaponManager.Instance.WeaponCombinationType;

        var isBerserkMode = _onwerObject.GetAbility<BerserkAbility>().IsOn;

        if (isBerserkMode)
        {
            _attackPresets = _weaponAnimationContainers.GetBerserkAttackPresets();
            _currentSlashVFXContainer = _slashVFXContainers.GetBerserkSlashVFXContainer();
        }
        else
        {
            _attackPresets = _weaponAnimationContainers.GetAttackPresets(weaponCombination);
            _currentSlashVFXContainer = _slashVFXContainers.GetSlashVFXContainer(weaponCombination);
        }

        // switch (Tool_WeaponManager.Instance.LeftHand)
        // {
        //     case Enum_WeaponType.Axe:
        //         SetLeftWeapon("vfx/slash_0");
        //         break;
        //     case Enum_WeaponType.Sword:
        //         SetLeftWeapon("vfx/slash_1");
        //         break;
        //     case Enum_WeaponType.Blunt:
        //         SetLeftWeapon("vfx/slash_2");
        //         break;
        // }
    }

    public AttackPreset GetAttackPreset(int index)
    {
        if (_attackPresets == null)
        {
            return null;
        }

        if (index < 0 || index >= _attackPresets.Count)
        {
            return _attackPresets[0];
        }

        return _attackPresets[index];
    }

    public List<AttackPreset> GetAttackPresetList()
    {
        return _attackPresets;
    }
    
    /// <param name="order">공격애니메이션 순서 ex) ss_attack_1, ss_attack_2 .... ss_attack_n </param>
    public void PlaySlashVFX(int order, float animationSpeed = 1f)
    {
        if (_currentSlashVFXContainer == null)
        {
            Debug.LogError($"_currentSlashVFXContainer가 없습니다.");
            return;
        }
        
        _currentSlashVFXContainer.PlaySlashVFX(order, animationSpeed);
    }

    public void StopSlashVFX()
    {
        _currentSlashVFXContainer.StopCurrentSlashVFX();
    }
}