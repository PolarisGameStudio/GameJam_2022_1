using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolAbility : CharacterAbility
{
    private AttackPreset _selectedAttackPreset;
    public AttackPreset SelectedAttackPreset => _selectedAttackPreset;
    
    private int _skillPresetIndex = 0;
    public int SkillPresetIndex => _skillPresetIndex;

    private int _attackCount = 0;

    private bool _continuousAttack;
    public bool ContinuousAtack => _continuousAttack;

    private bool _randomAttack;
    public bool RandomAttack => _randomAttack;

    public void ChangeContinuousAttackMode(bool isOn)
    {
        _continuousAttack = isOn;
    }
    
    public void ChangeRandomAttackMode(bool isOn)
    {
        _randomAttack = isOn;
    }

    public int GetAttackCount()
    {
        return _attackCount++;
    }

    public void SetAttackPreset(AttackPreset selectedAttackPreset)
    {
        _selectedAttackPreset = selectedAttackPreset;
    }
    
    public void SetSkillPresetIndex(int index)
    {
        _skillPresetIndex = index;
    }
}
