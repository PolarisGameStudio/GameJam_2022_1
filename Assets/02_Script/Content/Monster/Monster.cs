using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Monster
{
    public readonly Stat Stat = new Stat();

    public readonly TBL_MONSTER _data;

    public Monster(TBL_MONSTER monsterData)
    {
        _data = monsterData;

        InitStat();
    }

    // 스테이지 몬스터 공식
    private void InitStat()
    {
        Stat.Init();
        
        Stat[Enum_StatType.Damage]      = _data.Damage;
        Stat[Enum_StatType.MaxHealth]   = _data.Health;
        Stat[Enum_StatType.AttackSpeed] = 1;        // todo : 테이블로 빼야할지 체크
        Stat[Enum_StatType.MoveSpeed]   = 5;        // todo : 테이블로 빼야할지 체크
        Stat[Enum_StatType.DetectRange] = 3;        // todo : 테이블로 빼야할지 체크
        Stat[Enum_StatType.AttackRange] = 4;        // todo : 테이블로 빼야할지 체크
    }
}
