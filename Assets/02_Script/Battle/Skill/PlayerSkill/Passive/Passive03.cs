using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//적에게 피해를 입으면 2초 동안 공격력과 치명타 피해량이 (20+2*SLV)% 만큼 증가한다.
public class Passive03 : PlayerPassiveSkill
{
    public override void InitSkill(PlayerSkillData data, PlayerObject playerObject)
    {
        base.InitSkill(data, playerObject);

        data.CoolTime = 7.3f;

        _stat[Enum_StatType.Damage] = 20;
        _stat[Enum_StatType.CriticalDamage] = 20;
    }

    public override void OnHit()
    {        
        if (!IsSkillEnable())
        {
            return;
        }
        
        AddPlayerBuff(2);
        
        ResetCoolTime();
    }
}