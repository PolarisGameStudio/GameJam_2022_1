using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerSubstainSkill : PlayerActiveSkill
{
    public bool IsRandomTarget; 
    
    public override bool TryUseSkill()
    {
        if (!IsRandomTarget)
        {
            return base.TryUseSkill();
        }
        
        if (!CanUseSKill())
        {
            return false;
        }

        var monsters = BattleManager.Instance.CurrentBattle.MonsterObjects;
        var target = monsters[Random.Range(0, monsters.Count)];

        var targetMonsterList = FindTargetFromRandomPoint(target.Position, _data.Distance);

        var aliveTargetExist = targetMonsterList.Count(monster => monster.IsAlive) > 0;

        if (!aliveTargetExist)
        {
            return false;
        }
        
        _coolTimer = 0;

        SafeSetActive(true);
        
        Active(targetMonsterList);

        return true;
    }
    
    protected override IEnumerator SkillDamageCoroutine(List<CharacterObject> targets)
    {
        var damage = _playerObject.GetAbility<PlayerAttackAbility>().GetDamage();

        var second = new WaitForSecondsRealtime(1);
        
        for (int i = 0; i <= _data.Time; ++i)
        {
          
            if (IsRandomTarget)
            { 
                var monsters = BattleManager.Instance.CurrentBattle.MonsterObjects;

                monsters.RemoveAll(x => x.IsDeath);
                
                if (monsters.Count != 0)
                {
                    var target = monsters[Random.Range(0, monsters.Count)];

                    targets = FindTargetFromRandomPoint(target.Position, _data.Distance);

                    _skillVFX.transform.position = target.Position;
            
                    _skillVFX.Stop();
                    _skillVFX.Play();
                }
            }
            else
            {
                targets = FindTargetFromPlayer(_data.Distance);
                _skillVFX.transform.position = _playerObject.SkillEffectPos.position;
            
                _skillVFX.Stop();
                _skillVFX.Play();
            }

            if (targets != null && targets.Count > 0)
            {
                for (int j = 0; j < targets.Count; ++j)
                {
                    if (!targets[j].isActiveAndEnabled || targets[j].IsDeath)
                    {
                        continue;
                    }

                    targets[j].TryTakeHit((Value / 100f) * damage.Value, damage.Type);
                }
            }

            yield return second;
        }

        yield return second;
        
        Hide();
    }
}
