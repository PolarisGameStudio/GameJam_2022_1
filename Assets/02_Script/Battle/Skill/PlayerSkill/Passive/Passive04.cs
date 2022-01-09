using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 적이 사망할 때마다 3초 동안 치명타 피해량과 이동속도가 (15+SLV)% 증가한다.
public class Passive04 : PlayerPassiveSkill
{
    public override void InitSkill(PlayerSkillData data, PlayerObject playerObject)
    {
        base.InitSkill(data, playerObject);

        _stat[Enum_StatType.CriticalDamage] = 15;
        _stat[Enum_StatType.MoveSpeed] = 15;
    }

    public override void OnEnemyKill()
    {
        AddPlayerBuff(3);
    }
}
