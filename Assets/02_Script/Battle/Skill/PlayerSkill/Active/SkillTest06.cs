using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//	
//전방으로 심연의 파도를 일으켜 적중한 모든 적에게 공격력의 (350+20*SLV)% 피해를 입히고 5초 동안 이동속도를 50% 감소시킨다.
public class SkillTest06 : PlayerActiveSkill
{
    [SerializeField] private int _hitCount;
    [SerializeField] private float _hitInterval;

    public override void InitSkill(PlayerSkillData data, PlayerObject playerObject)
    {
        base.InitSkill(data, playerObject);

        _data.Damage = 350;
        _data.CoolTime = 14.1f;
    }

    protected override IEnumerator SkillDamageCoroutine(List<CharacterObject> targets)
    {
        var playerAttack = _playerObject.GetAbility<PlayerAttackAbility>().GetDamage();

        var damage = (_data.Damage / 100f) * playerAttack.Value;
        
        var interval = new WaitForSecondsRealtime(_hitInterval);

        for (int i = 0; i < _hitCount; i++)
        {
            for (int j = 0; j < targets.Count; ++j)
            {
                if (!targets[j].isActiveAndEnabled || targets[j].IsDeath)
                {
                    continue;
                }
                
            }

            yield return interval;
        }
        
        yield return new WaitForSecondsRealtime(2f);
        
        Hide();
    }
}