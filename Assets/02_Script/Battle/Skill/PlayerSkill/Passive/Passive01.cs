using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//일반 공격 시 20% 확률로 5초간 공격속도와 이동속도가 (15+SLV)% 증가한다.
public class Passive01 : PlayerPassiveSkill
{
    public override void InitSkill(PlayerSkillData data, PlayerObject playerObject)
    {
        base.InitSkill(data, playerObject);

        data.CoolTime = 12.2f;

        _stat[Enum_StatType.AttackSpeed] = 15;
        _stat[Enum_StatType.MoveSpeed] = 15;
    }

    public override void OnAttack(CharacterObject characterObject)
    {
        if (!IsSkillEnable())
        {
            return;
        }
        
        if (Random.Range(0, 100) < 20)
        {
            AddPlayerBuff(5);
            ResetCoolTime();   
        }
    }
}
