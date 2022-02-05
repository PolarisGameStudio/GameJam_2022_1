using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerRandomTargetSkill : PlayerActiveSkill
{
    
    public override bool TryUseSkill()
    {
        if (!CanUseSKill())
        {
            return false;
        }

        var monsters = BattleManager.Instance.CurrentBattle.MonsterObjects;
        
        monsters.RemoveAll(x => x.IsDeath);
        
        var target = monsters[Random.Range(0, monsters.Count)];

        var targetMonsterList = FindTargetFromRandomPoint(target.Position, _data.Distance);

        var aliveTargetExist = targetMonsterList.Count(monster => monster.IsAlive) > 0;

        if (!aliveTargetExist)
        {
            return false;
        }
        
        _coolTimer = 0;

        SafeSetActive(true);

        if (_skillVFX != null)
        {
            _skillVFX.Stop();
            _skillVFX.Play();
        }

        if (_animator != null)
        {
            _animator.enabled = false;
            _animator.enabled = true;
        }

        _skillVFX.transform.position = target.Position;
        
        Active(targetMonsterList);

        return true;
    }
    
    protected override IEnumerator SkillActiveCoroutine(List<CharacterObject> targets)
    {
        PlaySkillEffect();

        yield return _damageDelay;

        _skillCoroutine = StartCoroutine(SkillDamageCoroutine(targets));
    }
}