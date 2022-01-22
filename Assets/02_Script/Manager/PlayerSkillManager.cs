using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerSkillManager : SingletonBehaviour<PlayerSkillManager>
{
    // 스킬 데이터
    //todo: 스펙데이터 나올때까지 임시

    // -스펙데이터
    private List<PlayerSkillData> _dataList;
    public List<PlayerSkillData> DataList => _dataList;

    // -유저데이터 별도

    private const int SkillSlot_Count = 4;

    public List<int> EquippedActiveSkillIndex = new List<int>(SkillSlot_Count) {-1, -1, -1, -1};
    public List<int> EquippedPassiveSkillIndex = new List<int>(SkillSlot_Count) {-1, -1, -1, -1};

    // 스킬 프리팹(효과 & 이펙트)

    [SerializeField] private Transform _activeSkillParent;
    private List<PlayerActiveSkill> _activeSkillList;

    [SerializeField] private Transform _passiveSkillParent;
    private List<PlayerPassiveSkill> _passiveSkillList;

    public int ActiveSkillCount => _activeSkillList.Count;


    // protected override void Awake()
    // {
    //     base.Awake();
    //     InitActiveSkill();
    //     InitPassiveSkill();
    // }
    //
    // private void InitActiveSkill()
    // {
    //     _activeSkillList = new List<PlayerActiveSkill>(_activeSkillParent.childCount);
    //
    //     _activeSkillList = _activeSkillParent.GetComponentsInChildren<PlayerActiveSkill>().ToList();
    //
    //     for (var i = 0; i < _activeSkillList.Count; i++)
    //     {
    //         _activeSkillList[i].InitSkill(new PlayerSkillData(index: i), BattleManager.Instance.PlayerObject);
    //         _activeSkillList[i].gameObject.SetActive(false);
    //     }
    // }
    //
    // private void InitPassiveSkill()
    // {
    //     _passiveSkillList = new List<PlayerPassiveSkill>(_passiveSkillParent.childCount);
    //
    //     _passiveSkillList = _passiveSkillParent.GetComponentsInChildren<PlayerPassiveSkill>().ToList();
    //
    //     for (var i = 0; i < _passiveSkillList.Count; i++)
    //     {
    //         _passiveSkillList[i].InitSkill(new PlayerSkillData(index: i), BattleManager.Instance.PlayerObject);
    //         _passiveSkillList[i].gameObject.SetActive(false);
    //     }
    // }
    //
    // public PlayerActiveSkill GetActiveSkill(int index)
    // {
    //     if (_activeSkillList.Count <= index || index < 0)
    //     {
    //         return null;
    //     }
    //
    //     return _activeSkillList[index];
    // }
    //
    // public PlayerPassiveSkill GetPassiveSkill(int index)
    // {
    //     if (_passiveSkillList.Count <= index || index < 0)
    //     {
    //         return null;
    //     }
    //
    //     return _passiveSkillList[index];
    // }

    public bool EnabelPassive;

    public void EnablePassive(bool isOn)
    {
        EnabelPassive = isOn;
    }
}