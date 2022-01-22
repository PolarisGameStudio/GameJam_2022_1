using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Damage
{
    public double Value;
    public Enum_DamageType Type;
}

public class PlayerAttackAbility : CharacterAbility
{
    private float _attackCoolTime;
    public bool IsAttackPossible => _attackCoolTime <= 0;

    private List<AttackPreset> _attackPresets = new List<AttackPreset>();
    public int AttackPresetCount => _attackPresets.Count;

    public int MaxAttackCount = 3;

    public override void Init()
    {
        base.Init();

        _attackCoolTime = 0f;
    }

    public bool Attack(CharacterObject target)
    {
        var damage = GetDamage();

        if (target.TryTakeHit(damage.Value, damage.Type))
        {
            _onwerObject.OnAttack(target, damage.Value, damage.Type);
            
            return true;
        }

        return false;
    }

    public void DelayAttack(CharacterObject target, float delay)
    {
        if (delay <= 0f)
        {
            Attack(target);
            return;
        }

        StartCoroutine(DelayAttack_Coroutine(target, delay));
    }

    private IEnumerator DelayAttack_Coroutine(CharacterObject target, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (!target.isActiveAndEnabled || target.IsDeath)
        {
            yield break;
        }

        var damage = GetDamage();

        if (target.TryTakeHit(damage.Value, damage.Type))
        {
            _onwerObject.OnAttack(target, damage.Value, damage.Type);
        }
    }

    public Damage GetDamage()
    {
        Enum_DamageType damageType = Enum_DamageType.Normal;
        var isSuperCritical = IsSuperCritical();
        
        if (isSuperCritical)
        {
            damageType = Enum_DamageType.SuperCritical;
        }
        else
        {
            var isCritical = IsCritical();
            damageType = isCritical ? Enum_DamageType.Critical : Enum_DamageType.Normal;
        }
        

        var value = 0d;
        
        if (damageType == Enum_DamageType.SuperCritical)
        {
            value = GetSuperCriticalDamage();
        }
        else if (damageType == Enum_DamageType.Critical)
        {
            value = GetCriticalDamage();
        }
        else
        {
            value = GetNormalDamage();
        }

        return new Damage()
        {
            Value = value,
            Type = damageType,
        };
    }

    public bool IsCritical()
    {
        return Random.Range(0, 100f) <= _onwerObject.Stat[Enum_StatType.CriticalChance];
    }

    public bool IsSuperCritical()
    {
        return Random.Range(0, 100f) <= _onwerObject.Stat[Enum_StatType.SuperCriticalChance];
    }


    public double GetNormalDamage()
    {
        return _onwerObject.Stat[Enum_StatType.Damage];
    }

    public double GetCriticalDamage()
    {
        return GetNormalDamage() * _onwerObject.Stat[Enum_StatType.CriticalDamage] / 100f;
    }
    
    public double GetSuperCriticalDamage()
    {
        return GetNormalDamage() * _onwerObject.Stat[Enum_StatType.SuperCriticalDamage] / 100f;
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
}