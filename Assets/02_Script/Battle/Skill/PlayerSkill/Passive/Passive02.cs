using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//광폭화 상태에 돌입 후 4초 동안 치명타 피해량이 (30+3*SLV)% 증가한다.
public class Passive02 : PlayerPassiveSkill
{
    public override void InitSkill(PlayerSkillData data, PlayerObject playerObject)
    {
        base.InitSkill(data, playerObject);
        
        _stat[Enum_StatType.CriticalDamage] = 30;
    }

    public override void OnBerserkStart()
    {
        AddPlayerBuff(4);
    }
}