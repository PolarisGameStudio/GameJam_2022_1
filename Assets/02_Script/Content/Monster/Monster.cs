using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Monster
{
    public readonly Stat Stat = new Stat();

    public readonly SpecStageMonster MonsterData;
    
    private readonly Dictionary<Enum_BattleType, Action<bool>> _statInitActions = new Dictionary<Enum_BattleType, Action<bool>>((int) Enum_BattleType.Count);

    public Monster(SpecStageMonster monsterData)
    {
        MonsterData = monsterData;
        
        ActionInit();
    }

    private void ActionInit()
    {
        _statInitActions.Add(Enum_BattleType.None,    null);
        _statInitActions.Add(Enum_BattleType.Stage,   StatStageBattleInit);
        _statInitActions.Add(Enum_BattleType.Dungeon, StatDungeonBattleInit);
        _statInitActions.Add(Enum_BattleType.Tool,    null);
    }

    public void StatInit(Enum_BattleType battleType, bool isBoss = false)
    {
        _statInitActions[battleType]?.Invoke(isBoss);
    }


    // 스테이지 몬스터 공식
    private void StatStageBattleInit(bool isBoss)
    {
        Stat.Init();
        
        // Todo: 공식
        
        Stat[Enum_StatType.Damage]      = MonsterData.attack;
        Stat[Enum_StatType.MaxHealth]   = MonsterData.health;
        Stat[Enum_StatType.AttackSpeed] = 1;        // todo : 테이블로 빼야할지 체크
        Stat[Enum_StatType.MoveSpeed]   = 5;        // todo : 테이블로 빼야할지 체크
        Stat[Enum_StatType.DetectRange] = 3;        // todo : 테이블로 빼야할지 체크

        // Todo: 보스 테스트용(임시)
        if (isBoss)
        {
            Debug.LogError("보스 데미지, 헬스 더미");
            Stat[Enum_StatType.Damage] *= 10f;
            Stat[Enum_StatType.MaxHealth] *= 10f;
        }
    }

    // 던전 몬스터 공식
    private void StatDungeonBattleInit(bool isBoss)
    {
        Stat.Init();
        
        // Todo: 공식
    }
    
}
