using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;


// todo: 각 스킬 효과별로 상속해서 Active메소드 수정해서 사용
public class PlayerActiveSkill : PlayerSkill
{
    protected ParticleSystem _skillVFX;
    protected Animator _animator;

    protected Coroutine _skillCoroutine;

    public bool EffectOnPlayerPosition;
    public float DamageDelay;

    protected WaitForSeconds _damageDelay;
    
    
    [Button]
    public void SetLoop()
    {
        foreach (var c in GetComponentsInChildren<ParticleSystem>())
        {
            var main = c.main;

            main.loop = false;
        }
    }

    protected virtual void Awake()
    {
        _skillVFX = transform.GetChild(0).GetComponent<ParticleSystem>();
        _animator = GetComponent<Animator>();

        //_damageDelay = new WaitForSeconds(AttackPreset.DamageDelay);
        _damageDelay = new WaitForSeconds(DamageDelay);
    }

    public override bool TryUseSkill()
    {
        if (!CanUseSKill())
        {
            return false;
        }

        var targetMonsterList = FindTargetFromPlayer(_data.Distance);

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

        Active(targetMonsterList);

        return true;
    }

    public virtual void Active(List<CharacterObject> targets)
    {
        _skillCoroutine = StartCoroutine(SkillActiveCoroutine(targets));
    }

    protected virtual IEnumerator SkillActiveCoroutine(List<CharacterObject> targets)
    {
        if (EffectOnPlayerPosition)
        {
            transform.position = _playerObject.SkillEffectPos.position;
        }
        else
        {
            int hitCount = 0;
            Vector3 hitPosition = Vector3.zero;

            targets.ForEach(target =>
            {
                if (target.isActiveAndEnabled && !target.IsDeath)
                {
                    hitPosition += target.transform.position;
                    hitCount++;
                }
            });

            transform.position = hitPosition / hitCount;
        }

        PlaySkillEffect();

        yield return _damageDelay;

        _skillCoroutine = StartCoroutine(SkillDamageCoroutine(targets));
    }

    protected virtual IEnumerator SkillDamageCoroutine(List<CharacterObject> targets)
    {
        var damage = _playerObject.GetAbility<PlayerAttackAbility>().GetDamage();

        for (int i = 0; i < targets.Count; ++i)
        {
            if (!targets[i].isActiveAndEnabled || targets[i].IsDeath)
            {
                continue;
            }

            targets[i].TryTakeHit((Value / 100f) * damage.Value, damage.Type);
        }

        yield return new WaitForSecondsRealtime(2f);

        Hide();
    }

    protected virtual void PlaySkillEffect()
    {
        // AttackPreset.PlayBlackout();
        // AttackPreset.PlaySlow();
        // AttackPreset.PlayShake();
    }

    public void Hide()
    {
        if (_skillCoroutine != null)
        {
            StopCoroutine(_skillCoroutine);
            _skillCoroutine = null;
        }

        SafeSetActive(false);
    }

    public virtual string GetDescription()
    {
        return string.Format(_data.Description, Value, _data.SubValue, _data.Distance, _data.Time);
    }
}