﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//전방에 검은 폭발을 일으켜 가장 가까운 6명의 적에게 공격력의 (250+20SLV)% 피해를 입힌다.
//
public class SkillTest02 : PlayerActiveSkill
{
    public override void InitSkill(PlayerSkillData data, PlayerObject playerObject)
    {
        base.InitSkill(data, playerObject);

        _data.CoolTime = 6.6f;
        _data.Damage = 250f;
    }
}
