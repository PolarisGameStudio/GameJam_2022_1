using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatAbility : CharacterAbility
{
    private Stat _playerStat; // PlayerManager에서 관리하는 플레이어 기본 스탯

    public override void Init()
    {
        base.Init();
        
        _playerStat = DataManager.Container.Stat;
    }
    

    public void Calculate()
    {
        var stat = _onwerObject.Stat;
        
        int statCount = (int) Enum_StatType.Count;
        
        for (int i = 0; i < statCount; ++i)
        {
            stat[i] = _playerStat[i];
        }
        
        stat.Multiple(_onwerObject.GetAbility<BuffAbility>().BuffStat);
    }
    
}
