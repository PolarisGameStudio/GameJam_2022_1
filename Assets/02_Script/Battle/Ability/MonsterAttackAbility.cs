using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttackAbility : CharacterAbility
{
    private PlayerObject _playerObject;
    
    private float _attackCoolTime;
    public bool IsAttackPossible => _attackCoolTime <= 0;

    private AttackPreset _attackPreset;
    
    
    public override void Init()
    {
        base.Init();

        _attackCoolTime = 0f;

        if (_playerObject == null)
        {
            _playerObject = BattleManager.Instance.PlayerObject;
        }
    }
    
    public bool Attack()
    {
        var damage = GetDamage();
        
        if (_playerObject.TryTakeHit(damage, Enum_DamageType.Normal))
        {
            _onwerObject.OnAttack(_playerObject, damage, Enum_DamageType.Normal);
            
            return true;
        }
        
        return false;
    }

    public double GetDamage()
    {
        return _onwerObject.Stat[Enum_StatType.Damage];
    }
    
    public void SetAttackCoolTime(float time)
    {
        _attackCoolTime = time;
    }
    
    public override void ProcessAbility(float deltaTime)
    {
        _attackCoolTime -= deltaTime;

        if (_attackCoolTime < 0)
        {
            _attackCoolTime = 0;
        }
    }

    public void SetAttackPreset(AttackPreset attackPreset)
    {
        _attackPreset = attackPreset;
    }
}
