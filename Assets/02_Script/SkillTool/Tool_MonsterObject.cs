using System;
using System.Collections;
using System.Collections.Generic;
using Spine;
using Spine.Unity;
using UnityEditor;
using UnityEngine;

public class Tool_MonsterObject : MonsterObject
{
    private FSMAbility _fsmAbility;
    
    private void Start()
    {
        Init(Enum_CharacterType.StageNormalMonster, PlayerStatManager.Instance.Stat);

        _fsmAbility = GetAbility<FSMAbility>();

        Stat[Enum_StatType.MoveSpeed] = 0;
        //Stat[Enum_StatType.Health] = 99999990;
        _currentHealth = 99999990;
        Stat[Enum_StatType.MaxHealth] = 99999990;
    }


    protected override void OnDeath()
    {
        _alive = true;

        //Stat[Enum_StatType.Health] = 9999999;
        _currentHealth = 9999999;
    }
}
