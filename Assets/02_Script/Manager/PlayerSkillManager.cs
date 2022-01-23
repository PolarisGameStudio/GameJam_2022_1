using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerSkillManager : SingletonBehaviour<PlayerSkillManager>
{
    // -스펙데이터
    private List<TBL_SKILL> _dataList;
    public List<TBL_SKILL> DataList => _dataList;

    // 스킬 프리팹(효과 & 이펙트)

    private List<PlayerActiveSkill> _activeSkillList;

    protected override void Awake()
    {
        base.Awake();
        InitActiveSkill();
    }

    [Button]

    public void SetSkillOrder()
    {
        var list = GetComponentsInChildren<ParticleSystem>();

        foreach (var system in list)
        {
            system.GetComponent<Renderer>().sortingLayerName = "Entity";
            system.GetComponent<Renderer>().sortingOrder = 999;
        }
    }

    
    private void InitActiveSkill()
    {
        _activeSkillList = new List<PlayerActiveSkill>(TBL_SKILL.CountEntities);
    
        _activeSkillList = transform.GetComponentsInChildren<PlayerActiveSkill>().ToList();
    
        for (var i = 0; i < _activeSkillList.Count; i++)
        {
            _activeSkillList[i].InitSkill(TBL_SKILL.GetEntity(i), BattleManager.Instance.PlayerObject);
            _activeSkillList[i].gameObject.SetActive(false);
        }
    }

    public PlayerActiveSkill GetSkillPrefab(int index)
    {
        if (_activeSkillList.Count <= index || index < 0)
        {
            return null;
        }
    
        return _activeSkillList[index];
    }
}