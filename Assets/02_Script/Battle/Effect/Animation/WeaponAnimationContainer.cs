using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class WeaponAnimationContainer : MonoBehaviour
{
    [FormerlySerializedAs("Combination")] public Enum_WeaponCombinationType combinationType;

    [SerializeField] private List<AttackPreset> _attackPresetList;

    public List<AttackPreset> GetAttackPresets()
    {
        return _attackPresetList;
    }
}
