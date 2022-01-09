using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SlashVFXContainers : MonoBehaviour
{
    private List<SlashVFXContainer> _slashVFXContainers;
    [SerializeField] private SlashVFXContainer _berserkSlashVFXContainer;

    private SlashVFXContainer _currentSlashVFXContainer;
    
    private void Awake()
    {
        _slashVFXContainers = transform.GetComponentsInChildren<SlashVFXContainer>().ToList();
    }
    
    // public void RefreshSlashVFXContainer(Enum_WeaponCombinationType weaponCombinationType, bool isBerserkMode = false)
    // {
    //     if (isBerserkMode)
    //     {
    //         _currentSlashVFXContainer = _berserkSlashVFXContainer;
    //         return;
    //     }
    //     
    //     var matchedContainer = GetSlashVFX(weaponCombinationType);
    //
    //     if (matchedContainer == null)
    //     {
    //         Debug.LogError($"일치하는 SlashVFXContainer 가 없습니다. // 조합:{weaponCombinationType}");
    //         return;
    //     }
    //
    //     _currentSlashVFXContainer = matchedContainer;
    // }
    //
    
    public SlashVFXContainer GetSlashVFXContainer(Enum_WeaponCombinationType combinationType)
    {
        var matchedContainer = _slashVFXContainers.Find(container => container.combinationType == combinationType);

        if (matchedContainer == null)
        {
            return null;
        }
        
        return matchedContainer;
    }

    public SlashVFXContainer GetBerserkSlashVFXContainer()
    {
        return _berserkSlashVFXContainer;
    }

    //
    // /// <param name="order">공격애니메이션 순서 ex) ss_attack_1, ss_attack_2 .... ss_attack_n </param>
    // public void PlaySlashVFX(int order, float animationSpeed = 1f)
    // {
    //     if (_currentSlashVFXContainer == null)
    //     {
    //         Debug.LogError($"_currentSlashVFXContainer가 없습니다.");
    //         return;
    //     }
    //     
    //     Debug.Log($"{_currentSlashVFXContainer.combinationType} {order}번 검기이펙트 실행");
    //     _currentSlashVFXContainer.PlaySlashVFX(order, animationSpeed);
    // }
    //
    // private SlashVFXContainer GetSlashVFX(Enum_WeaponCombinationType combinationType)
    // {
    //     var matchedContainer = _slashVFXContainers.Find(container => container.combinationType == combinationType);
    //
    //     if (matchedContainer == null)
    //     {
    //         return null;
    //     }
    //     
    //     return matchedContainer;
    // }
}
