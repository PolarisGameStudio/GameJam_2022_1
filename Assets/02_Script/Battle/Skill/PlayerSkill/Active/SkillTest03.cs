using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTest03 : PlayerActiveSkill
{
    public override void InitSkill(PlayerSkillData data, PlayerObject playerObject)
    {
        base.InitSkill(data, playerObject);

        _data.CoolTime = 8.9f;
        _data.Damage = 300f;
    }
}
