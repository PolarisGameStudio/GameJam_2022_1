using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// todo: 스킬 애니메이션 있으면 State상태에서 애니메이션 실행 후 Idle로 (임모탈)
// todo: 없으면 State없이 PlayerSkillAbility 자체에서 쿨타임 체크 후 자동사용  (창술사)

public class PlayerSkillAbility : CharacterAbility
{
    private List<PlayerActiveSkill> _activeSkills = new List<PlayerActiveSkill>();
    private List<PlayerPassiveSkill> _passiveSkills = new List<PlayerPassiveSkill>();

    private float _skillIntervalTime = 1f;
    private float _skillIntervalTimer = 0;

    // todo:쿨타임 체크 후 자동 사용
    public override void ProcessAbility(float deltaTime)
    {
        base.ProcessAbility(deltaTime);

        _skillIntervalTimer += deltaTime;

        if (_activeSkills != null)
        {
            for (var i = 0; i < _activeSkills.Count; i++)
            {
                _activeSkills[i]?.UpdateCoolTime(deltaTime);
            }
        }

        if (_passiveSkills != null)
        {
            for (var i = 0; i < _passiveSkills.Count; i++)
            {
                _passiveSkills[i]?.UpdateCoolTime(deltaTime);
            }
        }

        if (!_onwerObject.GetAbility<MonsterDetectAbility>().HaveTarget)
        {
            return;
        }

        if (_skillIntervalTime > _skillIntervalTimer)
        {
            return;
        }

        foreach (var activeSkill in _activeSkills)
        {
            if (activeSkill == null)
            {
                continue;
            }
            
            if (activeSkill.TryUseSkill())
            {
                _skillIntervalTimer = 0;
                break;
            }
        }
    }

    public void RefreshSkill()
    {
        RefreshActiveSkill();
    }
    
    public void RefreshActiveSkill()
    {
        var activeSkillIndex = DataManager.SkillData.EquippedIndex;
            
        _activeSkills = new List<PlayerActiveSkill>(activeSkillIndex.Count);
    
        foreach (var index in activeSkillIndex)
        {
            if (index == -1)
            {
                _activeSkills.Add(null);
                continue;
            }
            
            var skill = PlayerSkillManager.Instance.GetSkillPrefab(index);
            _activeSkills.Add(skill);
        }
    }
    
    // public void RefreshPassiveSkill()
    // {
    //     var passiveSkillIndex = PlayerSkillManager.Instance.EquippedPassiveSkillIndex;
    //     _passiveSkills = new List<PlayerPassiveSkill>(passiveSkillIndex.Count);
    //
    //     foreach (var index in passiveSkillIndex)
    //     {
    //         var skill = PlayerSkillManager.Instance.GetPassiveSkill(index);
    //         _passiveSkills.Add(skill);
    //     }
    // }

    public void HideSkills()
    {
        if (_activeSkills != null)
        {
            return;
        }

        foreach (var activeSkill in _activeSkills)
        {
            activeSkill.Hide();
        }
    }
    
    
    
    
    // ========================================================================================================================================
    //========================================================================================================================================
    // 패시브 스킬 트리거
    
    public void OnDeath(double damage)
    {
        foreach (var passive in _passiveSkills)
        {
            passive.OnDeath(damage);
        }
    }

    public void OnHit()
    {
        foreach (var passive in _passiveSkills)
        {
            passive.OnHit();
        }
    }   
    
    public void OnEnemyKill()
    {
        foreach (var passive in _passiveSkills)
        {
            passive.OnEnemyKill();
        }
    }    
    
    public void OnAttack(CharacterObject target)
    {
        foreach (var passive in _activeSkills)
        {
            if (passive == null)
            {
                continue;
            }

            passive.OnPlayerAttack(target);
        }
    }

    public void OnBerserkStart()
    {
        foreach (var passive in _passiveSkills)
        {
            passive.OnBerserkStart();
        }
    }
}