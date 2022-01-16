using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 바닥에서 악의 손아귀가 나타나 6명의 적에게 공격력의 (230+15SLV)% 피해를 입히고 3초 동안 속박한다

public class SKillTest01 : PlayerActiveSkill
{
    public override void InitSkill(PlayerSkillData data, PlayerObject playerObject)
    {
        base.InitSkill(data, playerObject);
        
        _data.CoolTime = 9.1f;
        _data.Damage = 230;
    }

    public override void Active(List<CharacterObject> targets)
    {
        // var currentHealth = _playerObject.StatChange[Enum_StatType.Health];
        // currentHealth -= currentHealth * 0.3f;
        // _playerObject.StatChange[Enum_StatType.Health] = currentHealth;
        //
        // StatChange buffStat = new StatChange();
        // buffStat[Enum_StatType.Damage] = 30;
        //
        // _playerObject.AddBuff(new Buff(buffStat, 10f));

        base.Active(targets);
    }
}
    