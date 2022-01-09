using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponOption
{
    public Enum_StatType StatType;
    public Enum_WeaponOptionGrade Grade;
    public float Value;
    public bool IsLocked;
}

public class Weapon
{
    public readonly WeaponData WeaponData;
    public Enum_WeaponType WeaponType => WeaponData.WeaponType;
    public Enum_WeaponGradeType WeaponGradeType => WeaponGradeType;

    private bool _equipped;
    public bool IsEquipped => _equipped;

    private List<WeaponOption> _options;
    public List<WeaponOption> Options => _options;
    
    public Weapon(WeaponData weaponData, List<WeaponOption> options) 
    {
        WeaponData = weaponData;

        _options = options;
    }

    public void SetEquip(bool equipped)
    {
        _equipped = equipped;
    }

    public int GetRerollPrice()
    {
        int price = 2;
        
        foreach (var option in _options)
        {
            if (option.IsLocked)
            {
                price *= 2;
            }
        }
        
        return price;
    }

    public void ToggleLockOption(int optionIndex)
    {
        _options[optionIndex].IsLocked = !_options[optionIndex].IsLocked;
    }
    
    public void RerollOptions()
    {
        foreach (var option in _options)
        {
            if (option.IsLocked)
            {
                continue;
            }

            // todo: 관련 테이블 생기면 수정, 가챠 매니저랑 연결
            option.StatType = Enum_StatType.Damage;
            option.Grade = Enum_WeaponOptionGrade.A;

            option.Value = Random.Range(0f, 100f);
        }
    }
}
