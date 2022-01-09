using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class CharacterObject : GameBehaviour
{
    protected Enum_CharacterType _characterType;
    public Enum_CharacterType CharacterType => _characterType;

    public Stat Stat;

    protected bool _alive;
    public bool IsAlive => _alive;
    public bool IsDeath => !_alive;

    protected CharacterAbility[] _abilities;
    protected bool _abilitiesCached = false;

    [Header("모델")]
    public Transform Model;

    protected double _currentHealth; 
    public double CurrentHealth => _currentHealth;
    
    
    public void Init(Enum_CharacterType characterType, Stat stat = null)
    {
        _characterType = characterType;

        if (stat != null)
        {
            Stat = stat;
        }
        
        Model.localPosition = Vector3.zero;

        OnInit();

        _alive = true;
    }
    
    protected virtual void OnInit() {}

    protected virtual void CacheAbilities()
    {
        _abilities = GetComponents<CharacterAbility>();

        foreach (var ability in _abilities)
        {
            ability.Init();
        }

        _abilitiesCached = true;
    }

    protected virtual void InitAbilities()
    {
        if (!_abilitiesCached)
        {
            CacheAbilities();
            return;
        }
        
        foreach (var ability in _abilities)
        {
            ability.Init();
        }
    }
    
    public T GetAbility<T>() where T : CharacterAbility
    {
        if (!_abilitiesCached)
        {
            CacheAbilities();
        }
            
        foreach (CharacterAbility ability in _abilities)
        {
            if (ability is T characterAbility)
            {
                return characterAbility;
            }
        }
        
        Type searchedAbilityType = typeof(T);
        Debug.LogError("Failed to get ability! " + searchedAbilityType.ToString());
        return null;
    }

    public void SetAlive(bool isAlive)
    {
        _alive = isAlive;
    }

    public virtual void OnAttack(CharacterObject target, double damage, Enum_DamageType damageType) {}

    public bool TryTakeHit(double damage, Enum_DamageType damageType)
    {
        if (!_alive)
        {
            return false;
        }

        //Stat[Enum_StatType.Health] -= damage;
        _currentHealth -= damage;
        
        OnTakeHit(damage, damageType);

        if (_currentHealth <= 0)
        {
            Death();
        }

        return true;
    }

    public void Death()
    {
        _currentHealth = 0;

        _alive = false;
        
        OnDeath();
    }
    
    protected virtual void OnTakeHit(double damage, Enum_DamageType damageType) {}
    protected virtual void OnDeath() {}

    public void AddBuff(Buff buff)
    {
        GetAbility<BuffAbility>().AddBuff(buff);
        OnBuffChange();
    }
    
    public virtual void CalculateStat() {}

    protected virtual void OnBuffChange() {}
    

    protected virtual void Update()
    {
        var dt = Time.deltaTime;

        if (_abilities == null)
        {
            return;
        }
        
        foreach (var ability in _abilities)
        {
            ability.EarlyProcessAbility();
        }
        
        foreach (var ability in _abilities)
        {
            ability.ProcessAbility(dt);
        }
    }

    protected virtual void LateUpdate()
    {
        if (_abilities == null)
        {
            return;
        }
        
        foreach (var ability in _abilities)
        {
            ability.LateProcessAbility();
        } 
    }

    public virtual void Kill()
    {
        _alive = false;
        
        SafeSetActive(false);
    }
}
