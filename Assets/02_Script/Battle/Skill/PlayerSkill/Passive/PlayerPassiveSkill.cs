using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPassiveSkill : PlayerSkill
{
    protected Stat _stat = new Stat();
    
    public virtual void OnEquip()
    {
    }

    public virtual void OnUnEquip()
    {
    }

    public virtual void OnAttack(CharacterObject characterObject)
    {
    }

    public virtual void OnBerserkStart()
    {
    }

    public virtual void OnBerserkEnd()
    {
    }

    public virtual void OnHit()
    {
    }

    public virtual void OnEnemyKill()
    {
    }

    public virtual void OnDeath(double damage)
    {
    }

    public virtual bool IsDeBuffEnemyAdditionalDamage()
    {
        return false;
    }

    public virtual void OnDeBuffed()
    {
        //자신에게 걸려 있는 상태 이상 효과 1개당
    }

    protected void ResetCoolTime()
    {
        _coolTimer = 0;
    }

    protected void AddPlayerBuff(float duration)
    {
        _playerObject.AddBuff(new Buff(_stat, duration));
    }
}