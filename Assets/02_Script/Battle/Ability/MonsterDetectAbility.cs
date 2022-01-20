using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDetectAbility : DetectAbility
{
    protected readonly List<CharacterObject> _attackableTargets = new List<CharacterObject>(4);
    public List<CharacterObject> AttackableTargets => _attackableTargets;
    public CharacterObject AttackableTarget => _targets[0];

    public CharacterObject NearestAttackableTarget;

    public bool IsEnableAttack => NearestAttackableTarget != null && NearestAttackableTarget.IsAlive;
    
    public override void Init()
    {
        base.Init();
        
        _attackableTargets.Clear();
    }
    
    protected override void Detect()
    {
        _targets.Clear();
        _attackableTargets.Clear();

        var monsters = BattleManager.Instance.GetMonsters();

        if (monsters == null || monsters.Count == 0)
        {
            return;
        }
        
        float myX = transform.position.x;
        double detectRange = _onwerObject.Stat[Enum_StatType.DetectRange];
        double attackRange = _onwerObject.Stat[Enum_StatType.AttackRange];

        float minDistance = float.MaxValue;
        
        foreach (var monster in monsters)
        {
            if ( !monster.isActiveAndEnabled || monster.IsDeath)
            {
                continue;
            }
            
            float monsterX = monster.Position.x;

            var distance = Mathf.Abs(myX - monsterX);
            
            if (distance <= detectRange)
            {
                _targets.Add(monster);
            }

            if (distance <= attackRange)
            {
                _attackableTargets.Add(monster);
            }

            if (distance <= minDistance && distance <= attackRange)
            {
                minDistance = distance;
                NearestAttackableTarget = monster;
            }
        }
        
    }
}
