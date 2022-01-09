using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponAnimationContainers : MonoBehaviour
{ 
    private List<WeaponAnimationContainer> _weaponAnimationContainers;
    [SerializeField] private List<AttackPreset> _berserkAttackPresetList;
    
    private void InitAnimationContainers()
    {
        _weaponAnimationContainers = transform.GetComponentsInChildren<WeaponAnimationContainer>().ToList();
    }

    public List<AttackPreset> GetAttackPresets(Enum_WeaponCombinationType combinationType)
    {
        if (_weaponAnimationContainers == null)
        {
            InitAnimationContainers();
        }
        
        var matchedContainer = _weaponAnimationContainers.Find(container => container.combinationType == combinationType);

        if (matchedContainer == null)
        {
            return null;
        }
        
        return matchedContainer.GetAttackPresets();
    }

    public List<AttackPreset> GetBerserkAttackPresets()
    {
        return _berserkAttackPresetList;
    }
}
